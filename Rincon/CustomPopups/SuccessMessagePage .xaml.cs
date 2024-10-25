using System.Windows.Input;
using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class SuccessMessagePage
{

    IPopupNavigation popupNavigation;

    ICommand command;

    public SuccessMessagePage(IPopupNavigation popupNavigation, string message)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.Message.Text = message;
    }

    async void ConfirmEditUser_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }
}
