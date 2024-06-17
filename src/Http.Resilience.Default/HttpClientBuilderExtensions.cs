using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace Http.Resiliency;

public static class HttpClientBuilderExtensions
{
    public static IHttpResiliencePipelineBuilder AddHttpResiliencyPipeline(this IHttpClientBuilder builder, 
        ResilienceOptions? options = null)
        => builder.AddResilienceHandler(HttpResilienceConstants.DefaultPipelineKey, 
            _ => HttpResilience.GetResilienceHandler(options));
    
    public static IHttpResiliencePipelineBuilder AddHttpResiliencyPipeline(this IHttpClientBuilder builder, 
        string? pipelineName = null, ResilienceOptions? options = null)
        => builder.AddResilienceHandler(pipelineName ?? HttpResilienceConstants.DefaultPipelineKey, 
            _ => HttpResilience.GetResilienceHandler(options));
}