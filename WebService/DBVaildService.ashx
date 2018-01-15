<%@ WebHandler Language="C#" Class="DBVaildService" %>

using System;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
public class DBVaildService : IHttpHandler
{
    DataBaseFunc dbfunc = new DataBaseFunc();
    public void ProcessRequest(HttpContext context)
    {
        string array = context.Request["array"] ?? string.Empty;
        string msg = "Fail";
        if (array != "")
        {
            List<jsonEntity> jmodel = JsonConvert.DeserializeObject<List<jsonEntity>>(array);
            dbEntity db = new dbEntity();

            foreach (jsonEntity json in jmodel)
            {
                if (json.name.ToLower().IndexOf("source") >= 0)
                    db.Server_Name = json.value.ToLower();
                else if (json.name.ToLower().IndexOf("dbname") >= 0)
                    db.DB_Name = json.value.ToLower();
                else if (json.name.ToLower().IndexOf("user") >= 0)
                    db.DB_UserName = json.value.ToLower();
                else if (json.name.ToLower().IndexOf("password") >= 0)
                    db.DB_Password = json.value.ToLower();

            }
            string connectionstring = dbfunc.ConstituteConnectionString(db);
            msg = (dbfunc.TryDBConnect(connectionstring) == "" ? "Success" : "Fail");
        }
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        context.Response.Write(msg);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}