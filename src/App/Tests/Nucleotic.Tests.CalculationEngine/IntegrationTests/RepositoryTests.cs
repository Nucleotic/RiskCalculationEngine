using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nucleotic.Tests.CalculationEngine.IntegrationTests
{
    [TestFixture]
    public class RepositoryTests
    {
        private IRatingsRepository _repository;

        private void Setup()
        {
            _repository = new RatingsRepository();
            //_repository = (IRatingsRepository) RepositoryFactory.Instance("RatingsRepository");
        }

        [TestCase(1, 16)]
        public void GetAllRulesTypes(int version, int expectedRecords)
        {
            //Arrange
            Setup();

            //Act
            IEnumerable<RuleType> items = _repository.GetAllRuleTypes(version);

            //Assert
            var ruleTypes = items as IList<RuleType> ?? items.ToList();
            Assert.IsTrue(ruleTypes.Any(), "GetAllRuleTypes returned no items.");
            Assert.AreEqual(expectedRecords, ruleTypes.Count, string.Format("Not all RuleTypes returned for version {0}", version));
        }

        [TestCase(1, "Banding", 0)]
        [TestCase(1, "Attributes", 0)]
        public void GetRuleTypeByCriteriaType(int version, string criteriaType, int expectedRecords)
        {
            //Arrange

            Setup();

            //Act
            var ruleType = (DataRuleTypes)Enum.Parse(typeof(DataRuleTypes), criteriaType);
            IEnumerable<RuleType> items = _repository.GetRuleTypesByCriteriaType(version, ruleType, EngineType.MotorLoadings);

            //Assert
            var ruleTypes = items as IList<RuleType> ?? items.ToList();
            Assert.IsTrue(ruleTypes.Any(), "GetRuleTypesByCriteriaType returned no items.");
        }
    }
}