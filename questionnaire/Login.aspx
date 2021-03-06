<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="questionnaire.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <style>
        #login{
            border:0px solid #000000;
            text-align:center;
            margin:20px auto;
            padding:20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="login">
            <asp:PlaceHolder runat="server" ID="plcLogin">Account&nbsp&nbsp
            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox><br /><br />
                Password&nbsp
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br /><br />
                <asp:Button ID="btnCancel" runat="server" Text="返回列表" OnClick="btnCancel_Click"/>
                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnLogin" runat="server" Text="登入" OnClick="btnLogin_Click" /><br /><br />

                <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red">登入失敗，請檢查帳號密碼。</asp:Label>
            </asp:PlaceHolder>

            <asp:PlaceHolder runat="server" ID="plcUserPage">
                <asp:Literal ID="ltlAccount" runat="server"></asp:Literal><br />
                請前往 <a href="/BackAdmin/ListPageAdmin.aspx">後台 </a>
            </asp:PlaceHolder>
        </div>
    </form>
</body>
</html>
