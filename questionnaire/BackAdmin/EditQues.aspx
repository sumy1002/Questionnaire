<%@ Page Title="" Language="C#" MasterPageFile="~/BackAdmin/Admin.Master" AutoEventWireup="true" CodeBehind="EditQues.aspx.cs" Inherits="questionnaire.BackAdmin.mainPageA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <!--套用jQuery-->
    <%--<script src="https://cdn.staticfile.org/jquery/2.1.1/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.11.5/css/jquery.dataTables.css">
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.11.5/js/jquery.dataTables.js"></script>

    <style>
        li {
            width: auto;
            font-weight: 600;
            font-size: 18px;
        }

        #InfoDiv {
            border: 0px solid #ff0000;
            margin: 10px auto;
            text-align: center;
            padding: 10px;
            padding-top: 20px;
        }

        #divDynamic {
            border: 0px solid #000000;
            margin: 10px auto;
            text-align: center;
        }

        #InfoDate {
            text-align: right;
            margin-right: 20px;
        }

        #userInfo{
            width:70%;
        }

        .tdCenter{
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
    <div class="tab-content">
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#paper">問卷</a></li>
            <li id="li1" runat="server"><a id="tab2_tab" data-toggle="tab" href="#question">問題</a></li>
            <li><a data-toggle="tab" href="#userInfo">填寫資料</a></li>
            <li><a data-toggle="tab" href="#statistic">統計</a></li>
        </ul>
    </div>

    <div class="tab-content">
        <%-- 問卷資訊 --%>
        <asp:PlaceHolder ID="plcQuestionnaire" runat="server">
            <div id="paper" class="tab-pane fade in active">
                <p>
                    <asp:HiddenField ID="hfQid" runat="server" />
                    <asp:HiddenField ID="hfTitleID" runat="server" /><br />
                    <asp:Literal ID="ltltitle" runat="server">問卷名稱 </asp:Literal>
                    <asp:TextBox ID="txtTitle" runat="server" Width="250" Enabled="false"></asp:TextBox><br /><br />
                    <asp:Literal ID="ltlcontent" runat="server">描述內容 </asp:Literal>
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Enabled="false" Width="550" Height="150px"></asp:TextBox><br /><br />
                    <asp:Literal ID="ltlStart" runat="server">開始時間 </asp:Literal>
                    <asp:TextBox ID="txtStart" runat="server" TextMode="Date" Width="250" Enabled="false"></asp:TextBox><br><br />
                    <asp:Literal ID="ltlEnd" runat="server">結束時間 </asp:Literal>
                    <asp:TextBox ID="txtEnd" runat="server" TextMode="Date" Width="250" Enabled="false"></asp:TextBox><br /><br />
                    <asp:Literal ID="ltlEnable" runat="server">開放狀態 </asp:Literal>
                    <asp:CheckBox ID="ckbEnable" runat="server" Text="" Checked='<%# Eval("IsEnable") %>' Visible="false" />
                    <asp:RadioButton ID="rdbEnableT" runat="server" GroupName="Enable" Text="開放中" Enabled="false" />
                    <asp:RadioButton ID="rdbEnableF" runat="server" GroupName="Enable" Text="已關閉" Enabled="false" />
                    <br /><br />

                    <asp:Button ID="btnQuesEdit" runat="server" Text="編輯問卷資訊" OnClick="btnQuesEdit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" Visible="false" />&emsp;
                    <asp:Button ID="btnSend" runat="server" Text="送出" OnClick="btnSend_Click" Visible="false" />
                </p>
            </div>
        </asp:PlaceHolder>

        <%-- 問題 --%>
        <div id="question" class="tab-pane fade">
            <%-- 問題輸入 --%>
            <asp:PlaceHolder ID="plcQues" runat="server" Visible="false">
                <p>
                    <asp:Literal ID="ltlType" runat="server">種類</asp:Literal>
                    <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnAddCQ" runat="server" Text="Button" />
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
                    <asp:Button ID="btnQuesAdd" runat="server" Text="加入" OnClick="btnQuesAdd_Click" />
                    <asp:Button ID="btnQuesAddCancel" runat="server" Text="取消" OnClick="btnQuesAddCancel_Click" /><br />
                </p>
                <br />
            </asp:PlaceHolder>

            <br />
            <asp:ImageButton ID="imgbtnDel" runat="server" ImageUrl="~/images/del.png" OnClick="imgbtnDel_Click" OnClientClick="return confirm('確定要刪除嗎？')" />
            <asp:ImageButton ID="imgbtnPlus" runat="server" ImageUrl="~/images/plus.png" OnClick="imgbtnPlus_Click" />



            <%-- 修改問題 --%>
            <asp:PlaceHolder ID="plcUpdate" runat="server" Visible="false">
                <asp:Literal ID="ltlQuesEdit" runat="server">問題</asp:Literal>
                <asp:TextBox ID="txtQuesEdit" runat="server" Width="250px"></asp:TextBox>&nbsp;
                <asp:DropDownList ID="ddlQuesTypeEdit" runat="server"></asp:DropDownList>&nbsp;
                <asp:CheckBox ID="ckbNessEdit" runat="server" Text="必填" /><br />
                <asp:Label ID="lblQuesRedEdit" runat="server" Text="未輸入問題" Visible="false" ForeColor="Red"></asp:Label><br />
                <asp:Literal ID="ltlAnswerEdit" runat="server">回答</asp:Literal>
                <asp:TextBox ID="txtAnswerEdit" runat="server"></asp:TextBox>&nbsp;
                <span>(多個答案以；分隔)</span>&emsp;<br />
                <asp:Label ID="lblAnsRedEdit" runat="server" Text="選項格式錯誤" Visible="false" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblAnsRed2Edit" runat="server" Text="單選及多選選項必須以;分隔，且不可以;結尾" Visible="false" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblAnsRed3Edit" runat="server" Text="文字題無須輸入選項" Visible="false" ForeColor="Red"></asp:Label><br />
                <asp:Button ID="btnQuesAddEdit" runat="server" Text="確定修改" OnCommand="btnQuesAddEdit_Command" />
                <asp:Button ID="btnQuesAddEditCancel" runat="server" Text="取消" OnClick="btnQuesAddEditCancel_Click" /><br />
                <br />
            </asp:PlaceHolder>

            <%-- 問題列表 --%>
            <table border="1" id="tblQDetail">
                <tr>
                    <th width="20px"></th>
                    <th width="40px"  class="tdCenter">編號</th>
                    <th width="400px">問題</th>
                    <th width="100px" class="tdCenter">種類</th>
                    <th width="100px" class="tdCenter">必填</th>
                    <th></th>
                </tr>

                <asp:Repeater ID="rptQuesItem" runat="server">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfid" runat="server" Value='<%# Eval("QuesID") %>' />
                        <tr>
                            <td class="tdCenter">
                                <asp:CheckBox ID="ckbDel" runat="server" Checked="false" /></td>
                            <td  class="tdCenter">
                                <asp:Label ID="lblnumber" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblQues" runat="server" Text='<%#Eval("QuesTitle") %>'></asp:Label></td>
                            <td class="tdCenter">
                                <asp:Label ID="lblQType" runat="server" Text='<%#Eval("QuesType1") %>'></asp:Label></td>
                            <td class="tdCenter">
                                <asp:CheckBox ID="ckbNecessary" runat="server" Checked='<%#Eval("Necessary") %>' Enabled="false" /></td>
                            <td class="tdCenter">
                                <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="~/images/edit.png" CommandName='<%# Eval("QuesID") %>' OnCommand="imgbtnEdit_Command" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>

            <%-- 分頁 --%>
            <span id='table_QuesDetail'></span>
            <p></p>
        </div>

        <%-- 填寫資料 --%>
        <div id="userInfo" class="tab-pane fade">
            <asp:PlaceHolder runat="server" ID="plcInfo1">
                <asp:PlaceHolder ID="plcExport" runat="server" Visible="false" >
                    <br />
                    <asp:Button ID="btnExport" runat="server" Text="匯出成CSV檔" OnClick="btnExport_Click" OnClientClick="return confirm('確定要匯出所有資料嗎？')"/><br />
                    <br />
                </asp:PlaceHolder>
                <asp:HiddenField ID="hfUserID" runat="server" />
                <p></p>
                <br />
                <table border="1" id="QList" class="display">
                    <thead>
                        <tr>
                            <th>編號</th>
                            <th>姓名</th>
                            <th>填寫日期</th>
                            <th>觀看細節</th>
                        </tr>
                    </thead>

                    <tbody>
                        <asp:Repeater ID="rptDetail" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="8%" class="tdCenter"><asp:Label ID="lblNumber2" runat="server" Text=""></asp:Label></td>
                                    <td width="50%"><asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Label></td>
                                    <td width="40%"><asp:Label ID="lblCreateDate" runat="server" Text='<%#Eval("CreateDate") %>'></asp:Label></td>
                                    <td  width="70%"><asp:Button ID="btnDetail" runat="server" Text="觀看細節" OnCommand="btnDetail_Command" CommandName='<%#Eval("UserID") %>' />
                                        <asp:Button ID="btnBack" runat="server" Text="返回列表" Visible="false" OnClick="btnBack_Click"/>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>

                </table>
            </asp:PlaceHolder>

            <div id="InfoDiv">
                <asp:PlaceHolder runat="server" ID="plcInfo2" Visible="false">
                    <asp:Literal ID="ltlName" runat="server">姓名  </asp:Literal>
                    <asp:TextBox ID="txtName" runat="server" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Literal ID="ltlPhone" runat="server">手機  </asp:Literal>
                    <asp:TextBox ID="txtPhone" runat="server" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                        <br />
                    <br />
                    <asp:Literal ID="ltlEmail" runat="server">Email  </asp:Literal>
                    <asp:TextBox ID="txtEmail" runat="server" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                        <asp:Literal ID="ltlAge" runat="server">年齡  </asp:Literal>
                    <asp:TextBox ID="txtAge" runat="server" Enabled="false"></asp:TextBox>&nbsp;&nbsp;
                    <br />
                    <div id="InfoDate">
                        <asp:Literal ID="ltlDate" runat="server">填寫時間</asp:Literal>
                    </div>
                    <div id="divDynamic" align="center">
                        <asp:PlaceHolder ID="plcDynamic" runat="server"></asp:PlaceHolder>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>

        <%-- 統計 --%>
        <div id="statistic" class="tab-pane fade">
            <asp:PlaceHolder ID="plcStatistic" runat="server"></asp:PlaceHolder>
        </div>
    </div>

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
