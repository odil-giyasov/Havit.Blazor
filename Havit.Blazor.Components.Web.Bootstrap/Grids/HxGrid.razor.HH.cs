﻿using Havit.Collections;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;
[CascadingTypeParameter(nameof(TItem))]
public partial class HxGrid<TItem> : ComponentBase, IDisposable
{
	#region Grid Toolbar

	[Parameter] public EventCallback ToCsv { get; set; }    // CSV export button handler

	[Parameter] public EventCallback ToExcel { get; set; }  // Excel export button handler

	[Parameter] public RenderFragment ToolbarButtons { get; set; }  // Other possible buttons and components 

	#endregion

	#region Grid Page Sizer

	private bool pageSizerChangedValue;
	private bool isFirstParameterSet = true;   // to ensure pageSizerValue is set once in OnParametersSetAsync 

	private int pageSizerValue; // selected value in Grid Page Sizer
	public int PageSizerValue { get => pageSizerValue; }

	[Parameter] public EventCallback<int> PageSizer_ValueChanged { get; set; }  // event raised on Grid Page Sizer changes its value 

	private async void OnPageSizerValueChanged(int v)
	{
		pageSizerChangedValue = true;
		pageSizerValue = v;
		await PageSizer_ValueChanged.InvokeAsync(pageSizerValue);
		this.PageSize = v;

		await HandlePagerCurrentPageIndexChanged(0);
	}

	private List<GridPageSizerDDLItem> pageSizeEntries = new()
	{
		new GridPageSizerDDLItem{ Id = "20", Name = "20" },
		new GridPageSizerDDLItem{ Id = "50", Name = "50" },
		new GridPageSizerDDLItem{ Id = "0", Name = "All" }
	};

	#endregion

	private async Task OnRefreshButtonClicked()
	{
		await RefreshDataAsync();
	}


}


public class GridPageSizerDDLItem
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Group { get; set; }
}

/// <summary>
/// Values for Grid Page Sizer
/// </summary>
public enum GridPageSizerValue
{
	Size20 = 20,
	Size50 = 50,
	All = 0
}