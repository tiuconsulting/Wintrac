using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WintrackExceptionHandler.Entities
{
    /// <summary>
    /// Properties to be returned in the response.
    /// </summary>
    public class WintrackErrors
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
