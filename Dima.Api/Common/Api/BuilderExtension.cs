using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configurations.CONNECTION_STRING = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            Configurations.BACKEND_URL = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
            Configurations.BACKEND_URL = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(setupAction: x => x.CustomSchemaIds(n => n.FullName));
        }

        public static void AddSecurity(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                            .AddIdentityCookies();
        }

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configurations.CONNECTION_STRING));

            // Adicionando o serviço para trabalhar com identity
            builder.Services.AddIdentityCore<User>() // Adiciona ao serviço do Identity a classe customizada que foi criada de User
                            .AddRoles<IdentityRole<long>>() // Adiciona ao serviço do Identity a classe customizada que foi criada de Role
                            .AddEntityFrameworkStores<AppDbContext>() // Adicionando o serviço do banco que será usado para criação das tabelas do Identity
                            .AddApiEndpoints(); // Cria automaticamente os endpoint para o serviço de autenticação

            builder.Services.AddAuthorization();
        }

        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            // Habilitando o serviço no container de Cors para permitir que o serviço Blazor Web consuma a API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: ApiConfiguration.CORS_POLICY_NAME,
                                  policy =>
                                  {
                                      policy.WithOrigins(Configurations.BACKEND_URL,
                                                         Configurations.FRONTEND_URL)
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                            .AllowCredentials();
                                  });
            });
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
            builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
        }
    }
}