using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WintrackEntities
{
    /// <summary>
    /// Forms the request for fetching  the billing id details
    /// </summary>
    public class BillingTypeIdRequestModel
    {
        /// <summary>
        /// UseAll when true will include the All Billing Types
        /// </summary>
        public bool UseAll { get; set; }
        /// <summary>
        /// Check the active and inactive status
        /// Pass 'A' for active and 'I' for inactive
        /// </summary>
        public char Status { get; set; }
    }
}
