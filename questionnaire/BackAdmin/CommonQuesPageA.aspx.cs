using questionnaire.Managers;
using questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class CommonQuesPageA : System.Web.UI.Page
    {
        private CQManager _mgrCQ = new CQManager();
        private QuesTypeManager _mgrQuesType = new QuesTypeManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //常用問題列表繫結
                var quesList = this._mgrCQ.GetCQsList();
                this.rptCQ.DataSource = quesList;
                this.rptCQ.DataBind();

                //生成問題的編號
                int i = 1;
                foreach (RepeaterItem item in this.rptCQ.Items)
                {
                    Label lblNumber = item.FindControl("lblNumber") as Label;
                    lblNumber.Text = i.ToString();
                    i++;
                }
            }
        }

        #region 新增

        //新增問題按鈕
        protected void imgbtnPlus_Click(object sender, ImageClickEventArgs e)
        {
            this.plcAddCQ.Visible = true;
            this.imgbtnPlus.Visible = false;

            //問題類型下拉繫結
            var QTypeList = this._mgrQuesType.GetQuesTypesList();
            this.ddlQuesType.DataSource = QTypeList;
            this.ddlQuesType.DataValueField = "QuesTypeID";
            this.ddlQuesType.DataTextField = "QuesType1";
            this.ddlQuesType.DataBind();
        }

        //新增問題存進資料庫
        protected void btnQuesAdd_Click(object sender, EventArgs e)
        {
            //防呆過濾
            bool TitleCheck = !String.IsNullOrWhiteSpace(this.txtQues.Text);
            bool RadioHasChoice = false;
            bool CkbHasChoice = false;

            //檢查有無輸入問題標題
            if (TitleCheck == false)
                this.lblQuesRed.Visible = true;
            else
                this.lblQuesRed.Visible = false;

            //檢查單複選題有無輸入選項
            if (this.ddlQuesType.SelectedValue == "1") //文字
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
            else if (this.ddlQuesType.SelectedValue == "2") //單選
            {
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
            else if (this.ddlQuesType.SelectedValue == "3") //多選
            {
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

            //寫入資料庫
            if (TitleCheck == true && (RadioHasChoice == true || CkbHasChoice == true))
            {
                CQModel cqModel = new CQModel()
                {
                    CQTitle = this.txtQues.Text,
                    QuesTypeID = Convert.ToInt32(this.ddlQuesType.SelectedValue),
                    CQChoice = this.txtAnswer.Text,
                    Necessary = this.ckbNess.Checked,
                };
                this._mgrCQ.CreateCQ(cqModel);

                this.imgbtnPlus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('新增成功。');location.href='CommonQuesPageA.aspx';", true);
            }
        }

        //取消新增問題
        protected void btnQuesAddCancel_Click(object sender, EventArgs e)
        {
            this.txtQues.Text = String.Empty;
            this.txtAnswer.Text = String.Empty;
            this.ckbNess.Checked = false;
            this.plcAddCQ.Visible = false;

            this.lblAnsRed.Visible = false;
            this.lblQuesRed.Visible = false;

            this.imgbtnPlus.Visible = true;
        }

        #endregion

        #region 修改

        //修改問題按鈕
        protected void imgbtnedit_Command(object sender, CommandEventArgs e)
        {
            this.imgbtnPlus.Visible = false;
            this.plcAddCQ.Visible = false;
            this.plcEditCQ.Visible = true;

            //問題類型下拉繫結
            var QTypeList = this._mgrQuesType.GetQuesTypesList();
            this.ddlQuesTypeEdit.DataSource = QTypeList;
            this.ddlQuesTypeEdit.DataValueField = "QuesTypeID";
            this.ddlQuesTypeEdit.DataTextField = "QuesType1";
            this.ddlQuesTypeEdit.DataBind();

            //取得該問題的資料
            int id = Convert.ToInt32(e.CommandName);
            var item = this._mgrCQ.GetCQs(id);
            this.hfCQID.Value = e.CommandName;

            //判斷該問題有無答案
            bool hasChoise;
            if (item.QuesTypeID == 2 || item.QuesTypeID == 3)
                hasChoise = true;
            else
                hasChoise = false;

            if (!hasChoise)
            {
                this.txtQuesEdit.Text = item.CQTitle.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
            }
            else
            {
                this.txtQuesEdit.Text = item.CQTitle.ToString();
                this.txtAnswerEdit.Text = item.CQChoice.ToString();
                this.ddlQuesTypeEdit.SelectedValue = item.QuesTypeID.ToString();
                this.ckbNessEdit.Checked = item.Necessary;
            }
        }

        //確認修改問題
        protected void btnQuesEdit_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(this.hfCQID.Value);
            var item = this._mgrCQ.GetCQs(id);

            try
            {
                CQModel updateCQ = new CQModel()
                {
                    CQID = item.CQID,
                    CQTitle = this.txtQuesEdit.Text,
                    QuesTypeID = Convert.ToInt32(this.ddlQuesTypeEdit.SelectedValue),
                    CQChoice = this.txtAnswerEdit.Text,
                    Necessary = this.ckbNessEdit.Checked,
                };

                this._mgrCQ.UpdateCQ(updateCQ);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('變更成功。');location.href='CommonQuesPageA.aspx';", true);
                this.plcEditCQ.Visible = false;
                this.imgbtnPlus.Visible = true;
            }
            catch (Exception)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('入力した内容に誤りがあります。');location.href='adminMenu.aspx';", true);
            }
        }

        //取消修改問題
        protected void btnQuesAddCancelEdit_Click(object sender, EventArgs e)
        {
            this.txtQuesEdit.Text = String.Empty;
            this.txtAnswerEdit.Text = String.Empty;
            this.ckbNessEdit.Checked = false;
            this.plcEditCQ.Visible = false;
            this.imgbtnPlus.Visible = true;

            this.lblAnsRedEdit.Visible = false;
            this.lblQuesRedEdit.Visible = false;
        }

        #endregion

        #region 刪除

        //刪除
        protected void imgbtnDel_Command(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandName);
            this._mgrCQ.DeleteCQ(id);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('已刪除。');location.href='CommonQuesPageA.aspx';", true);
        }

        protected void imgbtnDel_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in this.rptCQ.Items)
            {
                HiddenField hfid = item.FindControl("hfcqid") as HiddenField;
                CheckBox ckbDel = item.FindControl("ckbCQ") as CheckBox;
                if (ckbDel.Checked && Int32.TryParse(hfid.Value, out int CQID))
                {
                    //把問題從資料庫中刪除
                    this._mgrCQ.DeleteCQ(Convert.ToInt32(hfid.Value));
                }
            }

            //導回正確的問卷編輯頁
            Response.Redirect($"CommonQuesPageA.aspx");
        }

        #endregion
    }
}