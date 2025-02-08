using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;

namespace NocturnalGroup.SimpleEndpoints;

/// <summary>
/// An endpoint in the Minimal API system.
/// </summary>
/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
public interface ISimpleEndpoint<in TParameters>
{
	/// <summary>
	/// Handles a HTTP request.
	/// </summary>
	Task<IResult> HandleRequestAsync(TParameters parameters);
}

/// <summary>
/// Extensions to register an <see cref="ISimpleEndpoint{TParameters}"/> into a <see cref="IEndpointRouteBuilder"/>.
/// </summary>
public static class SimpleEndpointT1Extensions
{
	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP GET requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapGet(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapGet<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapGet(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP POST requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPost(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPost<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapPost(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP PUT requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPut(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPut<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapPut(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP DELETE requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapDelete(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapDelete<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapDelete(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP PATCH requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPatch(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPatch<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapPatch(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified HTTP methods and pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <param name="httpMethods">HTTP methods that the endpoint will match.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapMethods(IEndpointRouteBuilder, string, IEnumerable{string}, Delegate)"/>
	public static IEndpointConventionBuilder MapMethods<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern,
		IEnumerable<string> httpMethods
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapMethods(pattern, httpMethods, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.Map(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder Map<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.Map(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.Map(IEndpointRouteBuilder, RoutePattern, Delegate)"/>
	public static IEndpointConventionBuilder Map<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		RoutePattern pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.Map(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
	/// requests for non-file-names with the lowest possible priority.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder, Delegate)"/>
	public static RouteHandlerBuilder MapFallback<TEndpoint, TParameters>(this IEndpointRouteBuilder builder)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapFallback(CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
	/// the provided pattern with the lowest possible priority.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <typeparam name="TParameters">The parameters used by the endpoint.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder, string, Delegate)"/>
	public static RouteHandlerBuilder MapFallback<TEndpoint, TParameters>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return builder.MapFallback(pattern, CreateEndpointDelegate<TEndpoint, TParameters>());
	}

	/// <summary>
	/// Returns a delegate that creates and invokes an <see cref="ISimpleEndpoint{TParameters}"/>.
	/// </summary>
	private static Delegate CreateEndpointDelegate<TEndpoint, TParameters>()
		where TEndpoint : ISimpleEndpoint<TParameters>
	{
		return async (IServiceProvider services, [AsParameters] TParameters request) =>
		{
			var endpoint = ActivatorUtilities.CreateInstance<TEndpoint>(services);
			return await endpoint.HandleRequestAsync(request).ConfigureAwait(false);
		};
	}
}
