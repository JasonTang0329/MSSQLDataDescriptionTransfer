using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataBaseConnectionSetting : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataBaseFunc dbfunc = new DataBaseFunc();
        string msg = "";
        dbEntity FromDb = new dbEntity();
        FromDb.Server_Name = txtFromSource.Text;
        FromDb.DB_Name = txtFromDbname.Text;
        FromDb.DB_UserName = txtFromUser.Text;
        FromDb.DB_Password = txtFromPassword.Text;
        Session["Fromdb"] = FromDb;

        dbEntity ToDb = new dbEntity();
        ToDb.Server_Name = txtToSource.Text;
        ToDb.DB_Name = txtToDbname.Text;
        ToDb.DB_UserName = txtToUser.Text;
        ToDb.DB_Password = txtToPassword.Text;
        Session["Todb"] = ToDb;

        string Fconnectionstring = dbfunc.ConstituteConnectionString(FromDb);
        string Tconnectionstring = dbfunc.ConstituteConnectionString(ToDb);

        msg = (dbfunc.TryDBConnect(Fconnectionstring) == "" && dbfunc.TryDBConnect(Tconnectionstring) == "" ? "連線驗證通過" : "連線驗證失敗");
        string Jscript = "alert('" + msg + "');" + (msg == "連線驗證通過" ? "window.location = 'DatabaseTableList.aspx';" : "");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", Jscript, true);

    }
}