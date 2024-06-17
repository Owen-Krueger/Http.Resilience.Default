using Http.Resilience.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Http.Resiliency.Tests;

public class HttpClientBuilderExtensionTests
{
    [Test]
    public void HttpClientBuilder_NoConfiguration_DefaultHandlerAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpClient(ClientKey)
            .AddHttpResilienceHandler();
        var serviceProvider = services.BuildServiceProvider();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory!.CreateClient(ClientKey);
        
        Assert.That(client, Is.Not.Null);
    }
    
    [Test]
    public void HttpClientBuilder_RetryConfigured_HandlerAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpClient(ClientKey)
            .AddHttpResilienceHandler(x => x.MaxRetryAttempts = 3);
        var serviceProvider = services.BuildServiceProvider();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory!.CreateClient(ClientKey);
        
        Assert.That(client, Is.Not.Null);
    }
    
    [Test]
    public void HttpClientBuilder_TimeoutProvided_HandlerAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpClient(ClientKey)
            .AddHttpResilienceHandler(TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory!.CreateClient(ClientKey);
        
        Assert.That(client, Is.Not.Null);
    }
    
    [Test]
    public void HttpClientBuilder_RetryConfiguredAndTimeoutProvided_HandlerAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpClient(ClientKey)
            .AddHttpResilienceHandler(x => x.MaxRetryAttempts = 3, TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory!.CreateClient(ClientKey);
        
        Assert.That(client, Is.Not.Null);
    }
    
    [Test]
    public void HttpClientBuilder_AllConfigurationProvided_HandlerAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpClient(ClientKey)
            .AddHttpResilienceHandler("pipeline_key", x => x.MaxRetryAttempts = 3, TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var clientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var client = clientFactory!.CreateClient(ClientKey);
        
        Assert.That(client, Is.Not.Null);
    }

    private const string ClientKey = "client_key";
}