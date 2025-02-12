using Microsoft.AspNetCore.Http;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit;

internal sealed class RouteOnlyEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test1");
		config.MapRoute("/test2");
	}
}

internal sealed class RouteWithMethodsEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapRoute("/test1", HttpMethod.Post, HttpMethod.Patch);
		config.MapRoute("/test2", HttpMethod.Post, HttpMethod.Patch);
	}
}

internal sealed class GlobalFallbackEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapFallback();
	}
}

internal sealed class PatternFallbackEndpoint : TestEndpointBase
{
	public override void Configure(IEndpointConfig config)
	{
		config.MapFallback("/test1");
		config.MapFallback("/test2");
	}
}

internal abstract class TestEndpointBase : ISimpleEndpoint<NoParameters>, ISimpleEndpoint<BaseParameters>
{
	public abstract void Configure(IEndpointConfig config);

	public Task<IResult> HandleRequestAsync(NoParameters _)
	{
		return Task.FromResult(Results.NoContent());
	}

	public Task<IResult> HandleRequestAsync(BaseParameters _)
	{
		return Task.FromResult(Results.NoContent());
	}
}
