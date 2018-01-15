using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// DataBaseFunc 的摘要描述
/// </summary>
public class DataBaseFunc
{
    public DataBaseFunc()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string ConstituteConnectionString(string SERVER_NAME, string DB_NAME, string DB_ACCOUNT, string DB_PASSWORD)
    {


        string ConnectionString = string.Format("data source={0}; user id={1}; password={2}; database={3}", SERVER_NAME, DB_ACCOUNT, DB_PASSWORD, DB_NAME);


        return ConnectionString;
    }
    public string ConstituteConnectionString(dbEntity db)
    {


        string ConnectionString = string.Format("data source={0}; user id={1}; password={2}; database={3}", db.Server_Name, db.DB_UserName, db.DB_Password, db.DB_Name);


        return ConnectionString;
    }
    public string TryDBConnect(string ConnectionString)//測試DB連線
    {
        string ErrorMessage = "";
        System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(ConnectionString);

        try
        {
            // 開啟資料庫連接。  
            sqlConn.Open();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally {
            sqlConn.Close();
            sqlConn.Dispose();
        }

        return ErrorMessage;
    }
}