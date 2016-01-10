// ----------------------------------------------------------------------------
// <copyright file="FakeCustomer.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a fake customer for testing purposes.
    /// </summary>
    public class FakeCustomer
    {
        #region Fields

        /// <summary>
        /// Defines the default address city.
        /// </summary>
        public const string DefaultAddressCity = "Madrid";

        /// <summary>
        /// Defines the default email address.
        /// </summary>
        public const string DefaultEmailAddress = "fakecontact@fakedomain.com";

        /// <summary>
        /// Defines the default first name.
        /// </summary>
        public const string DefaultFirstName = "HexaSystems Inc";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public FakeAddress Address
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>
        /// The birthday.
        /// </value>
        public DateTime? Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        public List<FakeEmail> Emails
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates the same fake customer for tests.
        /// </summary>
        /// <returns>
        /// The created same fake customer.
        /// </returns>
        public static FakeCustomer CreateSameForTests()
        {
            return new FakeCustomer()
            {
                Address = new FakeAddress()
                {
                    City = FakeCustomer.DefaultAddressCity,
                    Id = Guid.ParseExact("e9305aea-cd70-4ebb-bb27-a0c7ab989bde", "D"),
                    Street = "Spain Square"
                },
                Emails = new List<FakeEmail>()
                {
                    new FakeEmail()
                    {
                        Comment = "This is the principal email address",
                        Email = FakeCustomer.DefaultEmailAddress,
                        Id = Guid.ParseExact("478203bf-3703-4025-aa61-1cd295c03d9f", "D")
                    }
                },
                FirstName = FakeCustomer.DefaultFirstName,
                Id = Guid.ParseExact("967d31ec-f2cb-495e-a92d-b5d77bbb0daf", "D")
            };
        }

        #endregion Methods
    }
}