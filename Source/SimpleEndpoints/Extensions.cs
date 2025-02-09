using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace NocturnalGroup.SimpleEndpoints;

/// <summary>
/// Extensions to register an <see cref="ISimpleEndpoint{TParameters}"/> into a <see cref="IEndpointRouteBuilder"/>.
/// </summary>
public static class SimpleEndpointT1Extensions
{
	/// <summary>
	/// Maps an <see cref="ISimpleEndpoint{TParameters}"/> into a <see cref="IEndpointRouteBuilder"/>.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the endpoint to.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	public static void MapEndpoint<TEndpoint, TParameters>(this IEndpointRouteBuilder builder)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		// Generate a DI scope to enable scoped service resolving.
		var scope = builder.ServiceProvider.CreateScope();

		// Generate the endpoint configuration.
		var endpointConfig = new EndpointConfig();
		var endpointInstance = ActivatorUtilities.CreateInstance<TEndpoint>(scope.ServiceProvider);
		endpointInstance.Configure(endpointConfig);

		// Generate the handler delegate.
		var endpointDelegate = async (IServiceProvider services, [AsParameters] TParameters request) =>
		{
			var endpoint = ActivatorUtilities.CreateInstance<TEndpoint>(services);
			return await endpoint.HandleRequestAsync(request).ConfigureAwait(false);
		};

		// Routes Only.
		foreach (var pattern in endpointConfig.RoutesOnly)
		{
			endpointConfig.CustomizeDelegate(builder.Map(pattern, endpointDelegate));
		}

		// Routes with Methods.
		foreach (var (pattern, methods) in endpointConfig.RoutesWithMethods)
		{
			var stringMethods = methods.Select(x => x.Method);
			endpointConfig.CustomizeDelegate(builder.MapMethods(pattern, stringMethods, endpointDelegate));
		}

		// Global Fallback.
		if (endpointConfig.IsGlobalFallback)
		{
			endpointConfig.CustomizeDelegate(builder.MapFallback(endpointDelegate));
		}

		// Pattern Fallbacks.
		foreach (var pattern in endpointConfig.PatternFallbacks)
		{
			endpointConfig.CustomizeDelegate(builder.MapFallback(pattern, endpointDelegate));
		}
	}
}
