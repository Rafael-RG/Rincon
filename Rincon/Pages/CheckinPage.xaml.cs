using Rincon.ViewModels;

namespace Rincon.Pages;

/// <summary>
/// Entry form UI
/// </summary>
public partial class CheckinPage
{

	/// <summary>
	/// Receives the depedencies by DI
	/// </summary>
	public CheckinPage(CheckinViewModel viewModel) : base(viewModel, "Checkin")
	{
		InitializeComponent();
	}
}

