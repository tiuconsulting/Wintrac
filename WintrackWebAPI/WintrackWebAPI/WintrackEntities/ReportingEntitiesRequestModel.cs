using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WintrackEntities
{
    /// <summary>
    /// Pass all the params for the reporting model
    /// </summary>
    public class ReportingEntitiesRequestModel
    {
        public string UserVal { get; set; }
        /// <summary>
        /// All the details like Date criteria, from and to date, service center, client name , bank account and billing type is passed comma seperated string.
        /// The SP splits it to the required params to generate the Desired report
        /// </summary>
        public string RestrictionParamVal { get; set; }
        /// <summary>
        /// User who have logged in
        /// </summary>
        public string ExecUserParamVal { get; set; }
    }
}
