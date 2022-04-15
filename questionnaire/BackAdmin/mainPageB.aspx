<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="mainPageB.aspx.cs" Inherits="questionnaire.BackAdmin.mainPageB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <!--套用jQuery-->
    <script src="../JavaScript/jquery-tablepage-1.0.js"></script>
    <style>
        li {
            width: auto;
            font-weight: 600;
            font-size: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#paper">問卷</a></li>
        <li><a data-toggle="tab" href="#question">問題</a></li>
    </ul>

    <div class="tab-content">
        <asp:PlaceHolder ID="plc1" runat="server">
            <div id="paper" class="tab-pane fade in active">
                <p>
                    <asp:Literal ID="ltltitle" runat="server">問卷名稱</asp:Literal>
                    <asp:TextBox ID="txttitle" runat="server" Width="250"></asp:TextBox><br />
                    <asp:Literal ID="ltlcontent" runat="server">描述內容</asp:Literal>
                    <asp:TextBox ID="txtcontent" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:Literal ID="ltlStart" runat="server">開始時間</asp:Literal>
                    <asp:TextBox ID="txtStart" runat="server" TextMode="Date" Width="250"></asp:TextBox><br />
                    <asp:Literal ID="ltlEnd" runat="server">結束時間</asp:Literal>
                    <asp:TextBox ID="txtEnd" runat="server" TextMode="Date" Width="250"></asp:TextBox><br />
                    <br />
                    <asp:CheckBox ID="ckbEnable" runat="server" Text="已啟用" Checked="true" />
                    <br />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" />
                    &emsp;&emsp;&emsp;&emsp;&emsp;
                <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click"/>
                </p>
            </div>
        </asp:PlaceHolder>

        <div id="question" class="tab-pane fade">
            <p>
                <asp:Literal ID="ltlType" runat="server">種類</asp:Literal>
                <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList>
                <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button3_Click" OnClientClick="" />
                <br />
                <br />
                <asp:Literal ID="ltlQues" runat="server">問題</asp:Literal>
                <asp:TextBox ID="txtQues" runat="server" TextMode="MultiLine"></asp:TextBox>&nbsp;
                <asp:DropDownList ID="ddlQuesType" runat="server"></asp:DropDownList>&nbsp;
                <asp:CheckBox ID="ckbMust" runat="server" Text="必填" />
                <br />
                <asp:Literal ID="ltlAnswer" runat="server">回答</asp:Literal>
                <asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>&nbsp;
                <span>﹝多個答案以；分隔﹞</span>&emsp;
                <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" /><br />
            </p>

            <br />
            <asp:ImageButton ID="ImageButton1" runat="server" />
            <table border="1">
                <tr>
                    <th></th>
                    <th>編號</th>
                    <th>問題</th>
                    <th>種類</th>
                    <th>必填</th>
                    <th></th>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="CheckBox3" runat="server" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        <asp:CheckBox ID="CheckBox4" runat="server" />
                    </td>
                    <td><a>編輯</a></td>
                </tr>
            </table>
            <p></p>
            <asp:Button ID="Button1" runat="server" Text="取消" />
            &emsp;&emsp;&emsp;&emsp;&emsp;
            <asp:Button ID="Button2" runat="server" Text="送出" />
        </div>
    </div>

    <script>
        $("#tblUserInfo").tablepage($("#table_pageA2"), 10);
    </script>
</asp:Content>
