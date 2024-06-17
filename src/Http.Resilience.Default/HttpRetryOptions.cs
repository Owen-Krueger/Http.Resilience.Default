using System.Net;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace Http.Resilience.Default;

/// <summary>
/// Options for configuring the retry strategy.
/// </summary>
public static class HttpRetryOptions
{
    /// <summary>
    /// Configure the retry strategy options.
    /// </summary>
    public static RetryStrategyOptions ConfigureStrategyOptions(Action<RetryStrategyOptions>? configureOptions)
    {
        var options = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder()
                .Handle<InvalidOperationException>()
                .Handle<TaskCanceledException>()
                .Handle<UriFormatException>()
                .Handle<TimeoutRejectedException>()
                .Handle<HttpRequestException>()
                .HandleResult(response =>
                    {
                        if (response is not HttpResponseMessage result)
                        {
                            return false;
                        }

                        return !IsValidStatusCode(result.StatusCode);
                    }
                ),
            Delay = TimeSpan.FromSeconds(2),
            MaxRetryAttempts = 2,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
        };
        configureOptions?.Invoke(options);
        return options;
    }
    
    /// <summary>
    /// Check if the status code is 2xx or 4xx.
    /// </summary>
    private static bool IsValidStatusCode(HttpStatusCode statusCode)
        => (int)statusCode is >= 200 and <= 299 or >= 400 and <= 499;
}