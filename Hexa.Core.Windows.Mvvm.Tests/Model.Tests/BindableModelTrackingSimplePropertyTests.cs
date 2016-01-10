// ----------------------------------------------------------------------------
// <copyright file="BindableModelTrackingSimplePropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the tracking simple property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelTrackingSimplePropertyTests
    {
        #region Methods

        /// <summary>
        /// Test: Should accept changes.
        /// </summary>
        [TestMethod]
        public void ShouldAcceptChanges()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.FirstName = "Carlos";

            // Assert
            Assert.AreEqual("Carlos", wrappedModel.FirstName);
            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstNameOriginalValue);
            Assert.IsTrue(wrappedModel.FirstNameIsChanged);
            Assert.IsTrue(wrappedModel.IsChanged);

            wrappedModel.AcceptChanges();

            Assert.AreEqual("Carlos", wrappedModel.FirstName);
            Assert.AreEqual("Carlos", wrappedModel.FirstNameOriginalValue);
            Assert.IsFalse(wrappedModel.FirstNameIsChanged);
            Assert.IsFalse(wrappedModel.IsChanged);
        }

        /// <summary>
        /// Test: Should raise the property changed event for the first name is changed.
        /// </summary>
        [TestMethod]
        public void ShouldRaisePropertyChangedForFirstNameIsChanged()
        {
            // Arrange
            bool eventWasRaised = false;
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            wrappedModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(wrappedModel.FirstNameIsChanged))
                    {
                        eventWasRaised = true;
                    }
                };

            // Act
            wrappedModel.FirstName = "Carlos";

            // Assert
            Assert.IsTrue(eventWasRaised);
        }

        /// <summary>
        /// Test: Should raise the property changed event for the is changed.
        /// </summary>
        [TestMethod]
        public void ShouldRaisePropertyChangedForIsChanged()
        {
            // Arrange
            bool eventWasRaised = false;
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            wrappedModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(wrappedModel.IsChanged))
                    {
                        eventWasRaised = true;
                    }
                };

            // Act
            wrappedModel.FirstName = "Carlos";

            // Assert
            Assert.IsTrue(eventWasRaised);
        }

        /// <summary>
        /// Test: Should reject changes.
        /// </summary>
        [TestMethod]
        public void ShouldRejectChanges()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.FirstName = "Carlos";

            // Assert
            Assert.AreEqual("Carlos", wrappedModel.FirstName);
            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstNameOriginalValue);
            Assert.IsTrue(wrappedModel.FirstNameIsChanged);
            Assert.IsTrue(wrappedModel.IsChanged);

            wrappedModel.RejectChanges();

            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstName);
            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstNameOriginalValue);
            Assert.IsFalse(wrappedModel.FirstNameIsChanged);
            Assert.IsFalse(wrappedModel.IsChanged);
        }

        /// <summary>
        /// Test: Should set is changed.
        /// </summary>
        [TestMethod]
        public void ShouldSetIsChanged()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();

            // Act
            var wrappedModel = new BindableCustomer(model);

            // Assert
            Assert.IsFalse(wrappedModel.FirstNameIsChanged);
            Assert.IsFalse(wrappedModel.IsChanged);

            wrappedModel.FirstName = "Carlos";
            Assert.IsTrue(wrappedModel.FirstNameIsChanged);
            Assert.IsTrue(wrappedModel.IsChanged);

            wrappedModel.FirstName = FakeCustomer.DefaultFirstName;
            Assert.IsFalse(wrappedModel.FirstNameIsChanged);
            Assert.IsFalse(wrappedModel.IsChanged);
        }

        /// <summary>
        /// Test: Should store the original value.
        /// </summary>
        [TestMethod]
        public void ShouldStoreOriginalValue()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();

            // Act
            var wrappedModel = new BindableCustomer(model);

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstNameOriginalValue);

            wrappedModel.FirstName = "Carlos";
            Assert.AreEqual(FakeCustomer.DefaultFirstName, wrappedModel.FirstNameOriginalValue);
        }

        #endregion Methods
    }
}