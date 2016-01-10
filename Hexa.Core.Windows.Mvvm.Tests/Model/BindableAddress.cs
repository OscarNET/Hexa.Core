// ----------------------------------------------------------------------------
// <copyright file="BindableAddress.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a wrapped address for testing purposes.
    /// </summary>
    public class BindableAddress : BindableModel<FakeAddress>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableAddress"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public BindableAddress(FakeAddress address)
            : base(address)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the city is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the city is changed; otherwise, <c>false</c>.
        /// </value>
        public bool CityIsChanged => this.GetIsChanged(nameof(City));

        /// <summary>
        /// Gets the city original value.
        /// </summary>
        /// <value>
        /// The city original value.
        /// </value>
        public string CityOriginalValue => this.GetOriginalValue<string>(nameof(City));

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id
        {
            get { return this.GetValue<Guid>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the identifier is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the identifier is changed; otherwise, <c>false</c>.
        /// </value>
        public bool IdIsChanged => this.GetIsChanged(nameof(Id));

        /// <summary>
        /// Gets the identifier original value.
        /// </summary>
        /// <value>
        /// The identifier original value.
        /// </value>
        public Guid IdOriginalValue => this.GetOriginalValue<Guid>(nameof(Id));

        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        public string Street
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the street is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the street is changed; otherwise, <c>false</c>.
        /// </value>
        public bool StreetIsChanged => this.GetIsChanged(nameof(Street));

        /// <summary>
        /// Gets the street original value.
        /// </summary>
        /// <value>
        /// The street original value.
        /// </value>
        public string StreatOriginalValue => this.GetOriginalValue<string>(nameof(Street));

        #endregion Properties

        #region Methods

        /// <summary>
        /// Validates the wrapped model's properties.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.City))
            {
                yield return new ValidationResult("The city is mandatory!", new[] { nameof(City) });
            }
        }

        #endregion Methods
    }
}