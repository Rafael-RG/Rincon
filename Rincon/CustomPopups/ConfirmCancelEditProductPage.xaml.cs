using System.Windows.Input;
using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class ConfirmCancelEditProductPage
{

    IPopupNavigation popupNavigation;

    ICommand command;


    public ConfirmCancelEditProductPage(IPopupNavigation popupNavigation, ICommand command)
	{
		InitializeComponent();
		this.popupNavigation = popupNavigation;
        this.command = command;
	}

    async void  CloseEditProduct_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
        this.command.Execute("EditProductInventory");
    }

    async void ContinueEditProduct_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }
}
