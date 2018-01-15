<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="DatabaseColumnList.aspx.cs" Inherits="DatabaseColumnList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container  ">
        <asp:Button ID="btnback" runat="server" Text="←Back" class="btn btn-sm btn-warning" OnClick="btnback_Click" />

        <div class="row">
            <div class="col-md-12">
                <p class="text-center">
                    來源位置：<b><%="[From]." + HttpUtility.HtmlDecode(((dbEntity)Session["Fromdb"]).Server_Name) %></b> 資料庫名稱：<b><%=HttpUtility.HtmlDecode(((dbEntity)Session["Fromdb"]).DB_Name) %></b> 資料表名稱：<b><%=HttpUtility.HtmlDecode(Request.QueryString["FromSchema"]+"."+Request.QueryString["FromTable"]) %></b><br />
                    匯入位置：<b><%="[To]." +HttpUtility.HtmlDecode( ((dbEntity)Session["Todb"]).Server_Name) %></b> 資料庫名稱：<b><%=HttpUtility.HtmlDecode(((dbEntity)Session["Todb"]).DB_Name) %></b> 資料表名稱：<b><%=HttpUtility.HtmlDecode(Request.QueryString["ToSchema"]+"."+Request.QueryString["ToTable"]) %></b>
                </p>
                <asp:ListView ID="lvDBColumnList" runat="server" ItemPlaceholderID="itemPlaceholder" DataKeyNames="objname" OnItemCommand="lvDBColumnList_ItemCommand" OnItemDataBound="lvDBColumnList_ItemDataBound">
                    <LayoutTemplate>
                       <b>如果想在IDAT的備註顯示，請用&lt;MEMO&gt;&lt;/MEMO&gt;包起來</b>
                        <table class="table table-hover">
                            <thead>
                                <tr>
                                    <th class="text-center">來源欄位名稱</th>
                                    <th class="text-center">來源欄位中文別名</th>
                                    <th class="text-center">匯入欄位名稱</th>
                                    <th class="text-center">匯入欄位中文別名</th>
                                    <th class="text-center">功能</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </tbody>
                            <tr>
                                <td style="text-align: center; padding: 5px" colspan="5">
                                    <p>(自動同步僅同步資料表名稱與欄位名稱一致之欄位描述)</p>
                                    <asp:Button ID="btnSaveAll" runat="server" Text="所有欄位描述複製" class="btn btn-success" OnClick="btnSaveAll_Click" />
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="labTABLE_SCHEMA" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("TABLE_SCHEMA").ToString())%>'></asp:Label>.
                                <asp:Label ID="labTABLE_NAME" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("TABLE_NAME").ToString())%>'></asp:Label>.
                                <asp:Label ID="labColumn_Name" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("objname").ToString())%>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtColumn_CNAME" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("value").ToString())%>'></asp:TextBox>
                                <asp:Button ID="btnUpdate" runat="server" Text="更新" CommandName="FromDescriptUpdate" />
<%--                                <asp:Label ID="labColumn_CNAME" runat="server" Text='<%#Eval("value")%>'></asp:Label>--%>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:DropDownList ID="ddlToColumn" runat="server" DataValueField="objname" DataTextField="objname" OnSelectedIndexChanged="ddlToColumn_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:Label ID="labToColumn_value" runat="server" Text=''></asp:Label>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:Button ID="btnExe" runat="server" class="btn btn-sm btn-success btn-block" Text="欄位描述複製" CommandName="CopyColumnDesc" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <center>
                                <br />
                                目前無資料!
                            </center>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </div>

    </div>

</asp:Content>

