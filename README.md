<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleEndpoints</h1>
    </summary>
  </ul>
</div>

### Adding structure to Minimal APIs

---

SimpleEndpoints is a lightweight abstraction on top of Minimal APIs.
Allowing you to create endpoints with ease.

## Installing

You can install the package from [NuGet](https://www.nuget.org/packages/NocturnalGroup.SimpleEndpoints):

```shell
dotnet add package NocturnalGroup.SimpleEndpoints
```

## Usage

_A complete, but short, walkthrough of SimpleEndpoints can be found [here](Samples/SimpleEndpoints.Walkthrough/Program.cs)._

### Quickstart

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet<GreetEndpoint, GreetParameters>("/");
app.Run();

public readonly struct GreetParameters
{
  [FromQuery("name")]
  public string Name { get; init; }
}

public readonly struct GreetEndpoint : ISimpleEndpoint<GreetParameters>
{
  public GreetEndpoint()
  {
  }

  public Task<IResult> HandleRequestAsync(GreetParameters parameters)
  {
    return Task.FromResult(Results.Ok($"Hello, {parameters.Name}!"));
  }
}
```

## Versioning

SimpleEndpoints follows the [SemVer](https://semver.org/) versioning scheme.
