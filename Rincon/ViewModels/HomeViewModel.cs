//using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using iText.Bouncycastleconnector.Logs;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using Rincon.Common.ViewModels;
using Rincon.Models;

using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Home logic
    /// </summary>
    public partial class HomeViewModel : BaseViewModel
    {
        #region Properties

        //IFileSaver fileSaver;

        ///// <summary>
        ///// User logged
        ///// </summary>
        [ObservableProperty]
        private User user;

        //IFileSaver fileSaver;

        ///// <summary>
        ///// Cards
        ///// </summary>
        [ObservableProperty]
        private ObservableCollection<CardStock> cards;

        ///// <summary>
        ///// Home View
        ///// </summary>
        [ObservableProperty]
        private bool isHomeView;

        ///// <summary>
        ///// Magement Stock View
        ///// </summary>
        [ObservableProperty]
        private bool isMagementStockView;

        ///// <summary>
        ///// Tasks View
        ///// </summary>
        [ObservableProperty]
        private bool isTasksView;

        ///// <summary>
        ///// Orders View
        ///// </summary>
        [ObservableProperty]
        private bool isOrdersView;

        ///// <summary>
        ///// History View
        ///// </summary>
        [ObservableProperty]
        private bool isHistoryView;

        ///// <summary>
        ///// Add Product View
        ///// </summary>
        [ObservableProperty]
        private bool isAddProductView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isAddStockView;
        
        
        ///// <summary>
        ///// New movement View
        ///// </summary>
        [ObservableProperty]
        private bool isNewMovementView;


        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isInventoryView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isInventoryEditView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isInventoryEditProductView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isConfigurationView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isManagementOperatorsView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isCheckStockView;

        ///// <summary>
        ///// Edit stock View
        ///// </summary>
        [ObservableProperty]
        private bool isEditStockView;

        ///// <summary>
        ///// Add stock View
        ///// </summary>
        [ObservableProperty]
        private bool isUserLogin;

        ///// <summary>
        ///// Is Tirante
        ///// </summary>

        private bool isTiranteSelect;

        public bool IsTiranteSelect
        {
            get { return isTiranteSelect; }
            set
            {
                if (SetProperty(ref isTiranteSelect, value))
                {
                    OnPropertyChanged(nameof(IsMeasureSelect));
                    OnPropertyChanged(nameof(IsMachimbreOption));
                    OnPropertyChanged(nameof(IsDeckOption));
                    ReloadStates();
                }
            }
        }

        ///// <summary>
        ///// Is Polin
        ///// </summary>
        private bool isPolinSelect;

        public bool IsPolinSelect
        {
            get { return isPolinSelect; }
            set
            {
                if (SetProperty(ref isPolinSelect, value))
                {
                    OnPropertyChanged(nameof(IsMachimbreOption));
                    OnPropertyChanged(nameof(IsDeckOption));
                    ReloadStates();
                }
            }
        }

        ///// <summary>
        ///// Is Polin
        ///// </summary>
        private bool isMedioPolinSelect;

        public bool IsMedioPolinSelect
        {
            get { return isMedioPolinSelect; }
            set
            {
                if (SetProperty(ref isMedioPolinSelect, value))
                {
                    OnPropertyChanged(nameof(IsMachimbreOption));
                    OnPropertyChanged(nameof(IsDeckOption));
                    ReloadStates();
                }
            }
        }

        ///// <summary>
        ///// Is Tabla
        ///// </summary>

        private bool isTablaSelect;

        public bool IsTablaSelect
        {
            get { return isTablaSelect; }
            set
            {
                if (SetProperty(ref isTablaSelect, value))
                {
                    OnPropertyChanged(nameof(IsMeasureSelect));
                    OnPropertyChanged(nameof(IsMachimbreOption));
                    OnPropertyChanged(nameof(IsDeckOption));
                    ReloadStates();
                }
            }
        }

        public bool? IsMachimbreOption
        {
            get
            {
                return IsPolinSelect || IsTablaSelect || IsMedioPolinSelect;
            }
        }

        public bool? IsDeckOption
        {
            get
            {
                return IsTablaSelect;
            }
        }
        
        public bool? IsMeasureSelect
        {
            get
            {
                return IsTiranteSelect || IsTablaSelect;
            }
        }

        ///// <summary>
        ///// Cards
        ///// </summary>
        [ObservableProperty]
        private List<string> states;


        ///// <summary>
        ///// machimbres
        ///// </summary>
        [ObservableProperty]
        private List<string> machimbres;

        ///// <summary>
        ///// Check stock quantity pages
        ///// </summary>
        [ObservableProperty]
        private List<int> checkStockQuantityPages;

        ///// <summary>
        ///// Thickness
        ///// </summary>
        [ObservableProperty]
        private double thickness;

        ///// <summary>
        ///// Thickness
        ///// </summary>
        [ObservableProperty]
        private double diameter;

        ///// <summary>
        ///// Length
        ///// </summary>
        [ObservableProperty]
        private double length;

        ///// <summary>
        ///// Width
        ///// </summary>
        [ObservableProperty]
        private double width;

        ///// <summary>
        ///// Location
        ///// </summary>
        [ObservableProperty]
        private string location;

        ///// <summary>
        ///// Comments
        ///// </summary>
        [ObservableProperty]
        private string comments;

        ///// <summary>
        ///// Supplier
        ///// </summary>
        [ObservableProperty]
        private string supplier;

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        private bool isMachimbre; 

        public bool IsMachimbre
        {
            get { return isMachimbre; }
            set
            {
                if (SetProperty(ref isMachimbre, value))
                {
                    OnPropertyChanged(nameof(IsMachimbre));
                    if (value)
                    {
                        this.IsDeck = false;
                    }
                }
            }
        }

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        private bool isDeck; 

        public bool IsDeck
        {
            get { return isDeck; }
            set
            {
                if (SetProperty(ref isDeck, value))
                {
                    OnPropertyChanged(nameof(IsDeck));

                    if (value)
                    {
                        this.IsMachimbre = false;
                    }
                }
            }
        }

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        [ObservableProperty]
        private bool isVisibleListMachimbres;

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        [ObservableProperty]
        private bool isVisibleListStates;

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        [ObservableProperty]
        private bool isVisibleListProducts;

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        [ObservableProperty]
        private string selectedMachimbre;

        ///// <summary>
        ///// IsMachimbre
        ///// </summary>
        [ObservableProperty]
        private string selectedState;

        ///// <summary>
        ///// ProductCode
        ///// </summary>
        [ObservableProperty]
        private string productCode;

        ///// <summary>
        ///// Products to add
        ///// </summary>
        [ObservableProperty]
        private ObservableCollection<ProductStock> productsStock;

        ///// <summary>
        ///// Products to add
        ///// </summary>
        [ObservableProperty]
        private ObservableCollection<ProductStock> stock;

        ///// <summary>
        ///// Products to add
        ///// </summary>
        [ObservableProperty]
        private Product selectedProduct;

        ///// <summary>
        ///// List of products
        ///// </summary>
        [ObservableProperty]
        private List<Product> products;

        ///// <summary>
        ///// Selected product to add
        ///// </summary>

        private Product selectedProductToAdd;

        public Product SelectedProductToAdd
        {
            get { return selectedProductToAdd; }
            set
            {
                if (SetProperty(ref selectedProductToAdd, value))
                {
                    OnPropertyChanged(nameof(SelectedProductToAdd));

                    if (value != null)
                    {
                        this.LoadProductToAddCommand.Execute(value);
                    }

                }
            }
        }

        ///// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private bool isVisibleListAddStock;

        //// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private string productLocationEdit;

        //// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private string productCommentEdit;

        //// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private string produSupplierEdit;

        //// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private int selectedIndexSeachStock;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private Note newNote;


        //// <summary>
        ///// Notes
        ///// </summary>
        [ObservableProperty]
        private ObservableCollection<Note> notes;


        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string nameConfigurations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string passwordConfigurations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string repeatPasswordConfigrations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string selectedQuestionConfigurations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string responseConfigurations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string confirmUserConfigurations;

        //// <summary>
        ///// New note
        ///// </summary>
        [ObservableProperty]
        private string confirmPasswordConfigurations;

        [ObservableProperty]
        private List<string> questions;

        [ObservableProperty]
        private bool isVisibleListQuestions;

        [ObservableProperty]
        private bool confirmUserChanges;


        //// <summary>
        ///// Notes
        ///// </summary>
        [ObservableProperty]
        private ObservableCollection<Operator> operators;

        [ObservableProperty]
        private bool isEditOperatorView;

        [ObservableProperty]
        private bool isAddOperatorView;

        [ObservableProperty]
        private bool isOperatorsView;

        [ObservableProperty]
        private bool anyOperators;


        [ObservableProperty]
        private string operatorName;

        [ObservableProperty]
        private string operatorLastName;

        [ObservableProperty]
        private string operatorPin;

        [ObservableProperty]
        private string repeatOperatorPin;

        [ObservableProperty]
        private bool validateAddOperator;

        [ObservableProperty]
        private bool validateEditOperator;

        [ObservableProperty]
        private bool validateDeleteOperator;

        [ObservableProperty]
        private Operator operatorToDelete;

        [ObservableProperty]
        private Operator operatorToUpdate;

        [ObservableProperty]
        private string okMovementText;

        [ObservableProperty]
        private int quantityMovement;

        [ObservableProperty]
        private bool isSale;

        [ObservableProperty]
        private bool isChangeOfState;

        [ObservableProperty]
        private bool isLoss;

        [ObservableProperty]
        private string saleClient;

        [ObservableProperty]
        private string lossMotive;

        [ObservableProperty]
        private ObservableCollection<Product> dependingProducts;

        [ObservableProperty]
        private Product selectedDependingProducts;


        [ObservableProperty]
        private ProductStock productMovement;
        
        [ObservableProperty]
        private bool isSelectedProductMovement;

        [ObservableProperty]
        private bool isVisibleListNewMovement;

        [ObservableProperty]
        private ObservableCollection<ProductStock> productsWithStock;

        [ObservableProperty]
        private ObservableCollection<Movement> movements;

        [ObservableProperty]
        private ObservableCollection<Movement> movementsFilter;

        [ObservableProperty]
        private MovementTypes selectedFilterMovementType;
        
        [ObservableProperty]
        private Product selectedFilterMovementProduct;
        
        [ObservableProperty]
        private DateTime selectedFilterMovementDate;

        [ObservableProperty]
        private ObservableCollection<MovementTypes> movementTypes;

        [ObservableProperty]
        private ObservableCollection<Product> productsMovementsFilter;


        [ObservableProperty]
        private bool isVisibleListEditStock;

        [ObservableProperty]
        private ProductStock productEditStock;

        [ObservableProperty]
        private bool isSelectedProductEditStock;

        [ObservableProperty]
        private int quantityEditStock;


        [ObservableProperty]
        private bool isVisibleListNewTask;
        [ObservableProperty]
        private bool isSelectedProductTask;
        [ObservableProperty]
        private ProductStock productTask;
        [ObservableProperty]
        private bool isNoPriorityTask;
        [ObservableProperty]
        private bool isYesPriorityTask;
        [ObservableProperty]
        private int quantityTask;
        [ObservableProperty]
        private int quantityEditTask;
        [ObservableProperty]
        private Product selectedDerivateProducts;
        [ObservableProperty]
        private ObservableCollection<Product> derivateProducts;
        [ObservableProperty]
        private bool isCreateTaskView;
        [ObservableProperty]
        private bool isHistoryOfTaskView;
        [ObservableProperty]
        private bool isVisibleListProductsDerivateTask;
        [ObservableProperty]
        private string commentsTask;
        [ObservableProperty]
        private ObservableCollection<TaskItem> taskItems;
        [ObservableProperty]
        private TaskItem selectedTaskItem;
        [ObservableProperty]
        private bool isTaskItemEditView;
        [ObservableProperty]
        private bool isTaskItemView;


        [ObservableProperty]
        private bool isVisibleCreateBookingOrderView;
        [ObservableProperty]
        private bool isVisibleListBookingsView;
        [ObservableProperty]
        private bool isVisibleListOrdersView;
        [ObservableProperty]
        private bool isVisibleListProductsBookingOrders;
        ///// <summary>
        ///// Is visible list add stock
        ///// </summary>
        [ObservableProperty]
        private bool isVisibleListProiductsBookingOrder;
        [ObservableProperty]
        private ObservableCollection<ProductStock> productsBookingOrder;
        private Product selectedProductToAddBookingOrder;

        public Product SelectedProductToAddBookingOrder
        {
            get { return selectedProductToAddBookingOrder; }
            set
            {
                if (SetProperty(ref selectedProductToAddBookingOrder, value))
                {
                    OnPropertyChanged(nameof(SelectedProductToAddBookingOrder));

                    if (value != null)
                    {
                        this.LoadProductToAddBookingOrderCommand.Execute(value);
                    }

                }
            }
        }

        [ObservableProperty]
        private int quantityBookingOrder;
        [ObservableProperty]
        private string clientBookingOrder;
        [ObservableProperty]
        private DateTime dateBookingOrder;
        [ObservableProperty]
        private string addressBookingOrder;
        [ObservableProperty]
        private string commentsBookingOrder;
        [ObservableProperty]
        private bool isShipmentTask;
        [ObservableProperty]
        private bool isNoShipmentTask;
        [ObservableProperty]
        private bool isBooking;
        [ObservableProperty]
        private bool isOrder;
        [ObservableProperty]
        private string phoneBookingOrder;
        [ObservableProperty]
        private ObservableCollection<BookingOrder> bookingOrderItems;
        [ObservableProperty]
        private BookingOrder selectedBookingOrder;
        [ObservableProperty]
        private ObservableCollection<Order> orderItems;
        [ObservableProperty]
        private bool isVisibleOrderView;
        [ObservableProperty]
        private bool isVisibleBookingView;
        [ObservableProperty]
        private ObservableCollection<Booking> bookingItems;
        #endregion

        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public HomeViewModel(IServiceProvider provider /*,IFileSaver fileSaver*/) : base(provider)
        {
            //this.fileSaver = fileSaver;

            this.Questions = new List<string>
            {
                new ("Cual es el nombre de tu cancion favorita?"),
                new ("Cual es tu juego favorito?"),
                new ("Cual es el nombre de tu primer mascota?"),
                new ("Cual es el nombre del barrio en que naciste?")
            };


            this.SelectedFilterMovementDate = DateTime.Now;

            this.SelectedQuestionConfigurations = this.Questions.First();
        }

        public override Task Initialize()
        {
            return Task.Run(() =>
            {
                this.IsBusy = true;

                this.IsUserLogin = false;

               MainThread.BeginInvokeOnMainThread(() =>
               {
                   this.LoadDataCommand.Execute(null);
               });

                this.IsBusy = false;
            });
        }

        private async Task RefreshBar()
        {
            var stock = await this.DataService.LoadStockAsync();

            this.ProductsWithStock = new ObservableCollection<ProductStock>(stock);

            if (stock != null && stock.Any())
            {
                this.Stock = new ObservableCollection<ProductStock>(stock);
            }


            this.CheckStockQuantityPages = new List<int>();

            if (this.Stock != null && this.Stock.Any())
            {
                var count = 0;

                this.Cards = new ObservableCollection<CardStock>();

                this.Stock.ToList().ForEach(product =>
                {

                    product.Product = this.Products.Find(x => x.Id == product.Id);

                    this.Cards.Add(
                        new CardStock
                        {
                            Id = product.Id,
                            Description = product.Product.Description,
                            StockAvailable = product.Available,
                            StockReserved = product.Reserved,
                            Icon = $"{product.Product.WoodState.ToString().ToLower()}.png"
                        });

                    if (count == 0)
                    {
                        this.CheckStockQuantityPages.Add(1);
                    }
                    else if (count % 10 == 0)
                    {
                        this.CheckStockQuantityPages.Add((count / 10) + 1);
                    }

                    count++;
                });
            }
        }

        public ICommand LoadDataCommand => new Command(async () =>
        {
            try
            {
                this.User = await this.DataService.LoadLocalUserAsync();
                if (this.User == null)
                {
                    await this.NavigationService.Navigate<LoginViewModel>();
                }
                else
                {
                    this.IsUserLogin = this.User != null;
                    this.ChangeViewCommand.Execute("Home");
                }


                this.IsBusy = true;

                this.States = new List<string>();

                foreach (var item in Enum.GetValues(typeof(WoodState)))
                {
                    this.States.Add(item.ToString());
                }

                this.SelectedState = this.States.First();

                this.Machimbres = new List<string>();

                foreach (var item in Enum.GetValues(typeof(Machimbre)))
                {
                    this.Machimbres.Add(item.ToString());
                }

                this.SelectedMachimbre = this.Machimbres.First();

                this.Products = await this.DataService.LoadProductsAsync();

                await LoadNotes();

                await LoadTaskItems();

                await LoadBookingsItems();

                await RefreshBar();
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                //Error
            }
            finally
            {
                this.IsBusy = false;
            }

        });

        public ICommand LogoutViewCommand => new Command(async () =>
        {

            await NotificationService.ConfirmAsync("Cerrar Sesion", "¿Está seguro que desea cerrar sesion?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    await this.DataService.DeleteLocalUserAsync();

                    await this.NavigationService.Navigate<LoginViewModel>();
                }
            });
        });

        public ICommand ChangeViewCommand => new Command<string>(async (view) =>
        {
            switch (view)
            {
                case "Home":
                    this.IsHomeView = true;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "MagementStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = true;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "Tasks":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = true;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "CreateTask":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = true;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    this.IsSelectedProductTask = false;

                    await ReloadProductWithStock();

                    this.IsSale = true;
                    this.OkMovementText = "Confirmar Movimiento";
                    break;
                case "ManagementTasks":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = true;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await ReloadProductWithStock();
                    break;
                case "EditTaskItem":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = true;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "DetailTaskItem":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = true;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "Orders":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = true;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "History":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = true;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await this.ReloadMovementsAsync();
                    break;
                case "AddProduct":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = true;
                    this.IsAddStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsTiranteSelect = true;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "AddStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = true;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsVisibleListAddStock = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await ReloadProductWithStock();
                    break;
                case "Inventory":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = true;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.SelectedIndexSeachStock = 1;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "CheckStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = true;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.SelectedIndexSeachStock = 1;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "EditStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = true;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    this.QuantityEditStock = 0;
                    await ReloadProductWithStock();
                    break;
                case "EditProductInventory":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = true;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "EditProductDetaildInventory":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = true;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "Configuration":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = true;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    break;
                case "Operators":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = true;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;
                    break;
                case "NewMovement":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = true;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await ReloadProductWithStock();

                    this.IsSale = true;
                    this.OkMovementText = "Confirmar Movimiento";
                    break;
                case "CreateBookingOrder":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = true;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    this.IsNoShipmentTask = true;
                    this.IsBooking = true;
                    await ReloadProductWithStock();
                    break;
                case "ListBooking":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = true;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await this.LoadBookingsItems();
                    break;
                case "ListOrders":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = true;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = false;

                    await this.LoadOrdersItems();
                    break;
                case "Order":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = true;
                    this.IsVisibleBookingView = false;
                    break;
                case "Booking":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    this.IsInventoryView = false;
                    this.IsCheckStockView = false;
                    this.IsEditStockView = false;
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.IsConfigurationView = false;
                    this.IsManagementOperatorsView = false;
                    this.IsNewMovementView = false;
                    this.IsCreateTaskView = false;
                    this.IsHistoryOfTaskView = false;
                    this.IsTaskItemView = false;
                    this.IsTaskItemEditView = false;
                    this.IsVisibleCreateBookingOrderView = false;
                    this.IsVisibleListBookingsView = false;
                    this.IsVisibleListOrdersView = false;
                    this.IsVisibleOrderView = false;
                    this.IsVisibleBookingView = true;
                    break;
            }
        });


        #region Addproduct

        public ICommand CancelAddProductCommand => new Command(async () =>
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No",  (response) =>
            {
                if (response)
                {
                    ClearViewAddProduct();
                    this.ChangeViewCommand.Execute("Home");
                }
            });

        });

        public ICommand SelectStateCommand => new Command(() =>
        {
            this.IsVisibleListStates = !this.IsVisibleListStates;

        });


        public ICommand SelectProductMocvementCommand => new Command(() =>
        {
            this.IsVisibleListProducts = !this.IsVisibleListProducts;

        });

        public ICommand SelectMachimbreCommand => new Command(() =>
        {
            this.IsVisibleListMachimbres = !this.IsVisibleListMachimbres;

        });

        public ICommand SaveProductCommand => new Command(async () =>
        {
            await NotificationService.ConfirmAsync("Guardar", "¿Está seguro que desea guardar el producto?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(this.ProductCode))
                        {
                            await NotificationService.NotifyAsync("Falta completar campos", "Código no pueden ser vacio", "Cerrar");
                            return;
                        }
                        else if (this.IsTiranteSelect)
                        {
                            if (this.Thickness == 0 || this.Width == 0 || this.Length == 0)
                            {
                                await NotificationService.NotifyAsync("Falta completar campos", "Grosor, Ancho y Largo  no pueden ser 0", "Cerrar");
                                return;
                            }
                        }
                        else if (this.IsPolinSelect)
                        {
                            if (this.Diameter == 0 || this.Length == 0)
                            {
                                await NotificationService.NotifyAsync("Falta completar campos", "El diametro y Largo no pueden ser 0", "Cerrar");
                                return;
                            }
                        }
                        else if (this.IsMedioPolinSelect)
                        {
                            if (this.Diameter == 0 || this.Length == 0)
                            {
                                await NotificationService.NotifyAsync("Falta completar campos", "El diametro y Largo no pueden ser 0", "Cerrar");
                                return;
                            }
                        }
                        else if (this.IsTablaSelect)
                        {
                            if (this.Thickness == 0 || this.Width == 0 || this.Length == 0)
                            {
                                await NotificationService.NotifyAsync("Falta completar campos", "Grosor, Ancho y Largo  no pueden ser 0", "Cerrar");
                                return;
                            }
                        }


                        var product = new Product()
                        {
                            Comment = this.Comments,
                            Diameter = this?.Diameter,
                            Length = this?.Length,
                            Location = this?.Location,
                            Id = this.ProductCode,
                            Supplier = this?.Supplier,
                            Thickness = this?.Thickness,
                            Width = this?.Width,
                            Machimbre = this.IsMachimbre,
                            Deck = this.IsDeck,
                            ProductType = this.IsTiranteSelect ? ProductType.Tirante : this.IsPolinSelect ? ProductType.Polin : this.IsTablaSelect ? ProductType.Tabla : ProductType.MedioPolin,
                            WoodState =  (WoodState)Enum.Parse(typeof(WoodState),this.SelectedState),
                            MachimbreSate = (Machimbre)Enum.Parse(typeof(Machimbre),this.SelectedMachimbre),
                            Description = this.IsPolinSelect ? $"{this.Diameter} x {this.Length}" : $"{this.Thickness} x {this.Length} x {this.Width}",
                            DependOf = this.SelectedProduct != null ? this.SelectedProduct.Id : null
                        };

                        var result = await this.DataService.InsertOrUpdateItemsAsync<Product>(product);

                        if (result > 0)
                        {
                            this.Products = this.Products ?? new List<Product>();
                            this.Products.Add(product);
                        }
                        

                    }
                    catch
                    {
                        await NotificationService.NotifyAsync("Error", "Hubo un error al crear el product. Vuleva a intentar.", "Cerrar");
                        return;
                    }

                    await NotificationService.NotifyAsync("Creado", "Ha creado un nuevo producto", "Volver");

                    ClearViewAddProduct();

                    return;
                }
            });
        });

        [RelayCommand]
        private void ReloadStates()
        {
            this.States = new List<string>();

            if(this.IsPolinSelect || this.IsMedioPolinSelect)
            {
                foreach (var item in Enum.GetValues(typeof(WoodState)))
                {
                    if (item.ToString() == "Fresco" || item.ToString() == "Tratado")
                    {
                        this.States.Add(item.ToString());
                    }
                }
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(WoodState)))
                {
                    this.States.Add(item.ToString());
                }
            }
            this.SelectedState = this.States.First();
        }

        private void ClearViewAddProduct()
        {
            this.Comments = string.Empty;
            this.Diameter = 0;
            this.Length = 0;
            this.Location = string.Empty;
            this.ProductCode = string.Empty;
            this.Supplier = string.Empty;
            this.Thickness = 0;
            this.Width = 0;
            this.IsMachimbre = false;
            this.IsPolinSelect = false;
            this.IsTablaSelect = false;
            this.IsTiranteSelect = true;
        }
        #endregion


        #region AddStock

        public ICommand LoadProductToAddCommand => new Command<Product>(async (product) =>
        {
            this.ProductsStock ??= new ObservableCollection<ProductStock>();

            if (product != null)
            {
                var exist = false;
                
                foreach (var item in this.ProductsStock)
                {
                    if (item.Product == product) 
                    {
                        exist = true;
                    }

                }

                if (!exist)
                {
                    this.ProductsStock.Add(new ProductStock()
                    {
                        Id = product.Id,
                        Product = product,
                        Quantity = 1,
                    });
                }
                else 
                {
                    await NotificationService.NotifyAsync("Error", "El producto ya fue agregado", "Cerrar");
                }

                this.SelectedProductToAdd = null;

            }
        });

        public ICommand RemoveProductToAddedCommand => new Command<ProductStock>((product) =>
        {
            this.ProductsStock.Remove(product);
        });

        public ICommand CancelAddStockCommand => new Command(async () =>
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    ClearViewAddStock();
                    this.ChangeViewCommand.Execute("Home");
                }
            });
        });

        public ICommand OkAddStockCommand => new Command(async () =>
       {
           try
           {
               if (!this.ProductsStock.Any())
               {
                   await NotificationService.NotifyAsync("Error", "No se a cargado ningun producto", "Cerrar");
                   return;
               }

               this.ProductsStock.ToList().ForEach(x =>
               {
                   var existStock = this.Stock?.Where(y => y.Id == x.Id)?.FirstOrDefault();
                   if (existStock != null)
                   {
                       x.Quantity = x.Quantity + existStock.Quantity;
                   }
               });
               
               var result = await this.DataService.InsertOrUpdateStockAsync(this.ProductsStock.ToList());

           }
           catch (Exception ex)
           {
               await NotificationService.NotifyAsync("Error", "Hubo un error al agregar stock. Vuleva a intentar.", "Cerrar");
               return;
           }

           await NotificationService.NotifyAsync("Operacion Exitosa", "Stock actualizado", "Volver");

           ClearViewAddStock();

           await RefreshBar();

           return;
       });

        public ICommand SavePDFCommand => new Command(async () =>
        {
            try
            {  

               var result = await this.CreateSavePDFAsync(this.ProductsStock.ToList());

                if (result) 
                {
                    await NotificationService.NotifyAsync("Operacion Exitosa", "PDF guardado", "Volver");

                    return;
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al guardar el PDF. Vuleva a intentar.", "Cerrar");
                return;
            }
        });

        public ICommand SelectProductCommand => new Command(() =>
        {
            this.IsVisibleListAddStock = !this.IsVisibleListAddStock;

        });


        private void ClearViewAddStock()
        {
            this.ProductsStock = null;
        }

        #endregion


        #region EditStock

        [RelayCommand]
        private async Task<bool> EditStock()
        {
            try
            {
                if (this.ProductEditStock == null) 
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto", "Cerrar");
                    return false;
                }

                if (this.ProductEditStock.Reserved + this.ProductEditStock.Process > this.QuantityEditStock)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad de stock total no puede ser menor a la cantidad reservada y procesada", "Cerrar");
                    return false;
                }

                this.ProductEditStock.Quantity = this.QuantityEditStock;

                var result = await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(this.ProductEditStock);

                if (result > 0)
                {
                    this.ProductEditStock = null;
                    this.QuantityEditStock = 0;
                    this.IsSelectedProductEditStock = false;
                }
                else
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                await RefreshBar();
                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }


        [RelayCommand]
        private async Task CancelEditStock()
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    this.ChangeViewCommand.Execute("Home");
                }
            });
        }
        #endregion


        #region Movements

        [RelayCommand]
        private async Task CancelNewMovement() 
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    this.ChangeViewCommand.Execute("Home");
                }
            });
        }

        [RelayCommand]
        private async Task<bool> NewMovement()
        {
            try
            {
                if (this.ProductMovement == null)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto", "Cerrar");
                    return false;
                }

                if (this.QuantityMovement == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser 0", "Cerrar");
                    return false;
                }

                if (this.ProductMovement.Quantity < this.QuantityMovement)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser mayor a la cantidad en stock", "Cerrar");
                    return false;
                }

                this.ProductMovement.Quantity = this.ProductMovement.Quantity - this.QuantityMovement;
                
                if(this.IsChangeOfState)
                {
                    if (this.SelectedDependingProducts == null)
                    {
                        await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto derivado", "Cerrar");
                        return false;
                    }

                    var productStock = (await this.DataService.LoadStockAsync()).Where(x => x.Id == this.SelectedDependingProducts.Id).FirstOrDefault();

                    if (productStock != null) 
                    {
                        productStock.Quantity = productStock.Quantity + this.QuantityMovement;
                    }
                    else
                    {
                        productStock = new ProductStock()
                        {
                            Id = this.SelectedDependingProducts.Id,
                            Product = this.SelectedDependingProducts,
                            Quantity = this.QuantityMovement
                        };
                    }

                    var resultAddStock = await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(productStock);

                    if (resultAddStock == 0)
                    {
                        await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                        return false;
                    }

                }

                var resultQuitStock = await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(this.ProductMovement);

                var movement = new Movement
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.Now,
                    Quantity = this.QuantityMovement,
                    MovementType = this.IsSale ? MovementType.Venta.ToString() : this.IsChangeOfState ? MovementType.Procesado.ToString() : MovementType.Perdida.ToString(),
                    ProductName = $"{this.ProductMovement.Product.Id} - {this.ProductMovement.Product.Description}",
                };

                if (resultQuitStock > 0)
                {
                    this.ProductMovement = null;
                    this.QuantityMovement = 0;
                    this.IsSale = true;
                    this.IsChangeOfState = false;
                    this.IsLoss = false;
                    this.IsSelectedProductMovement = false;
                }
                else
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                var saveMovement = await this.DataService.InsertOrUpdateItemsAsync<Movement>(movement);

                if (saveMovement == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                await RefreshBar();
                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Atencion", "Hubo un error al realizar el movimiento. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }
        
        [RelayCommand]
        private async Task ReloadProductWithStock()
        {
            try
            {
                var productsStock = await this.DataService.LoadStockAsync();

                if (productsStock != null && productsStock.Any())
                {
                    productsStock = productsStock.Where(x => x.Quantity > 0).ToList();

                    productsStock.ForEach(x =>
                    {
                        x.Product = this.Products.Find(y => y.Id == x.Id);
                    });

                    this.ProductsWithStock = new ObservableCollection<ProductStock>(productsStock);
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task ReloadDependingProducts()
        {
            try
            {
                var depending = this.Products.Where(x => x.DependOf == this.ProductMovement.Id).ToList();

                this.DependingProducts = new ObservableCollection<Product>(depending);
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task ReloadDerivateProducts()
        {
            try
            {
                var derivate = this.Products.Where(x => x.DependOf == this.ProductTask.Id).ToList();

                this.DerivateProducts = new ObservableCollection<Product>(derivate);
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task ReloadMovementsAsync()
        {
            try
            {
                var movements = await this.DataService.LoadMovementsAsync();
                this.Movements = new ObservableCollection<Movement>(movements);
                this.MovementsFilter = new ObservableCollection<Movement>(this.Movements);

                this.ProductsMovementsFilter = new ObservableCollection<Product>(await this.DataService.LoadProductsAsync());

                var movementTypeList = Enum.GetValues(typeof(MovementType)).Cast<MovementType>().Select(x => new MovementTypes { Name = x.ToString() }).ToList();
                this.MovementTypes = new ObservableCollection<MovementTypes>(movementTypeList);

            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task ClearFiltersMovementsAsync()
        {
            try
            {
                this.MovementsFilter = new ObservableCollection<Movement>(this.Movements);

                this.SelectedFilterMovementDate = new DateTime(2023, 1, 1);

                this.SelectedFilterMovementProduct = null;

                this.SelectedFilterMovementType = null;

            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task ApplyFiltersMovementsAsync()
        {
            try
            {
                this.MovementsFilter = new ObservableCollection<Movement>(this.Movements);

                if (this.SelectedFilterMovementProduct != null)
                {
                    this.MovementsFilter = new ObservableCollection<Movement>(this.MovementsFilter.Where(x => x.ProductName == $"{this.SelectedFilterMovementProduct.Id} - {this.SelectedFilterMovementProduct.Description}"));
                }

                if (this.SelectedFilterMovementType != null && this.SelectedFilterMovementType.Name.ToLower() != "todos")
                {
                    this.MovementsFilter = new ObservableCollection<Movement>(this.MovementsFilter.Where(x => x.MovementType == this.SelectedFilterMovementType.Name));
                }

                if (this.SelectedFilterMovementDate.Year != 2023)
                {
                    this.MovementsFilter = new ObservableCollection<Movement>(this.MovementsFilter.Where(x => x.Date.Date.Year == this.SelectedFilterMovementDate.Date.Year && x.Date.Date.Month == this.SelectedFilterMovementDate.Date.Month && x.Date.Date.Day == this.SelectedFilterMovementDate.Date.Day));
                }

            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar los productos. Vuleva a intentar.", "Cerrar");
            }
        }

        #endregion


        #region Inventario
        public ICommand LBProductDetaildCommand => new Command<CardStock>(async (card) =>
        {
            try
            {
                var product = this.Products.Where(x => x.Id == card.Id).FirstOrDefault();

                if (product != null)
                {
                    this.SelectedProduct = product;
                    this.ChangeViewCommand.Execute("EditProductInventory");
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al actualizar el producto. Vuleva a intentar.", "Cerrar");
            }
        });

        public ICommand OkDeleteProductCommand => new Command<Product>(async (product) =>
        {
            try
            {

                var result = await this.DataService.DeleteItemAsync<Product>(product);

                if (result > 0)
                {

                    var productStock = this.Stock.Where(x => x.Product.Id == product.Id).First();

                    var cardStock = this.Cards.Where(x=>x.Id==product.Id).First();
                    await this.DataService.DeleteItemAsync<ProductStock>(productStock);

                    this.Products.Remove(product);
                    this.Cards.Remove(cardStock);
                    this.Stock.Remove(productStock);
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al eliminar el producto. Vuleva a intentar.", "Cerrar");
             }
        });

        public ICommand UpdateProductCommand => new Command<Product>(async (product) =>
        {
            try
            {

                var result = await this.DataService.InsertOrUpdateItemsAsync<Product>(product);

                if (result > 0)
                {

                    this.Stock.ToList().ForEach(x =>
                    {
                        if (x.Product.Id == product.Id)
                        {
                            x.Product = product;
                        }
                    });
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al actualizar el producto. Vuleva a intentar.", "Cerrar");
            }
        });
        #endregion


        #region Notas

        [RelayCommand]
        private async Task LoadNotes()
        {
            try
            {
                var notes = await this.DataService.LoadNotesAsync();

                if(notes!=null && notes.Any()) 
                {
                    this.Notes = new ObservableCollection<Note>(notes);
                    OnPropertyChanged(nameof(Notes));
                } 
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar las notas. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task<bool> AddedNote(object[] note) 
        {
            try
            {
                if (note[2] == null)
                {
                    this.NewNote = new Note
                    {
                        UserName = this.User.Name,
                        CreatedAt = DateTime.Now,
                        Id = Guid.NewGuid(),
                        Title = note[0].ToString(),
                        Content = note[1].ToString()
                    };
                }
                else 
                {
                    this.NewNote = (Note)note[2];
                    this.NewNote.CreatedAt = DateTime.Now;
                }

                var result = await this.DataService.InsertOrUpdateItemsAsync<Note>(this.NewNote);


                await LoadNotes();

                return true;
            }
            catch(Exception ex)
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al crear la nota. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }

        [RelayCommand]
        private async Task<bool> DeleteNoteAsync(Note note)
        {
            try
            {
                var result = await this.DataService.DeleteItemAsync<Note>(note);

                if (result > 0)
                {
                    this.Notes.Remove(note);
                }

                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al eliminar la nota. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }

        #endregion


        #region Configurations

        [RelayCommand]
        private async Task Configuration() 
        {
            this.NameConfigurations = this.User.Name;
            this.PasswordConfigurations = "";
            this.RepeatPasswordConfigrations = "";
            this.SelectedQuestionConfigurations = this.Questions.First();
            this.IsVisibleListQuestions = false;
            this.ResponseConfigurations = "";
            this.ConfirmUserConfigurations = "";
            this.ConfirmPasswordConfigurations = "";

            this.ConfirmUserChanges = false;

            this.ChangeViewCommand.Execute("Configuration");
        }

        public ICommand SelectQuestionCommand => new Command(() =>
        {
            this.IsVisibleListQuestions = !this.IsVisibleListQuestions;
        });

        [RelayCommand]
        private async Task CancelConfiguration()
        {
            this.ChangeViewCommand.Execute("Home");
        }

        public ICommand UpdateDataUserCommand => new Command(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(this.NameConfigurations) || string.IsNullOrEmpty(this.PasswordConfigurations) || string.IsNullOrEmpty(this.RepeatPasswordConfigrations) || string.IsNullOrEmpty(this.ResponseConfigurations) || string.IsNullOrEmpty(this.SelectedQuestionConfigurations))
                {
                    await NotificationService.NotifyAsync("Error", "Faltan completar campos", "Cerrar");
                    return;
                }

                if (this.PasswordConfigurations != this.RepeatPasswordConfigrations)
                {
                    await NotificationService.NotifyAsync("Error", "Las contraseñas no coinciden", "Cerrar");
                    return;
                }

                this.ConfirmUserChanges = true;

            }
            catch
            {

            }
        });

        [RelayCommand]
        private async Task CancelConfirmConfiguration()
        {
            this.ConfirmUserChanges = false;
        }

        [RelayCommand]
        private async Task<bool> OkConfirmConfiguration()
        {
            if (string.IsNullOrWhiteSpace(this.ConfirmUserConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("UserEmpty"), GetText("Close"));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.ConfirmPasswordConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("PasswordEmpty"), GetText("Close"));
                return false;
            }

            try
            {
                this.IsBusy = true;
                var user = await this.DataService.LoadUserAsync(this.ConfirmUserConfigurations, this.ConfirmPasswordConfigurations);

                var localUser = await this.DataService.LoadLocalUserAsync();


                if (user == null || (user.Name != localUser.Name && user.Password != localUser.Password))
                {
                    await NotificationService.NotifyAsync("Error", "Credenciales invalidas", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                user.Name = this.NameConfigurations;
                user.Password = this.PasswordConfigurations;
                user.Question = this.SelectedQuestionConfigurations;
                user.Answer = this.ResponseConfigurations;

                var result = await this.DataService.InsertOrUpdateItemsAsync<User>(user);

                if (result == 0)
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al actualizar el usuario. Por favor vuelva a intentar", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                await this.DataService.DeleteLocalUserAsync();
                await this.DataService.SaveLocalUserAsync(user);

                this.User.Name = user.Name;
                this.User.Password = user.Password;
                this.User.Question = user.Question;
                this.User.Answer = user.Answer;

                return true;
            }
            catch (Exception ex)
            {
                await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                this.IsBusy = false;
                await LogExceptionAsync(ex);
                return false;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        #endregion


        #region ManagementOperators
        [RelayCommand]
        private async Task ManagementOperators()
        {
            this.IsOperatorsView = true;
            this.IsEditOperatorView = false;
            this.IsAddOperatorView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;

            var operatorsList = await this.DataService.LoadOperatorsAsync();

            if (operatorsList != null && operatorsList.Any())
            {
                this.Operators = new ObservableCollection<Operator>(operatorsList);

            }

            this.AnyOperators = this.Operators!=null && this.Operators.Any()? true : false;

            this.ChangeViewCommand.Execute("Operators");
        }

        [RelayCommand]
        private async Task AddOperator()
        {
            this.OperatorName = "";
            this.OperatorLastName = "";
            this.OperatorPin = "";
            this.RepeatOperatorPin = "";

            this.IsAddOperatorView = true;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
        }

        [RelayCommand]
        private async Task CancelAddOperator()
        {
            this.IsAddOperatorView = false;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = true;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
        }

        [RelayCommand]
        private async Task OkAddOperator()
        {
            if (string.IsNullOrEmpty(this.OperatorName) || string.IsNullOrEmpty(this.OperatorLastName) || string.IsNullOrEmpty(this.OperatorPin) || string.IsNullOrEmpty(this.RepeatOperatorPin))
            {
                await NotificationService.NotifyAsync("Error", "Faltan completar campos", "Cerrar");
                return;
            }

            if (this.OperatorPin != this.RepeatOperatorPin)
            {
                await NotificationService.NotifyAsync("Error", "Los pines no coinciden", "Cerrar");
                return;
            }

            this.IsAddOperatorView = false;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = true;
            this.ValidateEditOperator = false;

            this.ConfirmUserConfigurations = "";
            this.ConfirmPasswordConfigurations = "";
        }

        [RelayCommand]
        private async Task CancelValidateAddOperator()
        {
            this.IsAddOperatorView = true;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
        }

        [RelayCommand]
        private async Task<bool> OkConfirmAddOperator()
        {
            if (string.IsNullOrWhiteSpace(this.ConfirmUserConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("UserEmpty"), GetText("Close"));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.ConfirmPasswordConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("PasswordEmpty"), GetText("Close"));
                return false;
            }

            try
            {
                this.IsBusy = true;
                var user = await this.DataService.LoadUserAsync(this.ConfirmUserConfigurations, this.ConfirmPasswordConfigurations);

                var localUser = await this.DataService.LoadLocalUserAsync();


                if (user == null || (user.Name != localUser.Name && user.Password != localUser.Password))
                {
                    await NotificationService.NotifyAsync("Error", "Credenciales invalidas", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                var operatorNew = new Operator
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = this.OperatorName,
                    LastName = this.OperatorLastName,
                    Pin = this.OperatorPin,
                    CreateDate = DateTime.Now,
                };

                var result = await this.DataService.InsertOrUpdateItemsAsync<Operator>(operatorNew);

                if (result > 0)
                {
                    this.Operators ??= new ObservableCollection<Operator>();
                    this.Operators.Add(operatorNew);

                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al agregar el operador. Por favor vuelva a intentar", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                this.IsAddOperatorView = false;
                this.IsEditOperatorView = false;
                this.IsOperatorsView = true;
                this.ValidateAddOperator = false;
                this.ValidateEditOperator = false;

                return true;
            }
            catch (Exception ex)
            {
                await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                this.IsBusy = false;
                await LogExceptionAsync(ex);
                return false;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task<bool> DeleteOperatorAsync()
        {
            if (string.IsNullOrWhiteSpace(this.ConfirmUserConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("UserEmpty"), GetText("Close"));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.ConfirmPasswordConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("PasswordEmpty"), GetText("Close"));
                return false;
            }

            try
            {
                this.IsBusy = true;
                var user = await this.DataService.LoadUserAsync(this.ConfirmUserConfigurations, this.ConfirmPasswordConfigurations);

                var localUser = await this.DataService.LoadLocalUserAsync();


                if (user == null || (user.Name != localUser.Name && user.Password != localUser.Password))
                {
                    await NotificationService.NotifyAsync("Error", "Credenciales invalidas", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                var result = await this.DataService.DeleteItemAsync<Operator>(this.OperatorToDelete);

                if (result > 0)
                {
                    this.Operators.Remove(this.OperatorToDelete);

                    this.IsAddOperatorView = false;
                    this.IsEditOperatorView = false;
                    this.IsOperatorsView = true;
                    this.ValidateAddOperator = false;
                    this.ValidateEditOperator = false;
                    this.ValidateDeleteOperator = false;

                    this.IsBusy = false;
                    return true;
                }
                else 
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al eliminar el operador. Vuleva a intentar.", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }
            }
            catch
            {
                this.IsBusy = false;
                await NotificationService.NotifyAsync("Error", "Hubo un error al eliminar el operador. Vuleva a intentar.", "Cerrar");
                return false;
            }
            this.IsBusy = false;
        }
        
        [RelayCommand]
        private async Task CancelDeleteOperator()
        {
            this.IsAddOperatorView = false;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = true;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
            this.ValidateDeleteOperator = false;
        }


        public ICommand ValidateDeleteOperatorAsyncCommand => new Command(async () =>
        {
            this.IsAddOperatorView = false;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
            this.ValidateDeleteOperator = true;

            this.ConfirmUserConfigurations = "";
            this.ConfirmPasswordConfigurations = "";
        });

        [RelayCommand]
        private async Task EditOperatorAsync()
        {
            this.OperatorName = this.OperatorToUpdate.Name;
            this.OperatorLastName = this.OperatorToUpdate.LastName;
            this.OperatorPin = this.OperatorToUpdate.Pin;
            this.RepeatOperatorPin = this.OperatorToUpdate.Pin;

            this.IsAddOperatorView = false;
            this.IsEditOperatorView = true;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = false;
        }

        [RelayCommand]
        private async Task OkValidateEditOperator()
        {
            if (string.IsNullOrEmpty(this.OperatorName) || string.IsNullOrEmpty(this.OperatorLastName) || string.IsNullOrEmpty(this.OperatorPin) || string.IsNullOrEmpty(this.RepeatOperatorPin))
            {
                await NotificationService.NotifyAsync("Error", "Faltan completar campos", "Cerrar");
                return;
            }

            if (this.OperatorPin != this.RepeatOperatorPin)
            {
                await NotificationService.NotifyAsync("Error", "Los pines no coinciden", "Cerrar");
                return;
            }

            this.IsAddOperatorView = false;
            this.IsEditOperatorView = false;
            this.IsOperatorsView = false;
            this.ValidateAddOperator = false;
            this.ValidateEditOperator = true;
            this.ValidateDeleteOperator = false;


            this.ConfirmUserConfigurations = "";
            this.ConfirmPasswordConfigurations = "";
        }

        [RelayCommand]
        private async Task<bool> OkEditOperatorAsync()
        {
            if (string.IsNullOrWhiteSpace(this.ConfirmUserConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("UserEmpty"), GetText("Close"));
                return false;
            }
            else if (string.IsNullOrWhiteSpace(this.ConfirmPasswordConfigurations))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("PasswordEmpty"), GetText("Close"));
                return false;
            }

            try
            {
                this.IsBusy = true;
                var user = await this.DataService.LoadUserAsync(this.ConfirmUserConfigurations, this.ConfirmPasswordConfigurations);

                var localUser = await this.DataService.LoadLocalUserAsync();


                if (user == null || (user.Name != localUser.Name && user.Password != localUser.Password))
                {
                    await NotificationService.NotifyAsync("Error", "Credenciales invalidas", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                this.OperatorToUpdate.Name = this.OperatorName;
                this.OperatorToUpdate.LastName = this.OperatorLastName;
                this.OperatorToUpdate.Pin = this.OperatorPin;

                var result = await this.DataService.InsertOrUpdateItemsAsync<Operator>(this.OperatorToUpdate);

                if (result > 0)
                {
                    this.Operators ??= new ObservableCollection<Operator>();

                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al editar el operador. Por favor vuelva a intentar", "Cerrar");
                    this.IsBusy = false;
                    return false;
                }

                this.IsAddOperatorView = false;
                this.IsEditOperatorView = false;
                this.IsOperatorsView = true;
                this.ValidateAddOperator = false;
                this.ValidateEditOperator = false;
                this.ValidateDeleteOperator = false;

                return true;
            }
            catch (Exception ex)
            {
                await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                this.IsBusy = false;
                await LogExceptionAsync(ex);
                return false;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        #endregion


        #region Tasks
        [RelayCommand]
        private async Task CancelCreateTask()
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    this.ChangeViewCommand.Execute("Home");
                }
            });
        }

        [RelayCommand]
        private async Task CancelEditTask()
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    this.ChangeViewCommand.Execute("ManagementTasks");
                }
            });
        }


        [RelayCommand]
        private async Task BackViewTask()
        {
            this.ChangeViewCommand.Execute("ManagementTasks");
        }

        [RelayCommand]
        private async Task<bool> CreateTask()
        {
            try
            {
                if (this.ProductTask == null)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto", "Cerrar");
                    return false;
                }

                if (this.QuantityTask == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser 0", "Cerrar");
                    return false;
                }

                if (this.ProductTask.Available < this.QuantityTask)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser mayor a la cantidad en stock", "Cerrar");
                    return false;
                }

                this.ProductTask.Process = this.ProductTask.Process + this.QuantityTask;

                if (this.SelectedDerivateProducts == null)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto derivado", "Cerrar");
                    return false;
                }

                var newTask = new TaskItem
                {
                    Priority = this.IsNoPriorityTask ? false : true,
                    Quantity = this.QuantityTask.ToString(),
                    CreatedAt = DateTime.Now,
                    ProductSourceId = this.ProductTask.Product.Id,
                    ProductDestinationId = this.SelectedDerivateProducts.Id,
                    Description = this.CommentsTask,
                    TaskStatus = Models.TaskStatus.Pendiente,
                    ProductSource = $"{this.ProductTask.Product.ProductType} {this.ProductTask.Product.Description} {this.ProductTask.Product.WoodState}", 
                    ProductDestination = $"{this.SelectedDerivateProducts.ProductType} {this.SelectedDerivateProducts.Description} {this.ProductTask.Product.WoodState}",
                };

                var saveTask = await this.DataService.InsertItemAsync<TaskItem>(newTask);

                if (saveTask == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al crear la tarea. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                var resultQuitStock = await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(this.ProductTask);

                if (resultQuitStock > 0)
                {
                    this.CommentsTask = string.Empty;
                    this.SelectedDerivateProducts = null;
                    this.ProductTask = null;
                    this.QuantityTask = 0;
                    this.IsNoPriorityTask = true;
                }
                else
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al cambiar el stock para el proceso. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                await RefreshBar();

                this.IsSelectedProductTask = false;
                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Atencion", "Hubo un error al crear la tarea. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }

        [RelayCommand]
        private async Task<bool> EditTask()
        {
            try
            {
                if (this.ProductTask == null)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto", "Cerrar");
                    return false;
                }

                if (this.QuantityTask == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser 0", "Cerrar");
                    return false;
                }

                if (this.ProductTask.Available + this.QuantityEditTask < this.QuantityTask)
                {
                    await NotificationService.NotifyAsync("Atencion", "La cantidad no puede ser mayor a la cantidad en stock", "Cerrar");
                    return false;
                }


                this.ProductTask.Process = this.ProductTask.Process - this.QuantityEditTask + this.QuantityTask;
               

                if (this.SelectedDerivateProducts == null)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado ningun producto derivado", "Cerrar");
                    return false;
                }


                this.SelectedTaskItem.Priority = this.IsNoPriorityTask ? false : true;
                this.SelectedTaskItem.Quantity = this.QuantityTask.ToString();
                this.SelectedTaskItem.CreatedAt = DateTime.Now;
                this.SelectedTaskItem.ProductSourceId = this.ProductTask.Product.Id;
                this.SelectedTaskItem.ProductDestinationId = this.SelectedDerivateProducts.Id;
                this.SelectedTaskItem.Description = this.CommentsTask;
                this.SelectedTaskItem.TaskStatus = Models.TaskStatus.Pendiente;
                this.SelectedTaskItem.ProductSource = $"{this.ProductTask.Product.ProductType}  {this.ProductTask.Product.Description} {this.ProductTask.Product.WoodState}";
                this.SelectedTaskItem.ProductDestination = $"{this.SelectedDerivateProducts.ProductType}  {this.SelectedDerivateProducts.Description} {this.SelectedDerivateProducts.WoodState}";

                var saveTask = await this.DataService.UpdateItemAsync<TaskItem>(this.SelectedTaskItem);

                if (saveTask == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al actualizar la tarea. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                var resultQuitStock = await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(this.ProductTask);

                if (resultQuitStock > 0)
                {
                    this.CommentsTask = string.Empty;
                    this.SelectedDerivateProducts = null;
                    this.ProductTask = null;
                    this.QuantityTask = 0;
                    this.IsNoPriorityTask = true;
                }
                else
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al cambiar el stock para el proceso. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                await RefreshBar();
                await LoadTaskItems();
                this.ChangeViewCommand.Execute("ManagementTasks");
                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Atencion", "Hubo un error al actualizar la tarea. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }

        public ICommand SelectProductDerivateTaskCommand => new Command(() =>
        {
            this.IsVisibleListProductsDerivateTask = !this.IsVisibleListProductsDerivateTask;

        });

        [RelayCommand]
        private async Task LoadTaskItems()
        {
            try
            {
                var taskItems = await this.DataService.LoadTaskItemsAsync();

                if (taskItems != null && taskItems.Any())
                {
                    this.TaskItems = new ObservableCollection<TaskItem>(taskItems);
                    OnPropertyChanged(nameof(TaskItems));
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar las notas. Vuleva a intentar.", "Cerrar");
            }
        }
        #endregion


        #region Booking and order

        public ICommand LoadProductToAddBookingOrderCommand => new Command<ProductStock>(async (product) =>
        {
            this.ProductsBookingOrder ??= new ObservableCollection<ProductStock>();

            if (product != null)
            {
                var exist = false;

                foreach (var item in this.ProductsBookingOrder)
                {
                    if (item.Product == product.Product)
                    {
                        exist = true;
                    }

                }

                if (!exist)
                {
                    this.ProductsBookingOrder.Add(new ProductStock()
                    {
                        Id = product.Id,
                        Product = product.Product,
                        Quantity = product.Quantity,
                    });
                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "El producto ya fue agregado", "Cerrar");
                }

                this.SelectedProductToAddBookingOrder = null;

            }
        });

        public ICommand RemoveProductToAddedBookingOrderCommand => new Command<ProductStock>((product) =>
        {
            this.ProductsBookingOrder.Remove(product);
        });

        [RelayCommand]
        private async Task<bool> CreateBookingOrder()
        {
            try
            {
                if (!this.IsBooking && !this.IsOrder)
                {
                    await NotificationService.NotifyAsync("Atencion", "No se a seleccionado el tipo de operacion", "Cerrar");
                    return false;
                }

                if (string.IsNullOrEmpty(this.ClientBookingOrder))
                {
                    await NotificationService.NotifyAsync("Atencion", "Debe ingresar la infromacion del cliente", "Cerrar");
                    return false;
                }

                if (string.IsNullOrEmpty(this.PhoneBookingOrder))
                {
                    await NotificationService.NotifyAsync("Atencion", "Debe ingresar el telefono del cliente", "Cerrar");
                    return false;
                }

                if (this.IsShipmentTask && string.IsNullOrEmpty(this.AddressBookingOrder)) 
                {
                    await NotificationService.NotifyAsync("Atencion", "Debe ingresar la direccion del cliente", "Cerrar");
                    return false;
                }

                if (this.ProductsBookingOrder == null || !this.ProductsBookingOrder.Any()) 
                {
                    await NotificationService.NotifyAsync("Atencion", "Debe seleccionar almenos un producto", "Cerrar");
                    return false;
                }

                var guid = Guid.NewGuid();

                var newBookingOrder = new BookingOrder
                {
                    BookingOrderId = guid,
                    Client = this.ClientBookingOrder,
                    Phone = this.PhoneBookingOrder,
                    Address = this.AddressBookingOrder,
                    IsBooking = this.IsBooking,
                    IsOrder = this.IsOrder,
                    OrderDate = this.DateBookingOrder,
                    Shipment = this.IsShipmentTask,
                    Comments = this.CommentsBookingOrder,
                };

                var saveBookingOrder = await this.DataService.InsertItemAsync<BookingOrder>(newBookingOrder);

                if (saveBookingOrder == 0)
                {
                    await NotificationService.NotifyAsync("Atencion", "Hubo un error al crear la tarea. Vuleva a intentar.", "Cerrar");
                    return false;
                }

                if (this.IsBooking)
                {
                    this.ProductsBookingOrder.ToList().ForEach(async product =>
                    {
                        var booking = new Booking
                        {
                            BookingId = guid,
                            ProductId = product.Product.Id,
                            Quantity = product.QuantityBookingOrder,
                            ProductName = product.Product.ToString()
                        };

                        await this.DataService.InsertItemAsync<Booking>(booking);

                        product.Reserved = product.Reserved + product.QuantityBookingOrder;

                        await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(product);

                    });
                }
                else 
                {
                    this.ProductsBookingOrder.ToList().ForEach(async product =>
                    {
                        var order = new Order
                        {
                            OrderId = guid,
                            ProductId = product.Product.Id,
                            Quantity = product.QuantityBookingOrder,
                            ProductName = product.Product.ToString()
                        };

                        await this.DataService.InsertItemAsync<Order>(order);

                        product.Quantity = product.Quantity - product.QuantityBookingOrder;

                        await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(product);
                    });
                }

                await RefreshBar();
                return true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Atencion", "Hubo un error al crear la tarea. Vuleva a intentar.", "Cerrar");
                return false;
            }
        }

        [RelayCommand]
        private async Task CancelCreateBookingOrder()
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", (response) =>
            {
                if (response)
                {
                    this.ChangeViewCommand.Execute("Home");
                }
            });
        }

        [RelayCommand]
        private async Task<bool> CancelOrder(BookingOrder bookingOrder)
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    bookingOrder.Status = OrderStatus.Cancelado;

                    await this.DataService.UpdateItemAsync<BookingOrder>(bookingOrder);

                    var orders = await this.DataService.LoadOrderDetailsItemsAsync(bookingOrder.BookingOrderId);

                    if (orders != null && orders.Any())
                    {
                        orders.ToList().ForEach(async order =>
                        {
                            var product = this.ProductsWithStock.Where(x => x.Product.Id == order.ProductId).FirstOrDefault();

                            if (product != null)
                            {
                                product.Quantity = product.Quantity + order.Quantity;

                                await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(product);
                            }
                        });
                    }
                }
            });

            await RefreshBar();
            return true;
        }

        [RelayCommand]
        private async Task<bool> ConfirmBooking(BookingOrder bookingOrder)
        {
            await NotificationService.ConfirmAsync("Confirmar", "¿Está seguro que desea confirmar la operación?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    bookingOrder.Status = bookingOrder.Shipment ? OrderStatus.Enviado : OrderStatus.Despachado;

                    bookingOrder.IsBooking = false;
                    bookingOrder.IsOrder = true;

                    await this.DataService.UpdateItemAsync<BookingOrder>(bookingOrder);
                }
            });

            await RefreshBar();
            return true;
        }

        [RelayCommand]
        private async Task ViewOrderAsync(BookingOrder bookingOrder)
        {
            try
            {
                var orders = await this.DataService.LoadOrderDetailsItemsAsync(bookingOrder.BookingOrderId);

                if (orders != null && orders.Any())
                {
                    this.SelectedBookingOrder = bookingOrder;
                    this.OrderItems = new ObservableCollection<Order>(orders);
                    this.ChangeViewCommand.Execute("Order");
                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al cargar la orden. Vuleva a intentar.", "Cerrar");

                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar la orden. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task LoadOrdersItems()
        {
            try
            {
                var orderItems = await this.DataService.LoadOrderItemsAsync();

                if (orderItems != null && orderItems.Any())
                {
                    this.BookingOrderItems = new ObservableCollection<BookingOrder>(orderItems);
                    OnPropertyChanged(nameof(BookingOrderItems));
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar las notas. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task LoadBookingsItems()
        {
            try
            {
                var bookingItems = await this.DataService.LoadBookingItemsAsync();

                if (bookingItems != null && bookingItems.Any())
                {
                    this.BookingOrderItems = new ObservableCollection<BookingOrder>(bookingItems);
                    OnPropertyChanged(nameof(BookingOrderItems));
                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar las notas. Vuleva a intentar.", "Cerrar");
            }
        }

        [RelayCommand]
        private async Task<bool> CancelBooking(BookingOrder bookingOrder)
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    await this.DataService.UpdateItemAsync<BookingOrder>(bookingOrder);

                    var bookings = await this.DataService.LoadBookingDetailsItemsAsync(bookingOrder.BookingOrderId);

                    if (bookings != null && bookings.Any())
                    {
                        bookings.ToList().ForEach(async booking =>
                        {
                            var product = this.ProductsWithStock.Where(x => x.Product.Id == booking.ProductId).FirstOrDefault();

                            if (product != null)
                            {
                                product.Quantity = product.Quantity + booking.Quantity;

                                await this.DataService.InsertOrUpdateItemsAsync<ProductStock>(product);
                            }
                        });
                    }
                }
            });

            await RefreshBar();
            return true;
        }

        [RelayCommand]
        private async Task ViewBookingAsync(BookingOrder bookingOrder)
        {
            try
            {
                var booking = await this.DataService.LoadBookingDetailsItemsAsync(bookingOrder.BookingOrderId);

                if (booking != null && booking.Any())
                {
                    this.SelectedBookingOrder = bookingOrder;
                    this.BookingItems = new ObservableCollection<Booking>(booking);
                    this.ChangeViewCommand.Execute("Booking");
                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al cargar la reserva. Vuleva a intentar.", "Cerrar");

                }
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", "Hubo un error al cargar la reserva. Vuleva a intentar.", "Cerrar");
            }
        }

        #endregion


        #region Reports

        private async Task<bool> CreateSavePDFAsync(List<ProductStock> productsStock)
        {
            var pdfName = $"Stock_{DateTime.Now.ToString("ddMMyyyy")}.pdf";

            var stream = new MemoryStream();

            using (PdfWriter writer = new PdfWriter(stream))
            {

                PdfDocument pdf = new PdfDocument(writer);

                Document document = new Document(pdf);

                Paragraph header = new Paragraph("Productos agregados")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(20);

                document.Add(header);
                Paragraph subHeader = new Paragraph($"Fecha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(15);
                document.Add(subHeader);

                LineSeparator ls = new LineSeparator(new SolidLine());
                document.Add(ls);

                var imagStream = await ConvertImageSourceToStreamAsync("logo_login.svg");

                iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory
                    .Create(imagStream))
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                document.Add(image);

                Paragraph footer = new Paragraph("Rincon")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(10);
                document.Add(footer);

                document.Close();

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                //var fileSaverResult = await this.fileSaver.SaveAsync(pdfName, stream, cancellationTokenSource.Token);

                return true;//fileSaverResult.IsSuccessful;
                
            }
        }

        private async Task<byte[]> ConvertImageSourceToStreamAsync(string imageName)
        {
            using var ms = new MemoryStream();
            using (var stream = await FileSystem.OpenAppPackageFileAsync(imageName))
                await stream.CopyToAsync(ms);
            return ms.ToArray();
        }

        [RelayCommand]
        private async Task GeneratePdfAsync()
        {
            try
            {
                // Ruta del archivo PDF en el almacenamiento local
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "pedido.pdf");

                // Ruta del logo (asegúrate de que esté en Resources/Images o AppDataDirectory)
                string logoPath = Path.Combine(FileSystem.AppDataDirectory, "logo.png");

                var productos = await this.DataService.LoadOrderDetailsItemsAsync(this.SelectedBookingOrder.BookingOrderId);

                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", $"{this.SelectedBookingOrder.Client}_{this.SelectedBookingOrder.Id}.pdf");

                // Llamar a la generación del PDF
                PdfGenerator.GenerateBookingOrderPdf(this.SelectedBookingOrder, productos, downloadsPath);

                // Mostrar mensaje de éxito
                await App.Current.MainPage.DisplayAlert("Éxito", "El PDF se generó correctamente.", "OK");

                // Abrir el PDF después de generarlo
                await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(downloadsPath) });
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Error al generar el PDF: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}
