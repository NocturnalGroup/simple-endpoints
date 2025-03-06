using Microsoft.AspNetCore.Http;

namespace NocturnalGroup.SimpleEndpoints.Parameters;

/// <summary>
/// A parameters type that provides access to the <see cref="HttpContext"/>.
/// </summary>
public readonly struct ContextParameters
{
	/// <inheritdoc cref="HttpContext"/>
	public HttpContext Context { get; init; }

	/// <inheritdoc cref="HttpRequest"/>
	public HttpRequest Request => Context.Request;

	/// <inheritdoc cref="HttpResponse"/>
	public HttpResponse Response => Context.Response;
}
