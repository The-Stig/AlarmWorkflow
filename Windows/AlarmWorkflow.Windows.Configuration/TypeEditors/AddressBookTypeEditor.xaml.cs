using System.Collections.Generic;
using System.Windows.Controls;
using AlarmWorkflow.Shared.Addressing;
using AlarmWorkflow.Shared.Core;
using AlarmWorkflow.Windows.ConfigurationContracts;

namespace AlarmWorkflow.Windows.Configuration.TypeEditors
{
    /// <summary>
    /// Interaction logic for AddressBookTypeEditor.xaml
    /// </summary>
    [Export("AddressBookTypeEditor", typeof(ITypeEditor))]
    public partial class AddressBookTypeEditor : UserControl, ITypeEditor
    {
        #region Static

        private static readonly List<IAddressProvider> _providers;

        static AddressBookTypeEditor()
        {
            _providers = ExportedTypeLibrary.ImportAll<IAddressProvider>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of entries to show in the UI.
        /// </summary>
        public List<EntryVM> Entries { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookTypeEditor"/> class.
        /// </summary>
        public AddressBookTypeEditor()
        {
            InitializeComponent();

            Entries = new List<EntryVM>();

            this.DataContext = this;
        }

        #endregion

        #region ITypeEditor Members

        object ITypeEditor.Value
        {
            get
            {
                return null;
                //return _addressBook.ToXml();
            }
            set
            {
                AddressBook addressBook = AddressBook.Parse((string)value);
                foreach (AddressBookEntry abe in addressBook.Entries)
                {
                    EntryVM entry = new EntryVM();
                    entry.Name = abe.Name;

                    Entries.Add(entry);
                }
            }
        }

        /// <summary>
        /// Gets the visual element that is editing the value.
        /// </summary>
        public System.Windows.UIElement Visual
        {
            get { return this; }
        }

        void ITypeEditor.Initialize(string editorParameter)
        {

        }

        #endregion

        #region Nested types

        public class EntryVM
        {
            public string Name { get; set; }
            public List<RowDetailVM> Rows { get; set; }

            public EntryVM()
            {
                Rows = new List<RowDetailVM>();
            }
        }

        public class RowDetailVM
        {
            public bool IsEnabled { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion
    }
}
