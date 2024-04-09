﻿using CommunityToolkit.Mvvm.ComponentModel;
using Rincon.Common.ViewModels;
using Rincon.Models;

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Home logic
    /// </summary>
    public partial class HomeViewModel : BaseViewModel
    {
        #region Properties
        ///// <summary>
        ///// Cards
        ///// </summary>
        [ObservableProperty]
        private List<CardStock> cards;

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

        ///// <summary>
        /// Is Measure
        /// </summary>
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

        #endregion

        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public HomeViewModel(IServiceProvider provider) : base(provider)
        {

        }

        public override async void OnAppearing()
        {
            this.ChangeViewCommand.Execute("Home");

            this.Cards = new List<CardStock>()
            {
                new ()
                {
                    Id = 111111,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 222222,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 333333,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 444444,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 555555,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 6666666,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 77777777,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                },
                new ()
                {
                    Id = 88888888,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr.png"
                }
            };

            this.States = new List<string>()
            {
                "Estado 1",
                "Esatdo 2",
                "Estado 3"
            };

            this.Products = await this.DataService.LoadItemsAsync();

            if (!this.AuthenticationService.IsAuthenticated())
            {
                await this.NavigationService.Navigate<LoginViewModel>();
            }
        }

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
                    break;
                case "MagementStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = true;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    break;
                case "Tasks":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = true;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    break;
                case "Orders":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = true;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    break;
                case "History":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = true;
                    this.IsAddProductView = false;
                    this.IsAddStockView = false;
                    break;
                case "AddProduct":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = true;
                    this.IsAddStockView = false;
                    break;
                case "AddStock":
                    this.IsHomeView = false;
                    this.IsMagementStockView = false;
                    this.IsTasksView = false;
                    this.IsOrdersView = false;
                    this.IsHistoryView = false;
                    this.IsAddProductView = false;
                    this.IsAddStockView = true;
                    break;
            }
        });

        public ICommand CancelAddProductCommand => new Command(async () =>
        {
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Yes", "No", async (response) =>
            {
                if (response)
                {
                    ClearViewAddProduct();
                    this.ChangeViewCommand.Execute("Home");
                }
            });

        });

        public ICommand SaveProductCommand => new Command(async () =>
        {
            await NotificationService.ConfirmAsync("Guardar", "¿Está seguro que desea guardar el producto?", "Yes", "No", async (response) =>
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
                            WoodState = WoodState.Cepillado,
                            MachimbreSate = Machimbre.machimbre3,
                            Description = this.IsPolinSelect ? $"{this.Diameter}" : $"{this.thickness} x {this.Length} x {this.Width}",

                        };

                        var result = await this.DataService.InsertOrUpdateItemsAsync<Product>(product);

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
                        Quantity = 1
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
            await NotificationService.ConfirmAsync("Cancelar", "¿Está seguro que desea cancelar la operación?", "Yes", "No", async (response) =>
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

               var result = await this.DataService.InsertOrUpdateStockAsync(this.ProductsStock.ToList());

           }
           catch
           {
               await NotificationService.NotifyAsync("Error", "Hubo un error al agregar stock. Vuleva a intentar.", "Cerrar");
               return;
           }

           await NotificationService.NotifyAsync("Operacion Exitosa", "Stock actualizado", "Volver");

           ClearViewAddProduct();

           return;
       });


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
            this.IsTiranteSelect = false;
        }

        private void ClearViewAddStock()
        {
            this.ProductsStock = null;
        }
    }
}
