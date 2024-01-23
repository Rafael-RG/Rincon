using MobileTemplate.ViewModels;

namespace MobileTemplate.Pages;

/// <summary>
/// Settings UI
/// </summary>
public partial class SettingsPage
{

	/// <summary>
	/// Receives the depedencies by DI
	/// </summary>
	public SettingsPage(SettingsViewModel viewModel) : base(viewModel, "Settings")
	{
		InitializeComponent();
	}
}

