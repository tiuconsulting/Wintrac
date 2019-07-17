using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WintrackWebAPI.App_Start
{
    public static class RouteConstants
    {
        public const string GetServiceCenterDetails = "~/api/wintrack/GetServiceCenterDetails/{useAll}";
        public const string GetBankAccountDetails = "~/api/wintrack/GetBankAccountDetails";
        public const string GetBillingTypeDetails = "~/api/wintrack/GetBillingTypeDetails";
        public const string GetClientDetails = "~/api/wintrack/GetClientDetails";
        public const string GenerateR1017Report = "~/api/wintrack/GenerateR1017Report";
    }
}