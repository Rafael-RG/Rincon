using MobileTemplate.Common.Interfaces;
using Microsoft.Extensions.Localization;
using MobileTemplate.Helpers;
using MobileTemplate.Resources.Strings;

namespace MobileTemplate.Services
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
