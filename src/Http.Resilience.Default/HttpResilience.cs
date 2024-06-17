using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace Http.Resiliency;

public static class HttpResilience
{
    public static ResiliencePipelineBuilder GetResilienceHandler(ResilienceOptions? options = null)
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
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            });
    }
}