using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Localization.Dictionaries.Xml
{
    public class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
    {

        public string SourceName { get; private set; }

        public ILocalizationDictionary DefaultDictionary { get; protected set; }

        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; private set; }

        protected LocalizationDictionaryProviderBase()
        {
            Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        }

        public virtual void Initialize(string sourceName)
        {
            SourceName = sourceName;
        }

        public void Extend(ILocalizationDictionary dictionary)
        {
            //Add
            ILocalizationDictionary existingDictionary;
            if (!Dictionaries.TryGetValue(dictionary.CultureInfo.Name, out existingDictionary))
            {
                Dictionaries[dictionary.CultureInfo.Name] = dictionary;
                return;
            }

            //Override
            var localizedStrings = dictionary.GetAllStrings();
            foreach (var localizedString in localizedStrings)
            {
                existingDictionary[localizedString.Name] = localizedString.Value;
            }
        }
    }
}
