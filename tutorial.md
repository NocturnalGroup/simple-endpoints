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

The easiest way to create an endpoint is to create a class/struct and implement the `ISimpleEndpoint` interface.
A new instance of your endpoint type will be created for each request through the `IServiceProvider`.
This means you can inject anything you need through the constructor.

The `HandleRequestAsync` method gets invoked when a request comes in.
It has a single parameter, which provides access to the `HttpContext`.
All you need to do, is return an `IResult` which represents the outcome of the request.
An easy way to do this is to use the methods on the `Microsoft.AspNetCore.Http.Results` type.

```csharp
public readonly struct GetUsersEndpoint : ISimpleEndpoint
{
  private readonly UserService _userService;

  public GetUsersEndpoint(UserService userService)
  {
    _userService = userService;
  }

  public Task<IResult> HandleRequestAsync(BaseParameters _)
  {
    var users = _userService.GetUsers();
    return Task.FromResult(Results.Ok(users));
  }
}
```

However, most endpoints will require access to request level information.
This can be achieved by providing a custom parameters type.
Under the hood, this type is registered with the `[AsParameters]` attribute.
This means you can easily get access to any request level information.

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

To use a custom parameters type, you need to implement the `ISimpleEndpoint<TParameters>` interface.
You'll then have your custom parameters in the `HandleRequestAsync` method.

_While it's possible to use the `[FromServices]` attribute, we suggest using the constructor.
There's no technical difference, it just keeps things consistent throughout your codebase._

```csharp
public readonly struct GetUserEndpoint : ISimpleEndpoint<GetUserParameters>
{
  private readonly UserService _userService;

  public GetUserEndpoint(UserService userService)
  {
    _userService = userService;
  }

  public Task<IResult> HandleRequestAsync(GetUserParameters parameters)
  {
    var user = _userService.GetUser(parameters.UserId);
    var result = user is null ? Results.NotFound() : Results.Ok(user);
    return Task.FromResult(result);
  }
}
```

## Endpoint Mapping

Mapping your endpoints is done just like Minimal APIs.
You need to manually specify which endpoint handle each route.
This allows a clear view into what is registered and where.
To achieve this, there are overrides for all the Map methods.
If an override is missing, please let us know!

```csharp
var app = builder.Build();
app.MapGet<GetUsersEndpoint>("/users"); // Base Parameters
app.MapGet<GetUserEndpoint, GetUserParameters>("/users/{UserId:int}"); // Custom Parameters
app.Run();
```
