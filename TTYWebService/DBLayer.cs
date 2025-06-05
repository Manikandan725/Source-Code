using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Generic.Util;
using Generic.Connection;
using System.Windows.Forms;


namespace TTYWebServiceConversion
{
    public class DbLayer
    {
        Database objdb;
        private const string MODULE_NAME = "TTYWebServiceConversion:DBLayer";
        public DbLayer()
        {
            try
            {
                objdb = new SqlDatabase(Driver.ConnectionString);
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(MODULE_NAME, "DBLayer", ex.Message);
            }
        }

        public DataSet GetCustomerDs()
        {
            try
            {
                using (var dbcmd = this.objdb.GetStoredProcCommand("USP_TTY_WSDATA_GetSubPrcess"))
                {
                    return this.objdb.ExecuteDataSet(dbcmd);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(MODULE_NAME, nameof(GetCustomerDs), ex.Message);
                return (DataSet)null;
            }
        }
        public DataSet GetReadyDocuments(int CustID, int ProjID, int ProcessId, int SubprocessId, int FormId)
        {
            try
            {
                using (var dbcmd = this.objdb.GetStoredProcCommand("USP_TTY_WSDATA_GetDcnDetails"))
                {
                    objdb.AddInParameter(dbcmd, "@CustId", DbType.Int32, CustID);
                    objdb.AddInParameter(dbcmd, "@ProjId", DbType.Int32, ProjID);
                    objdb.AddInParameter(dbcmd, "@ProcessId", DbType.Int32, ProcessId);
                    objdb.AddInParameter(dbcmd, "@FormId", DbType.Int32, FormId);
                    objdb.AddInParameter(dbcmd, "@SubprocessId", DbType.Int32, SubprocessId);
                    dbcmd.CommandTimeout = 600;
                    return this.objdb.ExecuteDataSet(dbcmd);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(MODULE_NAME, nameof(GetReadyDocuments), ex.Message);
                return (DataSet)null;
            }
        }

        public DataSet GetReadyXml(int CustID, int ProjID, int SubProcessId, string Dcn, string UserName, string PassWord, string TTYStatus)
        {
            try
            {
                using (var dbcmd = this.objdb.GetStoredProcCommand("USP_TTY_WSDATA_GetXmlDetails"))
                {
                    objdb.AddInParameter(dbcmd, "@CustId", DbType.Int32, CustID);
                    objdb.AddInParameter(dbcmd, "@ProjId", DbType.Int32, ProjID);
                    objdb.AddInParameter(dbcmd, "@SubProcessId", DbType.Int32, SubProcessId);
                    objdb.AddInParameter(dbcmd, "@Dcn", DbType.String, Dcn);
                    objdb.AddInParameter(dbcmd, "@UserName", DbType.String, UserName);
                    objdb.AddInParameter(dbcmd, "@PassWord", DbType.String, PassWord);
                    objdb.AddInParameter(dbcmd, "@TTYStatus", DbType.String, TTYStatus);
                    dbcmd.CommandTimeout = 600;
                    return this.objdb.ExecuteDataSet(dbcmd);
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(MODULE_NAME, nameof(GetReadyDocuments), ex.Message);
                return (DataSet)null;
            }
        }

        public bool UpdateReturnXml(int CustID, int ProjID, int SubProcessId, string Dcn, string ReturnXml, string SpName)
        {
            bool success = true;
            try
            {
                //using (var dbcmd = objdb.GetStoredProcCommand("USP_TTY_WSDATA_UpdateTTYStatus"))
                using (var dbcmd = objdb.GetStoredProcCommand(SpName))
                {
                    //int rowsaffected = 1;
                    objdb.AddInParameter(dbcmd, "@CustID", DbType.Int32, CustID);
                    objdb.AddInParameter(dbcmd, "@ProjID", DbType.Int32, ProjID);
                    objdb.AddInParameter(dbcmd, "@SubProcessId", DbType.Int32, SubProcessId);
                    objdb.AddInParameter(dbcmd, "@Dcn", DbType.String, Dcn);
                    objdb.AddInParameter(dbcmd, "@ReturnXml", DbType.String, ReturnXml);
                    dbcmd.CommandTimeout = 600;
                    objdb.ExecuteNonQuery(dbcmd);

                    //if (rowsaffected == 0 && sStatus == "WIP")
                    //    success = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                LogFile.WriteErrorLog(MODULE_NAME, "UpdateFileMaster", ex.Message, "DATA", CustID, ProjID);
            }
            return success;
        }
    }
}