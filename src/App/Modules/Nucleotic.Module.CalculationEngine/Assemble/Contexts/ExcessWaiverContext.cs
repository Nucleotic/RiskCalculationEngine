using Nucleotic.DataContracts.CalculationEngine;

namespace Nucleotic.Module.CalculationEngine.Assemble.Contexts
{
    public class ExcessWaiverContext : BaseContext
    {
        /// <summary>
        /// Gets or sets the policy age.
        /// </summary>
        /// <value>
        /// The policy age.
        /// </value>
        public int PolicyAge
        {
            get { return Get<int>(Constants.PolicyAgeKey); }
            set { Set(Constants.PolicyAgeKey, value); }
        }

        /// <summary>
        /// Gets or sets the risk address count.
        /// </summary>
        /// <value>
        /// The risk address count.
        /// </value>
        public int RiskAddressCount
        {
            get { return Get<int>(Constants.RiskAddressCountKey); }
            set { Set(Constants.RiskAddressCountKey, value); }
        }

        /// <summary>
        /// Gets or sets the type of all risk.
        /// </summary>
        /// <value>
        /// The type of all risk.
        /// </value>
        public AllRiskType AllRiskType
        {
            get { return Get<AllRiskType>(Constants.AllRiskTypeKey); }
            set { Set(Constants.AllRiskTypeKey, value); }

        }

        /// <summary>
        /// Gets or sets the monthly waiver flat rate.
        /// </summary>
        /// <value>
        /// The monthly waiver flat rate.
        /// </value>
        public decimal BasicMonthlyWaiverPremium
        {
            get { return Get<decimal>(Constants.BasicMonthlyWaiverPremiumKey); }
            set { Set(Constants.BasicMonthlyWaiverPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the annualized waiver premium.
        /// </summary>
        /// <value>
        /// The annualized waiver premium.
        /// </value>
        public decimal BasicAnnualisedWaiverPremium
        {
            get { return Get<decimal>(Constants.BasicAnnualisedWaiverPremiumKey); }
            set { Set(Constants.BasicAnnualisedWaiverPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the calculated waiver amount.
        /// </summary>
        /// <value>
        /// The calculated waiver amount.
        /// </value>
        public decimal TotalLossMonthlyWaiverPremium
        {
            get { return Get<decimal>(Constants.TotalLossMonthlyWaiverPremiumKey); }
            set { Set(Constants.TotalLossMonthlyWaiverPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the annualized total loss waiver premium.
        /// </summary>
        /// <value>
        /// The annualized total loss waiver premium.
        /// </value>
        public decimal TotalLossAnnualisedWaiverPremium
        {
            get { return Get<decimal>(Constants.TotalLossAnnualisedWaiverPremiumKey); }
            set { Set(Constants.TotalLossAnnualisedWaiverPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the excess amount.
        /// </summary>
        /// <value>
        /// The excess amount.
        /// </value>
        public decimal ExcessAmount
        {
            get { return Get<decimal>(Constants.AdditionalExcessKey); }
            set { Set(Constants.AdditionalExcessKey, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcessWaiverContext"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public ExcessWaiverContext(int version) : base(version) { }
    }
}
