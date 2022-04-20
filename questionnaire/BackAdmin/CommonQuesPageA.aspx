<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="CommonQuesPageA.aspx.cs" Inherits="questionnaire.BackAdmin.CommonQuesPageA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--套用jQuery-->
    <script src="../JavaScript/jquery-tablepage-1.0.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>常用問題管理</h3>

    <asp:ImageButton ID="imgbtnPlus" runat="server" ImageUrl="~/images/plus.png" Width="40px" OnClick="imgbtnPlus_Click" /><br />

    <%-- 新建問題 --%>
    <asp:PlaceHolder ID="plcAddCQ" runat="server" Visible="false">
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
        <asp:Button ID="btnQuesAdd" runat="server" Text="加入" OnClick="btnQuesAdd_Click"/>
        <asp:Button ID="btnQuesAddCancel" runat="server" Text="取消" OnClick="btnQuesAddCancel_Click"/><br />
    </asp:PlaceHolder>

    <%-- 修改問題 --%>
    <asp:PlaceHolder ID="plcEditCQ" runat="server" Visible="false">
        <asp:Literal ID="ltlQuesEdit" runat="server">問題</asp:Literal>
        <asp:TextBox ID="txtQuesEdit" runat="server" Width="250px"></asp:TextBox>&nbsp;
        <asp:DropDownList ID="ddlQuesTypeEdit" runat="server"></asp:DropDownList>&nbsp;
        <asp:CheckBox ID="ckbNessEdit" runat="server" Text="必填" /><br />
        <asp:Label ID="lblQuesRedEdit" runat="server" Text="未輸入問題" Visible="false" ForeColor="Red"></asp:Label><br />
        <asp:Literal ID="ltlAnswerEdit" runat="server">回答</asp:Literal>
        <asp:TextBox ID="txtAnswerEdit" runat="server"></asp:TextBox>&nbsp;
        <span>(多個答案以;分隔)</span>&emsp;<br />
        <asp:Label ID="lblAnsRedEdit" runat="server" Text="選項格式錯誤" Visible="false" ForeColor="Red"></asp:Label>
        <asp:Label ID="lblAnsRedEdit2" runat="server" Text="單選及多選選項必須以;分隔" Visible="false" ForeColor="Red"></asp:Label><br />
        <asp:Button ID="btnQuesEdit" runat="server" Text="確定修改" OnClick="btnQuesEdit_Click"/>
        <asp:Button ID="btnQuesAddCancelEdit" runat="server" Text="取消" OnClick="btnQuesAddCancelEdit_Click"/><br />
        <asp:HiddenField ID="hfCQID" runat="server" Value='<%# Eval("CQID") %>'/>
    </asp:PlaceHolder>

    <table border="1" id="tblA">
        <tr>
            <th></th>
            <th>編號</th>
            <th>問題</th>
            <th>選項</th>
            <th>必填</th>
            <th>編輯</th>
            <th>刪除</th>
        </tr>
        <asp:Repeater ID="rptCQ" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbCQ" runat="server" />
                    </td>
                    <td width="50px">
                        <asp:Label ID="lblNumber" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td width="250px">
                        <%# Eval("CQTitle") %>
                    </td>
                    <td width="250px">
                        <%# Eval("CQChoice") %>
                    </td>
                    <td width="50px">
                        <asp:CheckBox ID="ckbNess" runat="server" Enabled="false" Checked='<%#Eval("Necessary") %>' />
                    </td>
                    <td>
                        <asp:ImageButton ID="imgbtnedit" runat="server" ImageUrl="~/images/edit.png" Width="39px" CommandName='<%# Eval("CQID") %>' OnCommand="imgbtnedit_Command"/>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/images/del.png" Width="40px" CommandName='<%# Eval("CQID") %>' OnCommand="imgbtnDel_Command" OnClientClick="return confirm('確定要刪除嗎？')" />
                    </td>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <span id='table_pageA'></span>

    <script>
        $("#tblA").tablepage($("#table_pageA"), 10);
    </script>
</asp:Content>
