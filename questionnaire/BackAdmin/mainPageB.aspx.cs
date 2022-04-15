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
    public partial class mainPageB : System.Web.UI.Page
    {
        //private List<QuesDetailModel> _quesDetail = new List<QuesDetailModel>();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private CQManager _mgrCQ = new CQManager();
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

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int cqid = Convert.ToInt32(this.ddlType.SelectedValue.Trim());
            CQAndTypeModel CQs = this._mgrQuesType.GetCQType(cqid);

            if (CQs != null)
            {
                this.txtQues.Text = this.ddlType.SelectedItem.ToString();
                this.txtAnswer.Text = CQs.CQChoice;
                //this.ddlQuesType.SelectedItem = CQs.QuesTypeID;
                this.ddlQuesType.SelectedIndex = CQs.QuesTypeID - 1;

                var isEnable = CQs.IsEnable;
                if(isEnable)
                {
                    this.ckbMust.Checked = true;
                }
            }

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            //this.plc1.Visible = false;
        }
    }
}