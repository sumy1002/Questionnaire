<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="listPage.aspx.cs" Inherits="questionnaire.listPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--套用jQuery-->
    <script src="JavaScript/jquery-tablepage-1.0.js"></script>
    <style>
        #topDiv {
            border: 2px solid #000000;
            margin: 20px;
            padding-left: 30px;
            padding-top: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="topDiv">
        <p>
            <asp:Literal ID="ltlTitle" runat="server">問卷標題</asp:Literal>
            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
        </p>
        <p>
            <asp:Literal ID="ltlDate" runat="server">開始／結束</asp:Literal>
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>&nbsp;
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
            &emsp;&emsp;
            <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
        </p>
    </div>
    <asp:Literal ID="ltlMsg" runat="server" Visible="false"></asp:Literal>
    <br /><br />
    <table id="tbl">
        <tr>
            <th>編號</th>
            <th>問卷標題</th>
            <th>狀態</th>
            <th>開始時間</th>
            <th>結束時間</th>
            <th>觀看統計</th>
        </tr>
        <asp:Repeater ID="rptQues" runat="server">
            <ItemTemplate>
                <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("TitleID") %>'/>
                <asp:HiddenField ID="hfSta" runat="server" Value='<%# Eval("StartDate") %>'/>
                <asp:HiddenField ID="hfEnd" runat="server" Value='<%# Eval("EndDate") %>'/>
                <tr>
                    <td width="50px"><%# Eval("TitleID") %></td>
                    <td width="250px"><a href="Form.aspx?ID=<%#Eval("QuestionnaireID") %>"><%# Eval("Title") %></a></td>
                    <td width="150px"><asp:Literal ID="ltlState" runat="server" Text='<%# Eval("strIsEnable") %>'></asp:Literal></td>
                    <td width="150px"><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("strStartTime") %>'></asp:Literal></td>
                    <td width="150px"><asp:Literal ID="Literal2" runat="server" Text='<%# Eval("strEndTime") %>'></asp:Literal></td>
                    <td><a>前往</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <span id='table_page'></span>

    <script>
        $("#tbl").tablepage($("#table_page"), 10);
    </script>
</asp:Content>
