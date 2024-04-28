using System.Windows.Input;
using Mopups.Interfaces;
using Rincon.Models;

namespace Rincon.CustomPopups;

public partial class DeleteProductsQuestionPage
{
    IPopupNavigation popupNavigation;

    public Product Product;

    ICommand OkDeleteProductCommand;

    public DeleteProductsQuestionPage(IPopupNavigation popupNavigation, Product product, ICommand command)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.Product = product;
        this.ProductId.Text = product.Id;
        this.ProductDescription.Text = product.Description;
        this.ProductState.Text = product.WoodState.ToString();
        this.OkDeleteProductCommand = command;
    }

    private async void CancelDeleteProduct_Clicked(object sender, EventArgs e)
    {
        await this.popupNavigation.PopAllAsync();
    }

    private async void OkDeleteProduct_Clicked(object sender, EventArgs e)
    {
        this.OkDeleteProductCommand.Execute(this.Product);
        await this.popupNavigation.PopAllAsync();
        await popupNavigation.PushAsync(new NotificationSuccessDeleteProductPage(this.popupNavigation));
    }
}
