using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class NewQues : System.Web.UI.Page
    {
        #region Manager&變數

        Guid _questionnaireID = Guid.NewGuid();
        private CQManager _mgrCQ = new CQManager();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private static List<QuesDetailModel> _questionSession = new List<QuesDetailModel>();
        private static List<QuesAndTypeModel> queslist = new List<QuesAndTypeModel>();

        private static int editQ;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                queslist.Clear();

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

                this.txtStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        //加入自訂/常用問題
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
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

            if (this.ddlType.SelectedIndex == 0)
            {
                this.txtQues.Text = String.Empty;
                this.txtAnswer.Text = String.Empty;
                this.ddlQuesType.SelectedIndex = 0;
            }
        }

        //新增問題按鈕
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
            if (TitleCheck == true && (RadioHasChoice == true || CkbHasChoice == true))
            {
                QuesDetailModel ques = new QuesDetailModel();
                ques.QuestionnaireID = _questionnaireID;
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
                QuesAndTypeModel a = this._mgrQuesDetail.GetOneQues(Session["questionList"].ToString());

                queslist.Add(a);
                this.rptQuesItem.DataSource = queslist;
                this.rptQuesItem.DataBind();

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

                this.txtQues.Text = String.Empty;
                this.txtAnswer.Text = String.Empty;
                this.ddlQuesType.SelectedIndex = 0;
                this.ddlType.SelectedIndex = 0;
                this.ckbNess.Checked = false;
                Session.Remove("questionList");
            }
        }
        
        #region 編輯問題

        //編輯問題
        protected void btnQuesEdit_Command(object sender, CommandEventArgs e)
        {
            this.ddlType.Enabled = false;

            for (int i = 0; i < queslist.Count; i++)
            {
                if (queslist[i].QuesTitle == e.CommandName)
                {
                    if (queslist[i].QuesTypeID == 1)
                    {
                        this.txtQues.Text = queslist[i].QuesTitle;
                        this.txtAnswer.Text = String.Empty;
                        this.ddlQuesType.SelectedIndex = queslist[i].QuesTypeID;
                        this.ckbNess.Checked = queslist[i].Necessary;
                        editQ = i;
                    }
                    else
                    {
                        this.txtQues.Text = queslist[i].QuesTitle;
                        this.txtAnswer.Text = queslist[i].QuesChoice;
                        this.ddlQuesType.SelectedIndex = queslist[i].QuesTypeID - 1;
                        this.ckbNess.Checked = queslist[i].Necessary;
                        editQ = i;
                    }
                }
            }

            this.rptQuesItem.DataSource = queslist;
            this.rptQuesItem.DataBind();

            this.btnQuesAddEdit.Visible = true;
            this.btnEditCancel.Visible = true;
            this.btnQuesAdd.Visible = false;
        }

        //儲存編輯問題
        protected void btnQuesAddEdit_Click(object sender, EventArgs e)
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

            if (TitleCheck == true && (RadioHasChoice == true || CkbHasChoice == true))
            {
                if (queslist[editQ].QuesTypeID == 1)
                {
                    queslist[editQ].QuesTitle = this.txtQues.Text;
                    queslist[editQ].QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue);
                    queslist[editQ].Necessary = this.ckbNess.Checked;
                }
                else
                {
                    queslist[editQ].QuesTitle = this.txtQues.Text;
                    queslist[editQ].QuesChoice = this.txtAnswer.Text;
                    queslist[editQ].QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue);
                    queslist[editQ].Necessary = this.ckbNess.Checked;
                }

                this.rptQuesItem.DataSource = queslist;
                this.rptQuesItem.DataBind();

                this.btnQuesAddEdit.Visible = false;
                this.btnEditCancel.Visible = false;
                this.btnQuesAdd.Visible = true;

                this.txtQues.Text = String.Empty;
                this.txtAnswer.Text = String.Empty;
                this.ddlQuesType.SelectedIndex = 0;
                this.ckbNess.Checked = false;
                this.ddlType.Enabled = true;
            }

        }

        //取消編輯問題
        protected void btnEditCancel_Click(object sender, EventArgs e)
        {
            this.btnQuesAddEdit.Visible = false;
            this.btnEditCancel.Visible = false;
            this.btnQuesAdd.Visible = true;

            this.txtQues.Text = String.Empty;
            this.txtAnswer.Text = String.Empty;
            this.ddlQuesType.SelectedIndex = 0;
            this.ckbNess.Checked = false;
            this.ddlType.Enabled = true;
        }

        #endregion

        #region 刪除問題
        //刪除問題
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in this.rptQuesItem.Items)
            {
                HiddenField hf = item.FindControl("hfQuesList") as HiddenField;
                CheckBox ckbDel = item.FindControl("ckbDel") as CheckBox;
                Button btn = item.FindControl("btnQuesEdit") as Button;
                if (ckbDel.Checked)
                {
                    this._mgrQuesDetail.DelQuesList(hf.Value, queslist);
                }
            }

            this.rptQuesItem.DataSource = queslist;
            this.rptQuesItem.DataBind();
        }

        #endregion

        #region 建立問卷

        //建立問卷 寫入DB
        protected void btnCreateQ_Click(object sender, EventArgs e)
        {
            bool Qname = string.IsNullOrWhiteSpace(this.txtTitle.Text);
            bool QBody = string.IsNullOrWhiteSpace(this.txtContent.Text);
            bool QStart = string.IsNullOrWhiteSpace(this.txtStart.Text);
            bool QEnd = string.IsNullOrWhiteSpace(this.txtEnd.Text);

            if (!Qname && !QBody && !QStart && !QEnd)
            {
                this.lblname.Visible = false;
                this.lblContent.Visible = false;
                this.lblStart.Visible = false;
                this.lblEnd.Visible = false;

                QuesContentsModel ques = new QuesContentsModel();
                ques.QuestionnaireID = _questionnaireID;
                ques.Title = this.txtTitle.Text;
                ques.Body = this.txtContent.Text;
                ques.StartDate = Convert.ToDateTime(this.txtStart.Text).Date;
                ques.EndDate = Convert.ToDateTime(this.txtEnd.Text);
                ques.IsEnable = this.ckbEnable.Checked;

                //資料寫進Session後在寫進資料庫
                Session["Questionnaire"] = ques;
                this._mgrContent.CreateQues(ques);

                HttpContext.Current.Session["ID"] = ques.QuestionnaireID;

                //取得該問卷ID
                var Q = this._mgrContent.GetQuesContent((Guid)Session["ID"]);

                //新增問題 寫入DB
                int questionNo = 1;
                foreach (QuesDetailModel question in _questionSession)
                {
                    question.QuesID = questionNo;
                    question.TitleID = Q.TitleID;
                    question.QuestionnaireID = Q.QuestionnaireID;
                    _mgrQuesDetail.CreateQuesDetail(question);

                    questionNo++;
                }

                //回列表頁
                Response.Redirect("ListPageAdmin.aspx");
                //Response.Redirect("NewQues.aspx?ID=" + ques.QuestionnaireID);
            }
            else
            {
                if (Qname)
                    this.lblname.Visible = true;
                else
                    this.lblname.Visible = false;

                if (QBody)
                    this.lblContent.Visible = true;
                else
                    this.lblContent.Visible = false;

                if (QStart)
                    this.lblStart.Visible = true;
                else
                    this.lblStart.Visible = false;

                if (QEnd)
                    this.lblEnd.Visible = true;
                else
                    this.lblEnd.Visible = false;
            }
        }

        #endregion

        //取消新建問卷
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListPageAdmin.aspx");
        }

        //限制結束日期範圍
        protected void txtEnd_TextChanged(object sender, EventArgs e)
        {
            DateTime end = Convert.ToDateTime(this.txtEnd.Text);

            if(end < DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('截止日期不可為過去。')", true);
                this.txtEnd.Text = String.Empty;
            }
        }
    }
}