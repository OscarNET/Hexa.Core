// ----------------------------------------------------------------------------
// <copyright file="BindableModelComplexPropertyTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the complex property tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelComplexPropertyTests
    {
        #region Methods

        /// <summary>
        /// Test: Should initialize the address property.
        /// </summary>
        [TestMethod]
        public void ShouldInitializeAddressProperty()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);
            var expectedAddressModel = model.Address;

            // Act
            var actualAddress = wrappedModel.Address;

            // Assert
            Assert.IsNotNull(actualAddress);
            Assert.AreEqual(expectedAddressModel, actualAddress.Model);
        }

        #endregion Methods
    }
}