<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="left" style="list-style: none;">
    <summary>
      <h1>SimpleEndpoints</h1>
    </summary>
  </ul>
</div>

### Adding structure to Minimal APIs

[About](readme.md) · Docs · [License](license.txt) · [Contributing](contributing.md)

---

## Installing

Please see the installation instructions [here](readme.md#Installing).

## Endpoint Creation

A "simple" endpoint is a type that that implements the `ISimpleEndpoint<TParameters>` interface.
This interface takes in a parameters type and has two methods: `Configure` and `HandleRequestAsync`.

Parameters are the way your endpoint accesses request level information.
Out of the box, we provide a few parameter types, or you can even make you own.
We'll talk about this in more detail later!

The `Configure` method defines what routes your endpoint handles.
Your endpoint can handle as many routes as you'd like, it's completely up to you.

The `HandleRequestAsync` method is called when a matching request comes in.
All you need to do, is return an `IResult` which represents the outcome of the request.
An easy way to do this is to use the methods on the `Microsoft.AspNetCore.Http.Results` type.

Alongside your parameters, you're also given the `CanellationToken` for the request.
It's a good idea to pass this down to any operations that accept it.
It's pointless to do some work if the client has canceled the request!

Registering endpoints is done through the `MapEndpoint<TEndpoint, TParameters>` extension.
The extension will handle registering the endpoint into the request pipeline for you.

```csharp
var app = WebApplication.CreateBuilder(args).Build();
app.MapEndpoint<HelloWorldEndpoint, EmptyParameters>();
app.Run();

public sealed class HelloWorldEndpoint : ISimpleEndpoint<EmptyParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/", HttpMethod.Get);
  }

  public Task<IResult> HandleRequestAsync(EmptyParameters _, CancellationToken __)
  {
    return Task.FromResult(Results.Ok("Hello World!"));
  }
}
```

### Dependency Injection

For each request, a new instance of your endpoint is created.
The endpoint is created through the WebApplication's `IServiceProvider`.
So you can inject anything you need into your endpoint.

```csharp
public sealed class GetUsersEndpoint : ISimpleEndpoint<EmptyParameters>
{
  private readonly UserService _userService;

  public GetUsersEndpoint(UserService userService)
  {
    _userService = userService;
  }

  // ... Configure & HandleRequestAsync ...
}
```

### Parameters

Out of the box we provide the following parameter types:

- `EmptyParameters` is an empty struct, it provides no request information.
- `ContextParameters` provides access to the `HttpContext` for the request.

However, most endpoints need a custom parameter type.
A parameter type is just a simple C# `class` or `struct`.
Instances of the parameter type is done through Minimal APIs using the `[AsParameters]` attribute.
Allowing you to get easy access to any request level information.

```csharp
public sealed class GetUserParameters
{
  // Route Parameter
  [FromRoute]
  public int UserId { get; init; }

  // Query String
  // [FromQuery(Name = "search")]
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

public sealed class GetUserEndpoint : ISimpleEndpoint<GetUserParameters>
{
  // ... Ctor ...

  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/users/{UserId:int}", HttpMethod.Get);
  }

  public async Task<IResult> HandleRequestAsync(GetUserParameters parameters, CancellationToken _)
  {
    var user = await _userService.GetUserAsync(parameters.UserId);
    return user is null ? Results.NotFound() : Results.Ok(user);
  }
}
```

### Endpoint Customizing

Most routes require additional configuration like rate limiting or authorization.
This can be done through the `Customize` method.

```csharp
public sealed class HelloWorldEndpoint : ISimpleEndpoint<EmptyParameters>
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

  // ... HandleRequestAsync ...
}
```

### Route Only Endpoints

For endpoints that need to handle all methods, you can use the `MapRoute(string)` method.
Your endpoint will then handle any request for the specified pattern regardless of HTTP method.

```csharp
public sealed class RouteOnlyEndpoint : ISimpleEndpoint<EmptyParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapRoute("/");
  }

  // ... HandleRequestAsync ...
}
```

### Fallback Endpoints

To create a fallback endpoint, the `MapFallback()` and `MapFallback(string)` methods are provided.
Internally we call these "Global Fallback" and "Pattern Fallback" respectively.

- A global fallback matches requests for non-file-names with the lowest possible priority.
- A pattern fallback matches for the specified pattern with the lowest possible priority.

```csharp
public sealed class HelloWorldEndpoint : ISimpleEndpoint<EmptyParameters>
{
  public void Configure(IEndpointConfig config)
  {
    config.MapFallback(); // "Global Fallback"
    config.MapFallback("/fallback"); // "Pattern Fallback"
  }

  // ... HandleRequestAsync ...
}
```
