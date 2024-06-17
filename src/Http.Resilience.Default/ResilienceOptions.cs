using System.Net;
using Polly;

namespace Http.Resilience.Default;

/// <summary>
/// Options when setting up resilience for HTTP requests.
/// </summary>
public class ResilienceOptions
{
    /// <summary>
    /// Whether to use timeout for the HTTP request.
    /// </summary>
    public bool UseTimeout { get; set; } = true;
    
    /// <summary>
    /// Timeout duration for the HTTP request.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Delay duration between retries.
    /// </summary>
    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Number of retry attempts.
    /// </summary>
    public int MaxRetryAttempt { get; set; } = 2;
    
    /// <summary>
    /// Type of backoff strategy.
    /// </summary>
    public DelayBackoffType BackoffType { get; set; } = DelayBackoffType.Exponential;

    /// <summary>
    /// Whether to use jitter for the backoff strategy.
    /// </summary>
    public bool UseJitter { get; set; } = true;

    /// <summary>
    /// Validation logic for what responses don't need to be retried.
    /// </summary>
    public Func<HttpResponseMessage, bool> ResponseValidation { get; set; } = response => 
        IsValidStatusCode(response.StatusCode);

    /// <summary>
    /// Checks if status code is 2xx or 4xx.
    /// </summary>
    private static bool IsValidStatusCode(HttpStatusCode statusCode)
        => (int)statusCode is >= 200 and <= 299 or >= 400 and <= 499;
}