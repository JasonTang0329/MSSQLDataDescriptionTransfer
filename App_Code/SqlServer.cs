using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// SqlServer 的摘要描述
/// </summary>
public class SqlServer
{
    public SqlServer()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static SqlConnection OpenSqlConn(string cnstr)
    {
        SqlConnection icn = new SqlConnection();
        icn.ConnectionString = cnstr;
        if (icn.State == ConnectionState.Open) icn.Close();
        icn.Open();
        return icn;
    }

    public static DataTable GetSqlDataTable(string cnstr, string SqlString, List<Parameters> paras = null)
    {
        DataTable myDataTable = new DataTable();
        SqlConnection icn = null;
        icn = OpenSqlConn(cnstr);
        SqlCommand isc = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter(isc);
        isc.Connection = icn;
        isc.CommandText = SqlString;
        isc.CommandTimeout = 600;
        isc.Parameters.Clear();
        if (paras != null) {
            foreach (Parameters para in paras) {
                isc.Parameters.Add(para.ParamName, para.dbType).Value = para.PValue;
            }
        }
        DataSet ds = new DataSet();
        ds.Clear();
        da.Fill(ds);
        myDataTable = ds.Tables[0];
        if (icn.State == ConnectionState.Open) icn.Close();
        return myDataTable;
    }

    public static void SqlInsertUpdateDelete(string cnstr, string SqlSelectString, List<Parameters> paras = null)
    {
        SqlConnection icn = OpenSqlConn(cnstr);
        SqlCommand cmd = new SqlCommand(SqlSelectString, icn);
        SqlTransaction mySqlTransaction = icn.BeginTransaction();
        try
        {
            cmd.Transaction = mySqlTransaction;
            cmd.Parameters.Clear();
            if (paras != null)
            {
                foreach (Parameters para in paras)
                {
                    cmd.Parameters.Add(para.ParamName, para.dbType).Value = para.PValue;
                }
            }
            cmd.ExecuteNonQuery();
            mySqlTransaction.Commit();
        }
        catch (Exception ex)
        {
            mySqlTransaction.Rollback();
            throw (ex);
        }
        if (icn.State == ConnectionState.Open) icn.Close();
    }
}