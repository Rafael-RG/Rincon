﻿using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
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
        public ICommand LoginCommand => new Command(async () =>
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
                var user = await this.DataService.LoadUserAsync(this.Username, this.Password);

                if (user == null)
                {
                    await NotificationService.NotifyAsync("Error", "Credenciales invalidas", "Cerrar");
                    return;
                }

                var result = await this.DataService.SaveLocalUserAsync(user);

                if (result)
                {
                    await this.NavigationService.Navigate<HomeViewModel>();
                }
            }
            catch (Exception ex)
            {
                await NotificationService.NotifyAsync(GetText("Error"), (ex.Message), GetText("Close"));
                await LogExceptionAsync(ex);
            }
            finally
            {
                this.IsBusy = false;
            }
        });


        /// <summary>
        /// Login with active directory
        /// </summary>
        public ICommand CheckinCommand => new Command(async () =>
        {
            await this.NavigationService.Navigate<CheckinViewModel>();
        });


        /// <summary>
        /// Login with active directory
        /// </summary>
        public ICommand RecoveryPasswordCommand => new Command(async () =>
        {
            await this.NavigationService.Navigate<RecoverPasswordViewModel>();
        });



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
