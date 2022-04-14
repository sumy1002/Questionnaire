<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="questionnaire.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:PlaceHolder runat="server" ID="plcLogin">
            Account: 
            <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox><br />
            Password: 
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br />
            <asp:Button ID="btnLogin" runat="server" Text="登入" OnClick="btnLogin_Click" /><br />

            <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="plcUserPage">
            <asp:Literal ID="ltlAccount" runat="server"></asp:Literal><br />
            請前往 <a href="/BackAdmin/listPageA.aspx"> 後台 </a>
        </asp:PlaceHolder>
    </form>
</body>
</html>
