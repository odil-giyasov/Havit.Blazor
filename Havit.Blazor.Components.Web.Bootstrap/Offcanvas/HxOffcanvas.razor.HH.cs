namespace Havit.Blazor.Components.Web.Bootstrap;

public partial class HxOffcanvas
{
	private string BodyTemplateStyles => this.IsSkeletonVisible ? "display: none;" : "display: block;";
	private string SkeletonTemplateStyles => this.IsSkeletonVisible ? "display: block;" : "display: none;";

	private bool isSkeletonVisible;
	public bool IsSkeletonVisible
	{
		get => this.isSkeletonVisible;
		set
		{
			this.isSkeletonVisible = value;
			this.StateHasChanged();
		}
	}

}
