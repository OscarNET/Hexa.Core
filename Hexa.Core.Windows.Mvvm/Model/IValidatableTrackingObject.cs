// ----------------------------------------------------------------------------
// <copyright file="IValidatableTrackingObject.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System.ComponentModel;

    /// <summary>
    /// Defines a contract that provides support for tracking changes in objects.
    /// </summary>
    public interface IValidatableTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance's content is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance's content currently is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }

        #endregion Properties
    }
}