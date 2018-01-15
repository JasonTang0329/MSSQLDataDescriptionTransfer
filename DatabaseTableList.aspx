<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DatabaseTableList.aspx.cs" Inherits="DatabaseTableList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function pageback() {
            window.location = "DataBaseConnectionSetting.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container  ">
        <asp:Button ID="back" runat="server" Text="←Back" class="btn btn-sm btn-warning" OnClick="back_Click" />
        <div class="row">
            <div class="col-md-12">
                <p class="text-center">來源位置：<b><%="[From]." + HttpUtility.HtmlDecode(((dbEntity)Session["Fromdb"]).Server_Name) %></b> 資料庫名稱：<b><%=HttpUtility.HtmlDecode(((dbEntity)Session["Fromdb"]).DB_Name) %></b>，匯入位置：<b><%="[To]." + HttpUtility.HtmlDecode(((dbEntity)Session["Todb"]).Server_Name) %></b> 資料庫名稱：<b><%=HttpUtility.HtmlDecode(((dbEntity)Session["Todb"]).DB_Name) %></b></p>
                <asp:ListView ID="lvDBContent" runat="server" ItemPlaceholderID="itemPlaceholder" DataKeyNames="TABLE_SCHEMA,TABLE_NAME" OnItemCommand="lvDBContent_ItemCommand" OnItemDataBound="lvDBContent_ItemDataBound">
                    <LayoutTemplate>
                        <table class="table table-hover">
                            <thead>
                                <tr>
                                    <th class="text-center">來源資料表名稱</th>
                                    <th class="text-center">來源資料表中文別名</th>
                                    <th class="text-center">匯入資料表名稱</th>
                                    <th class="text-center">匯入資料表中文別名</th>
                                    <th class="text-center" colspan="2">功能</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                            </tbody>
                            <tr>
                                <td style="text-align: center; padding: 5px" colspan="6">
                                    <p>(自動同步僅同步資料表名稱與欄位名稱一致之欄位描述)</p>
                                    <asp:Button ID="btnSaveFromAll" runat="server" Text="所有來源資料表描述更新" class="btn btn-info" OnClick="btnSaveFromAll_Click" />
                                    <asp:Button ID="btnSaveAll" runat="server" Text="所有資料表描述複製" class="btn btn-success" OnClick="btnSaveAll_Click" />
                                    <asp:Button ID="btnSubmit" runat="server" Text="自動同步以選擇資料表欄位描述資訊" class="btn btn-primary" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="padding: 3px; text-align: left">
                                <asp:Label ID="labTABLE_SCHEMA" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("TABLE_SCHEMA").ToString())%>'></asp:Label>.
                                        <asp:Label ID="labTABLE_NAME" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("TABLE_NAME").ToString())%>'></asp:Label>
                                <asp:HiddenField ID="hidTABLE_Type" runat="server" Value='<%#HttpUtility.HtmlDecode(Eval("TABLE_TYPE").ToString().Replace("BASE ", ""))%>' />
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:TextBox ID="txtTABLE_CNAME" runat="server" Text='<%#HttpUtility.HtmlDecode(Eval("value").ToString())%>'></asp:TextBox>
                                <asp:HiddenField ID="hidTABLE_CNAME" runat="server" Value='<%#HttpUtility.HtmlDecode(Eval("value").ToString())%>' />
                                <asp:Button ID="btnUpdate" class="btn btn-sm btn-info" runat="server" Text="更新" CommandName="FromDescriptUpdate" />
                                <%--                                <asp:Label ID="labTABLE_CNAME" runat="server" Text='<%#Eval("value")%>'></asp:Label>--%>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:DropDownList ID="ddlToTable" runat="server" DataValueField="TABLE_NAME" DataTextField="TABLE_NAME" OnSelectedIndexChanged="ddlTable_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:Label ID="labToTABLE_CNAME" runat="server" Text=''></asp:Label>
                                <asp:HiddenField ID="hidToTABLE_SCHEMA" runat="server" Value="" />
                                <asp:HiddenField ID="hidToTABLE_Type" runat="server" Value="" />
                                <%--It maybe is table or view--%>
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:Button ID="btnExe" runat="server" class="btn btn-sm btn-success btn-block" Text="資料表描述複製" CommandName="CopyTableName" />
                            </td>
                            <td style="padding: 3px; text-align: center">
                                <asp:Button ID="btnMapping" runat="server" class="btn btn-sm btn-link btn-block" Text="前往欄位描述複製" Visible='<%# (Eval("TABLE_TYPE").ToString().Replace("BASE ", "") == "TABLE") %>' CommandName="CopyTableColumnName" />
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

