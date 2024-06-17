using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

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
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services, Action<RetryStrategyOptions> configureRetry)
        => services.AddHttpResiliencyPipeline(HttpResilienceConstants.DefaultPipelineKey, configureRetry);
    
    /// <summary>
    /// Adds a <see cref="ResiliencePipelineBuilder"/> to the <see cref="IServiceCollection"/> using the default
    /// resilience options. Timeout specified. If zero, no timeout.
    /// </summary>
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services, TimeSpan timeout)
        => services.AddHttpResiliencyPipeline(HttpResilienceConstants.DefaultPipelineKey, null, timeout);
    
    /// <summary>
    /// Adds a <see cref="ResiliencePipelineBuilder"/> to the <see cref="IServiceCollection"/> using the default
    /// resilience options. Optionally specifies a pipeline name, retry strategy options, and timeout.
    /// </summary>
    public static IServiceCollection AddHttpResiliencyPipeline(this IServiceCollection services, 
        string pipelineKey = HttpResilienceConstants.DefaultPipelineKey, Action<RetryStrategyOptions>? configureRetry = null, TimeSpan? timeout = null)
        => services.AddResiliencePipeline(pipelineKey, _ => HttpResilience.GetResiliencePipelineBuilder(configureRetry, timeout));
}