using Microsoft.AspNetCore.Mvc;
using NocturnalGroup.SimpleEndpoints;
using NocturnalGroup.SimpleEndpoints.Tests.Debug;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<UserService>();

var app = builder.Build();
app.MapGet<GetUsersEndpoint>("/users");
app.MapGet<GetUserEndpoint, GetUserParameters>("/users/{UserId:int}");
app.Run();

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

public struct GetUserParameters
{
	[FromRoute]
	public int UserId { get; init; }
}

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
