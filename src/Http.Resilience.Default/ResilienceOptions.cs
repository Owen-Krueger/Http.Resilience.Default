using System.Net;

namespace Http.Resiliency;

public class ResilienceOptions
{
    public bool UseTimeout { get; set; } = true;
    
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    public TimeSpan Delay { get; set; } = TimeSpan.FromSeconds(2);

    public int MaxRetryAttempt { get; set; } = 2;

    public Func<HttpResponseMessage, bool> ResponseValidation { get; set; } = response => 
        IsValidStatusCode(response.StatusCode);

    private static bool IsValidStatusCode(HttpStatusCode statusCode)
        => (int)statusCode is >= 200 and <= 299 or >= 400 and <= 499;
}