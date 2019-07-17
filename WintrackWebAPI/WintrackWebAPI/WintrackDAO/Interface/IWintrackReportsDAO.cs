using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WintrackEntities;

namespace WintrackDAO.Interface
{
    public interface IWintrackReportsDAO
    {
        long GenerateR1017Report(ReportingEntitiesRequestModel reportingEntities);
    }
}
