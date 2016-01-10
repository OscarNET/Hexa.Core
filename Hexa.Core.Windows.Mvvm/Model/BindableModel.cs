// ----------------------------------------------------------------------------
// <copyright file="BindableModel.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a wrapper class that provides support two-way data binding for models
    /// (Also provides support for custom validations and for tracking changes).
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class BindableModel<TModel> : NotifyDataErrorInfoBase, IValidatableTrackingObject, IValidatableObject
    {
        #region Fields

        /// <summary>
        /// Defines the original properties' values.
        /// </summary>
        private Dictionary<string, object> originalValues;

        /// <summary>
        /// Defines the tracking objects
        /// </summary>
        private List<IValidatableTrackingObject> trackingObjects;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableModel{TModel}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public BindableModel(TModel model)
        {
            Guard.IsNotNull(model, "model");
            this.Model = model;

            this.originalValues = new Dictionary<string, object>();
            this.trackingObjects = new List<IValidatableTrackingObject>();

            this.InitializeComplexProperties();
            this.InitializeCollectionProperties();
            this.Validate();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the wrapped model is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the wrapped model is changed since the last call to AcceptChanges; otherwise, <c>false</c>.
        /// </value>
        public bool IsChanged => (this.originalValues.Count > 0) || this.trackingObjects.Any(t => t.IsChanged);

        /// <summary>
        /// Gets a value indicating whether all properties of the wrapped model are valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if all properties of the wrapped model currently are valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid => !this.HasErrors && this.trackingObjects.All(t => t.IsValid);

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public TModel Model
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Accepts the changes of the wrapped model's properties.
        /// </summary>
        public void AcceptChanges()
        {
            this.originalValues.Clear();
            this.trackingObjects.ForEach(trackingObject => trackingObject.AcceptChanges());

            this.OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// Rejects the changes of the wrapped model's properties.
        /// </summary>
        public void RejectChanges()
        {
            this.originalValues.ForEach(
                originalValue =>
                {
                    PropertyInfo propertyInfo = typeof(TModel).GetProperty(originalValue.Key);
                    propertyInfo.SetValue(this.Model, originalValue.Value);
                });

            this.originalValues.Clear();
            this.trackingObjects.ForEach(trackingObject => trackingObject.RejectChanges());

            this.Validate();
            this.OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// Validates the wrapped model's properties.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

        /// <summary>
        /// Gets a value indicating whether the specified property is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///   <c>true</c> if the specified property is changed; otherwise, <c>false</c>.
        /// </returns>
        protected bool GetIsChanged(string propertyName)
        {
            Guard.IsNotNullNorEmpty(propertyName, "The property name is mandatory!");

            return this.originalValues.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets the original value of the specified property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The original value of the specified property.
        /// </returns>
        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            Guard.IsNotNullNorEmpty(propertyName, "The property name is mandatory!");

            return this.originalValues.ContainsKey(propertyName)
              ? (TValue)this.originalValues[propertyName]
              : this.GetValue<TValue>(propertyName);
        }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The value of the specified property.
        /// </returns>
        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            Guard.IsNotNullNorEmpty(propertyName, "The property name is mandatory!");

            PropertyInfo propertyInfo = this.Model.GetType().GetProperty(propertyName);
            TValue propertyValue = (TValue)propertyInfo.GetValue(this.Model);

            return propertyValue;
        }

        /// <summary>
        /// Initializes the collection properties of the wrapped model.
        /// </summary>
        protected virtual void InitializeCollectionProperties()
        {
        }

        /// <summary>
        /// Initializes the complex properties of the wrapped model.
        /// </summary>
        protected virtual void InitializeComplexProperties()
        {
        }

        /// <summary>
        /// Registers the specified collection.
        /// </summary>
        /// <typeparam name="TWrappedCollection">The type of the wrapped collection.</typeparam>
        /// <typeparam name="TCollection">The type of the collection.</typeparam>
        /// <param name="wrapperCollection">The wrapper collection.</param>
        /// <param name="collection">The collection.</param>
        protected void RegisterCollection<TWrappedCollection, TCollection>(
                    ChangeTrackingCollection<TWrappedCollection> wrapperCollection,
                    List<TCollection> collection)
                    where TWrappedCollection : BindableModel<TCollection>
        {
            wrapperCollection.CollectionChanged +=
                (sender, e) =>
                {
                    collection.Clear();
                    collection.AddRange(wrapperCollection.Select(w => w.Model));
                    this.Validate();
                };

            this.RegisterTrackingObject(wrapperCollection);
        }

        /// <summary>
        /// Registers the specified wrapped model.
        /// </summary>
        /// <typeparam name="T">The type of the model.</typeparam>
        /// <param name="wrappedModel">The wrapped model.</param>
        protected void RegisterComplex<T>(BindableModel<T> wrappedModel)
        {
            this.RegisterTrackingObject(wrappedModel);
        }

        /// <summary>
        /// Sets the value of the specified property.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetValue<TValue>(TValue newValue, [CallerMemberName] string propertyName = null)
        {
            Guard.IsNotNullNorEmpty(propertyName, "The property name is mandatory!");

            PropertyInfo propertyInfo = this.Model.GetType().GetProperty(propertyName);
            object currentValue = propertyInfo.GetValue(this.Model);

            if (!object.Equals(currentValue, newValue))
            {
                this.UpdateOriginalValue(currentValue, newValue, propertyName);
                propertyInfo.SetValue(this.Model, newValue);

                this.Validate();
                this.OnPropertyChanged(propertyName);
                this.OnPropertyChanged(propertyName + nameof(IsChanged));
            }
        }

        /// <summary>
        /// Registers the specified tracking object.
        /// </summary>
        /// <param name="trackingObject">The tracking object.</param>
        private void RegisterTrackingObject(IValidatableTrackingObject trackingObject)
        {
            if (!this.trackingObjects.Contains(trackingObject))
            {
                this.trackingObjects.Add(trackingObject);
                trackingObject.PropertyChanged += this.OnTrackingObjectPropertyChanged;
            }
        }

        /// <summary>
        /// Called when the tracking object's property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        private void OnTrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged))
            {
                this.OnPropertyChanged(nameof(IsChanged));
            }
            else if (e.PropertyName == nameof(IsValid))
            {
                this.OnPropertyChanged(nameof(IsValid));
            }
        }

        /// <summary>
        /// Updates the original value of the specified property.
        /// </summary>
        /// <param name="currentValue">The current value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Name of the property.</param>
        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            Guard.IsNotNullNorEmpty(propertyName, "The property name is mandatory!");

            if (!this.originalValues.ContainsKey(propertyName))
            {
                this.originalValues.Add(propertyName, currentValue);
                this.OnPropertyChanged(nameof(IsChanged));
            }
            else
            {
                if (object.Equals(this.originalValues[propertyName], newValue))
                {
                    this.originalValues.Remove(propertyName);
                    this.OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        /// <summary>
        /// Validates the wrapped model's properties.
        /// </summary>
        private void Validate()
        {
            ValidationContext context = new ValidationContext(this);
            List<ValidationResult> results = new List<ValidationResult>();

            this.ClearErrors();
            Validator.TryValidateObject(this, context, results, true);

            if (results.Any())
            {
                results
                    .SelectMany(validationResult => validationResult.MemberNames)
                    .Distinct()
                    .ForEach(
                        propertyName =>
                        {
                            this.Errors[propertyName] = results
                                .Where(validationResult => validationResult.MemberNames.Contains(propertyName))
                                .Select(validationResult => validationResult.ErrorMessage)
                                .Distinct()
                                .ToList();

                            this.OnErrorsChanged(propertyName);
                        });
            }

            this.OnPropertyChanged(nameof(IsValid));
        }

        #endregion Methods
    }
}