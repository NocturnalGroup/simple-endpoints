using Microsoft.AspNetCore.Mvc;
using NocturnalGroup.SimpleEndpoints;
using NocturnalGroup.SimpleEndpoints.Walkthrough;

// SimpleEndpoints doesn't require you to add anything to your DI container.
var builder = WebApplication.CreateBuilder(args);

// Mapping your endpoints is done just like Minimal APIs.
// You need to manually specify here which endpoints handle each route.
// This allows a clear view into what is registered and where.
//
// To achieve this, there are overrides for all the Map methods.
// If an override is missing, please let us know!
var app = builder.Build();
app.MapGet<GetUsersEndpoint>("/users"); // Base Parameters
app.MapGet<GetUserEndpoint, GetUserParameters>("/users/{UserId:int}"); // Custom Parameters
app.Run();

/// <summary>
/// Instead of a delegate, your endpoints are a struct/class.
///	We think this will add some clarity and structure to your projects.
///
/// Our goal is to keep things as minimal as possible.
/// We're a lightweight abstraction over Minimal APIs.
/// So bring your own favorite libraries for things like validation!
///
/// SimpleEndpoints takes advantage of the [AsParameters] attribute.
/// This is how request information is provided to the endpoint.
/// More about this later!
///
/// If you only need the HttpContext use the non-generic ISimpleEndpoint.
/// </summary>
public readonly struct GetUsersEndpoint : ISimpleEndpoint
{
	private readonly UserService _userService;

	/// <summary>
	/// Your endpoints are constructed through the DI container.
	/// So you can request anything that's in the container.
	/// </summary>
	public GetUsersEndpoint(UserService userService)
	{
		_userService = userService;
	}

	/// <summary>
	/// This is the method that is invoked when a request comes in.
	/// However, unlike Minimal APIs we enforce an IResult return type.
	/// This ensures it's clear in your code what you're responding.
	///
	/// ASP.NET Core comes with a ton of types that implement IResult.
	/// Theses can be found as static methods on the Microsoft.AspNetCore.Http.Results type.
	/// </summary>
	public Task<IResult> HandleRequestAsync(BaseParameters _)
	{
		var users = _userService.GetUsers();
		return Task.FromResult(Results.Ok(users));
	}
}

/// <summary>
/// As it was mentioned above, SimpleEndpoints uses the [AsParameters] attribute.
/// This allows you to obtain all the request information you need.
/// If you need more than the HttpContext (most do), you can provide your own type.
/// Below is an example of the attributes you can use.
/// </summary>
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

/// <summary>
/// We can then provide this parameters type as a generic argument.
/// Behind the scenes the Minimal API system will magically resolve everything.
///
/// It's also worth noting a new instance of your endpoint is created per request.
/// So don't store any long term state inside of it!
/// </summary>
public readonly struct GetUserEndpoint : ISimpleEndpoint<GetUserParameters>
{
	private readonly UserService _userService;

	/// <summary>
	/// Although it's possible to retrieve services through the parameters type.
	/// We recommend using the constructor for clarity.
	/// However, this is up to you and your team :)
	/// </summary>
	public GetUserEndpoint(UserService userService)
	{
		_userService = userService;
	}

	/// <summary>
	/// The request information we requested is given to us as an argument.
	/// That's pretty much it... It's pretty Simple!
	/// </summary>
	public Task<IResult> HandleRequestAsync(GetUserParameters parameters)
	{
		var user = _userService.GetUser(parameters.UserId);
		var result = user is null ? Results.NotFound() : Results.Ok(user);
		return Task.FromResult(result);
	}
}
