using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WintrackEntities;

namespace WintrackBO.Interface
{
    public interface IWintrackReportsBO
    {
        long GenerateR1017Report(ReportingEntitiesRequestModel reportingEntities);
    }
}
