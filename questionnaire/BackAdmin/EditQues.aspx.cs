using questionnaire.Managers;
using questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class mainPageA : System.Web.UI.Page
    {
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();
        private CQManager _mgrCQ = new CQManager();
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
                var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);

                //取資料放進輸入框
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
            }
        }

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
                this.txtQuesEdit.Text = item.QuesTitle.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
            }
            else
            {
                this.txtQuesEdit.Text = item.QuesTitle.ToString();
                this.txtAnswerEdit.Text = item.QuesChoice.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
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

        //建立問題按鈕
        protected void imgbtnPlus_Click(object sender, ImageClickEventArgs e)
        {
            this.plcQues.Visible = true;
            this.plcUpdate.Visible = false;
            this.imgbtnDel.Visible = false;
            this.imgbtnPlus.Visible = false;
        }

        //取消建立問題
        protected void btnQuesAddCancel_Click(object sender, EventArgs e)
        {
            this.plcQues.Visible = false;
            this.plcUpdate.Visible = false;
            this.imgbtnDel.Visible = true;
            this.imgbtnPlus.Visible = true;
        }

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

                    //取問卷ID
                    string ID = Request.QueryString["ID"];
                    this.Label1.Text = ID;
                    Guid id = new Guid(ID);

                    //導回正確的問卷編輯頁
                    Response.Redirect($"EditQues.aspx?ID={ID}");
                }
            }
        }
    }
}