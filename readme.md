<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleEndpoints</h1>
    </summary>
  </ul>
</div>

### Adding structure to Minimal APIs

About · [Getting Started](tutorial.md) · [License](license.txt) · [Contributing](contributing.md)

---

SimpleEndpoints is a lightweight abstraction on top of Minimal APIs.
Allowing you to create endpoints with ease.

## Installing

You can install SimpleEndpoints with [NuGet](https://www.nuget.org/packages/NocturnalGroup.SimpleEndpoints):

```shell
dotnet add package NocturnalGroup.SimpleEndpoints
```

## Quickstart

For a detailed walkthrough of SimpleEndpoints, check out our [tutorial](tutorial.md).

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapEndpoint<GreetEndpoint, GreetParameters>();
app.Run();

public readonly struct GreetParameters
{
  [FromQuery("name")]
  public string Name { get; init; }
}

public sealed class GreetEndpoint : ISimpleEndpoint<GreetParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/", HttpMethod.Get);
  }

  public Task<IResult> HandleRequestAsync(GreetParameters parameters)
  {
    return Task.FromResult(Results.Ok($"Hello, {parameters.Name}!"));
  }
}
```

## Versioning

We use [Semantic Versioning](https://semver.org/) to clearly communicate changes:

- Major version changes indicate breaking updates
- Minor version changes add features in a backward-compatible way
- Patch version changes include backward-compatible bug fixes
