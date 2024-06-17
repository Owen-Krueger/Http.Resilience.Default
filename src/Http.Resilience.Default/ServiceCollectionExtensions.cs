using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Http.Resilience.Default;

/// <summary>
/// Extension methods for adding a <see cref="ResiliencePipelineBuilder"/> to a <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="ResiliencePipelineBuilder"/> to the <see cref="IServiceCollection"/> using the default
    /// resilience options.
    /// </summary>
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services,
        ResilienceOptions? options = null)
        => services.AddHttpResiliencyPipeline(HttpResilienceConstants.DefaultPipelineKey, options);
    
    /// <summary>
    /// Adds a <see cref="ResiliencePipelineBuilder"/> to the <see cref="IServiceCollection"/> using the default
    /// resilience options. Optionally specifies a pipeline name.
    /// </summary>
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services, 
        string pipelineKey = HttpResilienceConstants.DefaultPipelineKey, ResilienceOptions? options = null)
        => services.AddResiliencePipeline(pipelineKey, _ => HttpResilience.GetResiliencePipelineBuilder(options));
}