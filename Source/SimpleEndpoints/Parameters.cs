using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NocturnalGroup.SimpleEndpoints;

/// <summary>
/// An empty parameters type for a <see cref="ISimpleEndpoint{TParameters}"/>.
/// </summary>
/// <remarks>Use this when your endpoint doesn't require access to request-level information.</remarks>
public readonly struct NoParameters { }

/// <summary>
/// A base set of parameters for a <see cref="ISimpleEndpoint{TParameters}"/>.
/// </summary>
/// <remarks>Use this when your endpoint only requires access to the <see cref="HttpContext"/>.</remarks>
public readonly struct BaseParameters
{
	/// <inheritdoc cref="HttpContext"/>
	[FromServices]
	public HttpContext Context { get; init; }

	/// <inheritdoc cref="HttpRequest"/>
	public HttpRequest Request => Context.Request;

	/// <inheritdoc cref="HttpResponse"/>
	public HttpResponse Response => Context.Response;
}
