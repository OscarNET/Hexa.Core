// ----------------------------------------------------------------------------
// <copyright file="BindableModelTrackingComplexPropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the tracking complex property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelTrackingComplexPropertyTests
    {
        #region Methods

        /// <summary>
        /// Test: Should accept changes for the address city changes.
        /// </summary>
        [TestMethod]
        public void ShouldAcceptChangesForAddressCityChanges()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.Address.City = "London";

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultAddressCity, wrappedModel.Address.CityOriginalValue);

            wrappedModel.AcceptChanges();

            Assert.IsFalse(wrappedModel.IsChanged);
            Assert.AreEqual("London", wrappedModel.Address.City);
            Assert.AreEqual("London", wrappedModel.Address.CityOriginalValue);
        }

        /// <summary>
        /// Test: Should raise then property changed event for the is changed when the address city is changed.
        /// </summary>
        [TestMethod]
        public void ShouldRaisePropertyChangedForIsChangedWhenAddressCityIsChanged()
        {
            // Arrange
            bool eventWasFired = false;
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            wrappedModel.PropertyChanged +=
                (sender, e) =>
                {
                    if (e.PropertyName == nameof(wrappedModel.IsChanged))
                    {
                        eventWasFired = true;
                    }
                };

            // Act
            wrappedModel.Address.City = "London";

            // Assert
            Assert.IsTrue(eventWasFired);
        }

        /// <summary>
        /// Test: Should reject changes for the address city changes.
        /// </summary>
        [TestMethod]
        public void ShouldRejectChangesForAddressCityChanges()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.Address.City = "London";

            // Assert
            Assert.AreEqual(FakeCustomer.DefaultAddressCity, wrappedModel.Address.CityOriginalValue);

            wrappedModel.RejectChanges();

            Assert.IsFalse(wrappedModel.IsChanged);
            Assert.AreEqual(FakeCustomer.DefaultAddressCity, wrappedModel.Address.City);
            Assert.AreEqual(FakeCustomer.DefaultAddressCity, wrappedModel.Address.CityOriginalValue);
        }

        /// <summary>
        /// Test: Should set is changed when the address city is changed.
        /// </summary>
        [TestMethod]
        public void ShouldSetIsChangedWhenAddressCityIsChanged()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.Address.City = "London";

            // Assert
            Assert.IsTrue(wrappedModel.IsChanged);

            wrappedModel.Address.City = FakeCustomer.DefaultAddressCity;
            Assert.IsFalse(wrappedModel.IsChanged);
        }

        #endregion Methods
    }
}