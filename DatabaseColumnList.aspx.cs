using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class DatabaseColumnList : System.Web.UI.Page
{
    DbTableHerper db = new DbTableHerper();
    DataBaseFunc dbFunc = new DataBaseFunc();
    protected DataTable FromColumnInfo
    {
        get { return ViewState["FromColumnInfo"] as DataTable ?? null; }
        set { ViewState["FromColumnInfo"] = value; }
    }
    protected DataTable ToColumnInfo
    {
        get { return ViewState["ToColumnInfo"] as DataTable ?? null; }
        set { ViewState["ToColumnInfo"] = value; }
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
        if (Session["Fromdb"] == null || Session["Todb"] == null
            || string.IsNullOrEmpty(Request.QueryString["FromTable"]) || string.IsNullOrEmpty(Request.QueryString["ToTable"])
            || string.IsNullOrEmpty(Request.QueryString["FromSchema"]) || string.IsNullOrEmpty(Request.QueryString["ToSchema"]))
        {
            Response.Redirect("~/DataBaseConnectionSetting.aspx");

        }
        if (!IsPostBack)
        {

            initpage();
        }
    }

    private void initpage()
    {
        FromDB = (dbEntity)Session["Fromdb"];
        ToDB = (dbEntity)Session["Todb"];
        listviewBind();


    }

    private void listviewBind()
    {
        string FromSchema = Request.QueryString["FromSchema"];
        string FromTable = Request.QueryString["FromTable"];
        string ToSchema = Request.QueryString["ToSchema"];
        string ToTable = Request.QueryString["ToTable"];

        FromColumnInfo = db.GetColumnList(dbFunc.ConstituteConnectionString(FromDB), FromSchema, FromTable);
        ToColumnInfo = db.GetColumnList(dbFunc.ConstituteConnectionString(ToDB), ToSchema, ToTable);

        lvDBColumnList.DataSource = FromColumnInfo;
        lvDBColumnList.DataBind();
    }

    protected void lvDBColumnList_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //來源描述複製到更新描述(單筆)
        if (string.Equals(e.CommandName, "CopyColumnDesc"))
        {

            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (((DropDownList)dataItem.FindControl("ddlToColumn")).SelectedValue != "")
            {
                string ToSchema = Request.QueryString["ToSchema"];
                string Table_Name = Request.QueryString["ToTable"];
                string Column_Name = ((DropDownList)dataItem.FindControl("ddlToColumn")).SelectedValue;
                string Column_CNAME = ((TextBox)dataItem.FindControl("txtColumn_CNAME")).Text;

                try
                {
                    if (db.CheckTableColumnisexists(dbFunc.ConstituteConnectionString(ToDB), ToSchema, Table_Name, Column_Name))
                    {
                        db.UpdateDBTableColumnDescription(dbFunc.ConstituteConnectionString(ToDB), ToSchema, Table_Name, Column_Name, Column_CNAME);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('匯入成功');", true);
                        listviewBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('查無欄位');", true);
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
        //來源描述更新
        if (string.Equals(e.CommandName, "FromDescriptUpdate")) {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            string FromSchemaSchema = Request.QueryString["FromSchema"];
            string Table_Name = Request.QueryString["FromTable"];
            string Column_Name = ((DropDownList)dataItem.FindControl("ddlToColumn")).SelectedValue;
            try
            {
                string Column_CNAME = ((TextBox)dataItem.FindControl("txtColumn_CNAME")).Text;
                db.UpdateDBTableColumnDescription(dbFunc.ConstituteConnectionString(FromDB), FromSchemaSchema, Table_Name, Column_Name, Column_CNAME);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新成功');", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "prompt", "alert('更新失敗');", true);
            }
        }
    }

    protected void lvDBColumnList_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        System.Web.UI.Control ddlToColumn = e.Item.FindControl("ddlToColumn");
        string ColumnName = ((Label)e.Item.FindControl("labColumn_Name")).Text;

        if (ddlToColumn != null)
        {
            (ddlToColumn as DropDownList).DataSource = ToColumnInfo;
            (ddlToColumn as DropDownList).DataBind();
            (ddlToColumn as DropDownList).Items.Insert(0, new ListItem(" ", ""));
            (ddlToColumn as DropDownList).DataValueField = "objname";
            (ddlToColumn as DropDownList).DataTextField = "objname";
            if ((ddlToColumn as DropDownList).Items.FindByValue(ColumnName) != null)
            {
                (ddlToColumn as DropDownList).SelectedValue = ColumnName;
                ddlToColumn_SelectedIndexChanged((ddlToColumn as DropDownList), null);
            }
        }
    }

    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (ListViewItem row in this.lvDBColumnList.Items)
            {
                if (((DropDownList)row.FindControl("ddlToColumn")).SelectedValue != "")
                {
                    string ToSchema = Request.QueryString["ToSchema"];
                    string Table_Name = Request.QueryString["ToTable"];
                    string Column_Name = ((DropDownList)row.FindControl("ddlToColumn")).SelectedValue;
                    string Column_CNAME = ((TextBox)row.FindControl("txtColumn_CNAME")).Text;
                    db.UpdateDBTableColumnDescription(dbFunc.ConstituteConnectionString(ToDB), ToSchema, Table_Name, Column_Name, Column_CNAME);

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

    protected void ddlToColumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlTable = sender as DropDownList;
        var query = from row in ToColumnInfo.AsEnumerable()
                    where row.Field<string>("objname").ToString() == ddlTable.SelectedValue
                    select row;


        Label labToColumn_value = (((DropDownList)sender).Parent as ListViewDataItem).FindControl("labToColumn_value") as Label;

        labToColumn_value.Text = "";
        foreach (DataRow value in query)
        {
            labToColumn_value.Text += value.Field<string>("value");
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DatabaseTableList.aspx");

    }
}