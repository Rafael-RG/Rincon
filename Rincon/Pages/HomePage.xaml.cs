
using Mopups.Interfaces;
using Rincon.Common.Pages;
using Rincon.CustomPopups;
using Rincon.ViewModels;
using System.Windows.Input;

namespace Rincon.Pages;

/// <summary>
/// Home UI
/// </summary>
public partial class HomePage
{
	IPopupNavigation popupNavigation;

	/// <summary>
	/// Receives the depedencies by DI
	/// </summary>
	public HomePage(HomeViewModel viewModel,IPopupNavigation popupNavigation) : base(viewModel, "Home")
	{
		InitializeComponent();

		this.popupNavigation = popupNavigation;
	}

	/// <summary>
	/// Popups confirm added stock.
	/// </summary>
	private async void AddStock_Clicked(object sender, EventArgs e)
	{
        await popupNavigation.PushAsync(new ConfirmAddStockPage(this.popupNavigation, ViewModel.ProductsStock.ToList(),ViewModel.OkAddStockCommand,ViewModel.SavePDFCommand));
    }
}