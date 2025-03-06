using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NocturnalGroup.SimpleEndpoints.Parameters;
using NocturnalGroup.SimpleEndpoints.Tests.Unit.Utils;
using Shouldly;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit;

public class ParametersTests
{
	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithEmptyParameters()
	{
		// Arrange
		await using var server = new TestServer(x => x.MapEndpoint<EmptyParametersEndpoint, EmptyParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response = await client.DeleteAsync("/empty");

		// Assert
		response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithContextParameters()
	{
		// Arrange
		await using var server = new TestServer(x => x.MapEndpoint<ContextParametersEndpoint, ContextParameters>());
		var client = await server.StartServerAsync();

		// Act
		var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/context")
		{
			Headers = { { "Test", "123" } }
		});

		// Assert
		response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task MapEndpoint_Should_MapSimpleEndpoint_WithCustomParameters()
	{
		// Arrange
		await using var server = new TestServer(
			x => x.MapEndpoint<CustomParametersEndpoint, CustomParametersEndpoint.Parameters>()
		);
		var client = await server.StartServerAsync();

		// Act
		var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "/custom")
		{
			Headers = { { "Test", "123" } }
		});

		// Assert
		response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
	}
}

internal sealed class EmptyParametersEndpoint : ISimpleEndpoint<EmptyParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapRoute("/empty", HttpMethod.Delete);
	}

	public Task<IResult> HandleRequestAsync(EmptyParameters parameters, CancellationToken _)
	{
		return Task.FromResult(Results.NoContent());
	}
}

internal sealed class ContextParametersEndpoint : ISimpleEndpoint<ContextParameters>
{
	public void Configure(IEndpointConfig config)
	{
		config.MapRoute("/context", HttpMethod.Delete);
	}

	public Task<IResult> HandleRequestAsync(ContextParameters parameters, CancellationToken _)
	{
		var response = parameters.Request.Headers["Test"] == "123" ? Results.NoContent() : Results.BadRequest();
		return Task.FromResult(response);
	}
}

internal sealed class CustomParametersEndpoint : ISimpleEndpoint<CustomParametersEndpoint.Parameters>
{
	internal sealed class Parameters
	{
		[FromHeader]
		public required string Test { get; init; }
	}

	public void Configure(IEndpointConfig config)
	{
		config.MapRoute("/custom", HttpMethod.Delete);
	}

	public Task<IResult> HandleRequestAsync(Parameters parameters, CancellationToken _)
	{
		var response = parameters.Test == "123" ? Results.NoContent() : Results.BadRequest();
		return Task.FromResult(response);
	}
}
