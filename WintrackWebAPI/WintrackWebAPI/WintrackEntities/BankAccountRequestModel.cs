using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WintrackEntities
{
    /// <summary>
    /// request model for fetching bank account
    /// </summary>
    public class BankAccountRequestModel
    {
        /// <summary>
        /// UseAll will include the top most value a All bank accounts when passed as true
        /// </summary>
        public bool UseAll { get; set; }
        /// <summary>
        /// Check the active and inactive status
        /// Pass 'A' for active and 'I' for inactive
        /// </summary>
        public char Status { get; set; }
    }
}
