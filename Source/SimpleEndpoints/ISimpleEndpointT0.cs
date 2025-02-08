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
public interface ISimpleEndpoint : ISimpleEndpoint<BaseParameters> { }

/// <summary>
/// Extensions to register an <see cref="ISimpleEndpoint"/> into a <see cref="IEndpointRouteBuilder"/>.
/// </summary>
public static class SimpleEndpointT0Extensions
{
	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP GET requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapGet(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapGet<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapGet(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP POST requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPost(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPost<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapPost(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP PUT requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPut(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPut<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapPut(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP DELETE requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapDelete(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapDelete<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapDelete(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP PATCH requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapPatch(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder MapPatch<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapPatch(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified HTTP methods and pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <param name="httpMethods">HTTP methods that the endpoint will match.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapMethods(IEndpointRouteBuilder, string, IEnumerable{string}, Delegate)"/>
	public static IEndpointConventionBuilder MapMethods<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern,
		IEnumerable<string> httpMethods
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapMethods(pattern, httpMethods, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.Map(IEndpointRouteBuilder, string, Delegate)"/>
	public static IEndpointConventionBuilder Map<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.Map(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that matches HTTP requests
	/// for the specified pattern.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.Map(IEndpointRouteBuilder, RoutePattern, Delegate)"/>
	public static IEndpointConventionBuilder Map<TEndpoint>(this IEndpointRouteBuilder builder, RoutePattern pattern)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.Map(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
	/// requests for non-file-names with the lowest possible priority.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder, Delegate)"/>
	public static RouteHandlerBuilder MapFallback<TEndpoint>(this IEndpointRouteBuilder builder)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapFallback(CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Adds a specialized <see cref="RouteEndpoint"/> to the <see cref="IEndpointRouteBuilder"/> that will match
	/// the provided pattern with the lowest possible priority.
	/// </summary>
	/// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to add the route to.</param>
	/// <param name="pattern">The route pattern.</param>
	/// <typeparam name="TEndpoint">The endpoint that will handle matching requests.</typeparam>
	/// <returns>A <see cref="RouteHandlerBuilder"/> that can be used to further customize the endpoint.</returns>
	/// <remarks>For more information, please see the referenced Map function.</remarks>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder, string, Delegate)"/>
	public static RouteHandlerBuilder MapFallback<TEndpoint>(
		this IEndpointRouteBuilder builder,
		[StringSyntax("Route")] string pattern
	)
		where TEndpoint : ISimpleEndpoint
	{
		return builder.MapFallback(pattern, CreateEndpointDelegate<TEndpoint>());
	}

	/// <summary>
	/// Returns a delegate that creates and invokes an <see cref="ISimpleEndpoint"/>.
	/// </summary>
	private static Delegate CreateEndpointDelegate<TEndpoint>()
		where TEndpoint : ISimpleEndpoint
	{
		return async (IServiceProvider services, HttpContext context) =>
		{
			var endpoint = ActivatorUtilities.CreateInstance<TEndpoint>(services);
			var parameters = new BaseParameters() { Context = context };
			return await endpoint.HandleRequestAsync(parameters).ConfigureAwait(false);
		};
	}
}
