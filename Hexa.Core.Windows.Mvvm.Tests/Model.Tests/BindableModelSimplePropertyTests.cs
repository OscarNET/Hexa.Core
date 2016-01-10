// ----------------------------------------------------------------------------
// <copyright file="BindableModelSimplePropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the simple property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelSimplePropertyTests
    {
        #region Methods

        /// <summary>
        /// Test: Should not raise property changed event if the property is set to the same value.
        /// </summary>
        [TestMethod]
        public void ShouldNotRaisePropertyChangedEventIfPropertyIsSetToSameValue()
        {
            // Arrange
            bool eventWasRaised = false;
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);
            wrappedModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(wrappedModel.FirstName))
                    {
                        eventWasRaised = true;
                    }
                };

            // Act
            wrappedModel.FirstName = FakeCustomer.DefaultFirstName;

            // Assert
            Assert.IsFalse(eventWasRaised);
        }

        /// <summary>
        /// Test: Should raise property changed event on property change.
        /// </summary>
        [TestMethod]
        public void ShouldRaisePropertyChangedEventOnPropertyChange()
        {
            // Arrange
            bool eventWasRaised = false;
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);
            wrappedModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(wrappedModel.FirstName))
                    {
                        eventWasRaised = true;
                    }
                };

            // Act
            wrappedModel.FirstName = "Carlos";

            // Assert
            Assert.IsTrue(eventWasRaised);
        }

        #endregion Methods
    }
}