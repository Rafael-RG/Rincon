using Rincon.Common.Interfaces;
using Rincon.Common.ViewModels;


namespace Rincon.ViewModels
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
