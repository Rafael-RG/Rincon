using Mopups.Interfaces;
using Rincon.Models;
using System.Windows.Input;

namespace Rincon.CustomPopups;

public partial class ConfirmAddStockPage
{
    IPopupNavigation popupNavigation;

    List<ProductStock> productStocks;

    public string DateTme { get; set; } = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

    ICommand okAddStockCommand;

    ICommand savePDFCommand;

    public ConfirmAddStockPage(IPopupNavigation popupNavigation, List<ProductStock> productStocks, ICommand okAddStockCommand, ICommand savePDFCommand)
	{
		InitializeComponent();
        this.popupNavigation = popupNavigation;
        this.productStocks = productStocks;
        ProductsStockToAdd.ItemsSource = productStocks;
        this.okAddStockCommand = okAddStockCommand;
        this.savePDFCommand = savePDFCommand;
        this.DateTime.Text = this.DateTme;
        this.productStocks.ForEach(ps => ps.DateTime = this.DateTme);
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

    private void SavePDF_Clicked(object sender, EventArgs e)
    {
        // Mostrar activity indicator y ocultar botones
        ActivityIndicatorPDF.IsVisible = true;
        ActivityIndicatorPDF.IsRunning = true;
        ButtonsContainer.IsVisible = false;
        
        // Ejecutar el comando
        this.savePDFCommand.Execute(this.productStocks);
        
        // Usar un timer para ocultar el activity indicator después de un tiempo
        // (ya que no tenemos acceso directo al estado del ViewModel)
        this.Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(2), () =>
        {
            ActivityIndicatorPDF.IsVisible = false;
            ActivityIndicatorPDF.IsRunning = false;
            ButtonsContainer.IsVisible = true;
        });
    }
}