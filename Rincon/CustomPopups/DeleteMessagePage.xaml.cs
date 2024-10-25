using System.Windows.Input;
using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class DeleteMessagePage
{

    IPopupNavigation popupNavigation;

    ICommand confirm;

    public DeleteMessagePage(IPopupNavigation popupNavigation, string message, ICommand confirm)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.Message.Text = message;
        this.confirm = confirm;
    }

    async void BackDeleteOperator_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }

    async void ConfirmDeleteOperator_Clicked(System.Object sender, System.EventArgs e)
    {
        this.confirm.Execute(null);
        await this.popupNavigation.PopAllAsync();
    }
}
