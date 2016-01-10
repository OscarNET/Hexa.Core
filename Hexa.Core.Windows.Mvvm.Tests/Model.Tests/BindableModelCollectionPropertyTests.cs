// ----------------------------------------------------------------------------
// <copyright file="BindableModelCollectionPropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the collection property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelCollectionPropertyTests
    {
        #region Fields

        /// <summary>
        /// Defines the customer model.
        /// </summary>
        private FakeCustomer customerModel;

        /// <summary>
        /// Defines the email model.
        /// </summary>
        private FakeEmail emailModel;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.emailModel = new FakeEmail()
            {
                Comment = "This is the secondary email address",
                Email = "noname@nodomain.com",
                Id = Guid.ParseExact("706fd94d-58f9-44da-9a1b-c64bd111f6f5", "D")
            };

            this.customerModel = FakeCustomer.CreateSameForTests();
            this.customerModel.Emails.Add(this.emailModel);
        }

        /// <summary>
        /// Test: Should be synchronized after adding email.
        /// </summary>
        [TestMethod]
        public void ShouldBeInSyncAfterAddingEmail()
        {
            // Arrange
            this.customerModel.Emails.Remove(this.emailModel);
            var wrappedModel = new BindableCustomer(this.customerModel);

            // Act
            wrappedModel.Emails.Add(new BindableEmail(this.emailModel));

            // Assert
            this.AssertIfModelEmailsCollectionIsInSync(wrappedModel);
        }

        /// <summary>
        /// Test: Should be in synchronized after clearing emails.
        /// </summary>
        [TestMethod]
        public void ShouldBeInSyncAfterClearingEmails()
        {
            // Arrange
            var wrappedModel = new BindableCustomer(this.customerModel);

            // Act
            wrappedModel.Emails.Clear();

            // Assert
            this.AssertIfModelEmailsCollectionIsInSync(wrappedModel);
        }

        /// <summary>
        /// Test: Should be synchronized after removing email.
        /// </summary>
        [TestMethod]
        public void ShouldBeInSyncAfterRemovingEmail()
        {
            // Arrange
            var wrappedModel = new BindableCustomer(this.customerModel);

            // Act
            var emailToRemove = wrappedModel.Emails.Single(wrappedEmail => (wrappedEmail.Model == this.emailModel));
            wrappedModel.Emails.Remove(emailToRemove);

            // Assert
            this.AssertIfModelEmailsCollectionIsInSync(wrappedModel);
        }

        /// <summary>
        /// Test: Should initialize the emails property.
        /// </summary>
        [TestMethod]
        public void ShouldInitializeEmailsProperty()
        {
            // Arrange
            var model = this.customerModel;

            // Act
            var wrappedModel = new BindableCustomer(model);

            // Assert
            Assert.IsNotNull(wrappedModel.Emails);
            this.AssertIfModelEmailsCollectionIsInSync(wrappedModel);
        }

        /// <summary>
        /// Checks if the model emails collection is synchronized.
        /// </summary>
        /// <param name="wrappedModel">The wrapped model.</param>
        private void AssertIfModelEmailsCollectionIsInSync(BindableCustomer wrappedModel)
        {
            Assert.AreEqual(this.customerModel.Emails.Count, wrappedModel.Emails.Count);

            Assert.IsTrue(
                this.customerModel.Emails.All(
                    emailModel => wrappedModel.Emails.Any(wrappedEMail => (wrappedEMail.Model == emailModel))));
        }

        #endregion Methods
    }
}