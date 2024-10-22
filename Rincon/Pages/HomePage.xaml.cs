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
    public HomePage(HomeViewModel viewModel, IPopupNavigation popupNavigation) : base(viewModel, "Home")
    {
        InitializeComponent();
        this.popupNavigation = popupNavigation;
    }

    /// <summary>
    /// Popups confirm added stock.
    /// </summary>
    private async void AddStock_Clicked(object sender, EventArgs e)
    {
        await popupNavigation.PushAsync(new ConfirmAddStockPage(this.popupNavigation, ViewModel.ProductsStock.ToList(), ViewModel.OkAddStockCommand, ViewModel.SavePDFCommand));
    }


    /// <summary>
	/// Popups added note.
	/// </summary>
	private async void AddNote_Clicked(object sender, EventArgs e)
    {
        await popupNavigation.PushAsync(new AddNotePage(this.popupNavigation, ViewModel.AddedNoteCommand));
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
        this.ViewModel.IsVisibleListAddStock = !this.ViewModel.IsVisibleListAddStock;
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
        var product = (Product)button.BindingContext;

        await popupNavigation.PushAsync(new DeleteProductsQuestionPage(this.popupNavigation, product, ViewModel.OkDeleteProductCommand));
    }

    /// <summary>
	/// Popups delete product.
	/// </summary>
	private void VisivilityProduct_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var product = (Product)button.BindingContext;

        this.ViewModel.SelectedProduct = product;

        this.ViewModel.IsInventoryEditView = true;

    }

    /// <summary>
    /// Popups delete product.
    /// </summary>
    private async void VisivilityNote_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var note = (Note)button.BindingContext;

        await popupNavigation.PushAsync(new AddNotePage(this.popupNavigation, ViewModel.AddedNoteCommand, note));

    }

    /// <summary>
	/// Popups delete product.
	/// </summary>
	private void DeleteNote_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var note = (Note)button.BindingContext;

        this.ViewModel.DeleteNoteCommand.Execute(note);

    }

    void BackToInventory_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.ChangeViewCommand.Execute("Inventory");
    }

    void EditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.ProductCommentEdit = this.ViewModel.SelectedProduct.Comment;

        this.ViewModel.ProductLocationEdit = this.ViewModel.SelectedProduct.Location;

        this.ViewModel.ProduSupplierEdit = this.ViewModel.SelectedProduct.Supplier;

        this.ViewModel.ChangeViewCommand.Execute("EditProductDetaildInventory");

    }

    async void CancelEditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        await popupNavigation.PushAsync(new ConfirmCancelEditProductPage(this.popupNavigation, this.ViewModel.ChangeViewCommand));
    }

    async void SaveEditProductPage_Clicked(System.Object sender, System.EventArgs e)
    {
        this.ViewModel.SelectedProduct.Comment = this.ViewModel.ProductCommentEdit;
        this.ViewModel.SelectedProduct.Location = this.ViewModel.ProductLocationEdit;
        this.ViewModel.SelectedProduct.Supplier = this.ViewModel.ProduSupplierEdit;

        this.ViewModel.UpdateProductCommand.Execute(this.ViewModel.SelectedProduct);

        this.ProductsSearchStockInventory.ItemsSource = this.ViewModel.Products;

        this.ProductsSearchStockInventory.SelectedItem = this.ViewModel.SelectedProduct;

        await popupNavigation.PushAsync(new SuccessEditProductPage(this.popupNavigation, this.ViewModel.ChangeViewCommand));
    }

    void SearchBarLateralBar_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                this.LateraBarStock.ItemsSource = this.ViewModel.Cards;
            }
            else
            {
                LateraBarStock.ItemsSource = this.ViewModel.Cards.Where(x => x.Id.ToLower().Contains(e.NewTextValue.ToLower())
                    || x.Description.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            }
        }
        catch
        {

        }

    }

    void States_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        this.ViewModel.SelectedState = (string)StatesList.SelectedItem;
        this.ViewModel.IsVisibleListStates = false;
    }

    void Machimbres_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        this.ViewModel.SelectedMachimbre = (string)MachimbreList.SelectedItem;
        this.ViewModel.IsVisibleListMachimbres = false;
    }

    void OnSearchStockInventoryTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ProductsSearchStockInventory.ItemsSource = this.ViewModel.Products;
            }
            else
            {
                ProductsSearchStockInventory.ItemsSource = this.ViewModel.Products.Where(x => x.Id.ToLower().Contains(e.NewTextValue.ToLower())
                    || x.Description.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            }
        }
        catch
        {

        }
    }

    void LateraBarStock_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var productCard = (CardStock)this.LateraBarStock.SelectedItem;
        this.ViewModel.LBProductDetaildCommand.Execute(productCard);
    }

    private void Menu_Clicked(object sender, EventArgs e)
    {
        popupNavigation.PushAsync(new MenuPage(this.popupNavigation, this.ViewModel.User, this.ViewModel.LogoutViewCommand, this.ViewModel.ConfigurationCommand, this.ViewModel.ManagementOperatorsCommand));
    }

    void Questions_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        this.ViewModel.SelectedQuestionConfigurations = (string)Questions.SelectedItem;
        this.ViewModel.IsVisibleListQuestions = false;
    }

    async void ConfirmChange_Clicked(object sender, EventArgs e)
    {
        this.ViewModel.OkConfirmConfigurationCommand.Execute(null);

        var command = this.ViewModel.OkConfirmConfigurationCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessEditUserPage(this.popupNavigation));
            this.ViewModel.ChangeViewCommand.Execute("Home");
        }
        
    }
}