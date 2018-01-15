using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
        btnLogout.Visible = (Session["Account"] != null);
    }


    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["Account"] = null;
        Response.Redirect(@"~/Login.aspx");
    }
}
