using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AlarmWorkflow.Shared.Core;
using AlarmWorkflow.Shared.Diagnostics;
using AlarmWorkflow.Shared.Settings;

namespace AlarmWorkflow.Shared.Addressing
{
    /// <summary>
    /// Implements the <see cref="IAddressBook"/>-interface.
    /// </summary>
    public sealed class AddressBook : IAddressBook, IStringSettingConvertible
    {
        #region Fields

        private List<IAddressProvider> _addressProviders;

        private List<AddressBookEntry> _entries;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of entries in this address book.
        /// </summary>
        public IList<AddressBookEntry> Entries
        {
            get { return _entries; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AddressBook"/> class from being created.
        /// </summary>
        private AddressBook()
        {
            _entries = new List<AddressBookEntry>();

            _addressProviders = new List<IAddressProvider>();
            _addressProviders.AddRange(ExportedTypeLibrary.ImportAll<IAddressProvider>());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses the given (XML) string into a corresponding <see cref="AddressBook"/>-instance.
        /// </summary>
        /// <param name="xmlContent"></param>
        /// <returns></returns>
        public static AddressBook Parse(string xmlContent)
        {
            AddressBook addressBook = new AddressBook();
            ((IStringSettingConvertible)addressBook).Convert(xmlContent);
            return addressBook;
        }

        private IAddressProvider GetAddressProvider(string type)
        {
            return _addressProviders.Find(p => p.AddressType == type);
        }

        private static bool IsEnabled(XElement customElementE)
        {
            return customElementE.TryGetAttributeValue("IsEnabled", true);
        }

        /// <summary>
        /// Creates an XML-document that describes this instance.
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("AddressBook");
            doc.Add(root);

            foreach (AddressBookEntry entry in _entries)
            {
                XElement entryE = new XElement("Entry");
                entryE.Add(new XAttribute("Name", entry.Name));

                foreach (var customData in entry.CustomData)
                {
                    IAddressProvider provider = GetAddressProvider(customData.Key);
                    XElement customDataE = provider.GetXmlData(customData.Value);
                    entryE.Add(customDataE);
                }

                root.Add(entryE);
            }

            return doc.ToString();
        }

        #endregion

        #region IAddressBook Members

        IEnumerable<AddressBookEntry> IAddressBook.GetEntries()
        {
            return _entries;
        }

        IEnumerable<Tuple<AddressBookEntry, TCustomData>> IAddressBook.GetCustomObjects<TCustomData>(string type)
        {
            foreach (AddressBookEntry entry in _entries)
            {
                object customData = null;
                if (!entry.CustomData.TryGetValue(type, out customData))
                {
                    continue;
                }
                yield return Tuple.Create<AddressBookEntry, TCustomData>(entry, (TCustomData)customData);
            }
        }

        #endregion

        #region IStringSettingConvertible Members

        void IStringSettingConvertible.Convert(string settingValue)
        {
            Logger.Instance.LogFormat(LogType.Debug, this, Properties.Resources.AddressBook_StartScanMessage);

            // Parse document
            XDocument doc = XDocument.Parse(settingValue);

            foreach (XElement entryE in doc.Root.Elements("Entry"))
            {
                AddressBookEntry entry = new AddressBookEntry();
                entry.Name = entryE.TryGetAttributeValue("Name", null);

                // Find all other custom attributes
                foreach (XElement customElementE in entryE.Elements())
                {
                    string providerType = customElementE.Name.LocalName;

                    IAddressProvider provider = GetAddressProvider(providerType);
                    if (provider == null)
                    {
                        continue;
                    }

                    if (!IsEnabled(customElementE))
                    {
                        continue;
                    }

                    object customObject = provider.ParseXElement(customElementE);
                    if (customObject == null)
                    {
                        continue;
                    }

                    entry.CustomData[providerType] = customObject;
                }

                _entries.Add(entry);
            }

            Logger.Instance.LogFormat(LogType.Debug, this, Properties.Resources.AddressBook_FinishScanMessage, _entries.Count);
        }

        string IStringSettingConvertible.ConvertBack()
        {
            return ToXml();
        }

        #endregion
    }
}
