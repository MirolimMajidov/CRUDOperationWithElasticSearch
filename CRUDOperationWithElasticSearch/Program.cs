using CRUDOperationWithElasticSearch.Models.Helpers;
using Elasticsearch.Net;
using Microsoft.OpenApi.Models;
using MyUser.Models;
using MyUser.Models.Helpers;
using MyUser.Services;

namespace MyUser;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region services

        // Add services to the container.
        builder.Services.AddSingleton<ElasticsearchConfig>(builder.Configuration.GetSection("ElasticsearchConfig").Get<ElasticsearchConfig>());
        builder.Services.AddSingleton<IElasticLowLevelClient>(provider =>
        {
            var config = provider.GetRequiredService<ElasticsearchConfig>();
            var pool = new SingleNodeConnectionPool(new Uri(config.Uri));
            var connectionSettings = new ConnectionConfiguration(pool);

            return new ElasticLowLevelClient(connectionSettings);
        });
        //builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(UserContext.ConnectionString));

        builder.Services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));

        builder.Services.AddAuthorizations();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test application APIs", Version = "v1" });

            // Add the JWT Bearer authentication scheme
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };
            c.AddSecurityDefinition("Bearer", securityScheme);

            // Use the JWT Bearer authentication scheme globally
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            });
        });

        #endregion

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<User>>();
            var users = new List<User>()
                {
                    new User(){ FirstName = "Jahonger", LastName = "Ahmedov", Username = "User1", Password = "User11" },
                    new User(){ FirstName = "Jake", LastName = "Esh" , Username = "User2", Password = "User22" },
                    new User(){ FirstName = "Rasul", LastName = "Azimov" , Username = "User3", Password = "User33" },
                };

            foreach (var user in users)
                userRepository.CreateAsync(user);

            var backpackRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<Backpack>>();
            var firstUser = users.First();
            var backpacks = new List<Backpack>()
                {
                    new Backpack(){ Name = "First", UserId = firstUser.Id },
                    new Backpack(){ Name = "Second", UserId = firstUser.Id },
                };

            foreach (var backpack in backpacks)
                backpackRepository.CreateAsync(backpack);
        }

        #region Middlewares

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        #endregion

        app.Run();
    }
}
