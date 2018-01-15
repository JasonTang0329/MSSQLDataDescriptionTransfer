using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// dbEntity 的摘要描述
/// </summary>
[Serializable]
public class dbEntity
{
    //伺服器IP Domain 
    public string Server_Name { get; set; }
    //Database Name 
    public string DB_Name { get; set; }
    //Database User
    public string DB_UserName { get; set; }
    //Database Passwrod
    public string DB_Password { get; set; }

}