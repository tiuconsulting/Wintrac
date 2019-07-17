using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WintrackEntities
{
    public class ClientDetailRequestModel
    {
        public int WhichId { get; set; }
        public string ServiceCenterId { get; set; }
        public bool UseAll { get; set; }
    }
}
