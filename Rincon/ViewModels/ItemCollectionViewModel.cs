using Rincon.Common.Interfaces;
using Rincon.Common.ViewModels;


namespace Rincon.ViewModels
{
    /// <summary>
    /// Sample viewmodel to show a collection of items
    /// </summary>
    public class ItemCollectionViewModel : BaseViewModel
    {
      
        /// <summary>
        /// Gets by DI the required services
        /// </summary>
        public ItemCollectionViewModel(IServiceProvider provider) : base(provider)
        {
        }
    }
}
