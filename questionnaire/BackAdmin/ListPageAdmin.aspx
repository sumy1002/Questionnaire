﻿<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="ListPageAdmin.aspx.cs" Inherits="questionnaire.BackAdmin.listPageA" %>

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
            <asp:TextBox ID="txtTitle" runat="server" OnTextChanged="txtTitle_TextChanged" autopostback=true></asp:TextBox>
        </p>
        <p>
            <asp:Literal ID="ltlDate" runat="server">開始／結束</asp:Literal>
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date" OnTextChanged="txtStartDate_TextChanged" autopostback=true></asp:TextBox>&nbsp;
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date" OnTextChanged="txtEndDate_TextChanged" autopostback=true></asp:TextBox>
            &emsp;&emsp;
            <asp:Button ID="btnSearch" runat="server" Text="搜尋" OnClick="btnSearch_Click"/>
        </p>
    </div>
    <br />
    <br />
    
    <asp:ImageButton ID="ImgBtnAdd" runat="server" ImageUrl="../images/plus.png" Width="40" OnClick="ImgBtnAdd_Click" />

    <table border="1" id="tblA">
        <tr>
            <th>編號</th>
            <th>問卷標題</th>
            <th>狀態</th>
            <th>開始時間</th>
            <th>結束時間</th>
            <th>觀看統計</th>
            <th>刪除</th>
        </tr>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="50px"><%# Eval("TitleID") %></td>
                    <td width="250px"><a herf="#"><%# Eval("Title") %></a></td>
                    <td width="80px"><%# Eval("strIsEnable") %></td>
                    <td  width="120px"><%# Eval("strStartTime") %></td>
                    <td width="120px"><%# Eval("strEndTime") %></td>
                    <td width="80px"><a href="EditQues.aspx?ID=<%#Eval("QuestionnaireID") %>">前往</a></td>
                    <td><asp:ImageButton ID="ImgBtnDel" runat="server" ImageUrl="../images/del.png" Width="40" /></td>
                </tr>

            </ItemTemplate>
        </asp:Repeater>
    </table>
    <span id='table_pageA'></span>

    <script>
        $("#tblA").tablepage($("#table_pageA"), 10);
    </script>
</asp:Content>