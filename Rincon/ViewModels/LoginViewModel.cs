﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rincon.Common.ViewModels;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Simple username / password login logic
    /// </summary>
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;


        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public LoginViewModel(IServiceProvider provider) : base(provider)
        {
        }


        /// <summary>
        /// DB login
        /// </summary>
        [RelayCommand]
        private async void Login()
        {
            if (this.IsBusy) return;

            if (string.IsNullOrWhiteSpace(this.Username))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("UserEmpty"), GetText("Close"));
                return;
            }
            else if (string.IsNullOrWhiteSpace(this.Password))
            {
                await NotificationService.NotifyAsync(GetText("Error"), GetText("PasswordEmpty"), GetText("Close"));
                return;
            }
            
            try
            {
                this.IsBusy = true;
                var result = await this.AuthenticationService.AuthenticateAsync(this.Username, this.Password);
                if (!result)
                {
                    //await NotificationService.NotifyAsync("LoginErrorTitle", "LoginError", "Close");
                    return;
                }

                if (await this.DataService.MustSynchronizeAsync())
                {
                    await this.NavigationService.Navigate<SynchronizationViewModel>();
                }
                else
                {
                    await this.NavigationService.Close(this);       
                }
            }
            catch (Exception ex)
            {
                //await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                await LogExceptionAsync(ex);
            }
        }


        /// <summary>
        /// Login with active directory
        /// </summary>
        [RelayCommand]
        private async void Checkin()
        {
            await this.NavigationService.Navigate<CheckinViewModel>();
        }




        /// <summary>
        /// Login when appears
        /// </summary>
        public override async void OnAppearing()
        {
            try
            {
                await this.AuthenticationService.LoadCredentialsAsync();
                if (this.AuthenticationService.IsAuthenticated())
                {
                    // shows the last username and 
                    // add a small delay to allow the UI to update itself before continuing
                    this.IsBusy = true;
                    this.Username = this.AuthenticationService.User.Name;
                    this.Password = "*******";
                    await Task.Delay(250);
                    await this.NavigationService.Close(this);
                }
            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                //await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                await LogExceptionAsync(ex);
            }

        }


    }
}
