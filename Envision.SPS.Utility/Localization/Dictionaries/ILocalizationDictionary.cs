using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Localization.Dictionaries
{
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// Culture of the dictionary.
        /// </summary>
        CultureInfo CultureInfo { get; }

        /// <summary>
        /// Gets/sets a string for this dictionary with given name (key).
        /// </summary>
        /// <param name="name">Name to get/set</param>
        string this[string name] { get; set; }

        /// <summary>
        /// Gets a <see cref="LocalizedString"/> for given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name (key) to get localized string</param>
        /// <returns>The localized string or null if not found in this dictionary</returns>
        LocalizedString GetOrNull(string name);

        IList<LocalizedString> GetAllStrings();
    }
}
