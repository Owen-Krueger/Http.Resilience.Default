using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly.Retry;

namespace Http.Resilience.Default;

/// <summary>
/// Extension methods for adding a <see cref="IHttpStandardResiliencePipelineBuilder"/> to a <see cref="HttpClient"/>
/// using a <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Adds a <see cref="IHttpStandardResiliencePipelineBuilder"/> to the <see cref="HttpClient"/> using the default
    /// resilience options.
    /// </summary>
    public static IHttpResiliencePipelineBuilder AddHttpResilienceHandler(this IHttpClientBuilder builder, 
        Action<RetryStrategyOptions> configureRetry)
        => builder.AddHttpResilienceHandler(HttpResilienceConstants.DefaultPipelineKey, configureRetry);
    
    /// <summary>
    /// Adds a <see cref="IHttpStandardResiliencePipelineBuilder"/> to the <see cref="HttpClient"/> using the default
    /// resilience options. Timeout specified. If zero, no timeout.
    /// </summary>
    public static IHttpResiliencePipelineBuilder AddHttpResilienceHandler(this IHttpClientBuilder builder, 
        TimeSpan timeout)
        => builder.AddHttpResilienceHandler(HttpResilienceConstants.DefaultPipelineKey, null, timeout);
    
    /// <summary>
    /// Adds a <see cref="IHttpStandardResiliencePipelineBuilder"/> to the <see cref="HttpClient"/> using the default
    /// resilience options. Timeout specified. If zero, no timeout.
    /// </summary>
    public static IHttpResiliencePipelineBuilder AddHttpResilienceHandler(this IHttpClientBuilder builder, 
        Action<RetryStrategyOptions> configureRetry, TimeSpan timeout)
        => builder.AddHttpResilienceHandler(HttpResilienceConstants.DefaultPipelineKey, configureRetry, timeout);
    
    /// <summary>
    /// Adds a <see cref="IHttpStandardResiliencePipelineBuilder"/> to the <see cref="HttpClient"/> using the default
    /// resilience options. Optionally specifies a pipeline name, retry strategy options, and timeout.
    /// </summary>
    public static IHttpResiliencePipelineBuilder AddHttpResilienceHandler(this IHttpClientBuilder builder, 
        string? pipelineName = null, Action<RetryStrategyOptions>? configureRetry = null, TimeSpan? timeout = null)
        => builder.AddResilienceHandler(pipelineName ?? HttpResilienceConstants.DefaultPipelineKey, 
            _ => HttpResilience.GetResiliencePipelineBuilder(configureRetry, timeout));
}