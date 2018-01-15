<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container col-md-8 col-md-offset-2">
        <h2 class="form-signin-heading">請登入使用</h2>
        <label for="txtAccount" class="sr-only">Account</label>
        <asp:TextBox ID="txtAccount" runat="server" class="form-control" placeholder="Account" required="" autofocus=""></asp:TextBox>
        <label for="txtPassword" class="sr-only">Password</label>
        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control" placeholder="Password" required=""></asp:TextBox>
        <asp:Button ID="btnLogin" class="btn btn-lg btn-primary btn-block" OnClick="btnLogin_Click" runat="server" Text="登入" />
        <asp:CustomValidator ID="CustomValidator1" runat="server" ForeColor="Red"
            OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>

    </div>
</asp:Content>

