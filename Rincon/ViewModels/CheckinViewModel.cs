using CommunityToolkit.Mvvm.ComponentModel;
using Rincon.Common.ViewModels;
using System.Windows.Input;
using Rincon.Models;

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
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string repeatPassword;

        [ObservableProperty]
        private string answer;

        [ObservableProperty]
        private string seletedQuestion;

        [ObservableProperty]
        private bool isVisibleListQuestions;


        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public CheckinViewModel(IServiceProvider provider) : base(provider)
        {
            this.Questions = new List<string>
            {
                new ("Cual es el nombre de tu cancion favorita?"),
                new ("Cual es tu juego favorito?"),
                new ("Cual es el nombre de tu primer mascota?"),
                new ("Cual es el nombre del barrio en que naciste?")
            };

            this.SeletedQuestion = this.Questions.First();
        }


        /// <summary>
        /// DB login
        /// </summary>
        public ICommand CheckinCommand => new Command(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(RepeatPassword) || string.IsNullOrEmpty(Answer) || string.IsNullOrEmpty(SeletedQuestion) )
                {
                    await NotificationService.NotifyAsync("Error", "Faltan completar campos", "Cerrar");
                    return;
                }

                if (Password != RepeatPassword)
                {
                    await NotificationService.NotifyAsync("Error", "Las contraseñas no coinciden", "Cerrar");
                    return;
                }

                var user = new User()
                {
                     Name = this.Username,
                     Password = this.Password,
                     Question = this.SeletedQuestion,
                     Answer = this.Answer,
                     Role= Role.Admin
                };

                var result = await this.DataService.InsertOrUpdateItemsAsync<User>(user);

                if (result > 0)
                {
                    await NotificationService.NotifyAsync("Creado", "Se creo el usuario", "Cerrar");
                    this.CleanData();
                    await this.NavigationService.Navigate<LoginViewModel>();
                }
                else
                {
                    await NotificationService.NotifyAsync("Error", "Hubo un error al crear el usuario. Por favor vuelva a intentar", "Cerrar");
                }
            }
            catch
            {

            }
        });

        /// <summary>
        /// DB login
        /// </summary>
        public ICommand CancelCheckinCommand => new Command(async () =>
        {
            this.CleanData();
            await this.NavigationService.Navigate<LoginViewModel>();
        });

        /// <summary>
        /// DB login
        /// </summary>
        public ICommand SelectQuestionCommand => new Command(() =>
        {
            this.IsVisibleListQuestions = !this.IsVisibleListQuestions;
        });

        private void CleanData()
        {
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.RepeatPassword = string.Empty;
            this.Answer = string.Empty;
            this.SeletedQuestion = this.Questions.First();
        }
    }
}
