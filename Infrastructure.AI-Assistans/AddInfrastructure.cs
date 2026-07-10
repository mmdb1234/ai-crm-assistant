
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
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                Console.WriteLine("DATABASE_URL:");
                Console.WriteLine(databaseUrl);
                options.UseNpgsql(
                     Environment.GetEnvironmentVariable("DATABASE_URL")??configuration.GetConnectionString("DefaultConnection"));
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

            services.AddSingleton<
                IChatIngestionService,
                ChatIngestionService>();

            services.AddHostedService<ChatIngestionService>(sp =>
                (ChatIngestionService)sp.GetRequiredService<IChatIngestionService>());

            return services;
        }
    }

}
