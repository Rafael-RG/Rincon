using Mopups.Interfaces;
using Rincon.CustomPopups;
using Rincon.Models;
using Rincon.ViewModels;

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

    private void OnAddStockTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListAddStock = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsAddStock.ItemsSource = this.ViewModel.Products;
        }
        else
        {
            ProductsAddStock.ItemsSource = this.ViewModel.Products.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    private void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((Product)ProductsAddStock.SelectedItem == null) return;
        this.ViewModel.LoadProductToAddCommand.Execute((Product)ProductsAddStock.SelectedItem);
        this.ViewModel.IsVisibleListAddStock = false;
        this.SearchBarAddStock.Unfocus();
    }

    void SearchBar_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListAddStock = true;
    }

    void SearchBar_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListAddStock = false;
    }

    void TapGestureRecognizerInAddStock_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        this.SearchBarAddStock.Unfocus();
    }
}