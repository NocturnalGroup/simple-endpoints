using System.Net;
using Microsoft.AspNetCore.Http;
using NocturnalGroup.SimpleEndpoints.Parameters;
using NocturnalGroup.SimpleEndpoints.Tests.Unit.Utils;
using Shouldly;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit;

public class RoutingTests
{
	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithRoute()
	{
		// Arrange
		await using var server = new TestServer(x => x.MapEndpoint<RouteEndpoint, EmptyParameters>());
		var client = await server.StartServerAsync();

		// Act
		var route1Get = await client.GetAsync("/test-1");
		var route1Delete = await client.DeleteAsync("/test-1");

		var route2Get = await client.GetAsync("/test2");
		var route2Delete = await client.DeleteAsync("/test-2");

		// Assert
		route1Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route1Delete.StatusCode.ShouldBe(HttpStatusCode.NoContent);

		route2Get.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		route2Delete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithMethodRoute()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<MethodRouteEndpoint, EmptyParameters>());
		var client = await server.StartServerAsync();

		// Act
		var route1Get = await client.GetAsync("/test-1");
		var route1Post = await client.PostAsync("/test-1", new StringContent(""));
		var route1Delete = await client.DeleteAsync("/test-1");

		var route2Get = await client.GetAsync("/test-2");
		var route2Post = await client.PostAsync("/test-2", new StringContent(""));
		var route2Delete = await client.DeleteAsync("/test-2");

		var route3Get = await client.GetAsync("/test-3");
		var route3Post = await client.PostAsync("/test-3", new StringContent(""));
		var route3Delete = await client.DeleteAsync("/test-3");

		// Assert
		route1Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route1Post.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
		route1Delete.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);

		route2Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route2Post.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route2Delete.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);

		route3Get.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		route3Post.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		route3Delete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithGlobalFallback()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<GlobalFallbackEndpoint, EmptyParameters>());
		var client = await server.StartServerAsync();

		// Act
		var route1Get = await client.GetAsync("/test-1");
		var route1Delete = await client.DeleteAsync("/test-1");

		var route2Get = await client.GetAsync("/test-2");
		var route2Delete = await client.DeleteAsync("/test-2");

		// Assert
		route1Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route1Delete.StatusCode.ShouldBe(HttpStatusCode.NoContent);

		route2Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route2Delete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_RouteFallback()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<RouteFallbackEndpoint, EmptyParameters>());
		var client = await server.StartServerAsync();

		// Act
		var route1Get = await client.GetAsync("/test-1");
		var route1Delete = await client.DeleteAsync("/test-1");

		var route2Get = await client.GetAsync("/test-2");
		var route2Delete = await client.DeleteAsync("/test-2");

		// Assert
		route1Get.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		route1Delete.StatusCode.ShouldBe(HttpStatusCode.NoContent);

		route2Get.StatusCode.ShouldBe(HttpStatusCode.NotFound);
		route2Delete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
	}
}

internal sealed class RouteEndpoint : ISimpleEndpoint<EmptyParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test-1");
	}

	public Task<IResult> HandleRequestAsync(EmptyParameters _, CancellationToken __)
	{
		return Task.FromResult(Results.NoContent());
	}
}

internal sealed class MethodRouteEndpoint : ISimpleEndpoint<EmptyParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test-1", HttpMethod.Get);
		config.MapRoute("/test-2", HttpMethod.Get, HttpMethod.Post);
	}

	public Task<IResult> HandleRequestAsync(EmptyParameters _, CancellationToken __)
	{
		return Task.FromResult(Results.NoContent());
	}
}

internal sealed class GlobalFallbackEndpoint : ISimpleEndpoint<EmptyParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapFallback();
	}

	public Task<IResult> HandleRequestAsync(EmptyParameters _, CancellationToken __)
	{
		return Task.FromResult(Results.NoContent());
	}
}

internal sealed class RouteFallbackEndpoint : ISimpleEndpoint<EmptyParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapFallback("/test-1");
	}

	public Task<IResult> HandleRequestAsync(EmptyParameters _, CancellationToken __)
	{
		return Task.FromResult(Results.NoContent());
	}
}
