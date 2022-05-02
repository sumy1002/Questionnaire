<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="ListPageAdmin.aspx.cs" Inherits="questionnaire.BackAdmin.listPageA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--套用jQuery-->
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.css">
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.js"></script>

    <style>
        #topDiv {
            border: 2px solid #000000;
            padding-left: 30px;
            padding-top: 20px;
            margin-top: 10px;
        }

        div {
            border: 0px solid #000000;
        }

        .divCenter {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="topDiv">
        <p>
            <asp:Literal ID="ltlTitle" runat="server">問卷標題</asp:Literal>
            <asp:TextBox ID="txtTitle" runat="server" OnTextChanged="txtTitle_TextChanged" AutoPostBack="true"></asp:TextBox>
        </p>
        <p>
            <asp:Literal ID="ltlDate" runat="server">開始／結束</asp:Literal>
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="true"></asp:TextBox>&nbsp;
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" OnTextChanged="txtEndDate_TextChanged" AutoPostBack="true"></asp:TextBox>&emsp;&emsp;
            <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click" />
        </p>
    </div>
    <asp:Literal ID="ltlMsg" runat="server" Visible="false"></asp:Literal><br /><br />

    <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="../images/plus.png" Width="40" OnClick="ImgBtnAdd_Click" />
    <asp:ImageButton ID="ImgBtnClose" runat="server" ImageUrl="../images/del.png" Width="40px" OnClick="ImgBtnClose_Click" OnClientClick="return confirm('確定要刪除所選取的所有問卷嗎？')" />

    <table border="1" id="QList" class="display">
        <thead>
            <tr>
                <th></th>
                <th>編號</th>
                <th>問卷標題</th>
                <th>狀態</th>
                <th>開始時間</th>
                <th>結束時間</th>
                <th  class="divCenter">觀看統計<br />與編輯</th>
                <th>關閉/開啟</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <asp:HiddenField ID="hfID" runat="server" Value='<%#Eval("QuestionnaireID") %>' />
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("IsEnable") %>' Visible="false" />
                        <tr id="trQList" runat="server">
                            <td><asp:CheckBox ID="ckbDel" runat="server"  /></td>
                            <td width="50px">
                                <div class="divCenter">
                                    <asp:Label ID="lblTitleID" runat="server" Text='<%# Eval("TitleID") %>'></asp:Label>
                                </div>
                            </td>
                            <td width="350px">
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label></td>
                            <td width="70px">
                                <asp:Label ID="lblIsEnable" runat="server" Text='<%# Eval("strIsEnable") %>'></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblSDT" runat="server" Text='<%# Eval("strStartTime") %>'></asp:Label></td>
                            <td width="100px">
                                <asp:Label ID="lblEDT" runat="server" Text='<%# Eval("strEndTime") %>'></asp:Label></td>
                            <td width="85px" >
                                <div class="divCenter">
                                    <a href="EditQues.aspx?ID=<%#Eval("QuestionnaireID") %>">前往</a>
                                </div>
                            </td>
                            <td width="90px">
                                <div class="divCenter">
                                    <asp:ImageButton ID="ImgBtnClose" runat="server" ImageUrl="../images/del.png" Width="40px" CommandName='<%# Eval("QuestionnaireID") %>' OnCommand="ImgBtnDel_Command" OnClientClick="return confirm('確定要關閉問卷嗎？')" />
                                </div>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

    <script>
        $(document).ready(function () {
            $('#QList').DataTable({
                "searching": false,
                language: {
                    //url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/zh-HANT.json",
                    "lengthMenu": "顯示 _MENU_ 項結果<br />*點擊標題進入修改，點擊上方加號新增問卷",
                    "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                    "paginate": {
                        "first": "第一頁",
                        "last": "尾頁",
                        "next": "下一頁",
                        "previous": "前一頁"
                    },
                },
                "bLengthChange": false,
                "aaSorting": [[1, "desc"]],
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [0,2,3,4,5,6,7] }]
            });

        });
    </script>
</asp:Content>
