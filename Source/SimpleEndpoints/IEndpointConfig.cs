using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace NocturnalGroup.SimpleEndpoints;

/// <summary>
/// The configuration of a SimpleEndpoint.
/// </summary>
public interface IEndpointConfig
{
	/// <summary>
	/// Maps this endpoint to match requests for the specified pattern.
	/// </summary>
	public IEndpointConfig MapRoute([StringSyntax("Route")] string pattern);

	/// <summary>
	/// Maps this endpoint to match requests for the specified HTTP methods and pattern.
	/// </summary>
	/// <code>
	/// config.MapRoute("/users/{UserId:int}", "GET"); // Get Only
	/// config.MapRoute("/users/{UserId:int}", "GET", "POST"); // Get and Post
	/// </code>
	public IEndpointConfig MapRoute([StringSyntax("Route")] string pattern, params string[] methods);

	/// <summary>
	/// Maps this endpoint to match requests for the specified HTTP methods and pattern.
	/// </summary>
	/// <code>
	/// config.MapRoute("/users/{UserId:int}", HttpMethod.Get); // Get Only
	/// config.MapRoute("/users/{UserId:int}", HttpMethod.Get, HttpMethod.Post); // Get and Post
	/// </code>
	public IEndpointConfig MapRoute([StringSyntax("Route")] string pattern, params HttpMethod[] methods);

	/// <summary>
	/// Maps this endpoint to match requests for non-file-names with the lowest possible priority.
	/// </summary>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder,System.Delegate)"/>
	public IEndpointConfig MapFallback();

	/// <summary>
	/// Maps this endpoint to match the provided pattern with the lowest possible priority.
	/// </summary>
	/// <seealso cref="EndpointRouteBuilderExtensions.MapFallback(IEndpointRouteBuilder,string,System.Delegate)"/>
	public IEndpointConfig MapFallback([StringSyntax("Route")] string pattern);

	/// <summary>
	/// Applies additional configuration to the endpoint, such as authorization.
	/// </summary>
	/// <code>
	/// config.Customize(c =>
	/// {
	///   c.AllowAnonymous();
	/// });
	/// </code>
	public IEndpointConfig Customize(Action<RouteHandlerBuilder> builder);
}

/// <summary>
/// Internal implementation of <see cref="IEndpointConfig"/>
/// </summary>
internal class EndpointConfig : IEndpointConfig
{
	// Routes
	public List<string> Routes { get; } = [];
	public List<Tuple<string, string[]>> MethodRoutes { get; } = [];

	// Fallback
	public bool IsGlobalFallback { get; private set; }
	public List<string> FallbackRoutes { get; } = [];

	// Customization
	public Action<RouteHandlerBuilder> CustomizeDelegate { get; private set; } = _ => { };

	/// <inheritdoc />
	public IEndpointConfig MapRoute(string pattern)
	{
		Routes.Add(pattern);
		return this;
	}

	/// <inheritdoc />
	public IEndpointConfig MapRoute(string pattern, params string[] methods)
	{
		MethodRoutes.Add(Tuple.Create(pattern, methods));
		return this;
	}

	/// <inheritdoc />
	public IEndpointConfig MapRoute(string pattern, params HttpMethod[] methods)
	{
		MethodRoutes.Add(Tuple.Create(pattern, methods.Select(x => x.Method).ToArray()));
		return this;
	}

	/// <inheritdoc />
	public IEndpointConfig MapFallback()
	{
		IsGlobalFallback = true;
		return this;
	}

	/// <inheritdoc />
	public IEndpointConfig MapFallback(string pattern)
	{
		FallbackRoutes.Add(pattern);
		return this;
	}

	/// <inheritdoc />
	public IEndpointConfig Customize(Action<RouteHandlerBuilder> builder)
	{
		CustomizeDelegate = builder;
		return this;
	}
}
