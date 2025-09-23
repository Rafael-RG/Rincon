using Rincon.ViewModels;

namespace Rincon.Pages;

/// <summary>
/// Login UI
/// </summary>
public partial class LoginPage
{
	/// <summary>
	/// Receives the dependencies by DI
	/// </summary>
	public LoginPage(LoginViewModel viewModel) : base(viewModel, "Login")
	{
		InitializeComponent();
	}

	/// <summary>
	/// Handles the visual effect when operators button is pressed
	/// </summary>
	private void OnOperatorsPressed(object sender, EventArgs e)
	{
		if (sender is ImageButton button)
		{
			button.Scale = 0.95;
			button.Opacity = 0.8;
			button.BackgroundColor = Color.FromArgb("#3A5E3C");
		}
	}

	/// <summary>
	/// Restores the normal state when operators button is released
	/// </summary>
	private void OnOperatorsReleased(object sender, EventArgs e)
	{
		if (sender is ImageButton button)
		{
			button.Scale = 1.0;
			button.Opacity = 1.0;
			button.BackgroundColor = Color.FromArgb("#4f6650ff");
		}
	}
}
