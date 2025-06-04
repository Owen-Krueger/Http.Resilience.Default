** Archived in favor of Microsoft's resillience packages: https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience **



# Http Resilience Default NuGET Package

[![.NET](https://github.com/Owen-Krueger/Http.Resilience.Default/actions/workflows/dotnet.yaml/badge.svg)](https://github.com/Owen-Krueger/Http.Resilience.Default/actions/workflows/dotnet.yaml)

## Resilience Pipelines

This package provides a resilience pipeline for HttpClient, with sensible defaults. The pipeline created can be used as is or configured further.

By default, pipelines are created with the following policies:
- HTTP requests will be retried if the following exceptions are thrown:
  - TaskCanceledException
  - TimeoutRejectedException
- HTTP requests will be retried if the response status code is any that don't fall within the 200-299 or 400-499 ranges.
- Requests will be timed out after 30 seconds.
- Requests will be retried twice (three total times).
- Requests will be delayed by two seconds between retries.
- Retries will be exponentially backed off.
- Retries will utilize jitters, which will slightly randomize the delay between retries.

To set up a resilience pipeline, use the following code:

``` C#
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder();
var pipeline = pipelineBuilder.Build();
```

Retry configuration or timeouts can be configured:
``` C#
// Default options.
var pipeLineBuilder = HttpResilience.GetResiliencePipelineBuilder();

// Pipeline key specified
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder("my_resilience_pipeline");

// Retry configured.
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
});

// Timeout configured.
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder(TimeSpan.FromSeconds(10));

// Retry and timeout configured.
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));

// Key, retry, and timeout configured.
var pipelineBuilder = HttpResilience.GetResiliencePipelineBuilder("my_resilience_pipeline", x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));
```

When using these methods, you're expected to manage the pipeline and HttpClients yourself. This package also provides extensions for `IServiceCollection` and `IHttpClientBuilder` to make it easier to set up and use resilience pipelines.

Unless specified, the pipeline will use the key `default_resilience_pipeline`.

## IServiceCollection Extensions

This package provides a `AddHttpResiliencePipeline` extension method for `IServiceCollection` that will add a resilience pipeline to the service collection. This sets up the pipeline similar to the example above, but allows you to dependency inject the pipeline into other services.

This method can be used as follows:

``` C#
// Default options.
services.AddHttpResiliencePipeline();

// Pipeline key specified
services.AddHttpResiliencePipeline("my_resilience_pipeline");

// Retry configured.
services.AddHttpResiliencePipeline(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
});

// Timeout configured.
services.AddHttpResiliencePipeline(TimeSpan.FromSeconds(10));

// Retry and timeout configured.
services.AddHttpResiliencePipeline(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));

// Key, retry, and timeout configured.
services.AddHttpResiliencePipeline("my_resilience_pipeline", x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));
```

Unless specified, the pipeline will use the key `default_resilience_pipeline`. This key can be used to retrieve the pipeline from the service collection.

Consuming the pipeline in a service can be done as follows:

``` C#
ResiliencePipeline pipelineProvider = serviceProvider.GetRequiredService<ResiliencePipeline<string>>();
var pipeline = provider.GetPipeline(HttpResilienceConstants.DefaultPipelineKey);
```

## IHttpClientBuilder Extensions

This package provides a `AddHttpResilienceHandler` extension method for `IHttpClientBuilder` that will add a resilience pipeline to the service collection. This sets up the pipeline similar to the example above, but allows you to dependency inject the pipeline into other services.

This method can be used as follows:

``` C#
const string clientName = "my_client";

// Default options.
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler();

// Pipeline key specified
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler("my_resilience_pipeline");

// Retry configured.
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
});

// Timeout configured.
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler(TimeSpan.FromSeconds(10));

// Retry and timeout configured.
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler(x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));

// Key, retry, and timeout configured.
services.AddHttpClient(ClientKey)
  .AddHttpResilienceHandler("my_resilience_pipeline", x =>
{
    x.Delay = TimeSpan.FromSeconds(5);
    x.BackoffType = BackoffType.Linear;
}, TimeSpan.FromSeconds(10));
```

Unless specified, the pipeline will use the key `default_resilience_pipeline`. This key is used internally within the `HttpClient`.
