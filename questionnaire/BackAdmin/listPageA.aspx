<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="listPageA.aspx.cs" Inherits="questionnaire.BackAdmin.listPageA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--套用jQuery-->
    <script src="../JavaScript/jquery-tablepage-1.0.js"></script>
    <style>
        #topDiv {
            border: 2px solid #000000;
            padding-left: 30px;
            padding-top: 20px;
            margin-top: 10px;
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
            <asp:Button ID="btnSearch" runat="server" Text="搜尋" />
        </p>
    </div>
    <br />
    <br />
    <asp:ImageButton ID="ImgBtnDel" runat="server" ImageUrl="../images/deleteICON.png" Width="70" />
    <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="../images/addICON.png" Width="50" OnClick="ImgBtnAdd_Click" />

    <asp:Repeater ID="rptList" runat="server">
        <ItemTemplate>
                <tr>
                    <th></th>
                    <th>編號</th>
                    <th>問卷標題</th>
                    <th>狀態</th>
                    <th>開始時間</th>
                    <th>結束時間</th>
                    <th>觀看統計</th>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </td>
                    <td><%# Eval("TitleID") %></td>
                    <td><a><%# Eval("Title") %></a></td>
                    <td><%# Eval("IsEnable") %></td>
                    <td><%# Eval("StartDate") %></td>
                    <td><%# Eval("EndDate") %></td>
                    <td><a>前往</a></td>
                </tr>
        </ItemTemplate>
    </asp:Repeater>

    <span id='table_pageA'></span>

    <script>
        $("#tblA").tablepage($("#table_pageA"), 10);
    </script>
</asp:Content>
