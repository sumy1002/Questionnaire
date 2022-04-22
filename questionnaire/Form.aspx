<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="questionnaire.mainPage" %>

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
    <%-- 投票狀態及日期 --%>
    <div class="col-lg-12" align="right">
        <asp:Literal ID="ltlVote" runat="server"></asp:Literal><br />
        <asp:Literal ID="lvlTime" runat="server"></asp:Literal>
    </div>

    <%-- 標題&內文 --%>
    <div class="col-lg-12" align="center">
        <asp:HiddenField ID="hfID" runat="server" />
        <h2><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
        <h4><asp:Literal ID="ltlContent" runat="server"></asp:Literal></h4>
    </div>

    <%-- 必填個人資訊 --%>
    <div id="userInfoQues" align="center">
        <p></p>
        <br />
        <asp:Literal ID="ltlName" runat="server">姓名</asp:Literal>
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
        <asp:Literal ID="ltlPhone" runat="server">手機</asp:Literal>
        <asp:TextBox ID="txtPhone" runat="server" TextMode="Phone"></asp:TextBox><br />
        <asp:Literal ID="ltlEmail" runat="server">Email</asp:Literal>
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email"></asp:TextBox><br />
        <asp:Literal ID="ltlAge" runat="server">年齡</asp:Literal>
        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" min="1"></asp:TextBox><br />
    </div>

    <%-- 問卷 --%>
    <div align="center">
        <asp:PlaceHolder ID="plcDynamic" runat="server"></asp:PlaceHolder>
    </div>

    <%-- 問題數量及送出 --%>
    <div id="btnSpace">
        <asp:Literal ID="ltlQCount" runat="server"></asp:Literal>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
        <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" />
    </div>

</asp:Content>
