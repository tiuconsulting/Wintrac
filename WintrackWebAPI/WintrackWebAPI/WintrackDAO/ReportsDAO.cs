using Microsoft.ApplicationBlocks.Data;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WintrackDAO.Interface;
using WintrackEntities;

namespace WintrackDAO
{
    public class ReportsDAO: IWintrackReportsDAO
    {
        //connection string to connect to the SSIS DB
        private static readonly string sqlConnectionString = Convert.ToString(ConfigurationManager.AppSettings["ReportsConnString"]);

        /// <summary>
        /// Invokes the SP exec_r1017.
        /// This SP will trigger the SSIS package to generate the R1017 report
        /// </summary>
        /// <param name="reportingEntities">List of the reporting request model to trigger the SSIS package</param>
        /// <returns>Queue Id is returned</returns>
        public long GenerateR1017Report(ReportingEntitiesRequestModel reportingEntities)
        {
            long executionID = 0;
            SqlDataReader dr = null;
            try
            {
                //Declare the params array and provide the required values
                SqlParameter[] arParms = new SqlParameter[4];

                arParms[0] = new SqlParameter("@NTUserVal", SqlDbType.NVarChar, 200);
                arParms[0].Value = reportingEntities.UserVal;

                arParms[1] = new SqlParameter("@RestrictionParamVal", SqlDbType.NVarChar, 200);
                arParms[1].Value = reportingEntities.RestrictionParamVal;

                arParms[2] = new SqlParameter("@ExecUserParamVal", SqlDbType.NVarChar, 200);
                arParms[2].Value = reportingEntities.ExecUserParamVal;
                
                // since this is the output param of the SP the direction is set
                arParms[3] = new SqlParameter("@execution_id", SqlDbType.BigInt);
                arParms[3].Direction = ParameterDirection.Output;

                // Execute the stored procedure
                dr = SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, "exec_r1017", arParms);
                //data reader is closed as output params will only be available after the reader is closed  
                dr.Close();

                // get the execution id for further check 
                if(arParms[3].Value != DBNull.Value)
                {
                    executionID = Convert.ToInt64(arParms[3].Value);
                }
            }
            finally
            {
                if (dr != null && !dr.IsClosed)
                {
                    dr.Close();
                }
            }
            return executionID;
        }
    }
}
