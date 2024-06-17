using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Http.Resiliency;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services,
        ResilienceOptions? options = null)
        => services.AddHttpResiliencyPipeline(HttpResilienceConstants.DefaultPipelineKey, options);
    
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services, 
        string pipelineKey = HttpResilienceConstants.DefaultPipelineKey, ResilienceOptions? options = null)
        => services.AddResiliencePipeline(pipelineKey, _ => HttpResilience.GetResilienceHandler(options));
}