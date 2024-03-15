using Rincon.Common.ViewModels;
using Rincon.Models;
using System.Collections.ObjectModel;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Home logic
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        ///// <summary>
        ///// Cards
        ///// </summary>
        public ObservableCollection<CardStock> Cards { get;  set; }


        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public HomeViewModel(IServiceProvider provider) :  base(provider)
        {
            
        }


        public override async void OnAppearing()
        {
            this.Cards = new ObservableCollection<CardStock>()
            {
                new CardStock()
                {
                    Id = 111111,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 222222,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 333333,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 444444,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 555555,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 6666666,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 77777777,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                },
                new CardStock()
                {
                    Id = 88888888,
                    Description = "2” x 1” x 3,30m.",
                    StockAvailable = 10000,
                    StockReserved = 10000,
                    Icon = "cr"
                }
            };
            if (!this.AuthenticationService.IsAuthenticated())
            {
                await this.NavigationService.Navigate<LoginViewModel>();
            }
        }

        ///// <summary>
        ///// Prepares the local variables
        ///// </summary>
        //public override void Prepare()
        //{
        //    this.Menu = new List<MenuOption>(new List<MenuOption> {
        //        new MenuOption
        //        {
        //            Text = GetText("Option"),
        //            Icon = IconConstants.Alert,
        //            Command = new MvxCommand(() =>
        //            {                        
        //            }),
        //        },
        //        new MenuOption
        //        {
        //            Text = GetText("EntryForm"),
        //            Icon = IconConstants.FileDocumentEdit,
        //            Command = new MvxCommand(async () =>
        //            {
        //                 await this.NavigationService.Navigate<EntryFormViewModel>();
        //            }),
        //        },
        //        new MenuOption
        //        {
        //            Text = GetText("Camera"),
        //            Icon = IconConstants.Camera,
        //            Command = new MvxCommand(() =>
        //            {                       
        //            }),
        //        },
        //        new MenuOption
        //        {
        //            Text = GetText("Synchronize"),
        //            Icon = IconConstants.Refresh,
        //            Command = new MvxCommand(async () =>
        //            {
        //                await this.NavigationService.Navigate<SynchronizationViewModel>();
        //            }),
        //        },
        //    });
        //}

    }
}
