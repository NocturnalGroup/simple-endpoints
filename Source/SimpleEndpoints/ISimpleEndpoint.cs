using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace NocturnalGroup.SimpleEndpoints;

/// <summary>
/// An endpoint that handles a HTTP request
/// </summary>
/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
public interface ISimpleEndpoint<in TParameters>
{
	/// <summary>
	/// Writes the endpoint configuration into the provided <see cref="IEndpointConfig"/>.
	/// </summary>
	/// <code>
	/// public void Configure(IEndpointConfig config)
	/// {
	///	  config.MapRoute("/users/{UserId:int}", HttpMethod.Get);
	///	  config.Customize(c =>
	///	  {
	///	    c.AllowAnonymous();
	///	  });
	/// }
	/// </code>
	void Configure(IEndpointConfig config);

	/// <summary>
	/// Handles a HTTP request.
	/// </summary>
	Task<IResult> HandleRequestAsync(TParameters parameters, CancellationToken ct);
}

/// <summary>
/// Extensions methods for <see cref="ISimpleEndpoint{TParameters}"/>.
/// </summary>
public static class SimpleEndpointExtensions
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
		using var scope = builder.ServiceProvider.CreateScope();

		// Create an instance of the endpoint.
		var endpointConfigInstance = ActivatorUtilities.CreateInstance<TEndpoint>(scope.ServiceProvider);

		// Retrieve the endpoint configuration.
		var endpointConfig = new EndpointConfig();
		endpointConfigInstance.Configure(endpointConfig);

		// Generate the handler delegate.
		var endpointDelegate =
			async (
				CancellationToken ct,
				IServiceProvider services,
				[AsParameters] TParameters request
			) =>
			{
				var endpointRequestInstance = ActivatorUtilities.CreateInstance<TEndpoint>(services);
				return await endpointRequestInstance.HandleRequestAsync(request, ct).ConfigureAwait(false);
			};

		// Routes.
		foreach (var pattern in endpointConfig.Routes)
		{
			endpointConfig.CustomizeDelegate(builder.Map(pattern, endpointDelegate));
		}

		// Method Routes.
		foreach (var (pattern, methods) in endpointConfig.MethodRoutes)
		{
			endpointConfig.CustomizeDelegate(builder.MapMethods(pattern, methods, endpointDelegate));
		}

		// Global Fallback.
		if (endpointConfig.IsGlobalFallback)
		{
			endpointConfig.CustomizeDelegate(builder.MapFallback(endpointDelegate));
		}

		// Fallback Routes.
		foreach (var pattern in endpointConfig.FallbackRoutes)
		{
			endpointConfig.CustomizeDelegate(builder.MapFallback(pattern, endpointDelegate));
		}
	}
}
