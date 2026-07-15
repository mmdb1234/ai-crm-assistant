
using Domain.AI_Assistans.Interfaces;
using Features.AI_Assistans.Services;
using Infrastructure.AI_Assistans.AI;
using Infrastructure.AI_Assistans.Factories;
using Infrastructure.AI_Assistans.Persistence;
using Infrastructure.AI_Assistans.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.AI_Assistans
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                //var host = Environment.GetEnvironmentVariable("PGHOST");
                //var port = Environment.GetEnvironmentVariable("PGPORT");
                //var database = Environment.GetEnvironmentVariable("PGDATABASE");
                //var username = Environment.GetEnvironmentVariable("PGUSER");
                //var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

                //var connectionString =
                //    $"Host={host};" +
                //    $"Port={port};" +
                //    $"Database={database};" +
                //    $"Username={username};" +
                //    $"Password={password};" +
                //    "SSL Mode=Require;" +
                //    "Trust Server Certificate=true;";

                //options.UseNpgsql(connectionString);
            });
            services.AddScoped<IAppDbContext, AppDbContext>(sp =>sp.GetRequiredService<AppDbContext>());
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IChatConnectionRepository, ChatConnectionRepository>();

            services.Configure<AIProvidersOptions>(configuration.GetSection("AIProviders"));

            services.AddHttpClient();

            services.AddKeyedScoped<
                IAIAnalysisService,
                OpenAIAnalysisService>("OpenAI");

            services.AddKeyedScoped<
                IAIAnalysisService,
                DeepSeekAnalysisService>("DeepSeek");

            services.AddKeyedScoped<
                IAIAnalysisService,
                GeminiAnalysisService>("Gemini");

            services.AddKeyedScoped<
                IAIAnalysisService, 
                OpenRouterAnalysisService>("OpenRouter");

            services.AddScoped<
                IAIAnalysisServiceFactory,
                AIAnalysisServiceFactory>();

            services.AddScoped<ITelegramBotService, TelegramBotService>();

            services.AddSingleton<
                IChatIngestionService,
                ChatIngestionService>();

            services.AddHostedService<ChatIngestionService>(sp =>
                (ChatIngestionService)sp.GetRequiredService<IChatIngestionService>());

            return services;
        }
    }

}
