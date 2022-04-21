<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="NewQues.aspx.cs" Inherits="questionnaire.BackAdmin.NewQues" %>

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
            <div id="paper" class="tab-pane fade in active">
                <p>
                    <asp:Literal ID="ltltitle" runat="server">問卷名稱</asp:Literal>
                    <asp:TextBox ID="txtTitle" runat="server" Width="250"></asp:TextBox><br />
                    <asp:Literal ID="ltlcontent" runat="server">描述內容</asp:Literal>
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                    <asp:Literal ID="ltlStart" runat="server">開始時間</asp:Literal>
                    <asp:TextBox ID="txtStart" runat="server" TextMode="Date" Width="250"></asp:TextBox><br />
                    <asp:Literal ID="ltlEnd" runat="server">結束時間</asp:Literal>
                    <asp:TextBox ID="txtEnd" runat="server" TextMode="Date" Width="250"></asp:TextBox><br />
                    <br />
                    <asp:CheckBox ID="ckbEnable" runat="server" Text="已啟用" Checked="true" />
                    <br />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" />
                    &emsp;&emsp;&emsp;&emsp;&emsp;
                <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" />
                </p>
            </div>

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
                <asp:CheckBox ID="ckbNess" runat="server" Text="必填" /><br />
                <asp:Label ID="lblQuesRed" runat="server" Text="未輸入問題" Visible="false" ForeColor="Red"></asp:Label><br />
                <asp:Literal ID="ltlAnswer" runat="server">回答</asp:Literal>
                <asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>&nbsp;
                <span>(多個答案以；分隔)</span>&emsp;<br />
                <asp:Label ID="lblAnsRed" runat="server" Text="選項格式錯誤" Visible="false" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblAnsRed2" runat="server" Text="單選及多選選項必須以;分隔，且不可以;結尾" Visible="false" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblAnsRed3" runat="server" Text="文字題無須輸入選項" Visible="false" ForeColor="Red"></asp:Label><br />
                <asp:Button ID="btnQuesAdd" runat="server" Text="加入" OnClick="btnQuesAdd_Click" /><br />
            </p>

            <br />
            <asp:ImageButton ID="ImageButton1" runat="server" />

            <asp:HiddenField ID="hfQuesList" runat="server" />
            <table border="1" id="tblUserInfo">
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
                                <asp:Label ID="lblnumber" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQues" runat="server" Text='<%#Eval("QuesTitle") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQType" runat="server" Text='<%#Eval("QuesType1") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckbNecessary" runat="server" Checked='<%#Eval("Necessary") %>' Enabled="false" />
                            </td>
                            <td><a>編輯</a></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <span id='table_pageA'></span>

            <p></p>
            <asp:Button ID="Button1" runat="server" Text="取消" />
            &emsp;&emsp;&emsp;&emsp;&emsp;
            <asp:Button ID="btnCreateQ" runat="server" Text="送出" OnClick="btnCreateQ_Click" />
        </div>
    </div>

    <script>
        $("#tblUserInfo").tablepage($("#table_pageA"), 10);
    </script>
</asp:Content>
