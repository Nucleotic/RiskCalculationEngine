using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.Module.CalculationEngine.Controllers;
using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine.Model.Services;
using NUnit.Framework;

namespace Nucleotic.Tests.CalculationEngine.UnitTests
{
    [TestFixture]
    public class CalculationTests
    {
        private List<ClaimItemRequest> _claims;
        private List<string> _securityFeatures;

        private void Setup()
        {
            _claims = new List<ClaimItemRequest>();
            _claims.AddRange(new[]
            {
                new ClaimItemRequest
                {
                    RiskItemDescription = "TestClaim1Within12Months",
                    ClaimAmount = 21300.00m,
                    DateOfLoss = DateTime.Now.AddDays(-200),
                    ClaimType = nameof(ClaimType.OwnDamage)
                },
                new ClaimItemRequest
                {
                    RiskItemDescription = "TestClaim2Within12Months",
                    ClaimAmount = 1200.00m,
                    DateOfLoss = DateTime.Now.AddDays(-120),
                    ClaimType = nameof(ClaimType.Windscreen)
                }
            });
            _securityFeatures = new List<string>();
            _securityFeatures.AddRange(new[] { nameof(SecurityFeatures.Tracker), nameof(SecurityFeatures.AntiSmashFilm), nameof(SecurityFeatures.FactoryImmobiliser) });
        }

        [TestCase("TEST-DEF-001", 6000000.00, 1, 89, 6)]
        [TestCase("TEST-DEF-002", 100000, 1, 55, 9)]
        [TestCase("TEST-DEF-003", 10000000, 1, 32, 0)]
        [TestCase("TEST-DEF-004", 50000.00, 1, 16, 1)]
        public void GivenAContext_WithExceptionValues_ShouldReturnDefferredResult(string policyNumber, decimal baseCover, int riskItemId, int age, int claimsCount)
        {
            //Arrange
            var claims = new List<ClaimItemRequest>();
            for (var i = 0; i < claimsCount; i++)
            {
                claims.Add(new ClaimItemRequest { ClaimType = "OwnDamage" });
            }
            var controller = new UnderwritingCalculationsController();

            //Act
            var request = new VehicleCalculationRequest
            {
                PolicyNumber = policyNumber,
                BasicSumInsured = baseCover,
                RiskItemId = riskItemId,
                RiskItemCoverType = "Comprehensive",
                DriverAge = age,
                PostalCode = "0001",
                HasCreditShortfallCover = false,
                Claims = claims,
                VehicleDetails = new VehicleDetailsRequest()
                {
                    Make = "Exception Test Make",
                    Model = "Exception Test Model",
                    Year = 1901,
                    VehicleType = "PassengerVehicle",
                    VehicleUse = "Private",
                    IsHighPerformance = false,
                    SecurityFeatures = _securityFeatures,
                    RiskItemCoverType = "Comprehensive"
                },
                ProductType = "Domestic",
                BrokerId = 47,
                AdditionalExcess = 0.00m
            };
            var result = controller.CalculateMotorLoadings(request);

            //Assert
            Assert.IsTrue(result.IsDeclinedReferred);
        }

        [TestCase("TEST-PAS-001", 200000.00, 1, "Comprehensive", 22, "0157", true, "AUDI", "A6 2004 - ON", 2007, "PassengerVehicle", false, false, 47, 0.00, 3.50, 7.25, 14500.08, 1208.34, false)]
        [TestCase("TEST-PAS-002", 135000.00, 13485, "Comprehensive", 37, "0157", true, "LAND ROVER", "FREELANDER II 3.2 i6 HSE A/T", 2007, "4x4 Vehicle", false, false, 1, 0.00, 4.70, 8.66, 11692.24, 974.35, false)]
        public void GivenAContext_WithVehicleDetails_ShouldReturnLoadingResult(string policyNumber, decimal baseCover, int riskItemId, string assetInsuranceCoverType, int age, string postalCode,
            bool hasCreditShortfallCover, string make, string model, int year, string vehicleType, bool usedCommercially, bool isHighPerformance, int brokerId, decimal additionalExcess,
            decimal expectedBaseRate, decimal expectedCalculatedRate, decimal expectedAnnualPremium, decimal expectedMonthlyPremium, bool expectedDeclinedValue)
        {
            //Arrange
            _claims = null;
            Setup();
            var controller = new UnderwritingCalculationsController();

            //Act
            var request = new VehicleCalculationRequest()
            {
                PolicyNumber = policyNumber,
                BasicSumInsured = baseCover,
                RiskItemId = riskItemId,
                RiskItemCoverType = assetInsuranceCoverType,
                DriverAge = age,
                PostalCode = "0001",
                HasCreditShortfallCover = false,
                Claims = _claims,
                VehicleDetails = new VehicleDetailsRequest()
                {
                    Make = make,
                    Model = model,
                    Year = year,
                    VehicleType = vehicleType,
                    VehicleUse = "Private",
                    IsHighPerformance = isHighPerformance,
                    SecurityFeatures = _securityFeatures,
                    RiskItemCoverType = assetInsuranceCoverType
                },
                ProductType = "Domestic",
                BrokerId = brokerId,
                AdditionalExcess = additionalExcess
            };
            var result = controller.CalculateMotorLoadings(request);

            //Assert
            Assert.IsFalse(result == null);
            Assert.AreEqual(expectedBaseRate, result.BaseRate);
            Assert.AreEqual(expectedCalculatedRate, decimal.Round(result.CalculatedRate, 2));
            Assert.AreEqual(expectedAnnualPremium, result.CalculatedAnnualPremium);
            Assert.AreEqual(expectedMonthlyPremium, result.CalculatedMonthlyPremium);
            Assert.AreEqual(expectedDeclinedValue, result.IsDeclinedReferred);
        }
    }
}