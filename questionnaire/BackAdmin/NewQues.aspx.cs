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
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private CQManager _mgrCQ = new CQManager();
        List<QuesAndTypeModel> _quesSession = new List<QuesAndTypeModel>();
        //private static List<QuesAndTypeModel> _quesSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            //_quesSession = HttpContext.Current.Session["qusetionModel"] as List<QuesAndTypeModel>;

            if (!IsPostBack)
            {
                //_quesSession = HttpContext.Current.Session["qusetionModel"] as List<QuesAndTypeModel>;

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

        private void InitRpt(List<QuesAndTypeModel> questionList)
        {
            if (questionList != null || questionList.Count > 0)
            {
                int i = 1;
                this.rptQuesItem.Visible = true;
                this.rptQuesItem.DataSource = questionList;
                this.rptQuesItem.DataBind();
                foreach (RepeaterItem item in this.rptQuesItem.Items)
                {
                    Label lblNumber = item.FindControl("lblNumber") as Label;
                    lblNumber.Text = i.ToString();
                    i++;
                }
            }
            else
                this.rptQuesItem.Visible = false;
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

        //生成問卷
        protected void btnSend_Click(object sender, EventArgs e)
        {
            //this.plc1.Visible = false;
        }

        protected void btnQuesAdd_Click(object sender, EventArgs e)
        {
            ////編號
            //this.lblnumber.Text += (number.ToString() + "<br />");
            //number = number + 1;

            //this.lblQues.Text += (this.txtQues.Text + "<br />");
            //this.lblQType.Text += (this.ddlQuesType.SelectedItem.ToString() + "<br />");

            //bool Ness = this.ckbNess.Checked;
            //if (Ness)
            //{
            //    this.ckbNecessary.Checked = true;
            //}

            QuesAndTypeModel ques = new QuesAndTypeModel();
            ques.QuesTitle = this.txtQues.Text.Trim();
            ques.QuesChoice = this.txtAnswer.Text.Trim();
            ques.QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue);
            ques.QuesType1 = (this.ddlQuesType.SelectedItem.ToString()).Trim();
            ques.Necessary = this.ckbNess.Checked;
            _quesSession.Add(ques);

            HttpContext.Current.Session["qusetionModel"] = _quesSession;
            InitRpt(_quesSession);
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