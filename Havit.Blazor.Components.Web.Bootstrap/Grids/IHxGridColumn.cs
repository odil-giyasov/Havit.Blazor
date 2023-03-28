namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid column.
/// </summary>
public interface IHxGridColumn<TItem>
{
	/// <summary>
	/// Returns the unique column identifier.
	/// </summary>
	string GetId();

	/// <summary>
	/// Indicates whether the column is visible (otherwise the column is hidden).
	/// It is not suitable to conditionaly display the column using @if statement in the markup code.
	/// </summary>
	bool IsVisible();

	/// <summary>
	/// Column will have expand/collapse element by default. Grid will render an additional row under the main row to expand/collapse hidden details for the row
	/// </summary>
	/// <returns></returns>
	bool HasExpandCollapseElement();

	/// <summary>
	/// Get column order (for scenarios where column order can be modified).
	/// Default should be <c>0</c>.
	/// When columns have same order they should render in the order of their registration (Which is usually the same as the column appereance in the source code.
	/// But it differs when the column is displayed conditionaly using @if statement.).
	/// </summary>
	int GetOrder();

	/// <summary>
	/// Sorting of the column.
	/// </summary>
	SortingItem<TItem>[] GetSorting();

	/// <summary>
	/// Sorting of the column.
	/// </summary>
	int? GetDefaultSortingOrder();

	/// <summary>
	/// Returns header cell template.
	/// </summary>
	GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context);

	/// <summary>
	/// Returns data cell template for the specific item.
	/// </summary>
	GridCellTemplate GetItemCellTemplate(TItem item);

	/// <summary>
	/// Returns data cell expand/collapse element template for the specific item.
	/// </summary>
	GridCellTemplate GetItemExpandCollapseElementTemplate(TItem item);

	/// <summary>
	/// Returns data cell expand/collapse container template for the specific item.
	/// </summary>
	GridCellTemplate GetItemExpandCollapseContainerTemplate(TItem item);

	/// <summary>
	/// Returns placeholder cell template.
	/// </summary>
	GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context);

	/// <summary>
	/// Returns footer cell template.
	/// </summary>
	GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context);
}
