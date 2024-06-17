using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace Http.Resilience.Default;

/// <summary>
/// Factory class for creating a resilience pipeline.
/// </summary>
public static class HttpResilience
{
    /// <summary>
    /// Get a resilience pipeline builder with any specified options.
    /// </summary>
    public static ResiliencePipelineBuilder GetResiliencePipelineBuilder(ResilienceOptions? options = null)
    {
        options ??= new ResilienceOptions();
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<InvalidOperationException>()
                    .Handle<TaskCanceledException>()
                    .Handle<UriFormatException>()
                    .Handle<TimeoutRejectedException>()
                    .Handle<HttpRequestException>()
                    .HandleResult(response =>
                        response is HttpResponseMessage result && options.ResponseValidation(result)),
                Delay = options.Delay,
                MaxRetryAttempts = options.MaxRetryAttempt,
                BackoffType = options.BackoffType,
                UseJitter = options.UseJitter
            });
    }
}