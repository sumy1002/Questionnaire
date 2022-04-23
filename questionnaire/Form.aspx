<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="questionnaire.mainPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #divTiCon {
            border: 0px solid #000000;
        }

            #divTiCon #divTitle {
                margin: 30px;
            }

            #divTiCon #divContent {
                margin: 30px;
            }

        #divUserInfo {
            margin: 20px auto;
            border: 0px solid #000000;
            text-align: left;
            line-height: 35px;
        }

        #divDynamic {
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
    <div class="col-lg-12" align="right">
        <asp:Literal ID="ltlVote" runat="server"></asp:Literal><br />
        <asp:Literal ID="lvlTime" runat="server"></asp:Literal>
    </div>

    <%-- 標題&內文 --%>
    <div id="divTiCon" class="col-lg-12" align="center">
        <div id="divTitle">
            <asp:HiddenField ID="hfID" runat="server" />
            <h2>
                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal></h2>
        </div>
        <div id="divContent" class="col-lg-5">
            <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
        </div>
    </div>

    <%-- 必填個人資訊 --%>
    <div id="divUserInfo" class="col-lg-5" align="center">
        <asp:Literal ID="ltlName" runat="server">姓名</asp:Literal>&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtName" runat="server" Height="24px"></asp:TextBox><br />
        <asp:Literal ID="ltlPhone" runat="server">手機</asp:Literal>&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtPhone" runat="server" TextMode="Phone" Height="24px"></asp:TextBox><br />
        <asp:Literal ID="ltlEmail" runat="server">Email</asp:Literal>&nbsp;
        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" Height="24px"></asp:TextBox><br />
        <asp:Literal ID="ltlAge" runat="server">年齡</asp:Literal>&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="txtAge" runat="server" TextMode="Number" min="1" Height="24px"></asp:TextBox>
    </div>

    <%-- 問卷 --%>
    <div id="divDynamic" class="col-lg-5" align="center">
        <asp:PlaceHolder ID="plcDynamic" runat="server"></asp:PlaceHolder>
    </div>

    <%-- 問題數量及送出 --%>
    <div id="btnSpace">
        <asp:PlaceHolder ID="plcBtn" runat="server">
            <asp:Literal ID="ltlQCount" runat="server"></asp:Literal>
            <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
            <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" />
        </asp:PlaceHolder>
    </div>

</asp:Content>
