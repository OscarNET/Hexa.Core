// ----------------------------------------------------------------------------
// <copyright file="ChangeTrackingCollection.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Represents a collection that provides support for tracking changes on its items.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ChangeTrackingCollection<TItem> : ObservableCollection<TItem>, IValidatableTrackingObject
        where TItem : class, IValidatableTrackingObject
    {
        #region Fields

        /// <summary>
        /// Defines the collection of the added items.
        /// </summary>
        private ObservableCollection<TItem> addedItems;

        /// <summary>
        /// Defines the collection of the modified items.
        /// </summary>
        private ObservableCollection<TItem> modifiedItems;

        /// <summary>
        /// Defines the original collection.
        /// </summary>
        private IList<TItem> originalCollection;

        /// <summary>
        /// Defines the collection of the removed items.
        /// </summary>
        private ObservableCollection<TItem> removedItems;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTrackingCollection{TItem}"/> class.
        /// </summary>
        /// <param name="items">Sequence of items.</param>
        public ChangeTrackingCollection(IEnumerable<TItem> items)
            : base(items)
        {
            this.originalCollection = this.ToList();

            this.AttachPropertyChangedHandler(this.originalCollection);

            this.addedItems = new ObservableCollection<TItem>();
            this.AddedItems = new ReadOnlyObservableCollection<TItem>(this.addedItems);

            this.modifiedItems = new ObservableCollection<TItem>();
            this.ModifiedItems = new ReadOnlyObservableCollection<TItem>(this.modifiedItems);

            this.removedItems = new ObservableCollection<TItem>();
            this.RemovedItems = new ReadOnlyObservableCollection<TItem>(this.removedItems);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the read-only collection of the added items.
        /// </summary>
        /// <value>
        /// The read-only collection of the added items.
        /// </value>
        public ReadOnlyObservableCollection<TItem> AddedItems
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this collection is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this collection is changed since the last call to AcceptChanges; otherwise, <c>false</c>.
        /// </value>
        public bool IsChanged
        {
            get
            {
                return
                    (this.AddedItems.Count > 0) ||
                    (this.ModifiedItems.Count > 0) ||
                    (this.RemovedItems.Count > 0);
            }
        }

        /// <summary>
        /// Gets a value indicating whether all items of this collection are valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all items of this collection currently are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid
        {
            get
            {
                return this.All(item => item.IsValid);
            }
        }

        /// <summary>
        /// Gets the read-only collection of the modified items.
        /// </summary>
        /// <value>
        /// The read-only collection of the modified items.
        /// </value>
        public ReadOnlyObservableCollection<TItem> ModifiedItems
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the read-only collection of the removed items.
        /// </summary>
        /// <value>
        /// The read-only collection of the removed items.
        /// </value>
        public ReadOnlyObservableCollection<TItem> RemovedItems
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Accepts the modifications of the items.
        /// </summary>
        public void AcceptChanges()
        {
            this.addedItems.Clear();
            this.modifiedItems.Clear();
            this.removedItems.Clear();

            this.ForEach(item => item.AcceptChanges());

            this.originalCollection = this.ToList();
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }

        /// <summary>
        /// Rejects the modifications of the items.
        /// </summary>
        public void RejectChanges()
        {
            this.addedItems.ToList().ForEach(item => this.Remove(item));
            this.modifiedItems.ToList().ForEach(item => item.RejectChanges());

            this.removedItems.ToList().ForEach(
                item =>
                {
                    item.RejectChanges();
                    this.Add(item);
                });

            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var added = this.Where(item => this.originalCollection.All(sourceItem => (sourceItem != item)));
            var removed = this.originalCollection.Where(sourceItem => this.All(item => (item != sourceItem)));
            var modified = this.Except(added).Except(removed).Where(item => item.IsChanged).ToList();

            this.AttachPropertyChangedHandler(added);
            this.DetachPropertyChangedHandler(removed);

            this.UpdateCollection(this.addedItems, added);
            this.UpdateCollection(this.removedItems, removed);
            this.UpdateCollection(this.modifiedItems, modified);

            base.OnCollectionChanged(e);
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
        }

        /// <summary>
        /// Attaches the PropertyChanged event handler for each item of the given sequence.
        /// </summary>
        /// <param name="items">Sequence of items.</param>
        private void AttachPropertyChangedHandler(IEnumerable<TItem> items)
        {
            items.ForEach(item => item.PropertyChanged += this.OnItemPropertyChanged);
        }

        /// <summary>
        /// Detaches the PropertyChanged event handler for each item of the given sequence.
        /// </summary>
        /// <param name="items">Sequence of items.</param>
        private void DetachPropertyChangedHandler(IEnumerable<TItem> items)
        {
            items.ForEach(item => item.PropertyChanged -= this.OnItemPropertyChanged);
        }

        /// <summary>
        /// Called when a item's property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsValid))
            {
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsValid)));
            }
            else
            {
                TItem item = (TItem)sender;

                if (this.addedItems.Contains(item))
                {
                    return;
                }

                if (item.IsChanged)
                {
                    if (!this.modifiedItems.Contains(item))
                    {
                        this.modifiedItems.Add(item);
                    }
                }
                else
                {
                    if (this.modifiedItems.Contains(item))
                    {
                        this.modifiedItems.Remove(item);
                    }
                }

                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsChanged)));
            }
        }

        /// <summary>
        /// Updates the specified collection with a given sequence.
        /// </summary>
        /// <param name="collection">The collection to update.</param>
        /// <param name="items">Sequence of items.</param>
        private void UpdateCollection(ObservableCollection<TItem> collection, IEnumerable<TItem> items)
        {
            collection.Clear();
            items.ForEach(item => collection.Add(item));
        }

        #endregion Methods
    }
}