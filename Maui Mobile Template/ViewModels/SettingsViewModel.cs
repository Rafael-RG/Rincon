using MobileTemplate.Common.Interfaces;
using MobileTemplate.Common.ViewModels;


namespace MobileTemplate.ViewModels
{
    /// <summary>
    /// Synchronization UI logic
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {
      
        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public SettingsViewModel(IServiceProvider provider) : base(provider)
        {
        }
    }
}
