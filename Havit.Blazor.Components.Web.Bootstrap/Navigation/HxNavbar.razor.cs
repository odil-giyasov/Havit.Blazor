﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.2/components/navbar/">Bootstrap 5 Navbar</see> component - responsive navigation header.<br/>
/// With support for branding, navigation, and more, including support for the collapse plugin.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxNavbar">https://havit.blazor.eu/components/HxNavbar</see>
/// </summary>
public partial class HxNavbar
{
	/// <summary>
	/// Color (background).
	/// Default is <see cref="ThemeColor.Light"/>.
	/// </summary>
	[Parameter] public ThemeColor Color { get; set; } = ThemeColor.Light;

	/// <summary>
	/// Color scheme.<br/>
	/// Default is <see cref="NavbarColorScheme.Light"/>.
	/// </summary>
	[Parameter] public NavbarColorScheme ColorScheme { get; set; } = NavbarColorScheme.Light;

	/// <summary>
	/// Responsive expand setting (breakpoint) for <see cref="HxNavbar"/>.<br/>
	/// Default is <see cref="NavbarExpand.LargeUp"/>.
	/// </summary>
	[Parameter] public NavbarExpand Expand { get; set; } = NavbarExpand.LargeUp;

	/// <summary>
	/// Element ID.
	/// (Autogenerated GUID if not set explicitly.)
	/// </summary>
	[Parameter] public string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Container CSS class. Deafult is <c>container-fluid</c>.
	/// </summary>
	[Parameter] public string ContainerCssClass { get; set; } = "container-fluid";

	/// <summary>
	/// Content of the navbar.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	protected internal string GetDefaultCollapseId()
	{
		return Id + "-collapse";
	}

	protected virtual string GetExpandCssClass()
	{
		return this.Expand switch
		{
			NavbarExpand.Always => "navbar-expand",
			NavbarExpand.SmallUp => "navbar-expand-sm",
			NavbarExpand.MediumUp => "navbar-expand-md",
			NavbarExpand.LargeUp => "navbar-expand-lg",
			NavbarExpand.ExtraLargeUp => "navbar-expand-xl",
			NavbarExpand.XxlUp => "navbar-expand-xxl",
			NavbarExpand.Never => null,
			_ => throw new InvalidOperationException($"Unknow {nameof(NavbarExpand)} value {this.Expand}.")
		};
	}

	protected virtual string GetColorSchemeCssClass()
	{
		return this.ColorScheme switch
		{
			NavbarColorScheme.Light => "navbar-light",
			NavbarColorScheme.Dark => "navbar-dark",
			NavbarColorScheme.None => null,
			_ => throw new InvalidOperationException($"Unknow {nameof(NavbarColorScheme)} value {this.ColorScheme}.")
		};
	}

	protected virtual string GetCssClass()
	{
		return CssClassHelper.Combine(
			"navbar",
			GetExpandCssClass(),
			GetColorSchemeCssClass(),
			this.Color.ToBackgroundColorCss(),
			this.CssClass);
	}
}
