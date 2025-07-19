using System.Windows.Input;
using Rincon.Common.ViewModels;
using Rincon.Models;
using Rincon.Common.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rincon.ViewModels
{
    public partial class OperatorsViewModel : BaseViewModel
    {
        public bool IsAnyPopupVisible => IsTaskDetailPopupVisible || IsAssignOperatorPopupVisible || IsFinishTaskPopupVisible || IsFinishTaskAuthPopupVisible || IsBusy;

        private readonly IDataService _dataService;

        private bool _isFinishTaskAuthPopupVisible;
        public bool IsFinishTaskAuthPopupVisible
        {
            get => _isFinishTaskAuthPopupVisible;
            set
            {
                if (SetProperty(ref _isFinishTaskAuthPopupVisible, value))
                    OnPropertyChanged(nameof(IsAnyPopupVisible));
            }
        }

        private bool _isFinishTaskPopupVisible;
        public bool IsFinishTaskPopupVisible
        {
            get => _isFinishTaskPopupVisible;
            set
            {
                if (SetProperty(ref _isFinishTaskPopupVisible, value))
                    OnPropertyChanged(nameof(IsAnyPopupVisible));
            }
        }

        private bool _isAssignOperatorPopupVisible;
        public bool IsAssignOperatorPopupVisible
        {
            get => _isAssignOperatorPopupVisible;
            set
            {
                if (SetProperty(ref _isAssignOperatorPopupVisible, value))
                    OnPropertyChanged(nameof(IsAnyPopupVisible));
            }
        }

        private bool _isTaskDetailPopupVisible;
        public bool IsTaskDetailPopupVisible
        {
            get => _isTaskDetailPopupVisible;
            set
            {
                if (SetProperty(ref _isTaskDetailPopupVisible, value))
                {
                    OnPropertyChanged(nameof(IsAnyPopupVisible));
                }

            }
        }

        [ObservableProperty]
        private TaskItem selectedTask;

        public List<TaskItem> PendingTasks { get; set; }
        public List<TaskItem> AssignedTasks { get; set; }

        public ICommand ClosePopupCommand => new Command(() =>
        {
            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            SelectedTask = null;
        });


        public ICommand ShowTaskPopupCommand => new Command<TaskItem>((task) =>
        {
            IsTaskDetailPopupVisible = false;
            IsAssignOperatorPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsFinishTaskAuthPopupVisible = false;

            if (task == null) return;
            if (task.TaskStatus == Rincon.Models.TaskStatus.Pendiente)
            {
                IsTaskDetailPopupVisible = true;
            }
            else if (task.TaskStatus == Rincon.Models.TaskStatus.Iniciada)
            {
                IsFinishTaskPopupVisible = true;
            }
        });

        public ICommand EnterOperatorCommand => new Command(() =>
        {
            IsAssignOperatorPopupVisible = false;
        });

        public ICommand ShowAssignOperatorPopupCommand => new Command(() =>
        {
            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsFinishTaskAuthPopupVisible = false;
            IsAssignOperatorPopupVisible = true;
        });

        public ICommand OperatorsCommand => new Command(async () =>
        {
            await NotificationService.NotifyAsync("Info", "Operator login command executed", "Close");
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            try
            {
                IsBusy = true;
                await this.NavigationService.Navigate<LoginViewModel>();
            }
            catch (Exception ex)
            {
                await NotificationService.NotifyAsync("Error", $"Error al navegar: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        });

        public OperatorsViewModel(IServiceProvider provider) : base(provider)
        {
            IsBusy = false;
            _dataService = provider.GetService(typeof(IDataService)) as IDataService;
            PendingTasks = new List<TaskItem>();
            AssignedTasks = new List<TaskItem>();
            LoadTasksAsync();
        }

        //FinishTaskAuthCommand
        public ICommand FinishTaskAuthCommand => new Command(() =>
        {
            IsFinishTaskAuthPopupVisible = false;
            SelectedTask = null;
            // Aquí puedes agregar la lógica para finalizar la tarea
            System.Diagnostics.Debug.WriteLine("Tarea finalizada con autenticación");
        });

        private async void LoadTasksAsync()
        {
            var allTasks = await _dataService.LoadTaskItemsAsync();

            // Log temporal para depuración
            System.Diagnostics.Debug.WriteLine($"Total tareas cargadas: {allTasks.Count}");
            foreach (var t in allTasks)
            {
                System.Diagnostics.Debug.WriteLine($"Tarea: Id={t.Id}, Description={t.Description}, Status={t.TaskStatus}");
            }

            PendingTasks = allTasks.Where(t => t.TaskStatus == Rincon.Models.TaskStatus.Pendiente).ToList();
            AssignedTasks = allTasks.Where(t => t.TaskStatus == Rincon.Models.TaskStatus.Iniciada).ToList();
            System.Diagnostics.Debug.WriteLine($"Pendientes: {PendingTasks.Count}, Asignadas: {AssignedTasks.Count}");

            IsBusy = false;
            OnPropertyChanged(nameof(PendingTasks));
            OnPropertyChanged(nameof(AssignedTasks));
            OnPropertyChanged(nameof(IsAnyPopupVisible));
        }
        
        public ICommand ReloadTasksCommand => new Command(() =>
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsAnyPopupVisible));
            LoadTasksAsync();
        });
    }
}