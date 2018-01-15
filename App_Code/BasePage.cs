using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public BasePage()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    protected override void OnPreInit(EventArgs e)
    {
        //Login Valid
        if (Session["Account"] == null)
        {

            Response.Redirect(@"~/Login.aspx");
        }

    }
}