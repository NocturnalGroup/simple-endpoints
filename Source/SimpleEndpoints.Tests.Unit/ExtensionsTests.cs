using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Shouldly;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit;

public class ExtensionsTests
{
	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_RouteOnly()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<RouteOnlyEndpoint, NoParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response1 = await client.DeleteAsync("/test1", TestContext.Current.CancellationToken);
		var response2 = await client.DeleteAsync("/test2", TestContext.Current.CancellationToken);
		var response3 = await client.DeleteAsync("/test3", TestContext.Current.CancellationToken);

		// Assert
		response1.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response2.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response3.StatusCode.ShouldBe(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_RouteWithMethods()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<RouteWithMethodsEndpoint, NoParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response1 = await client.PostAsync("/test1", null, TestContext.Current.CancellationToken);
		var response2 = await client.PatchAsync("/test1", null, TestContext.Current.CancellationToken);
		var response3 = await client.DeleteAsync("/test1", TestContext.Current.CancellationToken);

		var response4 = await client.PostAsync("/test2", null, TestContext.Current.CancellationToken);
		var response5 = await client.PatchAsync("/test2", null, TestContext.Current.CancellationToken);
		var response6 = await client.DeleteAsync("/test2", TestContext.Current.CancellationToken);

		// Assert
		response1.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response2.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response3.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);

		response4.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response5.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response6.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_GlobalFallback()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<GlobalFallbackEndpoint, NoParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response1 = await client.GetAsync("/test1", TestContext.Current.CancellationToken);
		var response2 = await client.GetAsync("/test2", TestContext.Current.CancellationToken);

		// Assert
		response1.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response2.StatusCode.ShouldBe(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_PatternFallback()
	{
		// Arrange
		await using var server = new TestServer(c => c.MapEndpoint<PatternFallbackEndpoint, NoParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response1 = await client.GetAsync("/test1", TestContext.Current.CancellationToken);
		var response2 = await client.GetAsync("/test2", TestContext.Current.CancellationToken);
		var response3 = await client.GetAsync("/test3", TestContext.Current.CancellationToken);

		// Assert
		response1.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response2.StatusCode.ShouldBe(HttpStatusCode.NoContent);
		response3.StatusCode.ShouldBe(HttpStatusCode.NotFound);
	}
}
