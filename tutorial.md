<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleEndpoints</h1>
    </summary>
  </ul>
</div>

### Adding structure to Minimal APIs

[About](readme.md) · Getting Started · [License](license.txt) · [Contributing](contributing.md)

---

## Installing

Please see the installation instructions [here](readme.md#Installing).

## Philosophy

The idea of SimpleEndpoints is to move your request delegates to a dedicated type.
Our goal is to keep things as minimal as possible.
So bring your own favorite libraries for things like validation!

## Endpoint Creation

To create an endpoint, you need to create a type that implements the `ISimpleEndpoint` interface.
The `ISimpleEndpoint` interface takes one generic parameter.
You can use either `NoParameters`, `BaseParameters`, or your own parameters type (more on that later).

When your application starts up, an instance of your endpoint will be created.
This instance of your endpoint is used to call the `Configure` method.
This method defines how your endpoint should be mapped in the router.

When a request comes in that matches your configuration, an instance of your endpoint will be created.
This instance of your endpoint is used to call the `HandleRequestAsync` method.
All you need to do, is return an `IResult` which represents the outcome of the request.
An easy way to do this is to use the methods on the `Microsoft.AspNetCore.Http.Results` type.

```csharp
public sealed class HelloWorldEndpoint : ISimpleEndpoint<NoParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/", HttpMethod.Get);
  }

  public Task<IResult> HandleRequestAsync(NoParameters _)
  {
    return Task.FromResult(Results.Ok("Hello World!"));
  }
}
```

### Dependency Injection

All instances of your endpoint will be created through the WebApplication's `IServiceProvider`.
So you can inject anything you need into your endpoint.

```csharp
public readonly struct GetUsersEndpoint : ISimpleEndpoint<BaseParameters>
{
  private readonly UserService _userService;

  public GetUsersEndpoint(UserService userService)
  {
    _userService = userService;
  }

  // ...
}
```

### Custom Parameters

Most endpoints will require access to request level information.
This can be achieved by creating a custom parameters type.
Under the hood, this type is registered with the `[AsParameters]` attribute.
Allowing you to get easy access to any request level information.

```csharp
public struct GetUserParameters
{
  // Route Parameter
  [FromRoute]
  public int UserId { get; init; }

  // Query String
  // [FromQuery("search")]
  // public string SearchQuery { get; init; }

  // Header Data
  // [FromHeader(Name = "X-CUSTOM-HEADER")]
  // public string CustomHeader { get; init; }

  // Body Data
  // [FromBody]
  // public User Body { get; init; }

  // Form Data
  // [FromForm]
  // public User Form { get; init; }
}
```

Using a custom parameters type works just like `NoParameters` and `BaseParameters`.
Place your custom type as the generic parameter and you're good to go.
You'll then have your parameters in the `HandleRequestAsync` method.

```csharp
public readonly struct GetUserEndpoint : ISimpleEndpoint<GetUserParameters>
{
  // ...

  public Task<IResult> HandleRequestAsync(GetUserParameters parameters)
  {
    var user = _userService.GetUser(parameters.UserId);
    var result = user is null ? Results.NotFound() : Results.Ok(user);
    return Task.FromResult(result);
  }
}
```

_While it's possible to use the `[FromServices]` attribute, we suggest using the constructor.
There's no technical difference, it just keeps things consistent throughout your codebase._

### Endpoint Customizing

Most routes require additional configuration like rate limiting or authorization.
This can be done through the `Customize(Action)` method.

```csharp
public sealed class HelloWorldEndpoint : ISimpleEndpoint<NoParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/", HttpMethod.Get);
    config.Customize(c =>
    {
      c.AllowAnonymous();
      c.RequireRateLimiting("example");
    });
  }

  public Task<IResult> HandleRequestAsync(NoParameters _)
  {
    return Task.FromResult(Results.Ok("Hello World!"));
  }
}
```


### Route Only Endpoints

For endpoints that need to handle all methods, you can use the `MapRoute(string)` overload.
Your endpoint will then handle any request for the specified pattern regardless of HTTP method.

```csharp
public sealed class RouteOnlyEndpoint : ISimpleEndpoint<NoParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/");
  }

  // ...
}
```

### Fallback Endpoints

To create a fallback endpoint, the `MapFallback()` and `MapFallback(string)` methods are provided.
Internally we call these "Global Fallback" and "Pattern Fallback" respectively.

- A global fallback matches requests for non-file-names with the lowest possible priority.
- A pattern fallback matches for the specified pattern with the lowest possible priority.

```csharp
public sealed class HelloWorldEndpoint : ISimpleEndpoint<NoParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapFallback(); // "Global Fallback"
    config.MapFallback("/fallback"); // "Pattern Fallback"
  }

  // ...
}
```

## Endpoint Mapping

To map your endpoints, use the `MapEndpoint` extension on your `WebApplication`.
This will then automatically wire everything up for you.

```csharp
var app = builder.Build();
app.MapEndpoint<GetUserEndpoint, GetUserParameters>();
app.Run();
```
