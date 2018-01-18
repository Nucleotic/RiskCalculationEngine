using Nucleotic.DataContracts.CalculationEngine.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nucleotic.Framework.Engine.Rules;

namespace Nucleotic.Tests.CalculationEngine.UnitTests
{
    [TestFixture]
    public class RulesEngineTests
    {
        #region Baseline Tests

        [Test]
        public void ChildProperties()
        {
            Order order = GetOrder();
            Rule rule = new Rule()
            {
                MemberName = "Customer.Country.CountryCode",
                Operator = ExpressionType.Equal.ToString("g"),
                TargetValue = "ZAF"
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();
            var compiledRule = engine.CompileRule<Order>(rule);
            bool passes = compiledRule(order);
            Assert.IsTrue(passes);

            order.Customer.Country.CountryCode = "USA";
            passes = compiledRule(order);
            Assert.IsFalse(passes);
        }

        [Test]
        public void ConditionalLogic()
        {
            Order order = GetOrder();
            Rule rule = new Rule()
            {
                Operator = ExpressionType.AndAlso.ToString("g"),
                Rules = new List<Rule>()
                {
                    new Rule(){ MemberName = "Customer.LastName", TargetValue = "Soap", Operator = "Equal"},
                    new Rule(){
                        Operator = "Or",
                        Rules = new List<Rule>(){
                            new Rule(){ MemberName = "Customer.FirstName", TargetValue = "Joe", Operator = "Equal"},
                            new Rule(){ MemberName = "Customer.FirstName", TargetValue = "Jenny", Operator = "Equal"}
                        }
                    }
                }
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();
            var fakeName = engine.CompileRule<Order>(rule);
            bool passes = fakeName(order);
            Assert.IsTrue(passes);

            order.Customer.FirstName = "Philip";
            passes = fakeName(order);
            Assert.IsFalse(passes);
        }

        [Test]
        public void BooleanMethods()
        {
            Order order = GetOrder();
            Rule rule = new Rule()
            {
                Operator = "HasItem",//The Order Object Contains a method named 'HasItem' that returns true/false
                Inputs = new List<object> { "Test" }
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();
            var boolMethod = engine.CompileRule<Order>(rule);
            bool passes = boolMethod(order);
            Assert.IsTrue(passes);

            var item = order.Items.First(x => x.ItemCode == "Test");
            item.ItemCode = "Changed";
            passes = boolMethod(order);
            Assert.IsFalse(passes);
        }

        [Test]
        public void ChildPropertyBooleanMethods()
        {
            Order order = GetOrder();
            Rule rule = new Rule()
            {
                MemberName = "Customer.FirstName",
                Operator = "EndsWith",//Regular method that exists on string.. As a note expression methods are not available
                Inputs = new List<object> { "ohn" }
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();
            var childPropCheck = engine.CompileRule<Order>(rule);
            bool passes = childPropCheck(order);
            Assert.IsTrue(passes);

            order.Customer.FirstName = "jane";
            passes = childPropCheck(order);
            Assert.IsFalse(passes);
        }

        [Test]
        public void RegexIsMatch()
        {
            Order order = this.GetOrder();
            Rule rule = new Rule()
            {
                MemberName = "Customer.FirstName",
                Operator = "IsMatch",
                TargetValue = @"^[a-zA-Z0-9]*$"
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();
            var regexCheck = engine.CompileRule<Order>(rule);
            bool passes = regexCheck(order);
            Assert.IsTrue(passes);

            order.Customer.FirstName = "--NoName";
            passes = regexCheck(order);
            Assert.IsFalse(passes);
        }

        #endregion Baseline Tests

        #region Application Specific Tests

        [TestCase(81, ExpressionType.LessThanOrEqual, "80")]
        [TestCase(17, ExpressionType.GreaterThanOrEqual, "18")]
        public void GivenTempContext_WithIncorrectAge_ShouldRaiseUnderwritingException(int age, ExpressionType expression, string targetValue)
        {
            //Arrange
            TempContext context = new TempContext { Age = age };
            Rule rule = new Rule()
            {
                MemberName = "DriverAge",
                Operator = expression.ToString("g"),
                TargetValue = targetValue
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();

            //Act
            var compiledRule = engine.CompileRule<TempContext>(rule);
            bool passes = compiledRule(context);

            //Assert
            Assert.IsFalse(passes);
        }

        [TestCase(5000000.00, ExpressionType.GreaterThan, "4999999.99")]
        [TestCase(15000000.00, ExpressionType.GreaterThan, "4999999.99")]
        public void GivenTempContext_WithExceedMaximumCover_ShouldRaiseUnderwritingException(decimal cover, ExpressionType expression, string targetValue)
        {
            //Arrange
            TempContext context = new TempContext { BasicSumInsured = cover };
            Rule rule = new Rule()
            {
                MemberName = "BasicSumInsured",
                Operator = expression.ToString("g"),
                TargetValue = targetValue
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();

            //Act
            var compiledRule = engine.CompileRule<TempContext>(rule);
            bool passes = compiledRule(context);

            //Assert
            Assert.IsTrue(passes);
        }

        [TestCase(5, ExpressionType.GreaterThanOrEqual, "5", true)]
        [TestCase(6, ExpressionType.GreaterThanOrEqual, "5", true)]
        [TestCase(4, ExpressionType.GreaterThanOrEqual, "5", false)]
        public void GivenTempContext_WithClaims_ShouldRaiseValidationCheckValue(int claimsNo, ExpressionType expression, string targetValue, bool shouldFail)
        {
            //Arrange
            var claims = new List<ClaimItem>();
            for (var i = 0; i < claimsNo; i++)
            {
                claims.Add(new ClaimItem());
            }

            TempContext context = new TempContext { Claims = claims, ClaimsCount = claims.Count };
            Rule rule = new Rule()
            {
                MemberName = "ClaimsCount",
                Operator = expression.ToString("g"),
                TargetValue = targetValue
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();

            //Act
            var compiledRule = engine.CompileRule<TempContext>(rule);
            bool passes = compiledRule(context);

            //Assert
            if (shouldFail) Assert.IsTrue(passes);
            else Assert.IsFalse(passes);
        }

        [TestCase(89, ExpressionType.LessThanOrEqual, "80", 6000000.00, ExpressionType.LessThanOrEqual, "4999999.99", 6, ExpressionType.LessThan, "5", true)]
        [TestCase(79, ExpressionType.LessThanOrEqual, "80", 4999999.98, ExpressionType.LessThanOrEqual, "4999999.99", 4, ExpressionType.LessThan, "5", false)]
        [TestCase(79, ExpressionType.LessThanOrEqual, "80", 4999999.98, ExpressionType.LessThanOrEqual, "4999999.99", 5, ExpressionType.LessThan, "5", true)]
        public void GivenTempContext_WithInvalidData_ShouldRaiseUnderwritingException(int age, ExpressionType exprAge, string targetAge, decimal cover, ExpressionType exprCover, string targetCover,
            int claimsNo, ExpressionType exprClaims, string targetClaims, bool shouldFail)
        {
            //Arrange
            var claims = new List<ClaimItem>();
            for (var i = 0; i < claimsNo; i++)
            {
                claims.Add(new ClaimItem());
            }

            TempContext context = new TempContext { Claims = claims, ClaimsCount = claims.Count, Age = age, BasicSumInsured = cover };
            Rule rule = new Rule()
            {
                Operator = ExpressionType.AndAlso.ToString("g"),
                Rules = new List<Rule>()
                {
                    new Rule(){ MemberName = "BasicSumInsured", TargetValue = targetCover, Operator = exprCover.ToString("g")},
                    new Rule(){ MemberName = "DriverAge", TargetValue = targetAge, Operator = exprAge.ToString("g")},
                    new Rule(){ MemberName = "ClaimsCount", TargetValue = targetClaims, Operator = exprClaims.ToString("g")}
                }
            };
            ExpressionRuleEngine engine = new ExpressionRuleEngine();

            //Act
            var compiledRule = engine.CompileRule<TempContext>(rule);
            bool passes = compiledRule(context);

            //Assert
            if (shouldFail) Assert.False(passes);
            else Assert.IsTrue(passes);
        }

        #endregion Application Specific Tests

        #region Test Data

        public class TempContext
        {
            public int Age { get; set; }

            public decimal BasicSumInsured { get; set; }

            public IEnumerable<ClaimItem> Claims { get; set; }

            public int ClaimsCount { get; set; }

            public bool IsDeclinedReferred { get; set; }
        }

        public class Order
        {
            public Order()
            {
                this.Items = new List<Item>();
            }

            public int OrderId { get; set; }

            public Customer Customer { get; set; }

            public List<Item> Items { get; set; }

            public bool HasItem(string itemCode)
            {
                return Items.Any(x => x.ItemCode == itemCode);
            }
        }

        public class Item
        {
            public decimal Cost { get; set; }

            public string ItemCode { get; set; }
        }

        public class Customer
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public Country Country { get; set; }
        }

        public class Country
        {
            public string CountryCode { get; set; }
        }

        #endregion Test Data

        #region Test Related Methods

        public Order GetOrder()
        {
            Order order = new Order()
            {
                OrderId = 1,
                Customer = new Customer()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Country = new Country()
                    {
                        CountryCode = "AUS"
                    }
                },
                Items = new List<Item>(){
                    new Item(){ ItemCode = "MM23", Cost=5.25M},
                    new Item(){ ItemCode = "LD45", Cost=5.25M},
                    new Item(){ ItemCode = "Test", Cost=3.33M},
                }
            };
            return order;
        }

        #endregion Test Related Methods
    }
}