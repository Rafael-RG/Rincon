//using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
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
        private bool isCheckStockView;

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
                }
            }
        }

        ///// <summary>
        ///// Is Polin
        ///// </summary>
        [ObservableProperty]
        private bool isPolinSelect;

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
                }
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
        [ObservableProperty]
        private bool isMachimbre; 

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



        #endregion

        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public HomeViewModel(IServiceProvider provider /*,IFileSaver fileSaver*/) : base(provider)
        {
            //this.fileSaver = fileSaver;
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


        public ICommand LogoutViewCommand => new Command(async () =>
        {

            await NotificationService.ConfirmAsync("Cerrar Sesion", "¿Está seguro que desea cerrar sesion?", "Si", "No", async (response) =>
            {
                if (response)
                {
                    var user = await this.DataService.LoadLocalUserAsync();

                    await this.DataService.DeleteLocalUserAsync(user);

                    await this.NavigationService.Navigate<LoginViewModel>();
                }
            });
        });

        public ICommand ChangeViewCommand => new Command<string>((view) =>
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
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

                    this.IsTiranteSelect = true;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;

                    this.IsVisibleListAddStock = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;
                    this.SelectedIndexSeachStock = 1;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = false;

                    this.SelectedIndexSeachStock = 1;
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
                    this.IsInventoryEditView = true;
                    this.IsInventoryEditProductView = false;
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
                    this.IsInventoryEditView = false;
                    this.IsInventoryEditProductView = true;
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
                            if (this.Diameter == 0)
                            {
                                await NotificationService.NotifyAsync("Falta completar campos", "El diametro no puede ser 0", "Cerrar");
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
                            ProductType = this.IsTiranteSelect ? ProductType.Tirante : this.IsPolinSelect ? ProductType.Polin : ProductType.Tabla,
                            WoodState =  (WoodState)Enum.Parse(typeof(WoodState),this.SelectedState),
                            MachimbreSate = (Machimbre)Enum.Parse(typeof(Machimbre),this.SelectedMachimbre),
                            Description = this.IsPolinSelect ? $"{this.Diameter}" : $"{this.Thickness} x {this.Length} x {this.Width}",

                        };

                        var result = await this.DataService.InsertOrUpdateItemsAsync<Product>(product);

                        if (result > 0)
                        {
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

        #endregion

        #region CheckStock


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
                
                await RefreshBar();
            }
            catch (Exception ex)
            {
                //Error
            }
            finally
            {
                this.IsBusy = false;
            }

        });

        [RelayCommand]
        private async Task Configuration() 
        {

        }

        [RelayCommand]
        private async Task ManagementOperators()
        {

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

        private void ClearViewAddStock()
        {
            this.ProductsStock = null;
        }

        private async Task RefreshBar()
        {
            var stock = await this.DataService.LoadStockAsync();

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
    }
}
