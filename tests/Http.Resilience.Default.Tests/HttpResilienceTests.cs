using System.Net;
using Http.Resilience.Default;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using Polly;

namespace Http.Resiliency.Tests;

public class HttpResilienceTests
{
    [TestCase(HttpStatusCode.OK)]
    [TestCase(HttpStatusCode.NoContent)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.NotFound)]
    public async Task ResiliencePipeline_ValidStatuses_NoRetry(HttpStatusCode statusCode)
    {
        var mock = new AutoMocker();
        var handler = mock.GetMock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(statusCode));
        var client = new HttpClient(handler.Object) { BaseAddress = new Uri("http://localhost") };
        var pipeline = HttpResilience.GetResiliencePipelineBuilder().Build();
        var response = await pipeline.ExecuteAsync(async ct => await client.GetAsync(string.Empty, ct));

        Assert.That(IsValidStatusCode(response.StatusCode), Is.True);
        handler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task ResiliencePipeline_InvalidStatuses_Retry()
    {
        var mock = new AutoMocker();
        var handler = mock.GetMock<HttpMessageHandler>();
        handler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));
        var client = new HttpClient(handler.Object) { BaseAddress = new Uri("http://localhost") };
        var pipeline = HttpResilience.GetResiliencePipelineBuilder(TimeSpan.FromSeconds(20)).Build();
        var response = await pipeline.ExecuteAsync(async ct => await client.GetAsync(string.Empty, ct));

        Assert.That(IsValidStatusCode(response.StatusCode), Is.True);
        handler.Protected().Verify("SendAsync", Times.Exactly(3), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }
    
    [Test]
    public async Task ResiliencePipeline_CustomProperties_ExpectedBehavior()
    {
        var mock = new AutoMocker();
        var handler = mock.GetMock<HttpMessageHandler>();
        handler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var client = new HttpClient(handler.Object) { BaseAddress = new Uri("http://localhost") };
        var pipeline = HttpResilience.GetResiliencePipelineBuilder(x =>
        {
            x.Delay = TimeSpan.Zero;
            x.UseJitter = false;
            x.MaxRetryAttempts = 4;
            x.BackoffType = DelayBackoffType.Linear;
        }, TimeSpan.FromSeconds(60)).Build();
        var response = await pipeline.ExecuteAsync(async ct => await client.GetAsync(string.Empty, ct));

        Assert.That(IsValidStatusCode(response.StatusCode), Is.False);
        handler.Protected().Verify("SendAsync", Times.Exactly(5), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }
    
    private static bool IsValidStatusCode(HttpStatusCode statusCode)
        => (int)statusCode is >= 200 and <= 299 or >= 400 and <= 499;
}