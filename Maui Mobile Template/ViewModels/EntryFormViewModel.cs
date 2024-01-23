﻿using MobileTemplate.Common.Interfaces;
using MobileTemplate.Common.ViewModels;


namespace MobileTemplate.ViewModels
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
