using System;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class ClaimsHistory
    {
        public DateTime ClaimDate { get; set; }

        public ClaimType ClaimType { get; set; }

        public decimal ClaimValue { get; set; }
    }
}