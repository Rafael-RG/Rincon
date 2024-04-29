using CommunityToolkit.Mvvm.ComponentModel;
using Rincon.Common.ViewModels;
using System.Windows.Input;
using Rincon.Models;

namespace Rincon.ViewModels
{
    /// <summary>
    /// Sample entry form
    /// </summary>
    public partial class RecoverPasswordViewModel : BaseViewModel
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
        private bool isVisibleFirstForm;

        [ObservableProperty]
        private bool isVisibleNewPassword;

        [ObservableProperty]
        private bool isVisibleOkRecovery;

        [ObservableProperty]
        private bool isVisibleListQuestions;


        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public RecoverPasswordViewModel(IServiceProvider provider) : base(provider)
        {
            this.Questions = new List<string>
            {
                new ("Cual es el nombre de tu cancion favorita?"),
                new ("Cual es tu juego favorito?"),
                new ("Cual es el nombre de tu primer mascota?"),
                new ("Cual es el nombre del barrio en que naciste?")
            };

            this.SeletedQuestion = this.Questions.First();

            this.IsVisibleFirstForm = true;
        }


        /// <summary>
        /// Recovery password first form
        /// </summary>
        public ICommand ConfirmRecoveryCommand => new Command(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Answer) || string.IsNullOrEmpty(SeletedQuestion) )
                {
                    await NotificationService.NotifyAsync("Error", "Faltan completar campos", "Cerrar");
                    return;
                }

                var user = await this.DataService.LoadRecoveryUserAsync(this.Username);

                if (user == null)
                {
                    await NotificationService.NotifyAsync("Error", $"No se encontro el usuarioi: {this.Username}", "Cerrar");
                    return;
                }
                else if (user.Question != this.SeletedQuestion)
                {
                    await NotificationService.NotifyAsync("Error", $"Pregunta incorrecta", "Cerrar");
                    return;
                }
                else if (user.Answer != this.Answer)
                {
                    await NotificationService.NotifyAsync("Error", $"Respuesta incorrecta", "Cerrar");
                    return;
                }

                this.IsVisibleFirstForm = false;
                this.IsVisibleNewPassword = true;
                this.IsVisibleOkRecovery = false;
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", $"Hubo un error al intentar recuperar la contraseña.", "Cerrar");
            }
        });

        /// <summary>
        /// Recovery password first form
        /// </summary>
        public ICommand RecoveryPasswordCommand => new Command(async () =>
        {
            try
            {
                if (string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.RepeatPassword))
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
                    Role = Role.Admin
                };

                var result = await this.DataService.InsertOrUpdateItemsAsync(user);

                if (result > 0)
                {
                    this.IsVisibleOkRecovery = true;
                }
                else if (user.Question != this.SeletedQuestion)
                {
                    await NotificationService.NotifyAsync("Error", $"Hubo un error al intentar recuperar la contraseña.", "Cerrar");
                    return;
                }

                this.IsVisibleFirstForm = false;
                this.IsVisibleNewPassword = false;
                this.IsVisibleOkRecovery = true;
            }
            catch
            {
                await NotificationService.NotifyAsync("Error", $"Hubo un error al intentar recuperar la contraseña.", "Cerrar");
            }
        });

        /// <summary>
        /// DB login
        /// </summary>
        public ICommand BackToLoginCommand => new Command(async () =>
        {
            this.CleanData();
            await this.NavigationService.Navigate<LoginViewModel>();
        });

        /// <summary>
        /// DB login
        /// </summary>
        public ICommand BackFirstFormCommand => new Command(async () =>
        {
            this.IsVisibleFirstForm = true;
            this.IsVisibleNewPassword = false;
        });

        /// <summary>
        /// DB login
        /// </summary>
        public ICommand SelectQuestionCommand => new Command(async () =>
        {
            this.IsVisibleListQuestions = !this.IsVisibleListQuestions;
        });

        private void CleanData()
        {
            this.Username = string.Empty;
            this.Answer = string.Empty;
            this.SeletedQuestion = this.Questions.First();
        }
    }
}
