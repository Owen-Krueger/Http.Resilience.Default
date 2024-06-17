using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

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
    public static IHttpResiliencePipelineBuilder AddHttpResiliencyPipeline(this IHttpClientBuilder builder, 
        ResilienceOptions? options = null)
        => builder.AddResilienceHandler(HttpResilienceConstants.DefaultPipelineKey, 
            _ => HttpResilience.GetResiliencePipelineBuilder(options));
    
    /// <summary>
    /// Adds a <see cref="IHttpStandardResiliencePipelineBuilder"/> to the <see cref="HttpClient"/> using the default
    /// resilience options. Optionally specifies a pipeline name.
    /// </summary>
    public static IHttpResiliencePipelineBuilder AddHttpResiliencyPipeline(this IHttpClientBuilder builder, 
        string? pipelineName = null, ResilienceOptions? options = null)
        => builder.AddResilienceHandler(pipelineName ?? HttpResilienceConstants.DefaultPipelineKey, 
            _ => HttpResilience.GetResiliencePipelineBuilder(options));
}