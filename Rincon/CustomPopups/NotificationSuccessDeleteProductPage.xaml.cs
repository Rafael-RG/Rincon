using Mopups.Interfaces;

namespace Rincon.CustomPopups;

public partial class NotificationSuccessDeleteProductPage
{
    IPopupNavigation popupNavigation;

    public NotificationSuccessDeleteProductPage(IPopupNavigation popupNavigation)
	{
		InitializeComponent();
		this.popupNavigation = popupNavigation;
	}

    private async void CloseDeleteProduct_Clicked(object sender, EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }
}
