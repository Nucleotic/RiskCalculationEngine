using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using System.Collections.Generic;
using Nucleotic.Common.Extensions.Attributes;

namespace Nucleotic.Module.CalculationEngine.Assemble.Contexts
{
    public partial class LoadingsContext : BaseContext
    {
        #region Configuration Related Data

        /// <summary>
        /// Gets or sets the cresta zones.
        /// </summary>
        /// <value>
        /// The cresta zones.
        /// </value>
        [CompareExclude]
        public IEnumerable<CrestaZone> CrestaZones
        {
            get { return Get<IEnumerable<CrestaZone>>(Constants.CrestaZonesKey); }
            set { Set(Constants.CrestaZonesKey, value); }
        }

        /// <summary>
        /// Gets or sets the broker loadings.
        /// </summary>
        /// <value>
        /// The broker loadings.
        /// </value>
        [CompareExclude]
        public IEnumerable<BrokerLoading> BrokerLoadings
        {
            get { return Get<IEnumerable<BrokerLoading>>(Constants.BrokerLoadingsKey); }
            set { Set(Constants.BrokerLoadingsKey, value); }
        }

        /// <summary>
        /// Gets or sets the vehicle loadings.
        /// </summary>
        /// <value>
        /// The vehicle loadings.
        /// </value>
        [CompareExclude]
        public IEnumerable<VehicleLoading> VehicleLoadings
        {
            get { return Get<IEnumerable<VehicleLoading>>(Constants.VehicleLoadingsKey); }
            set { Set(Constants.VehicleLoadingsKey, value); }
        }

        /// <summary>
        /// Gets or sets the claims history loadings.
        /// </summary>
        /// <value>
        /// The claims history loadings.
        /// </value>
        [CompareExclude]
        public IEnumerable<ClaimsHistoryLoading> ClaimsHistoryLoadings
        {
            get { return Get<IEnumerable<ClaimsHistoryLoading>>(Constants.ClaimsHistoryLoadingsKey); }
            set { Set(Constants.ClaimsHistoryLoadingsKey, value); }
        }

        #endregion Configuration Related Data

        #region Required Input Data

        /// <summary>
        /// Gets or sets the type of the asset insurance cover.
        /// </summary>
        /// <value>
        /// The type of the asset insurance cover.
        /// </value>
        public RiskCoverType RiskItemCoverType
        {
            get { return Get<RiskCoverType>(Constants.RiskItemCoverType); }
            set { Set(Constants.RiskItemCoverType, value); }
        }

        /// <summary>
        /// Gets or sets the type of the policy product.
        /// </summary>
        /// <value>
        /// The type of the policy product.
        /// </value>
        public ProductType PolicyProductType
        {
            get { return Get<ProductType>(Constants.ProductTypeKey); }
            set { Set(Constants.ProductTypeKey, value); }
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName
        {
            get { return Get<string>(Constants.ProductNameKey); }
            set { Set(Constants.ProductNameKey, value); }
        }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode
        {
            get { return Get<string>(Constants.PostalCodeKey); }
            set { Set(Constants.PostalCodeKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has credit shortfall cover.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has credit shortfall cover; otherwise, <c>false</c>.
        /// </value>
        public bool HasSpecializedShortfallCover
        {
            get { return Get<bool>(Constants.CreditShortfallCoverKey); }
            set { Set(Constants.CreditShortfallCoverKey, value); }
        }

        /// <summary>
        /// Gets or sets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        public BrokerDetails Broker
        {
            get { return Get<BrokerDetails>(Constants.BrokerKey); }
            set { Set(Constants.BrokerKey, value); }
        }

        /// <summary>
        /// Gets or sets the claims history for this policy.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public IEnumerable<ClaimItem> Claims
        {
            get { return Get<IEnumerable<ClaimItem>>(Constants.ClaimsHistoryKey); }
            set { Set(Constants.ClaimsHistoryKey, value); }
        }

        /// <summary>
        /// Gets or sets the claims count.
        /// </summary>
        /// <value>
        /// The claims count.
        /// </value>
        public int ClaimsCount
        {
            get { return Get<int>(Constants.ClaimsCountKey); }
            set { Set(Constants.ClaimsCountKey, value); }
        }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age
        {
            get { return Get<int>(Constants.AgeKey); }
            set { Set(Constants.AgeKey, value); }
        }

        /// <summary>
        /// Gets or sets the Additional excess.
        /// </summary>
        /// <value>
        /// The Additional excess.
        /// </value>
        public decimal AdditionalExcess
        {
            get { return Get<decimal>(Constants.AdditionalExcessKey); }
            set { Set(Constants.AdditionalExcessKey, value); }
        }

        /// <summary>
        /// Gets or sets the type of the flat rate calculation.
        /// </summary>
        /// <value>
        /// The type of the flat rate calculation.
        /// </value>
        public FlatRateCalculationTypes FlatRateCalculationType
        {
            get { return Get<FlatRateCalculationTypes>(Constants.FlatRateCalcTypeKey); }
            set { Set(Constants.FlatRateCalcTypeKey, value); }
        }

        /// <summary>
        /// Gets or sets the product count.
        /// </summary>
        /// <value>
        /// The product count.
        /// </value>
        public int ProductCount
        {
            get { return Get<int>(Constants.ProductCountKey); }
            set { Set(Constants.ProductCountKey, value); }
        }

        #endregion Required Input Data

        #region Calculated Values

        /// <summary>
        /// Gets or sets the calculated rate.
        /// </summary>
        /// <value>
        /// The calculated rate.
        /// </value>
        [CompareExclude]
        public decimal CalculatedRate
        {
            get { return Get<decimal>(Constants.CalculatedRateKey); }
            set { Set(Constants.CalculatedRateKey, value); }
        }

        /// <summary>
        /// Gets or sets the loaded factor.
        /// </summary>
        /// <value>
        /// The loaded factor.
        /// </value>
        [CompareExclude]
        public decimal LoadedFactor
        {
            get { return Get<decimal>(Constants.LoadedFactorKey); }
            set { Set(Constants.LoadedFactorKey, value); }
        }

        /// <summary>
        /// Gets or sets the cover additions.
        /// </summary>
        /// <value>
        /// The cover additions.
        /// </value>
        [CompareExclude]
        public List<decimal> CoverAdditions
        {
            get { return Get<List<decimal>>(Constants.CoverAdditionsKey); }
            set { Set(Constants.CoverAdditionsKey, value); }
        }


        /// <summary>
        /// Gets or sets the discretionary floor amount.
        /// </summary>
        /// <value>
        /// The discretionary floor amount.
        /// </value>
        [CompareExclude]
        public decimal DiscretionaryFloorAmountAnnual
        {
            get { return Get<decimal>(Constants.DiscretionaryFloorAmountAnnualKey); }
            set { Set(Constants.DiscretionaryFloorAmountAnnualKey, value); }
        }

        /// <summary>
        /// Gets or sets the discretionary floor amount monthly.
        /// </summary>
        /// <value>
        /// The discretionary floor amount monthly.
        /// </value>
        [CompareExclude]
        public decimal DiscretionaryFloorAmountMonthly
        {
            get { return Get<decimal>(Constants.DiscretionaryFloorAmountMonthlyKey); }
            set { Set(Constants.DiscretionaryFloorAmountMonthlyKey, value); }
        }

        #endregion Calculated Values

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingsContext"/> class.
        /// </summary>
        protected internal LoadingsContext(int version) : base(version)
        {
            CoverAdditions = new List<decimal>();
            FlatRateCalculationType = FlatRateCalculationTypes.None;
        }
    }
}