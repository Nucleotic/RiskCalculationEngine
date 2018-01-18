using EFCache;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.Framework.Logging;

namespace Nucleotic.DataContracts.CalculationEngine.Repository
{
    public abstract class RepositoryBase
    {
        internal readonly Logger<RepositoryBase> Logger = new Logger<RepositoryBase>();

        internal static readonly InMemoryCache Cache = new InMemoryCache();

        /// <summary>
        /// Gets a value indicating whether [use internal configuration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [use internal configuration]; otherwise, <c>false</c>.
        /// </value>
        internal bool UseInternalConfiguration { get; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; }

        /// <summary>
        /// The database entities
        /// </summary>
        internal EnginesEntities DbEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        protected RepositoryBase()
        {
            UseInternalConfiguration = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        protected RepositoryBase(string connectionString)
        {
            UseInternalConfiguration = false;
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets the engines entities connection.
        /// </summary>
        /// <returns></returns>
        internal EnginesEntities GetEnginesEntitiesConnection()
        {
            return DbEntities = UseInternalConfiguration ? new EnginesEntities() : new EnginesEntities(ConnectionString);
        }
    }
}