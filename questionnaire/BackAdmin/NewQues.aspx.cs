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
        //private List<QuesDetailModel> _quesDetail = new List<QuesDetailModel>();
        private CQManager _mgrCQ = new CQManager();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        Guid _questionnaireID = Guid.NewGuid();
        private static List<QuesDetailModel> _questionSession = new List<QuesDetailModel>();
        //private List<QuesDetailModel> _questionSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            }
        }

        //加入自訂/常用問題
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

        //生成問卷A送出
        protected void btnSend_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewQues.aspx#question");
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
                var queslist = this._mgrQuesDetail.GetQuesList(Session["questionList"].ToString());

                this.rptQuesItem.DataSource = queslist;
                this.rptQuesItem.DataBind();

                //Response.Write("<script>GoTotab2()</script>");

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
            }
        }

        //建立問卷 寫入DB
        protected void btnCreateQ_Click(object sender, EventArgs e)
        {
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            this.txtContent.Text = "123";
            this.txtTitle.Text = "123";
        }
    }
}