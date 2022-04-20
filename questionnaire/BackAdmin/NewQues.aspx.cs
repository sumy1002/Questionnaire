using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
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
                //this.ddlQuesType.SelectedItem = CQs.QuesTypeID;
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
            //this.plc1.Visible = false;
            
        }

        //新增問題按鈕
        protected void btnQuesAdd_Click(object sender, EventArgs e)
        {
            QuesDetailModel ques = new QuesDetailModel();
            //ques.QuestionnaireID = _questionnaireID;
            ques.QuesTitle = this.txtQues.Text.Trim();
            ques.QuesChoice = this.txtAnswer.Text.Trim();
            ques.QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue);
            ques.Necessary = this.ckbNess.Checked;

            _questionSession.Add(ques);
            HttpContext.Current.Session["qusetionModel"] = _questionSession;


            //HttpContext.Current.Session["qusetionModel"] = _quesSession;
            ////InitRpt(_quesSession);

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

        protected void btnCreateQ_Click(object sender, EventArgs e)
        {
            //建立問卷
            QuesContentsModel ques = new QuesContentsModel();
            ques.QuestionnaireID = _questionnaireID;
            ques.Title = this.txtTitle.Text;
            ques.Body = this.txtContent.Text;
            ques.StartDate = Convert.ToDateTime(this.txtStart.Text);
            ques.EndDate = Convert.ToDateTime(this.txtEnd.Text);
            ques.IsEnable = this.ckbEnable.Checked;

            //資料寫進Session後在寫進資料庫
            Session["Questionnaire"] = ques;
            this._mgrContent.CreateQues(ques);

            //取得該問卷ID
            var Q = this._mgrContent.GetQuesContent(ques.QuestionnaireID);

            //建立問題
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
        }

        //private void CreateRdb(QuestionModel question)
        //{
        //    RadioButtonList radioButtonList = new RadioButtonList();
        //    radioButtonList.ID = "Q" + question.QuestionNo;
        //    this.plcDynamic.Controls.Add(radioButtonList);
        //    string[] arrQue = question.Selection.Split(';');
        //    for (int i = 0; i < arrQue.Length; i++)
        //    {
        //        ListItem item = new ListItem(arrQue[i], i.ToString());
        //        radioButtonList.Items.Add(item);
        //    }
        //}
    }
}