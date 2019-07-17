using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WintrackDAO.Interface;
using WintrackEntities;

namespace WintrackDAO
{
    /// <summary>
    /// This is the class that connects to the database.
    /// We pass the store procedure along with the parameters requeired
    /// All the record set returned is read in the data reader and converted to the specific entity based on the SP and returned back to the BO
    /// </summary>
    public class CommonDAO: IWintrackDAO
    {
        private static readonly string sqlConnectionString = Convert.ToString(ConfigurationManager.AppSettings["SqlConnString"]);
        public List<ServiceCenterEntities> GetServiceCenterDetails(bool useAll)
        {
            List<ServiceCenterEntities> serviceCenterCollection = new List<ServiceCenterEntities>();
            SqlDataReader dr = null;
            try
            {
                using (dr = SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, "rp_GetServiceCenters"
                     , new SqlParameter("@UseAll", useAll)))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ServiceCenterEntities scEntities = new ServiceCenterEntities();
                            scEntities.ServiceCenterId = Convert.ToString(dr["ServiceCenterID"]);
                            scEntities.ServiceCenterName = Convert.ToString(dr["ServiceCenterName"]);
                            serviceCenterCollection.Add(scEntities);
                        }
                    }
                }
            }
            finally
            {
                if(dr != null && !dr.IsClosed) 
                {
                    dr.Close();
                }
            }
            
            return serviceCenterCollection;
        }
        public List<BankAccountEntities> GetBankAccountDetails(BankAccountRequestModel bankAccountRequestModel)
        {
            List<BankAccountEntities> acctDetails = new List<BankAccountEntities>();
            SqlDataReader dr = null;
            try
            {
                using (dr = SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, "rp_GetBankAccount"
                     , new SqlParameter("@UseAll", bankAccountRequestModel.UseAll), new SqlParameter("@Status", bankAccountRequestModel.Status)))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BankAccountEntities bankAccountEntities = new BankAccountEntities();
                            bankAccountEntities.BankAccountID = Convert.ToInt32(dr["BankAccountID"]);
                            bankAccountEntities.BankAccountDisplay = Convert.ToString(dr["BankAccountDisplay"]);
                            acctDetails.Add(bankAccountEntities);
                        }
                    }
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return acctDetails;
        }
        public List<BillingTypeEntities> GetBillingTypeDetails(BillingTypeIdRequestModel billingTypeIdRequestModel)
        {
            List<BillingTypeEntities> billingTypeDetails = new List<BillingTypeEntities>();
            SqlDataReader dr = null;
            try
            {
                using (dr = SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, "rp_GetBillingTypeID"
                     , new SqlParameter("@UseAll", billingTypeIdRequestModel.UseAll), new SqlParameter("@Status", billingTypeIdRequestModel.Status)))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BillingTypeEntities billingTypeEntities = new BillingTypeEntities();
                            billingTypeEntities.BillingTypeId = Convert.ToString(dr["BillingTypeID"]);
                            billingTypeEntities.DisplayValue = Convert.ToString(dr["DisplayValue"]);
                            billingTypeDetails.Add(billingTypeEntities);
                        }
                    }
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return billingTypeDetails;
        }
        public List<ClientDetailEntities> GetClientDetails(ClientDetailRequestModel clientDetailRequestModel)
        {
            List<ClientDetailEntities> clientDetailCollection = new List<ClientDetailEntities>();
            SqlDataReader dr = null;
            try
            {
                using (dr = SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, "rp_GetAllForServiceCenter"
                     , new SqlParameter("@WhichID", clientDetailRequestModel.WhichId)
                     , new SqlParameter("@ServiceCenterID", clientDetailRequestModel.ServiceCenterId)
                     , new SqlParameter("@UseAll", clientDetailRequestModel.UseAll)
                    ))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClientDetailEntities clientDetailEntities = new ClientDetailEntities();
                            clientDetailEntities.ClientName = Convert.ToString(dr["ClientName"]);
                            clientDetailEntities.ClientId = Convert.ToInt32(dr["ClientID"]);
                            clientDetailEntities.OrderNo = Convert.ToInt32(dr["OrderNumber"]);
                            clientDetailEntities.ProfitCenterID = Convert.ToString(dr["profitCenterID"]);
                            clientDetailCollection.Add(clientDetailEntities);
                        }
                    }
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return clientDetailCollection;
        }
    }
}
