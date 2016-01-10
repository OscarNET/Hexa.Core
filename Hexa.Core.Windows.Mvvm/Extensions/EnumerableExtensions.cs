// ----------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a helper class that provides extensions methods for sequences of items.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Methods

        /// <summary>
        /// Performs the specified action on each element of the given sequence.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="items">Sequence of items.</param>
        /// <param name="action">The action to perform on each element of the specified sequence.</param>
        public static void ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            if (items != null)
            {
                foreach (TItem item in items)
                {
                    action(item);
                }
            }
        }

        #endregion Methods
    }
}