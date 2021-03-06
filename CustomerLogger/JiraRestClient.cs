﻿using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Jira_REST
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraRestRequestHandler
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        //private const string m_sRequestUrl = "https://tstjira.esg.wsu.edu/rest/api/2/issue";
        private const string m_AuthorizationHash = "Basic Q3JpbXNvblJlc3RTZXJ2aWNlOkNRUHV5XiVYISExMg==";

        private JiraRestRequestBody m_RequestBody;
        private JiraRestResponseBody m_ResponseBody;

        //  Constructors    ///////////////////////////////////////////////////////////////////////
        //static JiraRestRequestHandler() {
        //    if (RequestURL == null){
        //        RequestURL = "https://tstjira.esg.wsu.edu/rest/api/2/issue";
        //    }

        //}
        public JiraRestRequestHandler()
        {
            m_RequestBody = new JiraRestRequestBody();
            m_ResponseBody = new JiraRestResponseBody();
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public static string RequestURL{
            get { return CustomerLogger.RegistryData.CustomerLogger_RegistryData.Rest_Ticketing_URL; }
            set { CustomerLogger.Cougtech_CustomerLogger.Ticketing_Rest_Url = value; }
        }
        public JiraRestRequestBody Request
        {
            get { return m_RequestBody; }
        }

        public string Response_Key
        {
            get { return m_ResponseBody.key; }
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        public void PostRequest()
        {
            //Load the response from the most recent release
            string serializedResponse = GetReleases();

            //Deserialize the response body and load into the respective member
            m_ResponseBody = new JavaScriptSerializer().Deserialize<JiraRestResponseBody>(serializedResponse);
            //m_ResponseBody = (JiraRestResponseBody)JsonConvert.DeserializeObject(serializedResponse);
        }

        //  Private Functions   ///////////////////////////////////////////////////////////////////

        private string GetReleases()
        {
            HttpWebRequest requestClient = (HttpWebRequest)WebRequest.Create(RequestURL);

            requestClient.Method = "POST";                                      //Set the request method to POST
            requestClient.ContentType = "application/json";                     //Set the request content type to JSON
            requestClient.Headers.Add("Authorization", m_AuthorizationHash);    //Enter the authorization hashcode into the headers

            //Serialize the JSON request body and enter into the client request stream
            using (StreamWriter requestWriter = new StreamWriter(requestClient.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                string serializedRequest = new JavaScriptSerializer().Serialize(m_RequestBody);
                requestWriter.Write(serializedRequest);                                 //Write serialized body to the request stream
                requestWriter.Flush();                                                  //Flush the request stream
                requestWriter.Close();                                                  //Close the request stream
            }

            string sResponse; //Response string

            //Load response
            using (HttpWebResponse response = (HttpWebResponse)requestClient.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                        sResponse = sr.ReadToEnd();
                }
            }

            return sResponse;
        }
    }
}
