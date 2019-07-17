using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WintrackBO.Interface;
using WintrackDAO.Interface;
using WintrackEntities;
using WintrackEntities.Error;

namespace WintrackBO
{
    public class WintrackReportsBO : IWintrackReportsBO
    {
        private readonly IWintrackReportsDAO _wintrackReportsDAO = null;
        public WintrackReportsBO(IWintrackReportsDAO wintrackReportsDAO)
        {
            _wintrackReportsDAO = wintrackReportsDAO;
        }

        public long GenerateR1017Report(ReportingEntitiesRequestModel reportingEntities)
        {
            ValidateReportingEntities(reportingEntities);
            long reportExecId = _wintrackReportsDAO.GenerateR1017Report(reportingEntities);
            return reportExecId;
        }

        private void ValidateReportingEntities(ReportingEntitiesRequestModel reportingEntities)
        {
            string errorMesssage;
            if (reportingEntities == null)
            {
                errorMesssage = "Reporting Parameters not passed";
                throw new WintrackValidationException("Error-WR100", errorMesssage);
            }
            if (string.IsNullOrEmpty(reportingEntities.UserVal))
            {
                errorMesssage = "UserVal cannot be empty or blank to generate report";
                throw new WintrackValidationException("Error-WR101", errorMesssage);
            }
            if (string.IsNullOrEmpty(reportingEntities.ExecUserParamVal))
            {
                errorMesssage = "ExecUserParamVal cannot be empty or blank to generate report";
                throw new WintrackValidationException("Error-WR102", errorMesssage);
            }
            if (string.IsNullOrEmpty(reportingEntities.RestrictionParamVal))
            {
                errorMesssage = "RestrictionParamVal cannot be empty or blank to generate report";
                throw new WintrackValidationException("Error-WR103", errorMesssage);
            }
        }
    }
}
