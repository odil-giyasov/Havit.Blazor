using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

public static class JSRuntimeExtensions
{
	internal static ValueTask<IJSObjectReference> ImportHavitBlazorBootstrapModuleAsync(this IJSRuntime jsRuntime, string moduleNameWithoutExtension)
	{
		var path = "./_content/HH.Havit.Blazor.Components.Web.Bootstrap/" + moduleNameWithoutExtension + ".js?v=" + HxSetup.VersionIdentifierHavitBlazorBootstrap;
		return jsRuntime.InvokeAsync<IJSObjectReference>("import", path);
	}
}