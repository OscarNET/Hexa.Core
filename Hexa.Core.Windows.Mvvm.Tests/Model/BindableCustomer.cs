// ----------------------------------------------------------------------------
// <copyright file="BindableCustomer.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Represents a wrapped customer for testing purposes.
    /// </summary>
    public class BindableCustomer : BindableModel<FakeCustomer>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableCustomer"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public BindableCustomer(FakeCustomer customer)
            : base(customer)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public BindableAddress Address
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>
        /// The birthday.
        /// </value>
        public DateTime? Birthday
        {
            get { return this.GetValue<DateTime?>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the birthday is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the birthday is changed; otherwise, <c>false</c>.
        /// </value>
        public bool BirthdayIsChanged => this.GetIsChanged(nameof(Birthday));

        /// <summary>
        /// Gets the birthday original value.
        /// </summary>
        /// <value>
        /// The birthday original value.
        /// </value>
        public DateTime? BirthdayOriginalValue => this.GetOriginalValue<DateTime?>(nameof(Birthday));

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        public ChangeTrackingCollection<BindableEmail> Emails
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the first name is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the first name is changed; otherwise, <c>false</c>.
        /// </value>
        public bool FirstNameIsChanged => this.GetIsChanged(nameof(FirstName));

        /// <summary>
        /// Gets the first name original value.
        /// </summary>
        /// <value>
        /// The first name original value.
        /// </value>
        public string FirstNameOriginalValue => this.GetOriginalValue<string>(nameof(FirstName));

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
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the last name is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the last name is changed; otherwise, <c>false</c>.
        /// </value>
        public bool LastNameIsChanged => this.GetIsChanged(nameof(LastName));

        /// <summary>
        /// Gets the last name original value.
        /// </summary>
        /// <value>
        /// The last name original value.
        /// </value>
        public string LastNameOriginalValue => this.GetOriginalValue<string>(nameof(LastName));

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
            if (string.IsNullOrWhiteSpace(this.FirstName))
            {
                yield return new ValidationResult("The firstname is mandatory!", new[] { nameof(FirstName) });
            }

            if (this.Emails.Count == 0)
            {
                yield return new ValidationResult("A customer must have an email!", new[] { nameof(Emails) });
            }
        }

        /// <summary>
        /// Initializes the collection properties of the wrapped model.
        /// </summary>
        protected override void InitializeCollectionProperties()
        {
            Guard.Against<ArgumentException>(this.Model.Emails == null, "The emails are mandatory!");

            this.Emails = new ChangeTrackingCollection<BindableEmail>(
                this.Model.Emails.Select(email => new BindableEmail(email)));
            this.RegisterCollection(this.Emails, this.Model.Emails);
        }

        /// <summary>
        /// Initializes the complex properties of the wrapped model.
        /// </summary>
        protected override void InitializeComplexProperties()
        {
            Guard.Against<ArgumentException>(this.Model.Address == null, "The address is mandatory!");

            this.Address = new BindableAddress(this.Model.Address);
            this.RegisterComplex(this.Address);
        }

        #endregion Methods
    }
}