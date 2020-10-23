﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Web.Models;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenGettingShowFilters
    {
        [Test, AutoData]
        public void Then_False_When_DeliveryModes_Empty_And_ProviderRatings_Empty(
            CourseProvidersViewModel model)
        {
            // Arrange
            foreach( var dm in model.DeliveryModes)
            {
                dm.Selected = false;
            };

            foreach (var pr in model.ProviderRatings)
            {
                pr.Selected = false;
            };

            // Act
            var expected = false;
            var actual = model.ShowFilters;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, AutoData]
        public void And_DeliveryModes_Are_Set_And_ProviderRatings_Empty_Then_True(
                   CourseProvidersViewModel model)
        {
            // Arrange
            foreach (var dm in model.DeliveryModes)
            {
                dm.Selected = true;
            };

            foreach (var pr in model.ProviderRatings)
            {
                pr.Selected = false;
            };

            // Act
            var expected = true;
            var actual = model.ShowFilters;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, AutoData]
        public void And_ProviderRatings_Are_Set_And_DeliveryModes_Empty_And_Then_True(
                   CourseProvidersViewModel model)
        {
            // Arrange
            foreach (var dm in model.DeliveryModes)
            {
                dm.Selected = false;
            };

            foreach (var pr in model.ProviderRatings)
            {
                pr.Selected = true;
            };

            // Act
            var expected = true;
            var actual = model.ShowFilters;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test, AutoData]
        public void And_ProviderRatings_Are_Set_And_DeliveryModes_Are_Set_Then_True(
                   CourseProvidersViewModel model)
        {
            // Arrange
            foreach (var dm in model.DeliveryModes)
            {
                dm.Selected = true;
            };

            foreach (var pr in model.ProviderRatings)
            {
                pr.Selected = true;
            };

            // Act
            var expected = true;
            var actual = model.ShowFilters;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
