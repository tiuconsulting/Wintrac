using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using WintrackEntities;
using System.Data;
using Newtonsoft.Json;
using WintrackReports.Helpers;

namespace WintrackReports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables
        // Get the base uri
        private string baseUri = Convert.ToString(ConfigurationManager.AppSettings["ApiBaseUrl"]);
        private string relativePath = Convert.ToString(ConfigurationManager.AppSettings["ReportCopyPath"]);
        private int dateTypeIndex = 0;
        
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            //Bind the Drop downs
            BindServiceCenterCB();
            BindBankAcctDetailsCB();
            BindBillingTypeDetailsCB();
        }

        /// <summary>
        /// Bind the client details Combo box on change of Service Center
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceCenter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedServiceCenterId = Convert.ToString(ServiceCenter.SelectedValue);
            BindClientDetailsCB(selectedServiceCenterId);
        }

        /// <summary>
        /// Method to generate the SSIS reports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunReport_Click(object sender, RoutedEventArgs e)
        {
            // set the date criteria  values 
            int dateType = FillSelectedRadioBtnValue();
            string fromDate = Convert.ToString(Convert.ToDateTime(FromDate.Text).ToString("yyyy-MM-dd  00:00:00.000"));
            string toDate = Convert.ToString(Convert.ToDateTime(Todate.Text).ToString("yyyy-MM-dd  00:00:00.000"));
            //set the service center ID
            string serviceCenterId = Convert.ToString(ServiceCenter.SelectedValue);
            //set the client Id
            string clientId = Convert.ToString(ClientName.SelectedValue);
            //set the bank account id
            string bankAcct = Convert.ToString(BankAccount.SelectedValue);
            //set the billing type id
            string billingTypeId = Convert.ToString(BillingType.SelectedValue);
            //create the request string
            string restrictionParamVal = string.Format("'{0}', '{1}', {2}, {3}, {4}, {5}, {6}", fromDate, toDate, dateType, serviceCenterId,
                                                clientId, bankAcct, billingTypeId);

            //Create the request model
            ReportingEntitiesRequestModel request = new ReportingEntitiesRequestModel();
            request.UserVal = "bmr3633"; // hardcoding this as we couldnt find the userVal
            request.RestrictionParamVal = restrictionParamVal;
            request.ExecUserParamVal = "test1";// hardcoding this as we couldnt find the ExecUserParamVal

            long ExecutionId = 0;
            ////Web API call to generate Reports
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                //set the url to be invoked
                string url = baseUri + "GenerateR1017Report";
                //set the content type for header
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //set he http request
                HttpRequestMessage reqmessage = new HttpRequestMessage(HttpMethod.Post, url);
                reqmessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //invoke the web API
                response = client.SendAsync(reqmessage).Result;
                // get the response and set the execution id
                if (response.IsSuccessStatusCode)
                {
                    ExecutionId = Convert.ToInt64(response.Content.ReadAsAsync<long>().Result);
                    //changing this message as in SSIS the request is queued.
                    MessageBox.Show("Request queued Successfully. Request Id = " + ExecutionId +". \n Your file will be generated and placed in " + 
                        relativePath + "directory");
                }
                else
                {
                    //display error message
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //Display the exception
                MessageBox.Show("Unhandled Exception Occured  : Message - " + ex.Message.ToString());
            }
            finally
            {
                //dispose the objects
                if(response != null)
                {
                    response.Dispose();
                    client.Dispose();
                }
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
 
        #region private custom methods
        /// <summary>
        /// Fill the Service Center Combo box
        /// </summary>
        private void BindServiceCenterCB()
        {
            List<ServiceCenterEntities> serviceCenterCB = null;
            //Web API call starts here
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                // useAll is set as true to return all the service centers
                string url = baseUri + "GetServiceCenterDetails/true";
                //set the content type 
                client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
                //the the request message
                HttpRequestMessage reqmessage = new HttpRequestMessage(HttpMethod.Get, url);
                //invoke the web api
                response = client.SendAsync(reqmessage).Result; 
                //fill the combobox on success or display error message when failed
                if (response.IsSuccessStatusCode)
                {
                    serviceCenterCB = response.Content.ReadAsAsync<List<ServiceCenterEntities>>().Result;
                    DataTable dt = new DataTable();
                    //Convert list to data table
                    dt = DTHelper.ToDataTable<ServiceCenterEntities>(serviceCenterCB);
                    ServiceCenter.ItemsSource = dt.DefaultView;
                    ServiceCenter.DisplayMemberPath = dt.Columns["ServiceCenterName"].ToString();
                    ServiceCenter.SelectedValuePath = dt.Columns["ServiceCenterId"].ToString();
                    ServiceCenter.SelectedIndex = 0;
                }
                else
                {
                    //display the error message if the reponse was not successful.
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch(Exception ex)
            {
                //Display the exception message
                MessageBox.Show("Unhandled Exception Occured  : Message - " + ex.Message.ToString());
            }
            finally
            {
                //dispose the objects
                if (response != null)
                {
                    response.Dispose();
                    client.Dispose();
                }
            }
        }
        /// <summary>
        /// Bind the BankAccount combo box. API used is GetBankAccountDetails
        /// </summary>
        private void BindBankAcctDetailsCB()
        {
            //set the request params.
            BankAccountRequestModel request = new BankAccountRequestModel();
            request.UseAll = true;
            request.Status = 'A';

            List<BankAccountEntities> bankAcctCB = null;

            // Make Api Call to GetBankAccountDetails and Fill the Combobox BankAccount
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                // create the request url
                string url = baseUri + "GetBankAccountDetails";
                //set the content type
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //set the request message 
                HttpRequestMessage reqmessage = new HttpRequestMessage(HttpMethod.Post, url);
                reqmessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //invoke the API
                response = client.SendAsync(reqmessage).Result;
                // fill the combo box on success or display error message when failed
                if (response.IsSuccessStatusCode)
                {
                    bankAcctCB = response.Content.ReadAsAsync<List<BankAccountEntities>>().Result;
                    DataTable dt = new DataTable();
                    //convert list to data table for filling the dropdown
                    dt = DTHelper.ToDataTable<BankAccountEntities>(bankAcctCB);
                    BankAccount.ItemsSource = dt.DefaultView;
                    BankAccount.DisplayMemberPath = dt.Columns["BankAccountDisplay"].ToString();
                    BankAccount.SelectedValuePath = dt.Columns["BankAccountID"].ToString();
                    BankAccount.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //Display exception 
                MessageBox.Show("Unhandled Exception Occured  : Message - " + ex.Message.ToString());
            }
            finally
            {
                //dispose the objects
                if(response != null)
                {
                    response.Dispose();
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Bind the billing type combo box
        /// </summary>
        private void BindBillingTypeDetailsCB()
        {
            //set the request params
            BillingTypeIdRequestModel request = new BillingTypeIdRequestModel();
            request.UseAll = true;
            request.Status = 'A';

            List<BillingTypeEntities> billingTypeEntities = null;

            // Make Api Call to GetBillingTypeDetails and Fill the Combobox BillingType
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                // set the web api url
                string url = baseUri + "GetBillingTypeDetails";
                //set the content types
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //set the request message 
                HttpRequestMessage reqmessage = new HttpRequestMessage(HttpMethod.Post, url);
                reqmessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                // invoke the api
                response = client.SendAsync(reqmessage).Result;
                // fill the combo box if successful reaponse is obtained, else display error in message box
                if (response.IsSuccessStatusCode)
                {
                    billingTypeEntities = response.Content.ReadAsAsync<List<BillingTypeEntities>>().Result;
                    DataTable dt = new DataTable();
                    //convert list to datatable and bind the combo box
                    dt = DTHelper.ToDataTable<BillingTypeEntities>(billingTypeEntities);
                    BillingType.ItemsSource = dt.DefaultView;
                    BillingType.DisplayMemberPath = dt.Columns["DisplayValue"].ToString();
                    BillingType.SelectedValuePath = dt.Columns["BillingTypeId"].ToString();
                    BillingType.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //show exception in message box
                MessageBox.Show("Unhandled Exception Occured  : Message - " + ex.Message.ToString());
            }
            finally
            {
                //dispose the objects
                if (response != null)
                {
                    response.Dispose();
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Binds the Client details Combo Box
        /// </summary>
        /// <param name="selectedServiceCenterId">Selected Service Center Id is passed</param>
        private void BindClientDetailsCB(string selectedServiceCenterId)
        {
            //set the request params
            ClientDetailRequestModel request = new ClientDetailRequestModel();

            request.ServiceCenterId = selectedServiceCenterId;
            //Set this to 0 based on the SP output
            request.WhichId = 0;
            request.UseAll = true;

            List<ClientDetailEntities> clientDetailsCB = null;

            // Make Api Call to GetClientDetails and Fill the Combobox ClientName
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;
            try
            {
                //create request url
                string url = baseUri + "GetClientDetails";
                //set the content type
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //set the request message
                HttpRequestMessage reqmessage = new HttpRequestMessage(HttpMethod.Post, url);
                reqmessage.Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                //invoke the API 
                response = client.SendAsync(reqmessage).Result;

                //fill the combo box if success else display error in message box
                if (response.IsSuccessStatusCode)
                {
                    clientDetailsCB = response.Content.ReadAsAsync<List<ClientDetailEntities>>().Result;
                    DataTable dt = new DataTable();
                    //convert the list to datatable to bind the dropdown
                    dt = DTHelper.ToDataTable<ClientDetailEntities>(clientDetailsCB);
                    ClientName.ItemsSource = dt.DefaultView;
                    ClientName.DisplayMemberPath = dt.Columns["ClientName"].ToString();
                    ClientName.SelectedValuePath = dt.Columns["ClientId"].ToString();
                    ClientName.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Error Code -" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                //display exception 
                MessageBox.Show("Unhandled Exception Occured  : Message - " + ex.Message.ToString());
            }
            finally
            {
                //dispose the objects
                if (response != null)
                {
                    response.Dispose();
                    client.Dispose();
                }
            }
        }
        /// <summary>
        /// Set the date type Id based on the radio button selected.
        /// </summary>
        /// <returns></returns>
        private int FillSelectedRadioBtnValue()
        {
            if (AccountingDate.IsChecked == true)
            {
                dateTypeIndex = 0;
            }
            if (ActivityDate.IsChecked == true)
            {
                dateTypeIndex = 1;
            }
            if (InvoiceDate.IsChecked == true)
            {
                dateTypeIndex = 2;
            }
            return dateTypeIndex;
        }
        #endregion
    }  
}
