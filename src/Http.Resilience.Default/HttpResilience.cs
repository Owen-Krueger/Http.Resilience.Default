using Polly;
using Polly.Retry;

namespace Http.Resilience.Default;

/// <summary>
/// Factory class for creating a resilience pipeline.
/// </summary>
public static class HttpResilience
{
    /// <summary>
    /// Get a resilience pipeline builder with any specified options. Timeout specified.
    /// If not specified, default is 30 seconds. If zero, no timeout.
    /// </summary>
    public static ResiliencePipelineBuilder GetResiliencePipelineBuilder(TimeSpan timeout)
        => GetResiliencePipelineBuilder(null, timeout);
    
    /// <summary>
    /// Get a resilience pipeline builder with any specified options. Retry strategy options and timeout can
    /// optionally be configured. If timeout is not specified, default is 30 seconds. If zero, no timeout.
    /// </summary>
    public static ResiliencePipelineBuilder GetResiliencePipelineBuilder(Action<RetryStrategyOptions>? configureRetry = null, TimeSpan? timeout = null)
    {
        var timeoutValue = timeout ?? TimeSpan.FromSeconds(30);
        var builder = new ResiliencePipelineBuilder()
            .AddRetry(HttpRetryOptions.ConfigureStrategyOptions(configureRetry));
        
        if (timeoutValue != TimeSpan.Zero)
        {
            builder.AddTimeout(timeoutValue);
        }

        return builder;
    }
}