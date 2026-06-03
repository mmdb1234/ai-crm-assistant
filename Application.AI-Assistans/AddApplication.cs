

using Microsoft.Extensions.DependencyInjection;

namespace Features.AI_Assistans
{
    public static class FeaturesDependencyInjection 
    { 
        public static IServiceCollection AddFeatures(this IServiceCollection services) 
        { 
            return services; 
        } 
    }
}
