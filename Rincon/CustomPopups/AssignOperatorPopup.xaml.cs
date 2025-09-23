using Microsoft.Maui.Controls;
using Rincon.ViewModels;
using System.ComponentModel;

namespace Rincon.CustomPopups
{
    public partial class AssignOperatorPopup : ContentView
    {
        public AssignOperatorPopup()
        {
            InitializeComponent();
            
            // Suscribirse a cambios del ViewModel cuando se asigne
            BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, System.EventArgs e)
        {
            if (BindingContext is OperatorsViewModel viewModel)
            {
                viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OperatorsViewModel.IsPinEnabled))
            {
                UpdatePinFieldState();
            }
        }

        private void UpdatePinFieldState()
        {
            if (BindingContext is OperatorsViewModel viewModel)
            {
                if (viewModel.IsPinEnabled)
                {
                    // Habilitar campo PIN
                    PinFrame.BackgroundColor = Color.FromArgb("#FFF6ED");
                    PinFrame.BorderColor = Color.FromArgb("#D3AA84");
                    PinEntry.Placeholder = "Ingrese PIN";
                    PinEntry.IsEnabled = true;
                    
                    // Habilitar botón
                    ConfirmButton.BackgroundColor = Color.FromArgb("#4A744C");
                    ConfirmButton.IsEnabled = true;
                }
                else
                {
                    // Deshabilitar campo PIN
                    PinFrame.BackgroundColor = Color.FromArgb("#F0F0F0");
                    PinFrame.BorderColor = Color.FromArgb("#C0C0C0");
                    PinEntry.Placeholder = "Seleccione operador primero";
                    PinEntry.IsEnabled = false;
                    
                    // Deshabilitar botón
                    ConfirmButton.BackgroundColor = Color.FromArgb("#A0A0A0");
                    ConfirmButton.IsEnabled = false;
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdatePinFieldState();
        }
    }
}
