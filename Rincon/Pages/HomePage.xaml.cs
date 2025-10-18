using Mopups.Interfaces;
using Rincon.CustomPopups;
using Rincon.Models;
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
    public HomePage(HomeViewModel viewModel, IPopupNavigation popupNavigation) : base(viewModel, "Home")
    {
        InitializeComponent();
        this.popupNavigation = popupNavigation;
        
        // Suscribirse a cambios en las propiedades para actualizar la UI
        this.ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(HomeViewModel.Products))
            {
                RefreshInventoryList();
            }
        };
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
        try
        {
        await popupNavigation.PushAsync(new AddNotePage(this.popupNavigation, ViewModel.AddedNoteCommand));
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
	/// Popups added booking.
	/// </summary>
	private async void AddBooking_Clicked(object sender, EventArgs e)
    {
        try
        {
            this.ViewModel.ChangeViewCommand.Execute("CreateBookingOrder");
        }
        catch (Exception ex)
        {
        }
    }


    /// <summary>
	/// Popups added booking.
	/// </summary>
	private async void AddTask_Clicked(object sender, EventArgs e)
    {
        try
        {
            this.ViewModel.ChangeViewCommand.Execute("CreateTask");
        }
        catch (Exception ex)
        {
        }
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
    /// Refresca la lista del inventario después de operaciones CRUD
    /// </summary>
    private void RefreshInventoryList()
    {
        try
        {
            // Si hay texto de búsqueda, aplicar el filtro nuevamente
            if (!string.IsNullOrWhiteSpace(this.SearchBarInventory?.Text))
            {
                var searchText = this.SearchBarInventory.Text;
                ProductsSearchStockInventory.ItemsSource = this.ViewModel.Products.Where(x => 
                    x.Id.ToLower().Contains(searchText.ToLower()) ||
                    x.Description.ToLower().Contains(searchText.ToLower())).ToList();
            }
            else
            {
                // Si no hay búsqueda, mostrar todos los productos
                ProductsSearchStockInventory.ItemsSource = this.ViewModel.Products;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al refrescar lista de inventario: {ex.Message}");
        }
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

        // Usar el método centralizado para refrescar la lista
        RefreshInventoryList();

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
        if (StatesList.SelectedItem == null) return;
        
        this.ViewModel.SelectedState = (string)StatesList.SelectedItem;
        this.ViewModel.IsVisibleListStates = false;
        
        // Limpiar la selección para permitir re-selección del mismo item
        StatesList.SelectedItem = null;
    }

    void Product_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (ProductList.SelectedItem == null) return;
        
        this.ViewModel.SelectedProduct = (Product)ProductList.SelectedItem;
        this.ViewModel.IsVisibleListProducts = false;
        
        // Limpiar la selección para permitir re-selección del mismo item
        ProductList.SelectedItem = null;
    }

    void Machimbres_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (MachimbreList.SelectedItem == null) return;
        
        this.ViewModel.SelectedMachimbre = (string)MachimbreList.SelectedItem;
        this.ViewModel.IsVisibleListMachimbres = false;
        
        // Limpiar la selección para permitir re-selección del mismo item
        MachimbreList.SelectedItem = null;
    }

    void OnSearchStockInventoryTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        // Usar el método centralizado para refrescar la lista
        RefreshInventoryList();
    }

    void LateraBarStock_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        try
        {
            // Validar que hay un item seleccionado
            if (this.LateraBarStock.SelectedItem == null)
            {
                return;
            }

            var productCard = (CardStock)this.LateraBarStock.SelectedItem;
            
            // Validar que el comando existe
            if (this.ViewModel?.LBProductDetaildCommand != null)
            {
                this.ViewModel.LBProductDetaildCommand.Execute(productCard);
            }

            // Limpiar la selección para permitir re-selección del mismo item
            this.LateraBarStock.SelectedItem = null;
        }
        catch (Exception ex)
        {
            // Log del error para debugging
            System.Diagnostics.Debug.WriteLine($"Error en LateraBarStock_SelectionChanged: {ex.Message}");
        }
    }

    /// <summary>
    /// Maneja el tap en el Frame del item de stock como alternativa al SelectionChanged
    /// </summary>
    private void Frame_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var frame = (Frame)sender;
            var productCard = (CardStock)frame.BindingContext;
            
            if (productCard != null && this.ViewModel?.LBProductDetaildCommand != null)
            {
                this.ViewModel.LBProductDetaildCommand.Execute(productCard);
                System.Diagnostics.Debug.WriteLine($"Frame_Tapped ejecutado para: {productCard.Description}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en Frame_Tapped: {ex.Message}");
        }
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
        //this.ViewModel.OkConfirmConfigurationCommand.Execute(null);

        var command = this.ViewModel.OkConfirmConfigurationCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessEditUserPage(this.popupNavigation));
            this.ViewModel.ChangeViewCommand.Execute("Home");
        }
        
    }

    async void ConfirmAddOperator_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.OkConfirmAddOperatorCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation,"Nuevo operario creado con exito"));
        }

    }
    
    async void ConfirmDeleteOperator_Clicked(object sender, EventArgs e)
    {
        var button = ((Button)sender);
        var operatorToDelete = (Operator)button.BindingContext;

        this.ViewModel.OperatorToDelete = operatorToDelete;

        
        await popupNavigation.PushAsync(new DeleteMessagePage(this.popupNavigation, $"¿Desea eliminar al Operario “{operatorToDelete.Name}”?", this.ViewModel.ValidateDeleteOperatorAsyncCommand));

    }

    async void OkDeleteOperator_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.DeleteOperatorCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"El Operario “{this.ViewModel.OperatorToDelete.Name}” ha sido eliminado"));
        }

    }
    
    async void OkEditOperator_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.OkEditOperatorCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Edición guardada correctamente"));
        }

    }


    async void ViewEditOperator_Clicked(object sender, EventArgs e)
    {
        var button = ((Button)sender);
        var operatorToUpdate = (Operator)button.BindingContext;

        this.ViewModel.OperatorToUpdate = operatorToUpdate;

        this.ViewModel.EditOperatorCommand.Execute(null);

    }

    private void CollectionNewMovement_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((ProductStock)ProductsNewMovement.SelectedItem == null) return;
        this.ViewModel.ProductMovement = (ProductStock)ProductsNewMovement.SelectedItem;
        this.ViewModel.ReloadDependingProductsCommand.Execute(null);
        this.ViewModel.SelectedDependingProducts = null;
        this.ViewModel.IsVisibleListNewMovement = false;
        this.ViewModel.IsSelectedProductMovement = true;
        this.SearchBarNewMovement.Unfocus();
    }

    void SearchBar_NewMovementFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewMovement = !this.ViewModel.IsVisibleListNewMovement;
    }

    void SearchBar_NewMovementUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewMovement = false;
    }

    private void OnNewMovementTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListNewMovement = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsNewMovement.ItemsSource = this.ViewModel.ProductsWithStock;
        }
        else
        {
            ProductsNewMovement.ItemsSource = this.ViewModel.ProductsWithStock.Where(x => x.Product.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    async void NewMovement_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.NewMovementCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se realizo el movimiento con exito!"));
        }

    }


    void SearchBar_EditStockFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListEditStock = !this.ViewModel.IsVisibleListEditStock;
    }

    void SearchBar_EditStockUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListEditStock = false;
    }

    private void OnEditStockTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListEditStock = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsEditStock.ItemsSource = this.ViewModel.ProductsWithStock;
        }
        else
        {
            ProductsEditStock.ItemsSource = this.ViewModel.ProductsWithStock.Where(x => x.Product.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }


    private void CollectionEditStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((ProductStock)ProductsEditStock.SelectedItem == null) return;
        this.ViewModel.ProductEditStock = (ProductStock)ProductsEditStock.SelectedItem;
        this.ViewModel.IsVisibleListEditStock = false;
        this.ViewModel.IsSelectedProductEditStock = true;
        this.SearchBarEditStock.Unfocus();
    }

    async void EditStock_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.EditStockCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se edito el stock con exito!"));
        }

    }


    //Tasks
    void SearchBar_NewTaskFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = !this.ViewModel.IsVisibleListNewTask;
    }

    void SearchBar_NewTaskUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = false;
    }

    private void OnNewTaskTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsNewTask.ItemsSource = this.ViewModel.ProductsWithStock;
        }
        else
        {
            ProductsNewTask.ItemsSource = this.ViewModel.ProductsWithStock.Where(x => x.Product.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    private void CollectionNewTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((ProductStock)ProductsNewTask.SelectedItem == null) return;
        this.ViewModel.ProductTask = (ProductStock)ProductsNewTask.SelectedItem;
        this.ViewModel.ReloadDerivateProductsCommand.Execute(null);
        this.ViewModel.SelectedDerivateProducts = this.ViewModel.DerivateProducts != null ? this.ViewModel.DerivateProducts.FirstOrDefault() : null;
        this.ViewModel.IsVisibleListNewTask = false;
        this.ViewModel.IsSelectedProductTask = this.ViewModel.ProductTask != null ? true : false;
        this.SearchBarNewTask.Unfocus();
    }

    async void CreateTask_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.CreateTaskCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se creo la tarea con exito!"));
        }

    }

    void ProductDerivate_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (ProductListDerivateTask.SelectedItem == null) return;
        
        this.ViewModel.SelectedDerivateProducts = (Product)ProductListDerivateTask.SelectedItem;
        this.ViewModel.IsVisibleListProductsDerivateTask = false;
        
        // Limpiar la selección para permitir re-selección del mismo item
        ProductListDerivateTask.SelectedItem = null;
    }

    void ProductDerivateEdit_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (ProductListDerivateEditTask.SelectedItem == null) return;
        
        this.ViewModel.SelectedDerivateProducts = (Product)ProductListDerivateEditTask.SelectedItem;
        this.ViewModel.IsVisibleListProductsDerivateTask = false;
        
        // Limpiar la selección para permitir re-selección del mismo item
        ProductListDerivateEditTask.SelectedItem = null;
    }

    void OnSearchTaskTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                TaskItemsSearch.ItemsSource = this.ViewModel.TaskItems;
            }
            else
            {
                TaskItemsSearch.ItemsSource = this.ViewModel.TaskItems.Where(x => x.TaskStatus.ToString().ToLower().Contains(e.NewTextValue.ToLower())
                    || x.ProductSource.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            }
        }
        catch
        {

        }
    }

    private void VisivilityTaskItemEdit_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (TaskItem)button.BindingContext;

        this.ViewModel.SelectedTaskItem = taskItem;

        this.ViewModel.ProductTask = this.ViewModel.ProductsWithStock.FirstOrDefault(x => x.Product.Id == taskItem.ProductSourceId);

        this.ViewModel.IsSelectedProductTask = true;

        this.ViewModel.ReloadDerivateProductsCommand.Execute(null);

        this.ViewModel.SelectedDerivateProducts = this.ViewModel.Products.FirstOrDefault(x => x.Id == taskItem.ProductDestinationId);

        this.ViewModel.QuantityTask = int.Parse(taskItem.Quantity);

        this.ViewModel.QuantityEditTask = int.Parse(taskItem.Quantity);

        this.ViewModel.CommentsTask = taskItem.Description;

        this.ViewModel.IsNoPriorityTask = !taskItem.Priority;

        this.ViewModel.IsYesPriorityTask = taskItem.Priority;

        this.ViewModel.IsTaskItemEditView = true;

    }

    private void VisivilityTaskItem_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (TaskItem)button.BindingContext;

        this.ViewModel.SelectedTaskItem = taskItem;
        this.ViewModel.ProductTask = this.ViewModel.ProductsWithStock.FirstOrDefault(x => x.Product.Id == taskItem.ProductSourceId);

        this.ViewModel.IsSelectedProductTask = true;

        this.ViewModel.SelectedDerivateProducts = this.ViewModel.Products.FirstOrDefault(x => x.Id == taskItem.ProductDestinationId);

        this.ViewModel.QuantityTask = int.Parse(taskItem.Quantity);

        this.ViewModel.CommentsTask = taskItem.Description;

        this.ViewModel.IsTaskItemView = true;

    }


    void SearchBar_EditTaskFocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = !this.ViewModel.IsVisibleListNewTask;
    }

    void SearchBar_EditTaskUnfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = false;
    }

    private void OnEditTaskTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListNewTask = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsEditTask.ItemsSource = this.ViewModel.ProductsWithStock;
        }
        else
        {
            ProductsEditTask.ItemsSource = this.ViewModel.ProductsWithStock.Where(x => x.Product.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    private void CollectionEditTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((ProductStock)ProductsEditTask.SelectedItem == null) return;
        this.ViewModel.ProductTask = (ProductStock)ProductsEditTask.SelectedItem;
        this.ViewModel.ReloadDerivateProductsCommand.Execute(null);
        this.ViewModel.SelectedDerivateProducts = null;
        this.ViewModel.IsVisibleListNewTask = false;
        this.ViewModel.IsSelectedProductTask = true;
        this.SearchBarEditTask.Unfocus();
    }

    async void EditTask_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.EditTaskCommand;
        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se actualizo la tarea con exito!"));
        }

    }

    //Booking and orders
    void SearchBarProductBookingOrder_Focused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListProductsBookingOrders = !this.ViewModel.IsVisibleListProductsBookingOrders;
    }

    void SearchBarProductBookingOrder_Unfocused(System.Object sender, Microsoft.Maui.Controls.FocusEventArgs e)
    {
        this.ViewModel.IsVisibleListProductsBookingOrders = false;
    }

    private void OnProductsBookingORderTextChanged(object sender, TextChangedEventArgs e)
    {
        this.ViewModel.IsVisibleListProiductsBookingOrder = true;

        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            ProductsBookingOrder.ItemsSource = this.ViewModel.Products;
        }
        else
        {
            ProductsBookingOrder.ItemsSource = this.ViewModel.Products.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower())
                || x.Id.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
        }
    }

    private void CollectionProductsBookingOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((ProductStock)ProductsBookingOrder.SelectedItem == null) return;
        this.ViewModel.LoadProductToAddBookingOrderCommand.Execute((ProductStock)ProductsBookingOrder.SelectedItem);
        this.ViewModel.IsVisibleListProiductsBookingOrder = false;
        this.SearchBarAddProductsBookingOrder.Unfocus();
    }

    async void CreateBookingOrder_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.CreateBookingOrderCommand;

        var text = this.ViewModel.IsBooking ? "la reserva" : "el pedido";

        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se creo {text} con exito!"));
        }

    }

    void OnSearchOrderTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                OrderItemsSearch.ItemsSource = this.ViewModel.BookingOrderItems;
            }
            else
            {
                OrderItemsSearch.ItemsSource = this.ViewModel.BookingOrderItems.Where(x => x.Client.ToString().ToLower().Contains(e.NewTextValue.ToLower())
                    || x.Id.ToString().ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            }
        }
        catch
        {

        }
    }

    private async void VisivilityCancelOrderItem_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (BookingOrder)button.BindingContext;

        var command = this.ViewModel.CancelOrderCommand;

        var result = await (Task<bool>)command.ExecuteAsync((BookingOrder)taskItem);


        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se cancelo la orden con exito!"));
        }
    }


    private async void VisivilityOrderItem_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (BookingOrder)button.BindingContext;

        var command = this.ViewModel.ViewOrderCommand;

        command.ExecuteAsync((BookingOrder)taskItem);

    }

    void OnSearchBookingTextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                BookingItemsSearch.ItemsSource = this.ViewModel.BookingOrderItems;
            }
            else
            {
                BookingItemsSearch.ItemsSource = this.ViewModel.BookingOrderItems.Where(x => x.Client.ToString().ToLower().Contains(e.NewTextValue.ToLower())
                    || x.Id.ToString().ToLower().Contains(e.NewTextValue.ToLower())).ToList();
            }
        }
        catch
        {

        }
    }


    private async void VisivilityCancelBookingItem_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (BookingOrder)button.BindingContext;

        var command = this.ViewModel.CancelBookingCommand;

        var result = await (Task<bool>)command.ExecuteAsync((BookingOrder)taskItem);


        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se cancelo la reserva con exito!"));
        }
    }


    private async void VisivilityBookingItem_Clicked(object sender, EventArgs e)
    {

        var button = ((Button)sender);
        var taskItem = (BookingOrder)button.BindingContext;

        var command = this.ViewModel.ViewBookingCommand;

        command.ExecuteAsync((BookingOrder)taskItem);

    }

    async void VisivilityConfirmBookingItem_Clicked(object sender, EventArgs e)
    {
        var command = this.ViewModel.ConfirmBookingCommand;

        var result = await (Task<bool>)command.ExecuteAsync(null);

        if (result)
        {
            await popupNavigation.PushAsync(new SuccessMessagePage(this.popupNavigation, $"Se confirmo la reserva y se creo el pedido con exito!"));
            
            // Navegar de vuelta a la lista de reservas
            this.ViewModel.ChangeViewCommand.Execute("ListBooking");
        }

    }
}