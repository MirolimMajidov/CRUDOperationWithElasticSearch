using CRUDOperationWithElasticSearch.Models.Helpers;
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

        builder.Services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));

        builder.Services.AddAuthorizations();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

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
        CreateTestData();

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


        void CreateTestData()
        {
            using (var scope = app.Services.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<User>>();
                var users = new List<User>()
                {
                    new User(){ Id = Guid.Parse("4E6B06C5-BFCE-4D94-9BA0-79C1D557BFAF"), FirstName = "Jahonger", LastName = "Ahmedov", Username = "User1", Password = "User11" },
                    new User(){ Id = Guid.Parse("E036847E-6B8F-489D-A8E3-59F45778B013"),  FirstName = "Jake", LastName = "Esh" , Username = "User2", Password = "User22" },
                    new User(){ Id = Guid.Parse("76ABF344-4036-42B4-8272-1B269A8A2369"),  FirstName = "Rasul", LastName = "Azimov" , Username = "User3", Password = "User33" },
                };

                foreach (var user in users)
                    userRepository.CreateAsync(user);

                var backpackRepository = scope.ServiceProvider.GetRequiredService<IEntityRepository<Backpack>>();
                var firstUser = users.First();
                var backpacks = new List<Backpack>()
                {
                    new Backpack(){ Id = Guid.Parse("10442774-9CD3-4BB0-B17B-84F69BFFF3D5"),  Name = "First", UserId = firstUser.Id },
                    new Backpack(){ Id = Guid.Parse("CB365C49-9A1A-41B6-A8CA-AFFF328BD7AA"),  Name = "Second", UserId = firstUser.Id },
                };

                foreach (var backpack in backpacks)
                    backpackRepository.CreateAsync(backpack);
            }
        }
    }
}
