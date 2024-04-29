using Rincon.ViewModels;

namespace Rincon.Pages;

public partial class RecoverPasswordPage
{
	public RecoverPasswordPage(RecoverPasswordViewModel viewModel) : base(viewModel, "Recover")
    {
		InitializeComponent();
	}

    void Questions_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        this.ViewModel.SeletedQuestion = (string)Questions.SelectedItem;
        this.ViewModel.IsVisibleListQuestions = false;
    }
}
