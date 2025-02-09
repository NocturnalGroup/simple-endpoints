using Microsoft.AspNetCore.Http;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit;

public sealed class RouteOnlyEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test1");
		config.MapRoute("/test2");
	}
}

public sealed class RouteWithMethodsEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test1", HttpMethod.Post, HttpMethod.Patch);
		config.MapRoute("/test2", HttpMethod.Post, HttpMethod.Patch);
	}
}

public sealed class GlobalFallbackEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapFallback();
	}
}

public sealed class PatternFallbackEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapFallback("/test1");
		config.MapFallback("/test2");
	}
}

public abstract class TestEndpointBase : ISimpleEndpoint<NoParameters>
{
	public abstract void Configure(IEndpointConfig config);

	public Task<IResult> HandleRequestAsync(NoParameters parameters)
	{
		return Task.FromResult(Results.NoContent());
	}
}
