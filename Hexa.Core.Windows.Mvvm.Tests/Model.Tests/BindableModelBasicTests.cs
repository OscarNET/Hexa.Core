// ----------------------------------------------------------------------------
// <copyright file="BindableModelBasicTests.cs" company="HexaSystems Inc">
//     Copyright © 2016 HexaSystems Inc.
//     Licensed under the Apache License, Version 2.0 (the "License")
// </copyright>
// ----------------------------------------------------------------------------
namespace Hexa.Core.Windows.Model.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Represents the basic tests for a wrapped model.
    /// </summary>
    [TestClass]
    public class BindableModelBasicTests
    {
        #region Methods

        /// <summary>
        /// Test: Should contain the model in the model property.
        /// </summary>
        [TestMethod]
        public void ShouldContainModelInModelProperty()
        {
            // Arrange
            var expectedModel = FakeCustomer.CreateSameForTests();

            // Act
            var wrappedModel = new BindableCustomer(expectedModel).Model;

            // Assert
            Assert.AreEqual(expectedModel, wrappedModel);
        }

        /// <summary>
        /// Test: Should get value of the underlying model property.
        /// </summary>
        [TestMethod]
        public void ShouldGetValueOfUnderlyingModelProperty()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);
            var expectedFirstName = model.FirstName;

            // Act
            var actualFirstName = wrappedModel.FirstName;

            // Assert
            Assert.AreEqual(expectedFirstName, actualFirstName);
        }

        /// <summary>
        /// Test: Should set value of the underlying model property.
        /// </summary>
        [TestMethod]
        public void ShouldSetValueOfUnderlyingModelProperty()
        {
            // Arrange
            var model = FakeCustomer.CreateSameForTests();
            var wrappedModel = new BindableCustomer(model);

            // Act
            wrappedModel.FirstName = "Carlos";
            var expectedFirstName = model.FirstName;

            // Assert
            Assert.AreEqual("Carlos", expectedFirstName);
        }

        /// <summary>
        /// Test: Should throw argument exception if the address is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowArgumentExceptionIfAddressIsNull()
        {
            try
            {
                // Arrange
                var model = FakeCustomer.CreateSameForTests();

                // Act
                model.Address = null;
                var wrappedModel = new BindableCustomer(model);
            }
            catch (ArgumentException exception)
            {
                // Assert (is handled by the ExpectedException)
                Assert.AreEqual("The address is mandatory!", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Test: Should throw argument exception if the emails collection is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowArgumentExceptionIfEmailsCollectionIsNull()
        {
            try
            {
                // Arrange
                var model = FakeCustomer.CreateSameForTests();

                // Act
                model.Emails = null;
                var wrappedModel = new BindableCustomer(model);
            }
            catch (ArgumentException exception)
            {
                // Assert (is handled by the ExpectedException)
                Assert.AreEqual("The emails are mandatory!", exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Test: Should throw argument null exception if the model is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionIfModelIsNull()
        {
            try
            {
                // Arrange
                FakeCustomer model = null;

                // Act
                var wrappedModel = new BindableCustomer(model);
            }
            catch (ArgumentNullException exception)
            {
                // Assert (is handled by the ExpectedException)
                Assert.AreEqual("model", exception.ParamName);
                throw;
            }
        }

        #endregion Methods
    }
}