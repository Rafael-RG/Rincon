using Rincon.Common.Interfaces;
using Microsoft.Extensions.Localization;
using Rincon.Helpers;
using Rincon.Resources.Strings;

namespace Rincon.Services
{
    public class LocalizationService : ILocalizationService
    {
        private IStringLocalizer<Texts> localizer;

        public LocalizationService()
        {
            this.localizer = ServiceHelper.GetService<IStringLocalizer<Texts>>();
        }


        public string GetText(string text)
        {
            var localizedText = this.localizer[text];
            return localizedText;
        }
    }
}
