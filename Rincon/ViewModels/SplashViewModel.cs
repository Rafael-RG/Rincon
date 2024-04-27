using Rincon.Common.ViewModels;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Sample entry form
    /// </summary>
    public partial class SplashViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public SplashViewModel(IServiceProvider provider) : base(provider)
        {
            
        }

        public override async void OnAppearing()
        {
            await Task.Delay(2000);
            await this.NavigationService.Navigate<HomeViewModel>();
        }
    }
}
