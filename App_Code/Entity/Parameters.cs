using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Parameters 的摘要描述
/// </summary>
public   class Parameters
{
    public string ParamName { get; set; }
    public SqlDbType dbType { get; set; }
    public string PValue { get; set; }
    public static Parameters getParameter(string paraName, SqlDbType dbType, string value)
    {
        Parameters para = new Parameters();
        para.ParamName = paraName;
        para.PValue = value;
        para.dbType = dbType;
        return para;
    }
}
