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

        [ObservableProperty]
        private string operatorUser;

        [ObservableProperty]
        private string operatorPin;

        [ObservableProperty]
        private int lossMaterial;

        [ObservableProperty]
        private string additionalComments;

        [ObservableProperty]
        private List<Operator> operators;

        [ObservableProperty]
        private Operator selectedOperator;

        public bool IsPinEnabled => SelectedOperator != null;
        
        public bool IsConfirmEnabled => SelectedOperator != null && !string.IsNullOrWhiteSpace(OperatorPin);

        public List<TaskItem> PendingTasks { get; set; }
        public List<TaskItem> AssignedTasks { get; set; }

        public ICommand ClosePopupCommand => new Command(() =>
        {
            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsAssignOperatorPopupVisible = false;
            SelectedTask = null;
            SelectedOperator = null;
            OperatorPin = string.Empty;
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

        public ICommand AssignedOperatorCommand => new Command(async () =>
        {

            try
            {
                if (SelectedOperator == null || string.IsNullOrWhiteSpace(OperatorPin))
                {
                    await NotificationService.NotifyAsync("Error", "Por favor, seleccione un operador e ingrese el PIN.", "OK");
                    return;
                }

                // Verificar que el PIN sea correcto para el operador seleccionado
                if (SelectedOperator.Pin != OperatorPin)
                {
                    await NotificationService.NotifyAsync("Error", "PIN incorrecto.", "OK");
                    return;
                }

                // Si el PIN es correcto, proceder a asignar la tarea
                this.SelectedTask.OperatorId = SelectedOperator.Id;
                this.SelectedTask.OperatorName = SelectedOperator.Name;
                this.SelectedTask.TaskStatus = Rincon.Models.TaskStatus.Iniciada;

                // Actualizar la tarea en el servicio de datos
                await _dataService.UpdateItemAsync(this.SelectedTask);
                
                // Recargar las tareas para reflejar los cambios
                LoadTasksAsync();

                IsBusy = true;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                System.Diagnostics.Debug.WriteLine($"Error al entrar como operador: {ex.Message}");
                await NotificationService.NotifyAsync("Error", $"Error al entrar como operador: {ex.Message}", "OK");
            }

            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsFinishTaskAuthPopupVisible = false;
            IsAssignOperatorPopupVisible = false;
            
            this.IsBusy = false;
            
        });

        public ICommand ShowAssignOperatorPopupCommand => new Command(() =>
        {
            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsFinishTaskAuthPopupVisible = false;
            IsAssignOperatorPopupVisible = true;
        });

        public ICommand ShowFinishTaskAuthPopupCommand => new Command(() =>
        {
            IsTaskDetailPopupVisible = false;
            IsFinishTaskPopupVisible = false;
            IsFinishTaskAuthPopupVisible = true;
            IsAssignOperatorPopupVisible = false;
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
            Operators = new List<Operator>();
            LoadTasksAsync();
            LoadOperatorsAsync();
        }

        public ICommand FinishTaskAuthCommand => new Command(async() =>
        {
            try
            {
                if (SelectedOperator == null || string.IsNullOrWhiteSpace(OperatorPin))
                {
                    await NotificationService.NotifyAsync("Error", "Por favor, seleccione un operador e ingrese el PIN.", "OK");
                    return;
                }

                // Verificar que el PIN sea correcto para el operador seleccionado
                if (SelectedOperator.Pin != OperatorPin)
                {
                    await NotificationService.NotifyAsync("Error", "PIN incorrecto.", "OK");
                    return;
                }

                // Si el operador existe, puedes proceder a finalizar la tarea
                this.SelectedTask.TaskStatus = Rincon.Models.TaskStatus.Finalizada;
                this.SelectedTask.ProcessErrorQuantity = this.LossMaterial.ToString();
                this.SelectedTask.OperatorComment = this.AdditionalComments;

                // Actualizar la tarea en el servicio de datos
                await _dataService.UpdateItemAsync(this.SelectedTask);

                var productsStock = await _dataService.LoadStockAsync();
                if (productsStock != null && productsStock.Any())
                {
                    var productSource = productsStock.FirstOrDefault(p => p.Id == this.SelectedTask.ProductSourceId);
                    if (productSource != null)
                    {
                        // Actualizar la cantidad del producto en stock
                        productSource.Process -= int.Parse(this.SelectedTask.Quantity);
                        await _dataService.UpdateItemAsync(productSource);
                    }

                    var productDestination = productsStock.FirstOrDefault(p => p.Id == this.SelectedTask.ProductDestinationId);
                    if (productDestination != null)
                    {
                        // Actualizar la cantidad del producto en stock
                        productDestination.Quantity += int.Parse(this.SelectedTask.Quantity) - int.Parse(this.SelectedTask.ProcessErrorQuantity);
                        await _dataService.UpdateItemAsync(productDestination);
                    }
                }

                // Recargar las tareas para reflejar los cambios
                LoadTasksAsync();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al finalizar tarea: {ex.Message}");
                await NotificationService.NotifyAsync("Error", $"Error al finalizar tarea: {ex.Message}", "OK");
            }

            IsFinishTaskAuthPopupVisible = false;
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

        private async void LoadOperatorsAsync()
        {
            try
            {
                var operatorsList = await _dataService.LoadOperatorsAsync();
                Operators = operatorsList?.ToList() ?? new List<Operator>();
                OnPropertyChanged(nameof(Operators));
                System.Diagnostics.Debug.WriteLine($"Operadores cargados: {Operators.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al cargar operadores: {ex.Message}");
                Operators = new List<Operator>();
            }
        }
        
        partial void OnSelectedOperatorChanged(Operator value)
        {
            OnPropertyChanged(nameof(IsPinEnabled));
            OnPropertyChanged(nameof(IsConfirmEnabled));
        }
        
        partial void OnOperatorPinChanged(string value)
        {
            OnPropertyChanged(nameof(IsConfirmEnabled));
        }
        
        public ICommand ReloadTasksCommand => new Command(() =>
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsAnyPopupVisible));
            LoadTasksAsync();
        });
    }
}