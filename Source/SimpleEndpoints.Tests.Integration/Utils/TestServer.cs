using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;

namespace NocturnalGroup.SimpleEndpoints.Tests.Unit.Utils;

/// <summary>
/// A WebApplication wrapper for unit tests.
/// </summary>
internal class TestServer : IAsyncDisposable
{
	private readonly WebApplication _app;

	public TestServer(Action<WebApplication> routeDelegate)
	{
		var builder = WebApplication.CreateBuilder();
		builder.WebHost.UseTestServer();
		_app = builder.Build();
		routeDelegate(_app);
	}

	public async Task<HttpClient> StartServerAsync()
	{
		await _app.StartAsync();
		return _app.GetTestClient();
	}

	public async ValueTask DisposeAsync()
	{
		await _app.StopAsync();
	}
}
