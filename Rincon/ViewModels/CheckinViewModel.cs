using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rincon.Common.ViewModels;
using System.Collections.ObjectModel;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Sample entry form
    /// </summary>
    public partial class CheckinViewModel : BaseViewModel
    {
        [ObservableProperty]
        private List<string> questions;

        [ObservableProperty]
        private List<string> username;

        [ObservableProperty]
        private List<string> password;
        
        [ObservableProperty]
        private List<string> answer;


        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public CheckinViewModel(IServiceProvider provider) : base(provider)
        {
            questions = new List<string>
            {
                new ("Opción 1"),
                new ("Opción 2"),
                new ("Opción 3")
            };
        }


        /// <summary>
        /// DB login
        /// </summary>
        [RelayCommand]
        private async void Checkin()
        {
       
        }

        /// <summary>
        /// DB login
        /// </summary>
        [RelayCommand]
        private async void CancelCheckin()
        {

        }
    }
}
