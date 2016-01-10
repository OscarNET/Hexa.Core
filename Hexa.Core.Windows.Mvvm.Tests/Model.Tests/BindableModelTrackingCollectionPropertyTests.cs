// ----------------------------------------------------------------------------
// <copyright file="BindableModelTrackingCollectionPropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the tracking collection property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelTrackingCollectionPropertyTests
    {
        #region Fields

        /// <summary>
        /// Defines the list of the wrapped emails.
        /// </summary>
        private List<BindableEmail> wrappedEmails;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var emailModelA = new FakeEmail()
            {
                Comment = "This is the principal email address",
                Email = FakeCustomer.DefaultEmailAddress,
                Id = Guid.ParseExact("478203bf-3703-4025-aa61-1cd295c03d9f", "D")
            };

            var emailModelB = new FakeEmail()
            {
                Comment = "This is the secondary email address",
                Email = "noname@nodomain.com",
                Id = Guid.ParseExact("706fd94d-58f9-44da-9a1b-c64bd111f6f5", "D")
            };

            this.wrappedEmails = new List<BindableEmail>()
            {
                new BindableEmail(emailModelA),
                new BindableEmail(emailModelB)
            };
        }

        /// <summary>
        /// Test: Should accept changes for the items changes.
        /// </summary>
        [TestMethod]
        public void ShouldAcceptChangesForItemsChanges()
        {
            // Arrange
            var emailToModify = this.wrappedEmails.First();
            var emailToRemove = this.wrappedEmails.Skip(1).First();
            var emailToAdd = new BindableEmail(
                new FakeEmail()
                {
                    Comment = "A new email address",
                    Email = "newaddress@nodomain.com",
                    Id = Guid.NewGuid()
                });

            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Act
            collection.Add(emailToAdd);
            collection.Remove(emailToRemove);
            emailToModify.Email = "modified@fakedomain.com";

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, emailToModify.EmailOriginalValue);

            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(1, collection.AddedItems.Count);
            Assert.AreEqual(1, collection.ModifiedItems.Count);
            Assert.AreEqual(1, collection.RemovedItems.Count);

            collection.AcceptChanges();

            Assert.AreEqual(2, collection.Count);
            Assert.IsTrue(collection.Contains(emailToModify));
            Assert.IsTrue(collection.Contains(emailToAdd));

            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);

            Assert.IsFalse(emailToModify.IsChanged);
            Assert.AreEqual("modified@fakedomain.com", emailToModify.Email);
            Assert.AreEqual("modified@fakedomain.com", emailToModify.EmailOriginalValue);

            Assert.IsFalse(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should not track added item as modified.
        /// </summary>
        [TestMethod]
        public void ShouldNotTrackAddedItemAsModified()
        {
            // Arrange
            var emailToAdd = new BindableEmail(
                new FakeEmail()
                {
                    Comment = "A new email address",
                    Email = "newaddress@nodomain.com",
                    Id = Guid.NewGuid()
                });

            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);
            collection.Add(emailToAdd);

            // Act
            emailToAdd.Email = "modified@fakedomain.com";

            // Assert
            Assert.IsTrue(emailToAdd.IsChanged);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(1, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.IsTrue(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should not track removed item as modified.
        /// </summary>
        [TestMethod]
        public void ShouldNotTrackRemovedItemAsModified()
        {
            // Arrange
            var emailToModifyAndRemove = this.wrappedEmails.First();
            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Act
            emailToModifyAndRemove.Email = "modified@fakedomain.com";

            // Assert
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.AreEqual(1, collection.ModifiedItems.Count);
            Assert.AreEqual(emailToModifyAndRemove, collection.ModifiedItems.First());
            Assert.IsTrue(collection.IsChanged);

            collection.Remove(emailToModifyAndRemove);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(1, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(emailToModifyAndRemove, collection.RemovedItems.First());
            Assert.IsTrue(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should reject changes for the items changes.
        /// </summary>
        [TestMethod]
        public void ShouldRejectChangesForItemsChanges()
        {
            // Arrange
            var emailToModify = this.wrappedEmails.First();
            var emailToRemove = this.wrappedEmails.Skip(1).First();
            var emailToAdd = new BindableEmail(
                new FakeEmail()
                {
                    Comment = "A new email address",
                    Email = "newaddress@nodomain.com",
                    Id = Guid.NewGuid()
                });

            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Act
            collection.Add(emailToAdd);
            collection.Remove(emailToRemove);
            emailToModify.Email = "modified@fakedomain.com";

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, emailToModify.EmailOriginalValue);

            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(1, collection.AddedItems.Count);
            Assert.AreEqual(1, collection.ModifiedItems.Count);
            Assert.AreEqual(1, collection.RemovedItems.Count);

            collection.RejectChanges();

            Assert.AreEqual(2, collection.Count);
            Assert.IsTrue(collection.Contains(emailToModify));
            Assert.IsTrue(collection.Contains(emailToRemove));

            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);

            Assert.IsFalse(emailToModify.IsChanged);
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, emailToModify.Email);
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, emailToModify.EmailOriginalValue);

            Assert.IsFalse(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should reject changes for the items changes with modified and removed item.
        /// </summary>
        [TestMethod]
        public void ShouldRejectChangesForItemsChangesWithModifiedAndRemovedItem()
        {
            // Arrange
            var email = this.wrappedEmails.First();
            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Act
            email.Email = "modified@fakedomain.com";
            collection.Remove(email);

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, email.EmailOriginalValue);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(1, collection.RemovedItems.Count);

            collection.RejectChanges();

            Assert.AreEqual(2, collection.Count);
            Assert.IsTrue(collection.Contains(email));

            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);

            Assert.IsFalse(email.IsChanged);
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, email.Email);
            Assert.AreEqual(FakeCustomer.DefaultEmailAddress, email.EmailOriginalValue);

            Assert.IsFalse(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should track added items.
        /// </summary>
        [TestMethod]
        public void ShouldTrackAddedItems()
        {
            // Arrange
            var emailToAdd = new BindableEmail(
                new FakeEmail()
                {
                    Comment = "A new email address",
                    Email = "newaddress@nodomain.com",
                    Id = Guid.NewGuid()
                });

            // Act
            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Assert
            Assert.AreEqual(2, collection.Count);
            Assert.IsFalse(collection.IsChanged);

            collection.Add(emailToAdd);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(1, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(emailToAdd, collection.AddedItems.First());
            Assert.IsTrue(collection.IsChanged);

            collection.Remove(emailToAdd);
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.IsFalse(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should track modified item.
        /// </summary>
        [TestMethod]
        public void ShouldTrackModifiedItem()
        {
            // Arrange
            var emailToModify = this.wrappedEmails.First();

            // Act
            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Assert
            Assert.AreEqual(2, collection.Count);
            Assert.IsFalse(collection.IsChanged);

            emailToModify.Email = "modified@fakedomain.com";
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(1, collection.ModifiedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.IsTrue(collection.IsChanged);

            emailToModify.Email = FakeCustomer.DefaultEmailAddress;
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.IsFalse(collection.IsChanged);
        }

        /// <summary>
        /// Test: Should track removed items.
        /// </summary>
        [TestMethod]
        public void ShouldTrackRemovedItems()
        {
            // Arrange
            var emailToRemove = this.wrappedEmails.First();

            // Act
            var collection = new ChangeTrackingCollection<BindableEmail>(this.wrappedEmails);

            // Assert
            Assert.AreEqual(2, collection.Count);
            Assert.IsFalse(collection.IsChanged);

            collection.Remove(emailToRemove);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(1, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.AreEqual(emailToRemove, collection.RemovedItems.First());
            Assert.IsTrue(collection.IsChanged);

            collection.Add(emailToRemove);
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(0, collection.AddedItems.Count);
            Assert.AreEqual(0, collection.RemovedItems.Count);
            Assert.AreEqual(0, collection.ModifiedItems.Count);
            Assert.IsFalse(collection.IsChanged);
        }

        #endregion Methods
    }
}