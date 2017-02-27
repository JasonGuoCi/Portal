using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Localization.Dictionaries
{
    public interface ILocalizationDictionaryProvider
    {
        ILocalizationDictionary DefaultDictionary { get; }

        IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        void Initialize(string sourceName);

        void Extend(ILocalizationDictionary dictionary);
    }
}
