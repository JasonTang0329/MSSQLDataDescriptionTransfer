using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    static List<List<string>> listAccounts = new List<List<string>>();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["Account"] != null)
                Response.Redirect("~/DataBaseConnectionSetting.aspx");
            else
            {
                listAccounts.Clear();
                List<string> listAccount = new List<string>();
                listAccount.Add("jason");
                listAccount.Add("Jason");
                listAccounts.Add(listAccount);


            }
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)//若必選欄位未驗證過
        {
            return;
        }


        Response.Redirect("~/DataBaseConnectionSetting.aspx");
    }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)//驗證帳密
    {
        Session.Remove("Account");
        int AccountIndex = listAccounts.FindIndex(delegate (List<string> list) { return list[0].ToUpper().Trim() == txtAccount.Text.Trim().ToUpper(); });
        if (AccountIndex < 0)
        {
            CustomValidator1.ErrorMessage = "查無此使用者";
            args.IsValid = false;//
            return;
        }
        else
        {
            if (listAccounts[AccountIndex][1].Trim() != txtPassword.Text.Trim())
            {
                CustomValidator1.ErrorMessage = "密碼錯誤";
                args.IsValid = false;//
                return;
            }
        }

        Session["Account"] = listAccounts[AccountIndex][0].Trim();
    }


}