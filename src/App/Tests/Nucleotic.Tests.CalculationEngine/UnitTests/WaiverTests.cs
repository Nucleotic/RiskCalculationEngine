using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model.Services;
using Nucleotic.Module.CalculationEngine.Controllers;
using NUnit.Framework;

namespace Nucleotic.Tests.CalculationEngine.UnitTests
{
    [TestFixture]
    public class WaiverTests
    {
        [TestCase(0.0, 0, 0, 0)]
        public void GivenExcessWaiverContext_AsMotorWaiver_ShouldReturnWaiverResult(decimal basicSuminsured, int policyAge, decimal expectedBasicWaiver, decimal expectedTotalLossWaiver)
        {
            //Arrange
            var controller = new UnderwritingCalculationsController();

            //Act
            var result = controller.CalculateExcessWaiver(new ExcessWaiverCalculationRequest
            {
                BasicSumInsured = 0.00m,
                AllRiskItemType = nameof(AllRiskType.Unspecified),
                RatingType = nameof(RiskItemRatingType.MotorVehicle),
                RiskItemFactor = 3,
                UseState = false
            });

            //Assert
            Assert.IsFalse(result == null);
            Assert.AreEqual(expectedBasicWaiver, result.MonthlyBasicWaiverPremium);
            Assert.AreEqual(expectedTotalLossWaiver, result.MonthlyTotalLossWaiverPremium);
        }
    }
}
