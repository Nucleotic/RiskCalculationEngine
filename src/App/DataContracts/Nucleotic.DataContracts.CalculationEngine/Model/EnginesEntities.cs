using EFCache;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public partial class EnginesEntities : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnginesEntities"/> class.
        /// </summary>
        public EnginesEntities() : base("name=EnginesEntities")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnginesEntities"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public EnginesEntities(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <exception cref="UnintentionalCodeFirstException"></exception>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BandValue> BandValues { get; set; }
        public virtual DbSet<BrokerLoading> BrokerLoadings { get; set; }
        public virtual DbSet<ClaimsHistoryLoading> ClaimsHistoryLoadings { get; set; }
        public virtual DbSet<CrestaZone> CrestaZones { get; set; }
        public virtual DbSet<EngineConfiguration> EngineConfigurations { get; set; }
        public virtual DbSet<PolicyRuleConfiguration> PolicyRuleConfigurations { get; set; }
        public virtual DbSet<RuleType> RuleTypes { get; set; }
        public virtual DbSet<RuleTypeAttribute> RuleTypeAttributes { get; set; }
        public virtual DbSet<RuleTypeExtension> RuleTypeExtensions { get; set; }
        public virtual DbSet<VehicleLoading> VehicleLoadings { get; set; }
        public virtual DbSet<PolicyRuleLoading> PolicyRuleLoadings { get; set; }
        public virtual DbSet<UnderwritingExclusion> UnderwritingExclusions { get; set; }
        public virtual DbSet<PolicyRuleParameter> PolicyRuleParameters { get; set; }
    }

    public class Configuration : DbConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            var transactionHandler = new CacheTransactionHandler(new InMemoryCache());
            AddInterceptor(transactionHandler);
            Loaded += (sender, args) => args.ReplaceService<DbProviderServices>((s, _) => new CachingProviderServices(s, transactionHandler, new CachingPolicy()));
        }
    }
}
