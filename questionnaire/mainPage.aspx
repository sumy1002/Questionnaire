<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="mainPage.aspx.cs" Inherits="questionnaire.mainPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    #userInfoQues {
        margin-left: 30px;
    }

    #btnSpace {
        text-align: right;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 align="center"><%# Eval("Title") %></h2>
    <br />
    <br />

    <div id="userInfoQues">
        <p></p>
        <br />
        <asp:Literal ID="ltlName" runat="server">姓名</asp:Literal>
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
        <asp:Literal ID="ltlPhone" runat="server">手機</asp:Literal>
        <asp:TextBox ID="txtPhone" runat="server" TextMode="Phone"></asp:TextBox><br />
        <asp:Literal ID="ltlEmail" runat="server">Email</asp:Literal>
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email"></asp:TextBox><br />
        <asp:Literal ID="ltlAge" runat="server">年齡</asp:Literal>
        <asp:TextBox ID="txtAge" runat="server" TextMode="Number"></asp:TextBox><br />
        <br />
        <br />
        <br />
        <p></p>
    </div>
    <br />
    <br />
    <p></p>共 ? 個問題<br />
    <div id="btnSpace">
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
    <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" />
    </div>
</asp:Content>
