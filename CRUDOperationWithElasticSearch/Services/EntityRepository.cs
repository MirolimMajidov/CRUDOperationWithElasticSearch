using Elasticsearch.Net;
using MyUser.Models;
using System.Reflection.Metadata;
using System.Text.Json;

namespace MyUser.Services;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
{
    private readonly IElasticLowLevelClient _elasticClient;
    private readonly string _indexName;

    public EntityRepository(IElasticLowLevelClient elasticClient)
    {
        _elasticClient = elasticClient;
        _indexName = typeof(TEntity).Name.ToLower();

        // Create the index if it does not exist
        EnsureIndexExists();
    }

    private void EnsureIndexExists()
    {
        var indexExistsResponse = _elasticClient.Indices.Exists<StringResponse>(_indexName);

        if (!indexExistsResponse.Success)
        {
            // Index does not exist, create it
            var createIndexResponse = _elasticClient.Indices.Create<StringResponse>(_indexName, PostData.Serializable(new { }));

            if (!createIndexResponse.Success)
            {
                // Handle error, e.g., log or throw an exception
                throw new Exception($"Failed to create index {_indexName}. Error: {createIndexResponse.Body}");
            }
        }
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var response = await _elasticClient.GetAsync<StringResponse>(_indexName, id.ToString());

        if (response.Success)
        {
            // Deserialize and return the entity
            return JsonSerializer.Deserialize<TEntity>(response.Body);
        }
        else
        {
            // Handle error, e.g., log or throw an exception
            return null;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        // Implement fetching all entities from Elasticsearch
        var searchResponse = await _elasticClient.SearchAsync<StringResponse>(_indexName, PostData.Serializable(new { }));

        if (searchResponse.Success)
        {
            // Deserialize and return the list of entities
            var documents = searchResponse.Body
                .Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(doc => JsonSerializer.Deserialize<TEntity>(doc));

            return documents;
        }
        else
        {
            // Handle error, e.g., log or throw an exception
            return Enumerable.Empty<TEntity>();
        }
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var indexResponse = await _elasticClient.IndexAsync<StringResponse>(_indexName, Guid.NewGuid().ToString(), PostData.Serializable(entity));

        if (indexResponse.Success)
        {
            return entity;
        }
        else
        {
            // Handle error, e.g., log or throw an exception
            return null;
        }
    }

    public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var updateResponse = await _elasticClient.UpdateAsync<StringResponse>(_indexName, id.ToString(), PostData.Serializable(entity));

        if (updateResponse.Success)
        {
            // Fetch and return the updated entity
            return await GetByIdAsync(id);
        }
        else
        {
            // Handle error, e.g., log or throw an exception
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var deleteResponse = await _elasticClient.DeleteAsync<StringResponse>(_indexName, id.ToString());

            return deleteResponse.Success;
        }
        catch
        {
            return false;
        }
    }
}