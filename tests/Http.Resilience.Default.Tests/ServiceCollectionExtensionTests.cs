using Http.Resilience.Default;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;

namespace Http.Resiliency.Tests;

public class ServiceCollectionExtensionTests
{
    [Test]
    public void ServiceCollection_NoConfiguration_DefaultPipelineAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpResiliencePipeline();
        var serviceProvider = services.BuildServiceProvider();
        var pipelineProvider = serviceProvider.GetService<ResiliencePipelineProvider<string>>();
        
        Assert.That(pipelineProvider, Is.Not.Null);
        Assert.DoesNotThrow(() => pipelineProvider!.GetPipeline(HttpResilienceConstants.DefaultPipelineKey));
    }
    
    [Test]
    public void ServiceCollection_RetryConfigured_PipelineAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpResiliencePipeline(x => x.MaxRetryAttempts = 3);
        var serviceProvider = services.BuildServiceProvider();
        var pipelineProvider = serviceProvider.GetService<ResiliencePipelineProvider<string>>();
        
        Assert.That(pipelineProvider, Is.Not.Null);
        Assert.DoesNotThrow(() => pipelineProvider!.GetPipeline(HttpResilienceConstants.DefaultPipelineKey));
    }
    
    [Test]
    public void ServiceCollection_TimeoutProvided_PipelineAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpResiliencePipeline(TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var pipelineProvider = serviceProvider.GetService<ResiliencePipelineProvider<string>>();
        
        Assert.That(pipelineProvider, Is.Not.Null);
        Assert.DoesNotThrow(() => pipelineProvider!.GetPipeline(HttpResilienceConstants.DefaultPipelineKey));
    }
    
    [Test]
    public void ServiceCollection_RetryConfiguredAndTimeoutProvided_PipelineAdded()
    {
        var services = new ServiceCollection();
        services.AddHttpResiliencePipeline(x => x.MaxRetryAttempts = 3, TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var pipelineProvider = serviceProvider.GetService<ResiliencePipelineProvider<string>>();
        
        Assert.That(pipelineProvider, Is.Not.Null);
        Assert.DoesNotThrow(() => pipelineProvider!.GetPipeline(HttpResilienceConstants.DefaultPipelineKey));
    }
    
    [Test]
    public void ServiceCollection_AllConfigurationProvided_PipelineAdded()
    {
        const string pipelineKey = "pipeline_key";
        var services = new ServiceCollection();
        services.AddHttpResiliencePipeline(pipelineKey, x => x.MaxRetryAttempts = 3, TimeSpan.FromSeconds(60));
        var serviceProvider = services.BuildServiceProvider();
        var pipelineProvider = serviceProvider.GetService<ResiliencePipelineProvider<string>>();
        
        Assert.That(pipelineProvider, Is.Not.Null);
        Assert.DoesNotThrow(() => pipelineProvider!.GetPipeline(pipelineKey));
    }
}