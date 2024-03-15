
using Rincon.Common.Pages;
using Rincon.ViewModels;

namespace Rincon.Pages;

/// <summary>
/// Home UI
/// </summary>
public partial class HomePage
{
	int count = 0;

	/// <summary>
	/// Receives the depedencies by DI
	/// </summary>
	public HomePage(HomeViewModel viewModel) : base(viewModel, "Home")
	{
		InitializeComponent();
		//this.BindingContext = viewModel;
	}
}

