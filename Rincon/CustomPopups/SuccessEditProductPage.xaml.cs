using System.Windows.Input;
using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class SuccessEditProductPage
{

    IPopupNavigation popupNavigation;

    ICommand command;

    public SuccessEditProductPage(IPopupNavigation popupNavigation, ICommand command)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.command = command;
    }

    async void CloseEditProduct_Clicked(System.Object sender, System.EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
        this.command.Execute("EditProductInventory");
    }
}