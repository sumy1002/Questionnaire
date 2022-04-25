<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="statisticPage.aspx.cs" Inherits="questionnaire.statisticPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-lg-8">
        <h2>
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
        <h4>
            <asp:Literal ID="ltlContent" runat="server"></asp:Literal></h4>
        <asp:PlaceHolder ID="plcDynamic" runat="server"></asp:PlaceHolder>
    </div>
</asp:Content>
