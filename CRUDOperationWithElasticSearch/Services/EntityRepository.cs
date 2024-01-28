using CRUDOperationWithElasticSearch.Models.Helpers;
using Elastic.Clients.Elasticsearch;
using MyUser.Models;

namespace MyUser.Services;

public class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ElasticsearchClient _elasticClient;
    private readonly string _indexName;

    public EntityRepository(ElasticsearchConfig elasticsearchConfig)
    {
        _indexName = typeof(TEntity).Name.ToLower();
        _elasticClient = new ElasticsearchClient(new Uri(elasticsearchConfig.Uri));

        // Create the index if it does not exist
        EnsureIndexExists();
    }

    private void EnsureIndexExists()
    {
        var indexExistsResponse = _elasticClient.Indices.Exists(_indexName);
        if (!indexExistsResponse.Exists)
        {
            // Index does not exist, create it
            var createIndexResponse = _elasticClient.Indices.Create(_indexName);
            if (!createIndexResponse.IsValidResponse)
            {
                var reason = createIndexResponse.ElasticsearchServerError?.Error?.Reason;
                throw new Exception($"Failed to create index {_indexName}. Error: {reason}");
            }
        }
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        var response = await _elasticClient.GetAsync<TEntity>(id, idx => idx.Index(_indexName));

        if (response.IsValidResponse)
            return response.Source;

        return default;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(int? from, int? size)
    {
        var indices = new List<string> { _indexName };
        var response = await _elasticClient.SearchAsync<TEntity>(s => s
            .Indices(indices.ToArray()) // pass the array of index names
            .From(from)
            .Size(size)
        );

        if (response.IsValidResponse)
            return response.Documents;

        return Enumerable.Empty<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var response = await _elasticClient.IndexAsync(entity, i => i.Index(_indexName).Id(entity.Id));
        if (response.IsValidResponse)
            return entity;

        return null;
    }

    public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var response = await _elasticClient.UpdateAsync<TEntity, TEntity>(_indexName, id, u => u.Doc(entity));

        if (response.IsValidResponse)
        {
            // Fetch and return the updated entity
            return await GetByIdAsync(id);
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _elasticClient.DeleteAsync<TEntity>(id);
        return response.IsValidResponse;
    }
}