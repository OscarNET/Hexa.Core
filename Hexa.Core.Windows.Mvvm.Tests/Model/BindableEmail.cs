// ----------------------------------------------------------------------------
// <copyright file="BindableEmail.cs" company="HexaSystems Inc">
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
    /// Represents a wrapped email for testing purposes.
    /// </summary>
    public class BindableEmail : BindableModel<FakeEmail>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableEmail"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        public BindableEmail(FakeEmail email)
            : base(email)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the email is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the email is changed; otherwise, <c>false</c>.
        /// </value>
        public bool EmailIsChanged => this.GetIsChanged(nameof(Email));

        /// <summary>
        /// Gets the email original value.
        /// </summary>
        /// <value>
        /// The email original value.
        /// </value>
        public string EmailOriginalValue => this.GetOriginalValue<string>(nameof(Email));

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
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Gets a value indicating whether the comment is changed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the comment is changed; otherwise, <c>false</c>.
        /// </value>
        public bool CommentIsChanged => this.GetIsChanged(nameof(Comment));

        /// <summary>
        /// Gets the comment original value.
        /// </summary>
        /// <value>
        /// The comment original value.
        /// </value>
        public string CommentOriginalValue => this.GetOriginalValue<string>(nameof(Comment));

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
            if (string.IsNullOrWhiteSpace(this.Email))
            {
                yield return new ValidationResult("The email is mandatory!", new[] { nameof(Email) });
            }

            if (!new EmailAddressAttribute().IsValid(this.Email))
            {
                yield return new ValidationResult("The email is not a valid email address!", new[] { nameof(Email) });
            }
        }

        #endregion Methods
    }
}