namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public struct Address
    {
        /// <summary>
        /// Gets or sets the unit building number.
        /// </summary>
        /// <value>
        /// The unit building number.
        /// </value>
        public string UnitBuildingNumber { get; set; }

        /// <summary>
        /// Gets or sets the street number.
        /// </summary>
        /// <value>
        /// The street number.
        /// </value>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the complex building.
        /// </summary>
        /// <value>
        /// The name of the complex building.
        /// </value>
        public string ComplexBuildingName { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address line3.
        /// </summary>
        /// <value>
        /// The address line3.
        /// </value>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Gets or sets the suburb.
        /// </summary>
        /// <value>
        /// The suburb.
        /// </value>
        public string Suburb { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the province.
        /// </summary>
        /// <value>
        /// The province.
        /// </value>
        public string Province { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public float Longitude { get; set; }
    }
}
