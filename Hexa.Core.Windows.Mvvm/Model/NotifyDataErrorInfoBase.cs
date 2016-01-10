// ----------------------------------------------------------------------------
// <copyright file="NotifyDataErrorInfoBase.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using ViewModel;

    /// <summary>
    /// Represents a base class that provides support for custom validations.
    /// </summary>
    public abstract class NotifyDataErrorInfoBase : ObservableBase, INotifyDataErrorInfo
    {
        #region Fields

        /// <summary>
        /// Defines the dictionary that contains the list of errors.
        /// </summary>
        protected readonly Dictionary<string, List<string>> Errors;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyDataErrorInfoBase"/> class.
        /// </summary>
        protected NotifyDataErrorInfoBase()
        {
            this.Errors = new Dictionary<string, List<string>>();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when the validation errors have changed.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance's content has validation errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance's content currently has validation errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors => this.Errors.Any();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the validation errors for a specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for.</param>
        /// <returns>
        /// The validation errors for the property.
        /// </returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return (propertyName != null) && this.Errors.ContainsKey(propertyName)
                ? this.Errors[propertyName]
                : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Clears the errors.
        /// </summary>
        protected void ClearErrors()
        {
            this.Errors.Keys
                .ToList()
                .ForEach(
                    propertyName =>
                    {
                        this.Errors.Remove(propertyName);
                        this.OnErrorsChanged(propertyName);
                    });
        }

        /// <summary>
        /// Raises the ErrorChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}