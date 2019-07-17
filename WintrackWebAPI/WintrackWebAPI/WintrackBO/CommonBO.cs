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
    public class CommonBO: ICommonBO
    {
        private readonly IWintrackDAO _wintrackDAO = null;

        public CommonBO(IWintrackDAO wintrackDAO)
        {
            _wintrackDAO = wintrackDAO;
        }

        public List<ServiceCenterEntities> GetServiceCenterDetails(bool useAll)
        {
            List<ServiceCenterEntities> serviceCenterCollection = _wintrackDAO.GetServiceCenterDetails(useAll);
            return serviceCenterCollection;
        }
        
        public List<BankAccountEntities> GetBankAccountDetails(BankAccountRequestModel bankAccountRequestModel)
        {
            ValidateBankAccountRequestModel(bankAccountRequestModel);
            List<BankAccountEntities> bankAccountEntitiesCollection = _wintrackDAO.GetBankAccountDetails(bankAccountRequestModel);
            return bankAccountEntitiesCollection;
        }

        public List<BillingTypeEntities> GetBillingTypeDetails(BillingTypeIdRequestModel billingTypeIdRequestModel)
        {
            ValidateBillingTypeIdRequestModel(billingTypeIdRequestModel);
            List<BillingTypeEntities> billingTypeEntitiesCollection = _wintrackDAO.GetBillingTypeDetails(billingTypeIdRequestModel);
            return billingTypeEntitiesCollection;
        }
        
        public List<ClientDetailEntities> GetClientDetails(ClientDetailRequestModel clientDetailRequestModel)
        {
            ValidateClientDetailRequestModel(clientDetailRequestModel);
            List<ClientDetailEntities> clientDetailsCollection = _wintrackDAO.GetClientDetails(clientDetailRequestModel);
            return clientDetailsCollection;
        }

        #region Private Methods
        private void ValidateBankAccountRequestModel(BankAccountRequestModel bankAccountRequestModel)
        {
            string errorMesssage;
            if (bankAccountRequestModel == null)
            {
                errorMesssage = "Bank Account Parameters not passed";
                throw new WintrackValidationException("Error-WCBA-100", errorMesssage);
            }
            if(string.IsNullOrEmpty(bankAccountRequestModel.Status.ToString()))
            {
                errorMesssage = "Sataus parameter not passed";
                throw new WintrackValidationException("Error-WCBA-101", errorMesssage);
            }
        }
        private void ValidateBillingTypeIdRequestModel(BillingTypeIdRequestModel billingTypeIdRequestModel)
        {
            string errorMesssage;
            if (billingTypeIdRequestModel == null)
            {
                errorMesssage = "Billing Type Id Parameters not passed";
                throw new WintrackValidationException("Error-WCBT-100", errorMesssage);
            }
            if (string.IsNullOrEmpty(billingTypeIdRequestModel.Status.ToString()))
            {
                errorMesssage = "Sataus parameter not passed";
                throw new WintrackValidationException("Error-WCBT-101", errorMesssage);
            }
        }

        private void ValidateClientDetailRequestModel(ClientDetailRequestModel clientDetailRequestModel)
        {
            string errorMesssage;
            if (clientDetailRequestModel == null)
            {
                errorMesssage = "Client deatils Parameters not passed";
                throw new WintrackValidationException("Error-WCCD-100", errorMesssage);
            }
            if (string.IsNullOrEmpty(clientDetailRequestModel.ServiceCenterId))
            {
                errorMesssage = "ServiceCenterId not passed";
                throw new WintrackValidationException("Error-WCCD-101", errorMesssage);
            }
        }
        #endregion
    }
}
