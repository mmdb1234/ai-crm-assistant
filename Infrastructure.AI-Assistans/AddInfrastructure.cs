
using Domain.AI_Assistans.Interfaces;
using Infrastructure.AI_Assistans.AI;
using Infrastructure.AI_Assistans.Factories;
using Infrastructure.AI_Assistans.Persistence;
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
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"));
            });

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


            return services;
        }
    }

}
