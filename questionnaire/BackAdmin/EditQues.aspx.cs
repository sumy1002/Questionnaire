using questionnaire.Manager;
using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class mainPageA : System.Web.UI.Page
    {
        #region Manager跟一些變數
        private static List<QuesDetailModel> _questionSession = new List<QuesDetailModel>();
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private UserInfoManager _mgrUserInfo = new UserInfoManager();
        private UserQuesDetailManager _mgrUserDetail = new UserQuesDetailManager();
        private StatisticManager _mgrSta = new StatisticManager();
        private CQManager _mgrCQ = new CQManager();

        int i = 1;
        string rdbAns;
        string[] ckbAns;
        string txt;
        bool checkInfo = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 資料繫結
                //取ID
                string ID = Request.QueryString["ID"];
                this.Label1.Text = ID;
                Guid id = new Guid(ID);

                //尋找該ID的問卷及問題列表
                var Ques = this._mgrContent.GetQuesContent(id);

                //取資料放進輸入框
                this.hfTitleID.Value = Ques.QuestionnaireID.ToString();
                this.txtTitle.Text = Ques.Title.ToString();
                this.txtContent.Text = Ques.Body.ToString();
                this.txtStart.Text = Ques.StartDate.ToString("yyyy-MM-dd");
                this.txtEnd.Text = Ques.EndDate.ToString("yyyy-MM-dd");
                this.ckbEnable.Checked = Ques.IsEnable;

                //判斷一下開放狀態
                if (this.ckbEnable.Checked)
                    this.rdbEnableT.Checked = true;
                else
                    this.rdbEnableF.Checked = true;

                //問題類型下拉繫結
                var QTypeList = this._mgrQuesType.GetQuesTypesList();
                this.ddlQuesType.DataSource = QTypeList;
                this.ddlQuesType.DataValueField = "QuesTypeID";
                this.ddlQuesType.DataTextField = "QuesType1";
                this.ddlQuesType.DataBind();

                //自訂、常用問題下拉繫結
                var TypeList = this._mgrCQ.GetCQsList();
                this.ddlType.DataSource = TypeList;
                this.ddlType.DataValueField = "CQID";
                this.ddlType.DataTextField = "CQTitle";
                this.ddlType.DataBind();

                this.ddlType.Items.Insert(0, new ListItem("自訂問題", "0"));

                //問題列表繫結
                var QList = this._mgrQuesDetail.GetQuesDetailAndTypeList(id);
                this.rptQuesItem.DataSource = QList;
                this.rptQuesItem.DataBind();

                //填寫狀況繫結
                var UserList = this._mgrUserInfo.GetUserInfoList(id);
                this.rptDetail.DataSource = UserList;
                this.rptDetail.DataBind();

                foreach (RepeaterItem item in this.rptDetail.Items)
                {
                    Label ltlDate = item.FindControl("lblCreateDate") as Label;
                    var DT = Convert.ToDateTime(ltlDate.Text);
                    ltlDate.Text = DT.ToString("yyyy-MM-dd");
                }

                #endregion

                //生成問題的編號
                if (QList != null || QList.Count > 0)
                {
                    int i = 1;
                    foreach (RepeaterItem item in this.rptQuesItem.Items)
                    {
                        Label lblNumber = item.FindControl("lblNumber") as Label;
                        lblNumber.Text = i.ToString();
                        i++;
                    }
                }

                //生成問題的編號
                if (UserList != null || UserList.Count > 0)
                {
                    int i = 1;
                    foreach (RepeaterItem item in this.rptDetail.Items)
                    {
                        Label lblNumber = item.FindControl("lblNumber2") as Label;
                        lblNumber.Text = i.ToString();
                        i++;
                    }
                }

                //統計頁
                Statistic();
            }
        }

        #region 編輯問題
        //編輯問題
        protected void imgbtnEdit_Command(object sender, CommandEventArgs e)
        {
            this.plcQues.Visible = false;
            this.plcUpdate.Visible = true;
            this.imgbtnDel.Visible = false;
            this.imgbtnPlus.Visible = false;

            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問題列表
            var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);

            //問題類型下拉繫結
            var QTypeList = this._mgrQuesType.GetQuesTypesList();
            this.ddlQuesTypeEdit.DataSource = QTypeList;
            this.ddlQuesTypeEdit.DataValueField = "QuesTypeID";
            this.ddlQuesTypeEdit.DataTextField = "QuesType1";
            this.ddlQuesTypeEdit.DataBind();

            //取得該問題的資料
            int Qid = Convert.ToInt32(e.CommandName);
            var item = this._mgrQuesDetail.GetOneQuesDetail(Qid);

            //判斷該問題有無答案
            bool hasChoise;
            if (item.QuesTypeID == 2 || item.QuesTypeID == 3)
                hasChoise = true;
            else
                hasChoise = false;

            if (!hasChoise)
            {
                this.btnQuesAddEdit.CommandName = item.QuesID.ToString();
                this.txtQuesEdit.Text = item.QuesTitle.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
            }
            else
            {
                this.btnQuesAddEdit.CommandName = item.QuesID.ToString();
                this.txtQuesEdit.Text = item.QuesTitle.ToString();
                this.txtAnswerEdit.Text = item.QuesChoice.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
            }
        }

        //儲存編輯問題
        protected void btnQuesAddEdit_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandName);
            var item = this._mgrQuesDetail.GetOneQuesDetail(id);

            try
            {
                QuesDetailModel updateQ = new QuesDetailModel()
                {
                    QuestionnaireID = item.QuestionnaireID,
                    QuesID = item.QuesID,
                    QuesTitle = this.txtQuesEdit.Text,
                    QuesTypeID = Convert.ToInt32(this.ddlQuesTypeEdit.SelectedValue),
                    QuesChoice = this.txtAnswerEdit.Text,
                    Necessary = this.ckbNessEdit.Checked,
                };

                this._mgrQuesDetail.UpdateQuesDetail(updateQ);

                //取ID
                string ID = Request.QueryString["ID"];
                Guid id2 = new Guid(ID);

                var QList = this._mgrQuesDetail.GetQuesDetailAndTypeList(id2);
                this.rptQuesItem.DataSource = QList;
                this.rptQuesItem.DataBind();

                this.plcUpdate.Visible = false;
                this.imgbtnDel.Visible = true;
                this.imgbtnPlus.Visible = true;

                this.txtQuesEdit.Text = String.Empty;
                this.txtAnswerEdit.Text = String.Empty;
                this.ckbNessEdit.Checked = false;
            }
            catch (Exception)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('入力した内容に誤りがあります。');location.href='adminMenu.aspx';", true);
            }
        }

        //取消編輯問題
        protected void btnQuesAddEditCancel_Click(object sender, EventArgs e)
        {
            this.txtQuesEdit.Text = String.Empty;
            this.txtAnswerEdit.Text = String.Empty;
            this.ckbNessEdit.Checked = false;
            this.plcUpdate.Visible = false;
            this.plcQues.Visible = false;
            this.imgbtnDel.Visible = true;
            this.imgbtnPlus.Visible = true;
        }
        #endregion

        #region 建立問題
        //建立問題
        protected void imgbtnPlus_Click(object sender, ImageClickEventArgs e)
        {
            this.plcQues.Visible = true;
            this.plcUpdate.Visible = false;
            this.imgbtnDel.Visible = false;
            this.imgbtnPlus.Visible = false;
        }

        //新增問題加入列表
        protected void btnQuesAdd_Click(object sender, EventArgs e)
        {
            #region 防呆
            bool TitleCheck = !String.IsNullOrWhiteSpace(this.txtQues.Text);
            bool RadioHasChoice = false;
            bool CkbHasChoice = false;

            //檢查有無輸入問題標題
            if (TitleCheck == false)
                this.lblQuesRed.Visible = true;
            else
                this.lblQuesRed.Visible = false;

            //檢查單複選題有無輸入選項
            //文字
            if (this.ddlQuesType.SelectedValue == "1")
            {
                var ansCheck1 = String.IsNullOrWhiteSpace(this.txtAnswer.Text);
                if (ansCheck1)
                {
                    RadioHasChoice = true;
                    CkbHasChoice = true;
                    this.lblAnsRed3.Visible = false;
                }
                else
                    this.lblAnsRed3.Visible = true;
            }
            //單選
            else if (this.ddlQuesType.SelectedValue == "2")
            {
                this.lblAnsRed3.Visible = false;
                //檢查是否有值
                if (!String.IsNullOrWhiteSpace(this.txtAnswer.Text))
                {
                    this.lblAnsRed.Visible = false;
                    //檢查有無分號
                    var ansCheck1 = Regex.IsMatch(this.txtAnswer.Text.Trim(), @";");
                    var ansCheck2 = !(Regex.IsMatch(this.txtAnswer.Text.Trim(), @";$"));
                    if (ansCheck1 && ansCheck2)
                    {
                        RadioHasChoice = true;
                        this.lblAnsRed.Visible = false;
                        this.lblAnsRed2.Visible = false;
                        this.lblAnsRed3.Visible = false;
                    }
                    else
                        this.lblAnsRed2.Visible = true;
                }
                else
                {
                    RadioHasChoice = false;
                    this.lblAnsRed.Visible = true;
                }
            }
            //多選
            else if (this.ddlQuesType.SelectedValue == "3")
            {
                this.lblAnsRed3.Visible = false;
                //檢查是否有值
                if (!String.IsNullOrWhiteSpace(this.txtAnswer.Text))
                {
                    this.lblAnsRed.Visible = false;

                    //檢查有無分號 且分號位子正確與否
                    var ansCheck1 = Regex.IsMatch(this.txtAnswer.Text.Trim(), @";");
                    var ansCheck2 = !(Regex.IsMatch(this.txtAnswer.Text.Trim(), @";$"));
                    if (ansCheck1 && ansCheck2)
                    {
                        CkbHasChoice = true;
                        this.lblAnsRed.Visible = false;
                        this.lblAnsRed2.Visible = false;
                        this.lblAnsRed3.Visible = false;
                    }
                    else
                        this.lblAnsRed2.Visible = true;
                }
                else
                {
                    CkbHasChoice = false;
                    this.lblAnsRed.Visible = true;
                }
            }
            #endregion

            //問題加入列表
            Guid titleID = new Guid(this.hfTitleID.Value);
            var QuesContent = this._mgrContent.GetQuesContent(titleID);

            //檢查輸入是否都正確
            if (TitleCheck == true && (RadioHasChoice == true || CkbHasChoice == true))
            {
                QuesDetailModel ques = new QuesDetailModel();
                ques.QuestionnaireID = titleID;
                ques.QuesTitle = this.txtQues.Text.Trim();
                ques.QuesChoice = this.txtAnswer.Text.Trim();
                ques.QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue);
                ques.Necessary = this.ckbNess.Checked;

                _questionSession.Add(ques);
                HttpContext.Current.Session["qusetionModel"] = _questionSession;

                //把內容以字串形式寫進Session
                Session["questionList"] += this.txtQues.Text.Trim() + "&";
                Session["questionList"] += this.txtAnswer.Text.Trim() + "&";
                Session["questionList"] += Convert.ToInt32(this.ddlQuesType.SelectedValue) + "&";
                Session["questionList"] += (this.ddlQuesType.SelectedItem.ToString()).Trim() + "&";
                Session["questionList"] += this.ckbNess.Checked + "$";

                //做拆字串的處理
                var queslist = this._mgrQuesDetail.GetQuesList(Session["questionList"].ToString());

                //this.rptQuesItem.DataSource = queslist;
                //this.rptQuesItem.DataBind();

                //新增問題 寫入DB
                int questionNo = 1;
                foreach (QuesDetailModel question in _questionSession)
                {
                    question.QuesID = questionNo;
                    question.TitleID = QuesContent.TitleID;
                    question.QuestionnaireID = QuesContent.QuestionnaireID;

                    //過濾掉已經存在的問題
                    var originalQ = this._mgrQuesDetail.GetTitleQuesDetail(question.QuesTitle);
                    if (originalQ == null)
                    {
                        _mgrQuesDetail.CreateQuesDetail(question);
                    }
                    questionNo++;
                }

                //生成問題的編號
                if (queslist != null || queslist.Count > 0)
                {
                    int i = 1;
                    foreach (RepeaterItem item in this.rptQuesItem.Items)
                    {
                        Label lblNumber = item.FindControl("lblNumber") as Label;
                        lblNumber.Text = i.ToString();
                        i++;
                    }
                }

                //取ID
                string ID = Request.QueryString["ID"];
                Guid id = new Guid(ID);

                var QList = this._mgrQuesDetail.GetQuesDetailAndTypeList(id);
                this.rptQuesItem.DataSource = QList;
                this.rptQuesItem.DataBind();

                this.plcQues.Visible = false;
                this.plcUpdate.Visible = false;
                this.imgbtnDel.Visible = true;
                this.imgbtnPlus.Visible = true;
            }
            this.txtQues.Text = String.Empty;
            this.txtAnswer.Text = String.Empty;
            this.ddlQuesType.SelectedIndex = 0;
            this.ckbNess.Checked = false;
        }

        //取消建立問題
        protected void btnQuesAddCancel_Click(object sender, EventArgs e)
        {
            this.plcQues.Visible = false;
            this.plcUpdate.Visible = false;
            this.imgbtnDel.Visible = true;
            this.imgbtnPlus.Visible = true;
        }
        #endregion

        #region 刪除問題
        //刪除按鈕
        protected void imgbtnDel_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in this.rptQuesItem.Items)
            {
                HiddenField hfid = item.FindControl("hfid") as HiddenField;
                CheckBox ckbDel = item.FindControl("ckbDel") as CheckBox;
                ImageButton imgbtnDel = item.FindControl("imgbtnEdit") as ImageButton;
                if (ckbDel.Checked && Int32.TryParse(hfid.Value, out int QuesID))
                {
                    //把問題從資料庫中刪除
                    this._mgrQuesDetail.DeleteQuesDetail(Convert.ToInt32(imgbtnDel.CommandName));
                }
            }

            //取問卷ID
            string ID = Request.QueryString["ID"];
            this.Label1.Text = ID;
            Guid id = new Guid(ID);

            //導回正確的問卷編輯頁
            Response.Redirect($"EditQues.aspx?ID={ID}");
        }
        #endregion

        #region 修改問卷資訊
        //修改問卷資訊
        protected void btnQuesEdit_Click(object sender, EventArgs e)
        {
            this.btnQuesEdit.Visible = false;
            this.btnCancel.Visible = true;
            this.btnSend.Visible = true;
            this.txtTitle.Enabled = true;
            this.txtContent.Enabled = true;
            this.txtStart.Enabled = true;
            this.txtEnd.Enabled = true;
            this.rdbEnableT.Enabled = true;
            this.rdbEnableF.Enabled = true;
        }

        //取消修改問卷資訊
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnQuesEdit.Visible = true;
            this.btnCancel.Visible = false;
            this.btnSend.Visible = false;
            this.txtTitle.Enabled = false;
            this.txtContent.Enabled = false;
            this.txtStart.Enabled = false;
            this.txtEnd.Enabled = false;
            this.rdbEnableT.Enabled = false;
            this.rdbEnableF.Enabled = false;
        }

        //儲存修改問卷資訊
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Guid titleID = new Guid(this.hfTitleID.Value);
            var QuesContent = this._mgrContent.GetQuesContent(titleID);
            bool IsEnable = this.rdbEnableF.Checked;

            QuesContentsModel Ques = new QuesContentsModel()
            {
                QuestionnaireID = QuesContent.QuestionnaireID,
                TitleID = QuesContent.TitleID,
                Title = this.txtTitle.Text,
                Body = this.txtContent.Text,
                StartDate = Convert.ToDateTime(this.txtStart.Text),
                EndDate = Convert.ToDateTime(this.txtEnd.Text),
                IsEnable = !IsEnable
            };

            //寫進資料庫
            this._mgrContent.UpdateQues(Ques);

            //取問卷ID
            string ID = Request.QueryString["ID"];

            //導回正確的問卷編輯頁
            Response.Redirect($"EditQues.aspx?ID={ID}");
        }

        #endregion

        #region 顯示填寫資訊
        protected void btnDetail_Command(object sender, CommandEventArgs e)
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            foreach (RepeaterItem item in this.rptDetail.Items)
            {
                Button btnDe = item.FindControl("btnDetail") as Button;
                if(btnDe.Text == "返回列表")
                {
                    checkInfo = true;
                }
            }

            //觀看細節
            if (checkInfo == false)
            {
                this.plcExport.Visible = true;

                this.plcInfo2.Visible = true;
                Guid userID = new Guid(e.CommandName);
                this.hfUserID.Value = e.CommandName.ToString();

                //取得問題清單
                List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);

                //建立問題
                foreach (QuesDetail question in questionList)
                {
                    string title = $"<br /><br />{i}. {question.QuesTitle}";
                    if (question.Necessary)
                        title += "(*)";
                    i = i + 1;
                    Literal ltlQuestion = new Literal();
                    ltlQuestion.Text = title + "<br/>";

                    this.plcDynamic.Controls.Add(ltlQuestion);

                    switch (question.QuesTypeID)
                    {
                        case 1:
                            CreateTxt(question);
                            break;
                        case 2:
                            CreateRdb(question);
                            break;
                        case 3:
                            CreateCkb(question);
                            break;
                    }
                }

                var UList = this._mgrUserInfo.GetUserInfoList2(userID);
                this.rptDetail.DataSource = UList;
                this.rptDetail.DataBind();

                foreach (RepeaterItem item in this.rptDetail.Items)
                {
                    Button btnDe = item.FindControl("btnDetail") as Button;
                    btnDe.Text = "返回列表";
                }

                for (int i = 0; i < UList.Count; i++)
                {
                    this.txtName.Text = UList[i].Name;
                    this.txtPhone.Text = UList[i].Phone;
                    this.txtEmail.Text = UList[i].Email;
                    this.txtAge.Text = UList[i].Age;
                    this.ltlDate.Text = "填寫日期  " + UList[i].CreateDate.ToString("yyyy-MM-dd");
                }
                checkInfo = true;
            }
            //返回列表
            else
            {
                this.plcExport.Visible = false;
                this.plcInfo2.Visible = false;

                //填寫狀況繫結
                var UserList = this._mgrUserInfo.GetUserInfoList(id);
                this.rptDetail.DataSource = UserList;
                this.rptDetail.DataBind();

                foreach (RepeaterItem item in this.rptDetail.Items)
                {
                    Label ltlDate = item.FindControl("lblCreateDate") as Label;
                    var DT = Convert.ToDateTime(ltlDate.Text);
                    ltlDate.Text = DT.ToString("yyyy-MM-dd");
                }
            }

            Statistic();
        }

        //建立單選
        private void CreateRdb(QuesDetail question)
        {
            Guid userId = new Guid(this.hfUserID.Value);
            var d = this._mgrUserDetail.GetUserInfo(userId);
            foreach (var item in d)
            {
                if (item.QuesID == question.QuesID)
                {
                    var detail = item.Answer.TrimEnd(';');
                    rdbAns = detail;
                }
            }

            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(radioButtonList);

            string[] arrQ = question.QuesChoice.Split(';');

            for (int i = 0; i < arrQ.Length; i++)
            {
                if (rdbAns == arrQ[i])
                {
                    RadioButton item = new RadioButton();
                    item.Text = arrQ[i].ToString();
                    item.ID = question.QuesID + i.ToString();
                    item.GroupName = "group" + question.QuesID;
                    this.plcDynamic.Controls.Add(item);
                    item.Checked = true;
                    item.Enabled = false;
                    this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
                }
                else
                {
                    RadioButton item = new RadioButton();
                    item.Text = arrQ[i].ToString();
                    item.ID = question.QuesID + i.ToString();
                    item.GroupName = "group" + question.QuesID;
                    this.plcDynamic.Controls.Add(item);
                    item.Checked = false;
                    item.Enabled = false;
                    this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
                }
            }
        }

        //建立複選
        private void CreateCkb(QuesDetail question)
        {
            Guid userId = new Guid(this.hfUserID.Value);
            var d = this._mgrUserDetail.GetUserInfo(userId);
            foreach (var item in d)
            {
                if (item.QuesID == question.QuesID)
                {
                    var detail = item.Answer.TrimEnd(';');
                    ckbAns = detail.Split(';');
                }
            }

            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(checkBoxList);

            string[] arrQ = question.QuesChoice.Split(';');

            for (int i = 0; i < arrQ.Length; i++)
            {
                CheckBox item = new CheckBox();
                item.Text = arrQ[i].ToString();
                item.ID = question.QuesID + i.ToString();
                item.Enabled = false;
                this.plcDynamic.Controls.Add(item);
                this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
                foreach (string ans in ckbAns)
                {
                    if (ans == arrQ[i])
                    {
                        item.Checked = true;
                        break;
                    }
                    else
                        item.Checked = false;
                }
            }
        }

        //建立文字
        private void CreateTxt(QuesDetail question)
        {
            Guid userId = new Guid(this.hfUserID.Value);
            var d = this._mgrUserDetail.GetUserInfo(userId);
            foreach (var item in d)
            {
                if (item.QuesID == question.QuesID)
                {
                    var detail = item.Answer.TrimEnd(';');
                    txt = detail;
                }
            }

            TextBox textBox = new TextBox();
            textBox.ID = "Q" + question.QuesID.ToString();
            textBox.Text = txt;
            textBox.Enabled = false;
            this.plcDynamic.Controls.Add(textBox);
        }
        #endregion

        #region 匯出
        //匯出按鈕
        protected void btnExport_Click(object sender, EventArgs e)
        {
            //取問卷ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問卷及問題列表
            var Ques = this._mgrContent.GetQuesContent(id);

            string Path = "D:\\CSV\\";

            //確認路徑是否存在
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            if (!File.Exists($"{Ques.Title}.csv"))
            {
                File.Create(Ques.Title);
            }

            string fullPath = $"D:\\CSV\\{Ques.Title}.csv";

            DataTable dtTable = new DataTable();
            SaveCSV(dtTable, fullPath);
        }

        //將資料寫入CSV
        public void SaveCSV(DataTable dt, string fullPath)
        {
            DataTable dtTable = new DataTable();
            DataRow row;

            //建立欄位
            dtTable.Columns.Add("Name", typeof(string));
            dtTable.Columns.Add("Phone", typeof(string));
            dtTable.Columns.Add("Email", typeof(string));
            dtTable.Columns.Add("Age", typeof(int));
            dtTable.Columns.Add("CreateDate", typeof(DateTime));

            //取問卷ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問卷及問題列表
            var Ques = this._mgrContent.GetQuesContent(id);
            var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);
            var UserInfo = this._mgrUserInfo.GetUserInfoList(id);

            try
            {
                //新增資料到DataTable
                for (int i = 0; i < UserInfo.Count; i++)
                {
                    row = dtTable.NewRow();
                    row["Name"] = UserInfo[i].Name;
                    row["Phone"] = UserInfo[i].Phone;
                    row["Email"] = UserInfo[i].Email;
                    row["Age"] = UserInfo[i].Age;
                    row["CreateDate"] = UserInfo[i].CreateDate;

                    for (int j = 0; j < QuesDetail.Count; j++)
                    {
                        if (dtTable.Columns.Contains($"Question{j + 1}") == false)
                        {
                            dtTable.Columns.Add($"Question{j + 1}", typeof(string));
                            dtTable.Columns.Add($"Answer{j + 1}", typeof(string));
                        }

                        row[$"Question{j + 1}"] = QuesDetail[j].QuesTitle;
                        Guid usID = UserInfo[i].UserID;
                        var thisUSans = this._mgrUserDetail.GetUserInfo(usID);
                        foreach (var ans in thisUSans)
                        {
                            if (ans.QuesID == QuesDetail[j].QuesID)
                                row[$"Answer{j + 1}"] = ans.Answer;
                        }
                    }
                    dtTable.Rows.Add(row);
                }

                FileInfo fi = new FileInfo(fullPath);

                //檔案不存在就建立檔案
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }

                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                string data = "";

                for (int i = 0; i < dtTable.Columns.Count; i++)//寫入列名
                {
                    data += dtTable.Columns[i].ColumnName.ToString();
                    if (i < dtTable.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);

                //寫入各行資料
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dtTable.Columns.Count; j++)
                    {
                        string str = dtTable.Rows[i][j].ToString();
                        //替換英文冒號 英文冒號需要換成兩個冒號
                        str = str.Replace("\"", "\"\"");
                        //含逗號 冒號 換行符的需要放到引號中
                        if (str.Contains(',') || str.Contains('"')
                          || str.Contains('\r') || str.Contains('\n'))
                        {
                            str = string.Format("\"{0}\"", str);
                        }

                        data += str;
                        if (j < dtTable.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", $"alert('匯出完成。');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", $"alert('匯出失敗。');", true);
            }
        }
        #endregion

        #region 統計頁
        private void Statistic()
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            this._mgrSta.GetStasticList(id);

            //取得該問卷的問題細節
            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
            List<StatisticModel> staList = _mgrSta.GetStasticList(id);

            foreach (QuesDetail question in questionList)
            {
                string q = $"<br/>{question.QuesTitle}";
                Label lblNess = new Label();

                //必填的部分
                if (question.Necessary)
                {
                    lblNess.Text = "(*必填)<br />";
                    lblNess.ForeColor = System.Drawing.Color.Red;
                    lblNess.Style.Value = "font-size:10px;";
                }
                Label lblQues = new Label();
                lblQues.Text = q;
                lblQues.Style.Value = "font-weight:bold";
                this.plcStatistic.Controls.Add(lblQues);
                this.plcStatistic.Controls.Add(lblNess);


                //複選&單選
                if (question.QuesTypeID != 1)
                {
                    List<StatisticModel> NoList = staList.FindAll(x => x.QuesID == question.QuesID);
                    int total = 0;

                    foreach (StatisticModel item in NoList)
                    {
                        item.AnsCount = NoList.Count;
                        total = item.AnsCount;
                    }

                    if (total == 0)
                    {
                        Literal ltlNoAns = new Literal();
                        ltlNoAns.Text = "尚無資料<br/>";
                        this.plcStatistic.Controls.Add(ltlNoAns);
                    }
                    else
                    {
                        //取問題的答案
                        string[] arrQue = question.QuesChoice.Split(';');
                        int ckbAns = 0;
                        for (int i = 0; i < arrQue.Length; i++)
                        {
                            ckbAns = 0;
                            int ansCount = 0;
                            string ans = arrQue[i].ToString();
                            foreach (var s in NoList)
                            {
                                var ansTrim = s.Answer.TrimEnd(';');
                                string[] sAns = ansTrim.Split(';');
                                ckbAns += sAns.Length;
                                foreach (string sans in sAns)
                                {
                                    if (sans == ans)
                                    {
                                        ansCount++;
                                    }
                                }
                            }

                            //篩選一下問題種類
                            switch (question.QuesTypeID)
                            {
                                //單選
                                case 2:
                                    Literal ltlAns = new Literal();
                                    ltlAns.Text = $"{arrQue[i]} : {ansCount * 100 / total}% ({ansCount}) <br />";
                                    this.plcStatistic.Controls.Add(ltlAns);
                                    break;
                                //複選
                                case 3:
                                    double aaa = Convert.ToInt32(((double)ansCount / ckbAns) * 100);
                                    Literal ltlAns2 = new Literal();
                                    ltlAns2.Text = $"{arrQue[i]} : {aaa}% ({ansCount}) <br />";
                                    this.plcStatistic.Controls.Add(ltlAns2);
                                    break;
                            }
                        }
                    }
                }
                //文字選項
                else
                {
                    Literal ltlSelection = new Literal();
                    ltlSelection.Text = "<br />無資料，回答為文字輸入<br/>";
                    this.plcStatistic.Controls.Add(ltlSelection);
                }
            }
        }
        #endregion

        protected void btnAddCQ_Click(object sender, EventArgs e)
        {
            int cqid = Convert.ToInt32(this.ddlType.SelectedValue.Trim());
            CQAndTypeModel CQs = this._mgrQuesType.GetCQType(cqid);

            if (CQs != null)
            {
                this.txtQues.Text = this.ddlType.SelectedItem.ToString();
                this.txtAnswer.Text = CQs.CQChoice;
                this.ddlQuesType.SelectedIndex = CQs.QuesTypeID - 1;

                var isEnable = CQs.Necessary;
                if (isEnable)
                {
                    this.ckbNess.Checked = true;
                }
            }
        }
    }
}