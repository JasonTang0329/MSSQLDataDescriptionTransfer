using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DatabaseTableList : BasePage
{
    DbTableHerper db = new DbTableHerper();
    DataBaseFunc dbFunc = new DataBaseFunc();
    ////dbEntity FromDB = new dbEntity();
    ////dbEntity ToDB = new dbEntity();
    protected DataTable FromInfo
    {
        get { return ViewState["FromInfo"] as DataTable ?? null; }
        set { ViewState["FromInfo"] = value; }
    }
    protected DataTable ToInfo
    {
        get { return ViewState["ToInfo"] as DataTable ?? null; }
        set { ViewState["ToInfo"] = value; }
    }
    protected dbEntity FromDB
    {
        get { return ViewState["FromDB"] as dbEntity ?? null; }
        set { ViewState["FromDB"] = value; }
    }
    protected dbEntity ToDB
    {
        get { return ViewState["ToDB"] as dbEntity ?? null; }
        set { ViewState["ToDB"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Fromdb"] == null || Session["Todb"] == null)
        {
            Response.Redirect("~/DataBaseConnectionSetting.aspx");

        }
        if (!IsPostBack)
        {

            initpage();
        }
    }

    /// <summary>
    /// 頁面載入
    /// </summary>
    private void initpage()
    {
        FromDB = (dbEntity)Session["Fromdb"];
        ToDB = (dbEntity)Session["Todb"];
        listviewBind();

    }
    /// <summary>
    /// 重新綁訂表格
    /// </summary>
    private void listviewBind()
    {
        FromInfo = db.GetTableList(dbFunc.ConstituteConnectionString(FromDB));
        ToInfo = db.GetTableList(dbFunc.ConstituteConnectionString(ToDB));

        lvDBContent.DataSource = FromInfo;
        lvDBContent.DataBind();
    }

    /// <summary>
    /// 下拉式選單異動
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTable_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlTable = sender as DropDownList;
        var query = from row in ToInfo.AsEnumerable()
                    where row.Field<string>("TABLE_NAME").ToString() == ddlTable.SelectedValue
                    select row;


        Label labToTABLE_CNAME = (((DropDownList)sender).Parent as ListViewDataItem).FindControl("labToTABLE_CNAME") as Label;
        HiddenField hidToTABLE_SCHEMA = (((DropDownList)sender).Parent as ListViewDataItem).FindControl("hidToTABLE_SCHEMA") as HiddenField;
        HiddenField hidToTABLE_Type = (((DropDownList)sender).Parent as ListViewDataItem).FindControl("hidToTABLE_Type") as HiddenField;

        labToTABLE_CNAME.Text = "";
        foreach (DataRow value in query)
        {
            labToTABLE_CNAME.Text += value.Field<string>("value");
            hidToTABLE_SCHEMA.Value += value.Field<string>("TABLE_SCHEMA");
            hidToTABLE_Type.Value += value.Field<string>("TABLE_TYPE").Replace("BASE ", "");
        }

    }

    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewItem row in this.lvDBContent.Items)
            {
                if (((DropDownList)row.FindControl("ddlToTable")).SelectedValue != "")
                {
                    string Table_Type = ((HiddenField)row.FindControl("hidToTABLE_Type")).Value;
                    string Table_Schema = ((HiddenField)row.FindControl("hidToTABLE_SCHEMA")).Value;
                    string Table_Name = ((DropDownList)row.FindControl("ddlToTable")).SelectedValue;
                    string Table_Cname = ((TextBox)row.FindControl("txtTABLE_CNAME")).Text;
                    if (db.CheckTableisexists(dbFunc.ConstituteConnectionString(ToDB), Table_Schema, Table_Name))
                    {
                        db.UpdateDBTableDescription(dbFunc.ConstituteConnectionString(ToDB), Table_Type, Table_Schema, Table_Name, Table_Cname);
                    }
                }

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新匯入成功');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新匯入失敗');", true);

        }
        finally
        {
            listviewBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewItem row in this.lvDBContent.Items)
            {
                if (((DropDownList)row.FindControl("ddlToTable")).SelectedValue != "")
                {

                    string FromTable_Name = ((Label)row.FindControl("labTABLE_NAME")).Text;
                    string FromTable_Schema = ((Label)row.FindControl("labTABLE_SCHEMA")).Text;

                    string ToTable_Name = ((DropDownList)row.FindControl("ddlToTable")).SelectedValue;
                    string ToTable_Schema = ((HiddenField)row.FindControl("hidToTABLE_SCHEMA")).Value;

                    DataTable FromColumnInfo = db.GetColumnList(dbFunc.ConstituteConnectionString(FromDB), FromTable_Schema, FromTable_Name);
                    DataTable ToColumnInfo = db.GetColumnList(dbFunc.ConstituteConnectionString(ToDB), ToTable_Schema, ToTable_Name);
                    var userRolesInfo1 = from u in FromColumnInfo.AsEnumerable()
                                         join ur in ToColumnInfo.AsEnumerable()
                                         on u.Field<string>("objname") equals ur.Field<string>("objname")
                                         where u.Field<string>("value").Trim() != ""
                                         select new
                                         {
                                             FromColumnName = u.Field<string>("objname"),
                                             FromColumnDesc = u.Field<string>("value"),
                                             ToColumnName = ur.Field<string>("objname"),
                                         };
                    foreach (var item in userRolesInfo1)
                    {
                        if (db.CheckTableColumnisexists(dbFunc.ConstituteConnectionString(ToDB), ToTable_Schema, ToTable_Name, item.ToColumnName))
                        {
                            db.UpdateDBTableColumnDescription(dbFunc.ConstituteConnectionString(ToDB), ToTable_Schema, ToTable_Name, item.ToColumnName, item.FromColumnDesc);
                        }
                    }

                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新匯入成功');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新匯入失敗');", true);

        }
        finally
        {
            listviewBind();
        }
    }



    protected void lvDBContent_ItemDataBound(object sender, ListViewItemEventArgs e)
    {

        System.Web.UI.Control ddlToTable = e.Item.FindControl("ddlToTable");
        string TableName = ((Label)e.Item.FindControl("labTABLE_NAME")).Text;

        if (ddlToTable != null)
        {
            (ddlToTable as DropDownList).DataSource = ToInfo;
            (ddlToTable as DropDownList).DataBind();
            (ddlToTable as DropDownList).Items.Insert(0, new ListItem(" ", ""));
            (ddlToTable as DropDownList).DataValueField = "TABLE_NAME";
            (ddlToTable as DropDownList).DataTextField = "TABLE_NAME";
            if ((ddlToTable as DropDownList).Items.FindByValue(TableName) != null)
            {
                (ddlToTable as DropDownList).SelectedValue = TableName;
                ddlTable_SelectedIndexChanged((ddlToTable as DropDownList), null);
            }
        }
    }

    protected void lvDBContent_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //使用來源資料表描述更新資料表描述
        if (string.Equals(e.CommandName, "CopyTableName"))
        {

            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (((DropDownList)dataItem.FindControl("ddlToTable")).SelectedValue != "")
            {
                string Table_Type = ((HiddenField)dataItem.FindControl("hidToTABLE_Type")).Value;
                string Table_Schema = ((HiddenField)dataItem.FindControl("hidToTABLE_SCHEMA")).Value;
                string Table_Name = ((DropDownList)dataItem.FindControl("ddlToTable")).SelectedValue;
                string Table_Cname = ((TextBox)dataItem.FindControl("txtTABLE_CNAME")).Text;
                try
                {
                    if (db.CheckTableisexists(dbFunc.ConstituteConnectionString(ToDB), Table_Schema, Table_Name))
                    {
                        db.UpdateDBTableDescription(dbFunc.ConstituteConnectionString(ToDB), Table_Type, Table_Schema, Table_Name, Table_Cname);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('匯入成功');", true);
                        listviewBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('查無此資料表');", true);

                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('匯入失敗');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('請選擇匯入資料表');", true);
            }
        }
        //到資料表欄位描述
        if (string.Equals(e.CommandName, "CopyTableColumnName"))
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (((DropDownList)dataItem.FindControl("ddlToTable")).SelectedValue != "")
            {
                string FromTable_Name = ((Label)dataItem.FindControl("labTABLE_NAME")).Text;
                string FromTable_Schema = ((Label)dataItem.FindControl("labTABLE_SCHEMA")).Text;

                string ToTable_Name = ((DropDownList)dataItem.FindControl("ddlToTable")).SelectedValue;
                string ToTable_Schema = ((HiddenField)dataItem.FindControl("hidToTABLE_SCHEMA")).Value;

                Response.Redirect("~/DatabaseColumnList.aspx?FromTable=" + HttpUtility.UrlEncode(FromTable_Name) + "&FromSchema=" + HttpUtility.UrlEncode(FromTable_Schema) + "&ToTable=" + HttpUtility.UrlEncode(ToTable_Name) + "&ToSchema=" + HttpUtility.UrlEncode(ToTable_Schema));
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('請選擇匯入資料表');", true);
            }

        }
        //來源描述更新
        if (string.Equals(e.CommandName, "FromDescriptUpdate"))
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            string Table_Type = ((HiddenField)dataItem.FindControl("hidTABLE_Type")).Value;
            string Table_Schema = ((Label)dataItem.FindControl("labTABLE_SCHEMA")).Text;
            string Table_Name = ((Label)dataItem.FindControl("labTABLE_NAME")).Text;
            string Table_Cname = ((TextBox)dataItem.FindControl("txtTABLE_CNAME")).Text;
            string Table_OCname = ((HiddenField)dataItem.FindControl("hidTABLE_CNAME")).Value;
            if (Table_Cname != Table_OCname)
            {
                try
                {
                    if (db.CheckTableisexists(dbFunc.ConstituteConnectionString(FromDB), Table_Schema, Table_Name))
                    {
                        db.UpdateDBTableDescription(dbFunc.ConstituteConnectionString(FromDB), Table_Type, Table_Schema, Table_Name, Table_Cname);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新成功');", true);
                        listviewBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('查無此資料表');", true);

                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新失敗');", true);
                }
            }
        }
    }

    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DataBaseConnectionSetting.aspx");

    }
    /// <summary>
    /// 來源資料表描述更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveFromAll_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewItem row in this.lvDBContent.Items)
            {
                string Table_Cname = ((TextBox)row.FindControl("txtTABLE_CNAME")).Text;
                string Table_OCname = ((HiddenField)row.FindControl("hidTABLE_CNAME")).Value;
                if (Table_Cname != Table_OCname)
                {
                    string Table_Type = ((HiddenField)row.FindControl("hidTABLE_Type")).Value;
                    string Table_Schema = ((Label)row.FindControl("labTABLE_SCHEMA")).Text;
                    string Table_Name = ((Label)row.FindControl("labTABLE_NAME")).Text;

                    if (db.CheckTableisexists(dbFunc.ConstituteConnectionString(FromDB), Table_Schema, Table_Name))
                    {
                        db.UpdateDBTableDescription(dbFunc.ConstituteConnectionString(FromDB), Table_Type, Table_Schema, Table_Name, Table_Cname);
                    }                    
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新來源描述成功');", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新來源描述失敗');", true);

        }
        finally
        {
            listviewBind();
        }
    }
}