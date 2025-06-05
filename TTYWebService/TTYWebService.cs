using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.IO;
using Ethos.Core;
using Generic.Util;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using System.Diagnostics;
using Generic.Zip;
using System.Text;
using System.Xml;
using System.Net;

namespace TTYWebServiceConversion
{
    [ThreadUsage(Level1MethodResult = ThreadMethodResult.MultipleItems,
       Level2MethodResult = ThreadMethodResult.MultipleItems
        , DedicatedLevel = DedicatedThreadLevel.NoDedicatedLevel)]
    internal class TTYWebService : EthosProcess<DataRow, DataRow, object>
    {
        private SqlDatabase _db = null;
        private readonly DbLayer objdb = new DbLayer();
        private const string MODULE_NAME = "TTYWebService";
        private DataTable dt = null;

        public TTYWebService()
            : base(typeof(TTYWebService))
        {
            //_db = new SqlDatabase(Generic.Connection.Driver.ConnectionString);
        }

        protected override IEnumerable<DataRow> GetProcessItems()
        {
            try
            {
                DataSet ds = this.objdb.GetCustomerDs();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        return (IEnumerable<DataRow>)ds.Tables[0].Select();
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("TTYWebService", nameof(GetProcessItems), ex.Message);
            }
            return (IEnumerable<DataRow>)null;
        }
        protected override IEnumerable<DataRow> GetProcessItems(DataRow parent)
        {
            try
            {
                int CustID = Convert.ToInt32(parent["CustID"]);
                int ProjID = Convert.ToInt32(parent["ProjID"]);
                int ProcessID = Convert.ToInt32(parent["ProcessID"]);
                int SubprocessID = Convert.ToInt32(parent["SubprocessID"]);
                int FormId = Convert.ToInt32(parent["FormId"]);
                DataSet ds = this.objdb.GetReadyDocuments(CustID, ProjID, ProcessID, SubprocessID, FormId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                        return (IEnumerable<DataRow>)ds.Tables[0].Select();
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("TTYWebService", "GetProcessItems(DataRow)", ex.Message);
            }
            return (IEnumerable<DataRow>)null;
        }
        protected override void ProcessItem(DataRow parent, DataRow child)
        {
            string sReason = "";
            int CustID = Convert.ToInt32(parent["CustID"]);
            int ProjID = Convert.ToInt32(parent["ProjID"]);
            int ProcessID = Convert.ToInt32(parent["ProcessID"]);
            int SubprocessID = Convert.ToInt32(parent["SubprocessID"]);
            int FormId = Convert.ToInt32(parent["FormId"]);
            string Dcn = Convert.ToString(child["Dcn"]);
            string UserName = Convert.ToString(parent["UserName"]);
            string PassWord = Convert.ToString(parent["PassWord"]);
            string ServiceUrl = Convert.ToString(parent["ServiceUrl"]);
            string TTYStatus = Convert.ToString(parent["TTYStatus"]);
            string SpName = Convert.ToString(parent["SpName"]);
            try
            {
                DataSet XmlDeatisl = this.objdb.GetReadyXml(CustID, ProjID, SubprocessID, Dcn, UserName, PassWord, TTYStatus);
                this.ShowGrid(parent, child, "WIP", Dcn);
                if (XmlDeatisl == null || XmlDeatisl.Tables[0].Rows.Count <= 0)
                {
                    sReason = Dcn + " ";
                    //this.UpdateDocStatus(CustId, ProjId, SubprocessID, Dcn, "WIP", "ERROR", sReason);
                    this.ShowGrid(parent, child, "Error", sReason);
                    return;
                }
                string xml = XmlDeatisl.Tables[0].Rows[0]["Output"].ToString();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpWebRequest request = CreateWebRequest(ServiceUrl);

                XmlDocument soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(xml);

                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        //Console.WriteLine(soapResult);
                        if (!string.IsNullOrEmpty(soapResult))
                        {
                            this.objdb.UpdateReturnXml(CustID, ProjID, SubprocessID, Dcn, soapResult, SpName);
                            this.ShowGrid(parent, child, "COMPLETED", Dcn);
                        }
                        else
                        {
                            this.ShowGrid(parent, child, "ERROR", Dcn);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(MODULE_NAME, "ProcessItem", ex.ToString());
                return;
            }

        }

        public static HttpWebRequest CreateWebRequest(string ServiceUrl)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ServiceUrl);
            webRequest.Headers.Add(@"SOAPAction:""POST""");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private void ShowGrid(
         DataRow parentrow,
         DataRow drBatchRow,
         string status,
         string reason)
        {
            int int32_1 = Convert.ToInt32(parentrow["CustID"]);
            int int32_2 = Convert.ToInt32(parentrow["ProjID"]);
            string str = Convert.ToString(drBatchRow["Dcn"]);
            try
            {
                //((EthosProcessBase<DataRow, DataRow, DataRow>) this).
                OnStatusUpdate(new ProcessEventArgs<DataRow, DataRow, object>()
                {
                    Level1Data = parentrow,
                    //Level2Data = childrow,
                    Level2Data = drBatchRow,
                    Status = status,
                    ErrorDescription = reason
                });
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("TTYWebService", "clsTTYWebService.ShowGrid", ex.Message + ":" + str, "APPLICATION", int32_1, int32_2);
            }
        }

        

        


    }
}
