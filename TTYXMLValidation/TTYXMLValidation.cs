// Decompiled with JetBrains decompiler
// Type: TTYValidate.TTYXMLValidation
// Assembly: TTYXMLValidation, Version=2.0.6828.36913, Culture=neutral, PublicKeyToken=null
// MVID: FEB2971C-CF92-47B4-A936-B4033AC2F55F
// Assembly location: C:\Users\maniamar\Downloads\TTYXMLValidation.exe

using Ethos.Core;
using Generic.Connection;
using Generic.Util;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace TTYValidate
{
  [ThreadUsage(DedicatedLevel = DedicatedThreadLevel.NoDedicatedLevel, Level1MethodResult = ThreadMethodResult.MultipleItems, Level2MethodResult = ThreadMethodResult.MultipleItems)]
  internal class TTYXMLValidation : EthosProcess<DataRow, DataRow, object>
  {
    private const string ModuleName = "TTYXMLValidation";
    private static Database _db;
    private DataSet _dscustomerDs;
    private DataSet _readyDocuments;

    public TTYXMLValidation()
      : base(typeof (TTYXMLValidation))
    {
      try
      {
        TTYXMLValidation._db = (Database) new SqlDatabase(Driver.ConnectionString);
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), nameof (TTYXMLValidation), ex.Message);
      }
    }

    protected override IEnumerable<DataRow> GetProcessItems()
    {
      try
      {
        this._dscustomerDs = this.GetCustomerDs();
        if (this._dscustomerDs != null)
        {
          if (this._dscustomerDs.Tables.Count > 0)
            return (IEnumerable<DataRow>) this._dscustomerDs.Tables[0].Select();
        }
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), nameof (GetProcessItems), ex.Message);
      }
      return (IEnumerable<DataRow>) null;
    }

    protected override IEnumerable<DataRow> GetProcessItems(DataRow parent)
    {
      try
      {
        this._readyDocuments = this.GetReadyDocuments(Convert.ToInt32(parent["CustID"]), Convert.ToInt32(parent["ProjID"]), Convert.ToInt32(parent["ProcessID"]), Convert.ToInt32(parent["SubprocessID"]), Convert.ToInt32(parent["FormId"]));
        return this._readyDocuments != null && this._readyDocuments.Tables.Count > 0 ? (IEnumerable<DataRow>) this._readyDocuments.Tables[0].Select() : (IEnumerable<DataRow>) null;
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), "GetProcessItems(DataRow)", ex.Message);
      }
      return (IEnumerable<DataRow>) null;
    }

    protected override void ProcessItem(DataRow parent, DataRow drBatch)
    {
      int int32_1 = Convert.ToInt32(parent["CustID"]);
      int int32_2 = Convert.ToInt32(parent["ProjID"]);
      Convert.ToInt32(parent["ProcessID"]);
      int int32_3 = Convert.ToInt32(parent["SubprocessID"]);
      Convert.ToString(parent["ProjName"]);
      Convert.ToInt32(parent["FormId"]);
      string Dcn = drBatch["Dcn"].ToString();
      string path1 = drBatch["IndiaFileURL"].ToString();
      string str1 = Convert.ToString(parent["RuleField"]);
      string str2 = Convert.ToString(parent["BackUpFile"]);
      string path2 = drBatch["FileName"].ToString();
      string str3 = Convert.ToString(parent["Ext"]);
      string[] strArray = Convert.ToString(parent["ViewNameField"]).Split('|');
      string PageList = "TRUE";
      string str4 = string.Empty;
      string str5 = string.Empty;
      string str6 = string.Empty;
      string empty1 = string.Empty;
      string str7 = string.Empty;
      string empty2 = string.Empty;
      try
      {
        this.ShowGrid(parent, drBatch, "WIP", "");
        if (!this.UpdateDocStatus(int32_1, int32_2, int32_3, Dcn, "", "READY", "WIP"))
          return;
        string str8 = Path.Combine(path1, path2);
        if (File.Exists(str8))
        {
          File.Copy(Path.Combine(path1, path2), Path.Combine(path1, Dcn + str2 + str3), true);
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.Load(str8);
          string str9 = string.Empty;
          bool flag = true;
          foreach (string str10 in strArray)
          {
            if (!flag)
              str9 += " or ";
            str9 = str9 + "@PageType='" + str10 + "'";
            flag = false;
          }
          int num1 = 0;
          int num2 = 0;
          int count = xmlDocument.SelectNodes(".//Page[" + str9 + "]").Count;
          for (int i = 0; i < count; ++i)
          {
            XmlNode selectNode = xmlDocument.SelectNodes(".//Page[" + str9 + "]")[i];
            if (selectNode != null)
            {
              PageList = "FALSE";
              string str11 = str1;
              char[] chArray = new char[1]{ '|' };
              foreach (string str12 in str11.Split(chArray))
              {
                try
                {
                  num1 = str1.Split('|').Length;
                  XmlNode xmlNode = selectNode.SelectSingleNode(".//Field[@FieldName='" + str12 + "']");
                  if (xmlNode != null)
                  {
                    if (xmlNode.SelectSingleNode(".//Output/DataSource[@Name='Rule14Engine']") != null)
                    {
                      if (xmlNode.SelectSingleNode(".//Output/DataSource[@Name='Rule14Engine']").Attributes["Value"] != null)
                      {
                        if (xmlNode.SelectSingleNode(".//Output/DataSource[@Name='Rule14Engine']").Attributes["Value"].Value.Trim() == string.Empty)
                        {
                          str5 = str5 == null ? str5 + str12 : str5 + "|" + str12;
                          ++num2;
                        }
                      }
                    }
                    else
                      str6 = str6 == null ? str6 + str12 : str6 + "|" + str12;
                  }
                  else
                    str7 = string.IsNullOrEmpty(str7) ? str12 : str7 + "|" + str12;
                }
                catch (Exception ex)
                {
                  LogFile.WriteErrorLog("TTYXMLValidation.exe", nameof (ProcessItem), "Dcn:" + Dcn + "," + ex.Message, "Application", int32_1, int32_2);
                  this.ShowGrid(parent, drBatch, "ERROR", ex.Message);
                  this.UpdateDocStatus(int32_1, int32_2, int32_3, Dcn, "", "WIP", "ERROR");
                  return;
                }
              }
            }
          }
          if (!string.IsNullOrEmpty(str6))
            str7 = "Rule14Engine Datasource Missing";
          else if (num1 == num2)
            str7 = "Rule14Engine Mandatory Field Data Missing";
          string ErrMsg = str7.Trim(',');
          this.ShowGrid(parent, drBatch, "COMPLETED", "");
          this.UpdateDocStatus(int32_1, int32_2, int32_3, Dcn, PageList, "WIP", "COMPLETED", ErrMsg);
        }
        else
        {
          str4 = str8 + " .xml is Missing in Indata Path";
          this.ShowGrid(parent, drBatch, "Error", "");
          this.UpdateDocStatus(int32_1, int32_2, int32_3, Dcn, "", "WIP", "Error");
        }
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog("TTYXMLValidation.exe", nameof (ProcessItem), ex.Message, "Application", int32_1, int32_2);
        this.ShowGrid(parent, drBatch, "ERROR", ex.Message);
        this.UpdateDocStatus(int32_1, int32_2, int32_3, Dcn, "", "WIP", "ERROR");
      }
    }

    private void ShowGrid(DataRow parentrow, DataRow drBatchRow, string status, string reason)
    {
      int int32_1 = Convert.ToInt32(parentrow["CustID"]);
      int int32_2 = Convert.ToInt32(parentrow["ProjID"]);
      string str = Convert.ToString(drBatchRow["Dcn"]);
      try
      {
        this.OnStatusUpdate(new ProcessEventArgs<DataRow, DataRow, object>()
        {
          Level1Data = parentrow,
          Level2Data = drBatchRow,
          Status = status,
          ErrorDescription = reason
        });
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), "clsTiffcreation.ShowGrid", ex.Message + ":" + str, "APPLICATION", int32_1, int32_2);
      }
    }

    public DataSet GetCustomerDs()
    {
      try
      {
        using (DbCommand storedProcCommand = TTYXMLValidation._db.GetStoredProcCommand("Usp_TTYXMLValidation_GetSubprocess"))
          return TTYXMLValidation._db.ExecuteDataSet(storedProcCommand);
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), nameof (GetCustomerDs), ex.Message);
        return (DataSet) null;
      }
    }

    private DataSet GetReadyDocuments(
      int custId,
      int projId,
      int processId,
      int subprocessId,
      int formid)
    {
      try
      {
        using (DbCommand storedProcCommand = TTYXMLValidation._db.GetStoredProcCommand("Usp_TTYXMLValidation_GetReadyDocuments"))
        {
          TTYXMLValidation._db.AddInParameter(storedProcCommand, "@CustID", DbType.Int32, (object) custId);
          TTYXMLValidation._db.AddInParameter(storedProcCommand, "@ProjId", DbType.Int32, (object) projId);
          TTYXMLValidation._db.AddInParameter(storedProcCommand, "@ProcessID", DbType.Int32, (object) processId);
          TTYXMLValidation._db.AddInParameter(storedProcCommand, "@SubprocessID", DbType.Int32, (object) subprocessId);
          TTYXMLValidation._db.AddInParameter(storedProcCommand, "@FormID", DbType.Int32, (object) formid);
          return TTYXMLValidation._db.ExecuteDataSet(storedProcCommand);
        }
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog(nameof (TTYXMLValidation), nameof (GetReadyDocuments), ex.Message, "ERROR", custId, projId);
        return (DataSet) null;
      }
    }

    private bool UpdateDocStatus(
      int CustId,
      int ProjId,
      int SubProcessId,
      string Dcn,
      string PageList,
      string CurrentStatus,
      string ChangeStatus,
      string ErrMsg = "")
    {
      bool flag = false;
      try
      {
        using (SqlConnection sqlConnection = new SqlConnection(TTYXMLValidation._db.ConnectionString))
        {
          using (SqlCommand sqlCommand = new SqlCommand("Usp_TTYXMLValidation_UpdateDocStatus"))
          {
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.Connection.Open();
            sqlCommand.Parameters.AddWithValue("@CustId", (object) CustId);
            sqlCommand.Parameters.AddWithValue("@ProjId", (object) ProjId);
            sqlCommand.Parameters.AddWithValue("@SubProcessId", (object) SubProcessId);
            sqlCommand.Parameters.AddWithValue("@Dcn", (object) Dcn);
            sqlCommand.Parameters.AddWithValue("@PageList", (object) PageList);
            sqlCommand.Parameters.AddWithValue("@CurrentStatus", (object) CurrentStatus);
            sqlCommand.Parameters.AddWithValue("@ChangeStatus", (object) ChangeStatus);
            if (!string.IsNullOrEmpty(ErrMsg))
              sqlCommand.Parameters.AddWithValue("@ErrMsg", (object) ErrMsg);
            sqlCommand.ExecuteNonQuery();
            flag = true;
            sqlCommand.Connection.Close();
          }
        }
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog("TTYXMLValidation.exe", "clsStarter.UpdateBatchStatus", ex.Message, "APPLICATION", CustId, ProjId);
        flag = false;
      }
      return flag;
    }
  }
}
