using Mopups.Interfaces;
using Rincon.CustomPopups;
using Rincon.Models;
using Rincon.ViewModels;
using StoreKit;

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

    private void OnSearchStockTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsSearchStock.ItemsSource = this.ViewModel.Stock;
        }
        else
        {
            ProductsSearchStock.ItemsSource = this.ViewModel.Stock.Where(x => x.Id.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Product.Description.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    /// <summary>
	/// Popups delete product.
	/// </summary>
	private async void DeleteProduct_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var productStock = (ProductStock)button.BindingContext;

        var product = productStock.Product;
        await popupNavigation.PushAsync(new DeleteProductsQuestionPage(this.popupNavigation, product, ViewModel.OkDeleteProductCommand));
    }

    /// <summary>
	/// Popups delete product.
	/// </summary>
	private void VisivilityProduct_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var productStock = (ProductStock)button.BindingContext;

        this.ViewModel.SelectedProductStock = productStock;

        this.ViewModel.IsInventoryEditView = true;

    }

    void BackToInventory_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.ChangeViewCommand.Execute("Inventory");
    }

    void EditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.ProductCommentEdit = this.ViewModel.SelectedProductStock.Product.Comment;

        this.ViewModel.ProductLocationEdit = this.ViewModel.SelectedProductStock.Product.Location;

        this.ViewModel.ProduSupplierEdit = this.ViewModel.SelectedProductStock.Product.Supplier;

        this.ViewModel.ChangeViewCommand.Execute("EditProductDetaildInventory");

    }

    async void CancelEditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        await popupNavigation.PushAsync(new ConfirmCancelEditProductPage(this.popupNavigation, this.ViewModel.ChangeViewCommand));
    }

    async void SaveEditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.SelectedProductStock.Product.Comment = this.ViewModel.ProductCommentEdit;
        this.ViewModel.SelectedProductStock.Product.Location = this.ViewModel.ProductLocationEdit;
        this.ViewModel.SelectedProductStock.Product.Supplier = this.ViewModel.ProduSupplierEdit;

        this.ViewModel.UpdateProductCommand.Execute(this.ViewModel.SelectedProductStock.Product);

        await popupNavigation.PushAsync(new SuccessEditProductPage(this.popupNavigation, this.ViewModel.ChangeViewCommand));
    }
}