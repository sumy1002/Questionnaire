<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="NewQuesDetail.aspx.cs" Inherits="questionnaire.BackAdmin.NewQuesDetail1" %>


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
        <li class="active"><a data-toggle="tab" href="#question">問題</a></li>
    </ul>

    <div class="tab-content">

        <div id="question" class="tab-pane fade">
            <p>
                <asp:Literal ID="ltlType" runat="server">種類</asp:Literal>
                <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                <asp:Button ID="btnAddCQ" runat="server" Text="Button" OnClick="btnAddCQ_Click" OnClientClick="" />
                <br />
                <br />
                <asp:Literal ID="ltlQues" runat="server">問題</asp:Literal>
                <asp:TextBox ID="txtQues" runat="server" Width="250px"></asp:TextBox>&nbsp;
                <asp:DropDownList ID="ddlQuesType" runat="server"></asp:DropDownList>&nbsp;
                <asp:CheckBox ID="ckbNess" runat="server" Text="必填" />
                <br />
                <asp:Literal ID="ltlAnswer" runat="server">回答</asp:Literal>
                <asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>&nbsp;
                <span>﹝多個答案以；分隔﹞</span>&emsp;
                <asp:Button ID="btnQuesAdd" runat="server" Text="加入" OnClick="btnQuesAdd_Click" /><br />
            </p>

            <br />
            <asp:ImageButton ID="ImageButton1" runat="server" />

            <asp:HiddenField ID="hfQuesList" runat="server" />
            <table border="1">
                <tr>
                    <th></th>
                    <th>編號</th>
                    <th>問題</th>
                    <th>種類</th>
                    <th>必填</th>
                    <th></th>
                </tr>
                <asp:Repeater ID="rptQuesItem" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ckbDel" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lblnumber" runat="server" ></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQues" runat="server" Text='<%#Eval("QuesTitle") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQType" runat="server" Text='<%#Eval("QuesType1") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbNecessary" runat="server" Checked='<%#Eval("Necessary") %>' Enabled="false"/>
                            </td>
                            <td><a>編輯</a></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>

            <p></p>
            <asp:Button ID="Button1" runat="server" Text="取消" />
            &emsp;&emsp;&emsp;&emsp;&emsp;
            <asp:Button ID="btnCreateQ" runat="server" Text="送出" OnClick="btnCreateQ_Click"/>
        </div>
    </div>

    <script>
        $("#tblUserInfo").tablepage($("#table_pageA2"), 10);
    </script>
</asp:Content>

