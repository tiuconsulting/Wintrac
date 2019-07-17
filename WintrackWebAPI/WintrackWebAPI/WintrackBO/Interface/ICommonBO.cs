using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WintrackEntities;

namespace WintrackBO.Interface
{
    public interface ICommonBO
    {
        List<ServiceCenterEntities> GetServiceCenterDetails(bool useAll);
        List<BankAccountEntities> GetBankAccountDetails(BankAccountRequestModel bankAccountRequestModel);
        List<BillingTypeEntities> GetBillingTypeDetails(BillingTypeIdRequestModel billingTypeIdRequestModel);
        List<ClientDetailEntities> GetClientDetails(ClientDetailRequestModel clientDetailRequestModel);


    }
}
