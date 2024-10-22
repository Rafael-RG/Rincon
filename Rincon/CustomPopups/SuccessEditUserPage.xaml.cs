using System.Windows.Input;
using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class SuccessEditUserPage
{

    IPopupNavigation popupNavigation;

    ICommand command;

    public SuccessEditUserPage(IPopupNavigation popupNavigation)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
    }

    async void ConfirmEditUser_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }
}
