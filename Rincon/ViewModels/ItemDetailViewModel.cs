using Rincon.Common.Interfaces;
using Rincon.Common.ViewModels;


namespace Rincon.ViewModels
{
    /// <summary>
    /// Logic to show info from an item selected
    /// </summary>
    public class ItemDetailViewModel : BaseViewModel
    {
      
        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public ItemDetailViewModel(IServiceProvider provider) : base(provider)
        {
        }
    }
}
