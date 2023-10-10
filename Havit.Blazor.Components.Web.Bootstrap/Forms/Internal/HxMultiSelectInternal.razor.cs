﻿using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxMultiSelectInternal<TValue, TItem> : IAsyncDisposable
{
	[Parameter] public string InputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public string InputText { get; set; }

	[Parameter] public bool EnabledEffective { get; set; }

	[Parameter] public List<TItem> ItemsToRender { get; set; }

	[Parameter] public List<int> SelectedIndexes { get; set; }

	[Parameter] public List<TValue> SelectedValues { get; set; }

	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	[Parameter] public List<TValue> Value { get; set; }

	[Parameter] public string NullDataText { get; set; }

	[Parameter] public EventCallback<SelectionChangedArgs> ItemSelectionChanged { get; set; }

	/// <summary>
	/// Custom CSS class to render with input-group span.
	/// </summary>
	[Parameter] public string InputGroupCssClass { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// Input-group at the end of the input.
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// Input-group at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	/// <summary>
	/// Enables filtering capabilities.
	/// </summary>
	[Parameter] public bool AllowFiltering { get; set; }

	[Parameter] public Func<TItem, string, bool> FilterPredicate { get; set; }

	[Parameter] public bool ClearFilterOnHide { get; set; }

	/// <summary>
	/// This event is fired when a dropdown element has been made visible to the user.
	/// </summary>
	[Parameter] public EventCallback<string> OnShown { get; set; }

	/// <summary>
	/// This event is fired when a dropdown element has been hidden from the user.
	/// </summary>
	[Parameter] public EventCallback<string> OnHidden { get; set; }

	/// <summary>
	/// Template that defines what should be rendered in case of empty items.
	/// </summary>
	[Parameter] public RenderFragment EmptyTemplate { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync(string elementId) => OnHidden.InvokeAsync(elementId);

	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync(string elementId) => OnShown.InvokeAsync(elementId);

	private IJSObjectReference jsModule;
	private readonly DotNetObjectReference<HxMultiSelectInternal<TValue, TItem>> dotnetObjectReference;
	private ElementReference elementReference;
	private ElementReference filterInputReference;
	private bool isShown;
	private string filterText = string.Empty;
	private bool disposed;

	public HxMultiSelectInternal()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}

			await jsModule.InvokeVoidAsync("initialize", elementReference, dotnetObjectReference, false);
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxMultiSelect));
	}

	private async Task HandleItemSelectionChangedAsync(bool newChecked, TItem item)
	{
		await ItemSelectionChanged.InvokeAsync(new SelectionChangedArgs
		{
			Checked = newChecked,
			Item = item
		});
	}

	private void HandleInputChanged(ChangeEventArgs e)
	{
		filterText = e.Value?.ToString() ?? string.Empty;
	}

	private List<TItem> GetFilteredItems()
	{
		if (!AllowFiltering || string.IsNullOrEmpty(filterText))
		{
			return ItemsToRender;
		}

		var filterPredicate = FilterPredicate ?? DefaultFilterPredicate;
		return ItemsToRender.Where(x => filterPredicate(x, filterText)).ToList();

		bool DefaultFilterPredicate(TItem item, string filter)
		{
			return string.IsNullOrWhiteSpace(filter) || TextSelector(item).Contains(filter, StringComparison.OrdinalIgnoreCase);
		}
	}

	public async ValueTask FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(elementReference, default))
		{
			throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
		}
		await elementReference.FocusAsync();
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxMultiSelect_HandleJsHidden")]
	public Task HandleJsHidden()
	{
		isShown = false;

		if (ClearFilterOnHide)
		{
			filterText = string.Empty;
		}

		return InvokeOnHiddenAsync(this.InputId);
	}

	[JSInvokable("HxMultiSelect_HandleJsShown")]
	public async Task HandleJsShown()
	{
		isShown = true;
		await filterInputReference.FocusAsync();
		await InvokeOnShownAsync(this.InputId);
	}

	/// <summary>
	/// Collapses the item.
	/// </summary>
	public async Task HideAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("hide", elementReference);
	}

	/// <summary>
	/// Expands the item.
	/// </summary>
	public async Task ShowAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("show", elementReference);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		disposed = true;

		if (jsModule != null)
		{
			try
			{
				await jsModule.InvokeVoidAsync("dispose", InputId);
				await jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}

		dotnetObjectReference?.Dispose();
	}



	public class SelectionChangedArgs
	{
		public bool Checked { get; set; }
		public TItem Item { get; set; }
	}
}