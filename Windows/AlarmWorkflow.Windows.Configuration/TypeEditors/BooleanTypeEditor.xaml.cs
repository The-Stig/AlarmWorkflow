﻿using System.Windows.Controls;
using AlarmWorkflow.Shared.Core;
using AlarmWorkflow.Windows.ConfigurationContracts;

namespace AlarmWorkflow.Windows.Configuration.TypeEditors
{
    /// <summary>
    /// Interaction logic for BooleanTypeEditorVisual.xaml
    /// </summary>
    [Export("BooleanTypeEditor", typeof(ITypeEditor))]
    [ConfigurationTypeEditor(typeof(System.Boolean))]
    public partial class BooleanTypeEditor : UserControl, ITypeEditor
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanTypeEditor"/> class.
        /// </summary>
        public BooleanTypeEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region ITypeEditor Members

        /// <summary>
        /// Gets/sets the value that is edited.
        /// </summary>
        public object Value
        {
            get { return chkValue.IsChecked; }
            set { chkValue.IsChecked = (bool)value; }
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
    }
}
