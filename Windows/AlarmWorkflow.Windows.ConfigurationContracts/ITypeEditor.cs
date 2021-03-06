﻿using System;
using System.Windows;

namespace AlarmWorkflow.Windows.ConfigurationContracts
{
    /// <summary>
    /// Defines a means for an editor that can edit an object of a given type.
    /// </summary>
    public interface ITypeEditor
    {
        /// <summary>
        /// Gets/sets the value that is edited.
        /// </summary>
        object Value { get; set; }
        /// <summary>
        /// Gets the visual element that is editing the value.
        /// </summary>
        UIElement Visual { get; }

        /// <summary>
        /// Initializes this TypeEditor-instance.
        /// </summary>
        /// <param name="editorParameter">An optional editor parameter.</param>
        void Initialize(string editorParameter);
    }
}
