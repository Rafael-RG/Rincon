using Mopups.Interfaces;
using Rincon.Models;
using System.Windows.Input;

namespace Rincon.CustomPopups;

public partial class ConfirmAddStockPage
{
    IPopupNavigation popupNavigation;

    List<ProductStock> productStocks;

    ICommand okAddStockCommand;

    public ConfirmAddStockPage(IPopupNavigation popupNavigation, List<ProductStock> productStocks, ICommand okAddStockCommand)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        ProductsStockToAdd.ItemsSource = productStocks;
        this.okAddStockCommand = okAddStockCommand;
    }

	private async void CancelAddStock_Clicked(object sender, EventArgs e)
	{
        await this.popupNavigation.PopAllAsync();
    }

    private async void OkAddStock_Clicked(object sender, EventArgs e)
    {
        this.okAddStockCommand.Execute(this.productStocks);
        await this.popupNavigation.PopAllAsync();
    }
}