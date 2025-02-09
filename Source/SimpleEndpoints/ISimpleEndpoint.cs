using Microsoft.AspNetCore.Http;

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
	Task<IResult> HandleRequestAsync(TParameters parameters);
}
