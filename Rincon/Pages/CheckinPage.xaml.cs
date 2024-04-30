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

    void Questions_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        this.ViewModel.SeletedQuestion = (string)Questions.SelectedItem;
        this.ViewModel.IsVisibleListQuestions = false;
    }
}

