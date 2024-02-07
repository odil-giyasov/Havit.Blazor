﻿using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Sidebar component - responsive navigation sidebar.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSidebar">https://havit.blazor.eu/components/HxSidebar</see>
/// </summary>
public partial class HxSidebar : ComponentBase
{
	/// <summary>
	/// Sidebar header.
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Sidebar items. Use <see cref="HxSidebarItem"/>.
	/// </summary>
	[Parameter] public RenderFragment ItemsTemplate { get; set; }

	/// <summary>
	/// <c>ExpandIcon</c> is obsolete and will be removed in a future release. Use <see cref="TogglerTemplate"/> if you want to render an icon.
	/// </summary>
	[Obsolete("ExpandIcon is obsolete and will be removed in a future release. Use TogglerTemplate if you want to render an icon.")]
	[Parameter] public IconBase ExpandIcon { get; set; }

	/// <summary>
	/// <c>CollapseIcon</c> is obsolete and will be removed in a future release. Use <see cref="TogglerTemplate"/> if you want to render an icon.
	/// </summary>
	[Obsolete("CollapseIcon is obsolete and will be removed in a future release. Use TogglerTemplate if you want to render an icon.")]
	[Parameter] public IconBase CollapseIcon { get; set; }

	/// <summary>
	/// Sidebar footer (e.g. logged user, language switch, ...).
	/// </summary>
	[Parameter] public RenderFragment<SidebarFooterTemplateContext> FooterTemplate { get; set; }

	/// <summary>
	/// Vertical toggler (desktop version) to be rendered instead of the default bar/arrow.
	/// </summary>
	[Parameter] public RenderFragment<SidebarTogglerTemplateContext> TogglerTemplate { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// ID of the root sidebar HTML element.
	/// (Autogenerated if not set.)
	/// </summary>
	[Parameter] public string Id { get; set; } = "hx-" + Guid.NewGuid().ToString("N");

	/// <summary>
	/// Indicates whether the <see cref="HxSidebar"/> is collapsed, can be used to alter the state (expand or collapse the sidebar).
	/// </summary>
	[Parameter] public bool Collapsed { get; set; } = false;
	/// <summary>
	/// Fires when the sidebar is expanded or collapsed.
	/// </summary>
	[Parameter] public EventCallback<bool> CollapsedChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="CollapsedChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeCollapsedChangedAsync(bool collapsed) => CollapsedChanged.InvokeAsync(collapsed);

	/// <summary>
	/// Whether multiple items can be in the expanded state at once.
	/// If set to <c>false</c>, upon item expansion, all other items are collapsed.
	/// The default is <c>true</c>.
	/// </summary>
	[Parameter] public bool MultipleItemsExpansion { get; set; } = true;

	/// <summary>
	/// The breakpoint below which the sidebar switches to the mobile version (exclusive).<br/>
	/// The default is <see cref="SidebarResponsiveBreakpoint.Medium"/>.
	/// </summary>
	[Parameter] public SidebarResponsiveBreakpoint ResponsiveBreakpoint { get; set; } = SidebarResponsiveBreakpoint.Medium;

	[Inject] protected IStringLocalizer<HxSpinner> Localizer { get; set; }

	protected internal string NavContentElementId => Id + "-nav-content";

	/// <summary>
	/// The ID of the immediate parent of the contained <see cref="HxSidebarItem"/> components.
	/// </summary>
	internal string _navId = "hx" + Guid.NewGuid().ToString("N");

	private string GetCollapsedCssClass() => Collapsed ? "collapsed" : null;

	private async Task HandleCollapseToggleClick()
	{
		Collapsed = !Collapsed;
		await InvokeCollapsedChangedAsync(Collapsed);
	}

	private string GetResponsiveCssClass(string cssClassPattern)
	{
		return ResponsiveBreakpoint switch
		{
			SidebarResponsiveBreakpoint.None => cssClassPattern.Replace("-??-", "-"), // !!! Simplified for the use case of this component.
			SidebarResponsiveBreakpoint.Small => cssClassPattern.Replace("??", "sm"),
			SidebarResponsiveBreakpoint.Medium => cssClassPattern.Replace("??", "md"),
			SidebarResponsiveBreakpoint.Large => cssClassPattern.Replace("??", "lg"),
			SidebarResponsiveBreakpoint.ExtraLarge => cssClassPattern.Replace("??", "xl"),
			SidebarResponsiveBreakpoint.Xxl => cssClassPattern.Replace("??", "xxl"),
			_ => throw new InvalidOperationException($"Unknown nameof(ResponsiveBreakpoint) value {ResponsiveBreakpoint}")
		};
	}
}
