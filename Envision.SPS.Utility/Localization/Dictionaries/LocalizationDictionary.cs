using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Localization.Dictionaries
{
    public class LocalizationDictionary : ILocalizationDictionary, IEnumerable<LocalizedString>
    {
        public CultureInfo CultureInfo { get; private set; }
        public virtual string this[string name]
        {
            get
            {
                var localizedString = GetOrNull(name);
                return localizedString == null ? null : localizedString.Value;
            }
            set
            {
                _dictionary[name] = new LocalizedString(name, value, CultureInfo);
            }
        }
        private readonly Dictionary<string, LocalizedString> _dictionary;

        /// <summary>
        /// Creates a new <see cref="LocalizationDictionary"/> object.
        /// </summary>
        /// <param name="cultureInfo">Culture of the dictionary</param>
        public LocalizationDictionary(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
            _dictionary = new Dictionary<string, LocalizedString>();
        }

        public virtual LocalizedString GetOrNull(string name)
        {
            LocalizedString localizedString;
            return _dictionary.TryGetValue(name, out localizedString) ? localizedString : null;
        }


        /// <inheritdoc/>
        public virtual IList<LocalizedString> GetAllStrings()
        {
            return _dictionary.Values.ToList();
        }

        /// <inheritdoc/>
        public virtual IEnumerator<LocalizedString> GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        protected bool Contains(string name)
        {
            return _dictionary.ContainsKey(name);
        }
    }
}
