<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="statisticPage.aspx.cs" Inherits="questionnaire.statisticPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #topDiv{
            margin-top:20px;
        }

        #divDynamic{
            margin: 20px auto;
            border: 0px solid #000000;
            text-align: left;
        }

        #btnSpace {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- 投票狀態及日期 --%>
    <div class="col-lg-10" align="right" id="topDiv">
        <asp:Literal ID="ltlVote" runat="server"></asp:Literal><br />
        <asp:Literal ID="ltlTime" runat="server"></asp:Literal>
    </div>

    <div class="col-lg-12" align="center">
        <h2>
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
        <h4>
            <asp:Literal ID="ltlContent" runat="server"></asp:Literal></h4>
    </div>

    <div id="divDynamic" class="col-lg-5" align="center">
        <asp:PlaceHolder ID="plcDynamic" runat="server"></asp:PlaceHolder>
    </div>

    <%-- 返回列表 --%>
    <div id="btnSpace" class="col-lg-10">
        <asp:Button ID="btnBack" runat="server" Text="返回列表" OnClick="btnBack_Click"/>
    </div>
</asp:Content>
