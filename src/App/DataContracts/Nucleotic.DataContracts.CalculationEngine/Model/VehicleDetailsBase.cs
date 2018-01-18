namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public abstract class VehicleDetailsBase
    {
        /// <summary>
        /// Gets or sets the make.
        /// </summary>
        /// <value>
        /// The make.
        /// </value>
        public virtual string Make { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public virtual string Model { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public virtual int Year { get; set; }

        /// <summary>
        /// Gets or sets the type of the vehicle.
        /// </summary>
        /// <value>
        /// The type of the vehicle.
        /// </value>
        public string VehicleType { get; set; }

        /// <summary>
        /// Gets or sets the vehicle use.
        /// </summary>
        /// <value>
        /// The vehicle use.
        /// </value>
        public string VehicleUse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is high performance.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is high performance; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsHighPerformance { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public virtual int DriverAge { get; set; }
    }
}