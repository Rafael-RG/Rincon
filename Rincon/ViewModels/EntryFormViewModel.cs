using Rincon.Common.Interfaces;
using Rincon.Common.ViewModels;


namespace Rincon.ViewModels
{
    /// <summary>
    /// Sample entry form
    /// </summary>
    public class EntryFormViewModel : BaseViewModel
    {
      
        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public EntryFormViewModel(IServiceProvider provider) : base(provider)
        {
        }
    }
}
