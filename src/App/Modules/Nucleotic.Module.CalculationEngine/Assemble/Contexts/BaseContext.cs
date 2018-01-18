using System;
using System.Collections.Generic;
using Nucleotic.Common.Extensions.Attributes;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory;

namespace Nucleotic.Module.CalculationEngine.Assemble.Contexts
{
    public abstract class BaseContext : AssemblorContext
    {
        #region Configuration Related Data

        /// <summary>
        /// Gets or sets the type of the engine.
        /// </summary>
        /// <value>
        /// The type of the engine.
        /// </value>
        [CompareExclude]
        public EngineType EngineType
        {
            get { return Get<EngineType>(Constants.EngineTypeKey); }
            set { Set(Constants.EngineTypeKey, value); }
        }

        /// <summary>
        /// Gets or sets the ratings calculator factory.
        /// </summary>
        /// <value>
        /// The ratings calculator factory.
        /// </value>
        [CompareExclude]
        public ILoadingsCalculatorFactory LoadingsCalculatorFactory
        {
            get { return Get<ILoadingsCalculatorFactory>(Constants.LoadingsCalculatorFactoryKey); }
            set { Set(Constants.LoadingsCalculatorFactoryKey, value); }
        }

        /// <summary>
        /// Gets or sets the excess waiver calculator factory.
        /// </summary>
        /// <value>
        /// The excess waiver calculator factory.
        /// </value>
        [CompareExclude]
        public IExcessWaiverCalculatorFactory ExcessWaiversCalculatorFactory
        {
            get { return Get<IExcessWaiverCalculatorFactory>(Constants.WaiversCalculatorFactoryKey); }
            set { Set(Constants.WaiversCalculatorFactoryKey, value); }
        }

        /// <summary>
        /// Gets or sets the calling context identifier.
        /// </summary>
        /// <value>
        /// The calling context identifier.
        /// </value>
        [CompareExclude]
        public Guid CallingContextId
        {
            get { return Get<Guid>(Constants.CallingContextKey); }
            set { Set(Constants.CallingContextKey, value); }
        }

        /// <summary>
        /// Gets or sets the name of the actioned by user.
        /// </summary>
        /// <value>
        /// The name of the actioned by user.
        /// </value>
        [CompareExclude]
        public string ActionedByUserName
        {
            get { return Get<string>(Constants.CallerUserNameKey); }
            set { Set(Constants.CallerUserNameKey, value); }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version
        {
            get { return Get<int>(Constants.VersionKey); }
            set { Set(Constants.VersionKey, value); }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [CompareExclude]
        public string EntityConnectionString
        {
            get { return Get<string>(Constants.ConnectionStringKey); }
            set { Set(Constants.ConnectionStringKey, value); }
        }

        /// <summary>
        /// Gets or sets the ratings provider catalog.
        /// </summary>
        /// <value>
        /// The ratings provider catalog.
        /// </value>
        [CompareExclude]
        public ProviderCatalog RatingsProviderCatalog
        {
            get { return Get<ProviderCatalog>(Constants.RatingsRepositoryKey); }
            set { Set(Constants.RatingsRepositoryKey, value); }
        }

        /// <summary>
        /// Gets or sets the band values.
        /// </summary>
        /// <value>
        /// The band values.
        /// </value>
        [CompareExclude]
        public IEnumerable<BandRange> BandValues
        {
            get { return Get<IEnumerable<BandRange>>(Constants.BandValuesKey); }
            set { Set(Constants.BandValuesKey, value); }
        }

        /// <summary>
        /// Gets or sets the extended band values.
        /// </summary>
        /// <value>
        /// The extended band values.
        /// </value>
        [CompareExclude]
        public IEnumerable<BandRange> ExtendedBandValues
        {
            get { return Get<IEnumerable<BandRange>>(Constants.ExtendedBandValuesKey); }
            set { Set(Constants.ExtendedBandValuesKey, value); }
        }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [CompareExclude]
        public IEnumerable<RuleAttribute> Attributes
        {
            get { return Get<IEnumerable<RuleAttribute>>(Constants.AttributesKey); }
            set { Set(Constants.AttributesKey, value); }
        }

        /// <summary>
        /// Gets or sets the extended attributes.
        /// </summary>
        /// <value>
        /// The extended attributes.
        /// </value>
        [CompareExclude]
        public IEnumerable<ExtendedRuleAttribute> ExtendedAttributes
        {
            get { return Get<IEnumerable<ExtendedRuleAttribute>>(Constants.ExtendedAttributesKey); }
            set { Set(Constants.ExtendedAttributesKey, value); }
        }

        /// <summary>
        /// Gets or sets the banded attributes.
        /// </summary>
        /// <value>
        /// The banded attributes.
        /// </value>
        public IEnumerable<BandedRuleAttribute> BandedAttributes
        {
            get { return Get<IEnumerable<BandedRuleAttribute>>(Constants.BandedAttributesKey); }
            set { Set(Constants.BandedAttributesKey, value); }
        }

        /// <summary>
        /// Gets or sets the extended banded attributes.
        /// </summary>
        /// <value>
        /// The extended banded attributes.
        /// </value>
        public IEnumerable<BandedRuleAttribute> ExtendedBandedAttributes
        {
            get { return Get<IEnumerable<BandedRuleAttribute>>(Constants.ExtendedBandedAttributesKey); }
            set { Set(Constants.ExtendedBandedAttributesKey, value); }
        }

        /// <summary>
        /// Gets or sets the rule extensions.
        /// </summary>
        /// <value>
        /// The rule extensions.
        /// </value>s
        public IEnumerable<RuleTypeExtension> RuleExtensions
        {
            get { return Get<IEnumerable<RuleTypeExtension>>(Constants.ExtendedRuleKey); }
            set { Set(Constants.ExtendedRuleKey, value); }
        }

        /// <summary>
        /// Gets or sets the underwriting exceptions.
        /// </summary>
        /// <value>
        /// The underwriting exceptions.
        /// </value>
        [CompareExclude]
        public IEnumerable<UnderwritingExclusion> UnderwritingExclusions
        {
            get { return Get<IEnumerable<UnderwritingExclusion>>(Constants.UnderwritingExclusionsKey); }
            set { Set(Constants.UnderwritingExclusionsKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [base calculations done].
        /// </summary>
        /// <value>
        /// <c>true</c> if [base calculations done]; otherwise, <c>false</c>.
        /// </value>
        public bool BaseCalculationsDone
        {
            get { return Get<bool>(Constants.BaseCalculationsDoneKey); }
            set { Set(Constants.BaseCalculationsDoneKey, value); }
        }

        #endregion Configuration Related Data

        #region Required Input Data

        /// <summary>
        /// Gets or sets the type of risk items that need assessment.
        /// </summary>
        /// <value>
        /// The type of the policy.
        /// </value>
        public RiskItemRatingType PolicyRiskItemRatingType
        {
            get { return Get<RiskItemRatingType>(Constants.RiskItemRatingTypeKey); }
            set { Set(Constants.RiskItemRatingTypeKey, value); }
        }


        /// <summary>
        /// Gets or sets the base cover.
        /// </summary>
        /// <value>
        /// The base cover.
        /// </value>
        public decimal BasicSumInsured
        {
            get { return Get<decimal>(Constants.BasicSumInsuredKey); }
            set { Set(Constants.BasicSumInsuredKey, value); }
        }

        /// <summary>
        /// Gets or sets the policy number.
        /// </summary>
        /// <value>
        /// The policy number.
        /// </value>
        public string PolicyNumber
        {
            get { return Get<string>(Constants.PolicyNumberKey); }
            set { Set(Constants.PolicyNumberKey, value); }
        }

        /// <summary>
        /// Gets or sets the asset extension identifier.
        /// </summary>
        /// <value>
        /// The asset extension identifier.
        /// </value>
        public int RiskItemId
        {
            get { return Get<int>(Constants.RiskItemId); }
            set { Set(Constants.RiskItemId, value); }
        }

        /// <summary>
        /// Gets or sets the asset details.
        /// </summary>
        /// <value>
        /// The asset details.
        /// </value>
        public dynamic AssetDetails
        {
            get { return Get<dynamic>(Constants.AssetDetailsKey); }
            set { Set(Constants.AssetDetailsKey, value); }
        }



        #endregion Required Input Data

        #region Calculated Data

        /// <summary>
        /// Gets or sets the base rate.
        /// </summary>
        /// <value>
        /// The base rate.
        /// </value>
        [CompareExclude]
        public decimal BaseRate
        {
            get { return Get<decimal>(Constants.BaseRateKey); }
            set { Set(Constants.BaseRateKey, value); }
        }

        /// <summary>
        /// Gets or sets the calculated annual premium.
        /// </summary>
        /// <value>
        /// The calculated annual premium.
        /// </value>
        [CompareExclude]
        public decimal CalculatedAnnualPremium
        {
            get { return Get<decimal>(Constants.CalculatedAnnualPremiumKey); }
            set { Set(Constants.CalculatedAnnualPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the calculated monthly premium.
        /// </summary>
        /// <value>
        /// The calculated monthly premium.
        /// </value>
        [CompareExclude]
        public decimal CalculatedMonthlyPremium
        {
            get { return Get<decimal>(Constants.CalculatedMonthlyPremiumKey); }
            set { Set(Constants.CalculatedMonthlyPremiumKey, value); }
        }

        /// <summary>
        /// Gets or sets the loadings.
        /// </summary>
        /// <value>
        /// The loadings.
        /// </value>
        [CompareExclude]
        public List<AssetLoading> Loadings
        {
            get { return Get<List<AssetLoading>>(Constants.AssetLoadingsKey); }
            set { Set(Constants.AssetLoadingsKey, value); }
        }

        /// <summary>
        /// Gets the calculation date.
        /// </summary>
        /// <value>
        /// The calculation date.
        /// </value>
        [CompareExclude]
        public DateTime CalculationDate
        {
            get { return Get<DateTime>(Constants.CalculationDateKey); }
            set { Set(Constants.CalculationDateKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is declined referred.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is declined referred; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeclinedReferred
        {
            get { return Get<bool>(Constants.DeclinedRefferredKey); }
            set { Set(Constants.DeclinedRefferredKey, value); }
        }

        /// <summary>
        /// Gets or sets the declined referred reason.
        /// </summary>
        /// <value>
        /// The declined referred reason.
        /// </value>
        [CompareExclude]
        public string DeclinedReferredReason
        {
            get { return Get<string>(Constants.ReasonKey); }
            set { Set(Constants.ReasonKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        [CompareExclude]
        public bool IsLoaded
        {
            get { return Get<bool>(Constants.IsLoadedKey); }
            set { Set(Constants.IsLoadedKey, value); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseContext"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        protected BaseContext(int version)
        {
            Version = version;
            Loadings = new List<AssetLoading>();
            CallingContextId = Guid.NewGuid();
            CalculationDate = DateTime.Now;
            IsLoaded = false;
        }
    }
}
