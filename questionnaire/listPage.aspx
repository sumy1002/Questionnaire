<%@ Page Title="" Language="C#" MasterPageFile="~/Index.Master" AutoEventWireup="true" CodeBehind="listPage.aspx.cs" Inherits="questionnaire.listPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--套用jQuery-->
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.css">
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.js"></script>

    <style>
        #topDiv {
            border: 2px solid #000000;
            margin: auto;
            padding-left: 30px;
            padding-top: 20px;
        }

        #tbDiv {
            margin: 20px auto;
        }

        .centerDiv {
            text-align: center;
        }

        #searchDiv {
            //margin-top: -20px;
            margin: 2px auto;
        }

        #bigDiv {
            margin: 20px auto;
        }

        #login{
            float:right;
            margin-top: -130px;
            margin-right: -50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="bigDiv" class="col-lg-10">
        <div>
            <%-- 搜尋框 --%>
            <div id="topDiv" class="col-lg-6">
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
            <%-- 登入 --%>
            <div id="login"><asp:Button ID="btnLogin" runat="server" Text="後台登入" OnClick="btnLogin_Click"/>
</div>
        </div>
        <div id="searchDiv" class="col-lg-6">
            &ensp;<asp:Literal ID="ltlMsg" runat="server" Visible="false"></asp:Literal>
        </div>

        <%-- 問卷清單 --%>
        <div id="tbDiv" class="col-lg-10">
            <table id="QList" class="display">
                <thead>
                    <tr>
                        <th>編號</th>
                        <th>問卷標題</th>
                        <th>狀態</th>
                        <th>開始時間</th>
                        <th>結束時間</th>
                        <th>觀看統計</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptQues" runat="server">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("TitleID") %>' />
                            <asp:HiddenField ID="hfSta" runat="server" Value='<%# Eval("StartDate") %>' />
                            <asp:HiddenField ID="hfEnd" runat="server" Value='<%# Eval("EndDate") %>' />
                            <tr>
                                <td width="45px">
                                    <div class="centerDiv">
                                        <%# Eval("TitleID") %>
                                    </div>
                                </td>
                                <td width="350px"><a href="Form.aspx?ID=<%#Eval("QuestionnaireID") %>"><%# Eval("Title") %></a></td>
                                <td width="100px">
                                    <asp:Literal ID="ltlState" runat="server" Text='<%# Eval("strIsEnable") %>'></asp:Literal></td>
                                <td width="100px">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("strStartTime") %>'></asp:Literal></td>
                                <td width="100px">
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("strEndTime") %>'></asp:Literal></td>
                                <td width="75px">
                                    <div class="centerDiv">
                                        <a href="statisticPage.aspx?ID=<%#Eval("QuestionnaireID") %>">前往</a>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    <span id='table_page'></span>

    <script>
        $(document).ready(function () {
            $('#QList').DataTable({
                "searching": false,
                language: {
                    //url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/zh-HANT.json",
                    "lengthMenu": "顯示 _MENU_ 項結果",
                    "info": "顯示第 _START_ 至 _END_ 項結果，共 _TOTAL_ 項",
                    "paginate": {
                        "first": "第一頁",
                        "last": "尾頁",
                        "next": "下一頁",
                        "previous": "前一頁"
                    },
                },
                "lengthMenu": [[10, 15, 20, "All"], [10, 15, 20, "All"]],
                "order": [[0, "desc"]],
            });

        });
    </script>
</asp:Content>
