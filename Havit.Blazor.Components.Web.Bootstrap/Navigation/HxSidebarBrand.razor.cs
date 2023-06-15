﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Brand for the <see cref="HxSidebar.HeaderTemplate"/>.
/// </summary>
public partial class HxSidebarBrand
{
	/// <summary>
	/// Brand long name.
	/// </summary>
	[Parameter] public string BrandName { get; set; }

	/// <summary>
	/// Brand logo.
	/// </summary>
	[Parameter] public RenderFragment<SidebarBrandLogoTemplateContext> LogoTemplate { get; set; }

	/// <summary>
	/// Brand short name.
	/// </summary>
	[Parameter] public string BrandNameShort { get; set; }

	/// <summary>
	/// <see cref="HxSidebar"/> containing the <see cref="HxSidebarBrand"/>.
	/// </summary>
	[CascadingParameter] protected HxSidebar ParentSidebar { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }


	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(ParentSidebar is not null, $"{nameof(HxSidebarBrand)} has to be placed inside {nameof(HxSidebar)}.");
	}
}
