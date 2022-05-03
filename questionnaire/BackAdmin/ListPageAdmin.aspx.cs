using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class listPageA : System.Web.UI.Page
    {
        private QuesContentsManager _mgrQues = new QuesContentsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //列表資料繫結
                string a = string.Empty;
                var quesList = this._mgrQues.GetQuesContentsList(a);
                this.rptList.DataSource = quesList;
                this.rptList.DataBind();

                //關閉中的問卷呈現紅色
                foreach (RepeaterItem item in this.rptList.Items)
                {
                    HiddenField hfID = item.FindControl("hfID") as HiddenField;
                    CheckBox ckbDel = item.FindControl("CheckBox1") as CheckBox;
                    Label lbl0 = item.FindControl("lblTitleID") as Label;
                    Label lbl1 = item.FindControl("lblTitle") as Label;
                    Label lbl2 = item.FindControl("lblIsEnable") as Label;
                    Label lbl3 = item.FindControl("lblSDT") as Label;
                    Label lbl4 = item.FindControl("lblEDT") as Label;
                    ImageButton imgbtnClose = item.FindControl("ImgBtnClose") as ImageButton;
                    if (!ckbDel.Checked && Guid.TryParse(hfID.Value, out Guid questionnaireID))
                    {
                        lbl0.ForeColor = Color.Red;
                        lbl1.ForeColor = Color.Red;
                        lbl2.ForeColor = Color.Red;
                        lbl3.ForeColor = Color.Red;
                        lbl4.ForeColor = Color.Red;

                        imgbtnClose.Visible = false;
                    }
                }

                //生成問卷的編號
                int i = this.rptList.Items.Count;
                foreach (RepeaterItem item in this.rptList.Items)
                {
                    Label lblTitleID = item.FindControl("lblTitleID") as Label;
                    lblTitleID.Text = i.ToString();
                    i--;
                }
            }
        }

        #region 新建

        //新建問卷按鈕
        protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            //去新建
            Response.Redirect("~/BackAdmin/NewQues.aspx");
        }

        #endregion

        #region 搜尋

        //搜尋按鈕
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var title = this.txtTitle.Text;
            var startDT = this.txtStartDate.Text;
            var endDT = this.txtEndDate.Text;

            //判斷是否有值
            bool titleSearch = String.IsNullOrWhiteSpace(title);
            bool StartDTSearch = String.IsNullOrWhiteSpace(startDT);
            bool EndDTSearch = String.IsNullOrWhiteSpace(endDT);

            //判斷是否以標題搜尋
            if (!titleSearch)
            {
                var quesList = TitleSearch(title);
                this.rptList.DataSource = quesList;
                this.rptList.DataBind();

                this.txtStartDate.Enabled = true;
                this.txtEndDate.Enabled = true;
                this.txtTitle.Text = String.Empty;

                //查無資料
                if (quesList.Count == 0 || quesList == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='ListPageAdmin.aspx';", true);
                }
                else
                {
                    var count = quesList.Count.ToString();
                    var text = "以 標題 搜尋 " + title + " ";
                    this.ltlMsg.Text = text + "，共有 " + count + " 項結果";
                    this.ltlMsg.Visible = true;
                }
            }
            //判斷是否以日期搜尋
            else if (!StartDTSearch || !EndDTSearch)
            {
                //起始結束日皆有值
                if (!StartDTSearch && !EndDTSearch)
                {
                    var sDT = Convert.ToDateTime(startDT);
                    var eDT = Convert.ToDateTime(endDT);

                    var quesList = BothDateSearch(sDT, eDT);
                    this.rptList.DataSource = quesList;
                    this.rptList.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='ListPageAdmin.aspx';", true);
                    }
                    else
                    {
                        var count = quesList.Count.ToString();
                        var text = "以 日期 搜尋從  " + startDT + "  到  " + endDT + "  之間";
                        this.ltlMsg.Text = text + "，共有 " + count + " 項結果";
                        this.ltlMsg.Visible = true;
                    }
                }
                //只有起始日有值
                else if (!StartDTSearch && EndDTSearch)
                {
                    var sDT = Convert.ToDateTime(startDT);

                    var quesList = OneDateSearch1(sDT);
                    this.rptList.DataSource = quesList;
                    this.rptList.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='ListPageAdmin.aspx';", true);
                    }
                    else
                    {
                        var count = quesList.Count.ToString();
                        var text = "以 日期 搜尋從  " + startDT + "  開始";
                        this.ltlMsg.Text = text + "，共有 " + count + " 項結果";
                        this.ltlMsg.Visible = true;
                    }
                }
                //只有結束日有值
                else if (StartDTSearch && !EndDTSearch)
                {
                    var eDT = Convert.ToDateTime(endDT);

                    var quesList = OneDateSearch2(eDT);
                    this.rptList.DataSource = quesList;
                    this.rptList.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='ListPageAdmin.aspx';", true);
                    }
                    else
                    {
                        var count = quesList.Count.ToString();
                        var text = "以 日期 搜尋到  " + endDT + "  截止";
                        this.ltlMsg.Text = text + "，共有 " + count + " 項結果";
                        this.ltlMsg.Visible = true;
                    }
                }
            }
            else
            {
                var t = string.Empty;
                var quesList = TitleSearch(t);
                this.rptList.DataSource = quesList;
                this.rptList.DataBind();

                this.ltlMsg.Visible = false;
            }

            //關閉中的問卷呈現紅色
            foreach (RepeaterItem item in this.rptList.Items)
            {
                HiddenField hfID = item.FindControl("hfID") as HiddenField;
                CheckBox ckbDel = item.FindControl("CheckBox1") as CheckBox;
                CheckBox ckb = item.FindControl("ckbDel") as CheckBox;
                Label lbl0 = item.FindControl("lblTitleID") as Label;
                Label lbl1 = item.FindControl("lblTitle") as Label;
                Label lbl2 = item.FindControl("lblIsEnable") as Label;
                Label lbl3 = item.FindControl("lblSDT") as Label;
                Label lbl4 = item.FindControl("lblEDT") as Label;
                ImageButton imgbtnClose = item.FindControl("ImgBtnClose") as ImageButton;
                if (!ckbDel.Checked && Guid.TryParse(hfID.Value, out Guid questionnaireID))
                {
                    lbl0.ForeColor = Color.Red;
                    lbl1.ForeColor = Color.Red;
                    lbl2.ForeColor = Color.Red;
                    lbl3.ForeColor = Color.Red;
                    lbl4.ForeColor = Color.Red;

                    imgbtnClose.Visible = false;
                    ckb.Visible = false;
                }
            }
        }

        #region 日期搜尋
        //以標題搜尋
        private List<QuesContentsModel> TitleSearch(string id)
        {
            var List = this._mgrQues.GetQuesContentsList(id);
            return List;
        }
        //以起始日搜尋
        private List<QuesContentsModel> OneDateSearch1(DateTime Sdt)
        {
            var List = this._mgrQues.GetQuesContentsList_DateStart(Sdt);
            return List;
        }
        //以結束日搜尋
        private List<QuesContentsModel> OneDateSearch2(DateTime Edt)
        {
            var List = this._mgrQues.GetQuesContentsList_DateEnd(Edt);
            return List;
        }
        //以起始日+結束日搜尋
        private List<QuesContentsModel> BothDateSearch(DateTime Sdt, DateTime Edt)
        {
            var List = this._mgrQues.GetQuesContentsList_Date2(Sdt, Edt);
            return List;
        }
        #endregion

        #region 限制搜尋功能

        //以標題搜尋時關閉日期搜尋功能
        protected void txtTitle_TextChanged(object sender, EventArgs e)
        {
            var title = this.txtTitle.Text;
            bool titleSearch = String.IsNullOrWhiteSpace(title);

            if (!titleSearch)
            {
                this.txtStartDate.Enabled = false;
                this.txtEndDate.Enabled = false;
            }
            else
            {
                this.txtStartDate.Enabled = true;
                this.txtEndDate.Enabled = true;
            }
        }
        //以日期搜尋時關閉標題搜尋功能
        protected void txtStartDate_TextChanged(object sender, EventArgs e)
        {
            var startDT = this.txtStartDate.Text;
            bool StartDTSearch = String.IsNullOrWhiteSpace(startDT);

            if (!StartDTSearch)
                this.txtTitle.Enabled = false;
            else
                this.txtTitle.Enabled = true;
        }
        //以日期搜尋時關閉標題搜尋功能
        protected void txtEndDate_TextChanged(object sender, EventArgs e)
        {
            var endDT = this.txtEndDate.Text;
            bool EndDTSearch = String.IsNullOrWhiteSpace(endDT);

            if (!EndDTSearch)
                this.txtTitle.Enabled = false;
            else
                this.txtTitle.Enabled = true;
        }

        #endregion

        #endregion

        #region 刪除/開啟/關閉問卷

        //關閉/開啟問卷
        protected void ImgBtnDel_Command(object sender, CommandEventArgs e)
        {
            Guid id = Guid.Parse(e.CommandName);
            this._mgrQues.DeleteQues(id);

            Response.Redirect("ListPageAdmin.aspx");
        }

        //刪除問卷
        protected void ImgBtnClose_Click(object sender, ImageClickEventArgs e)
        {
            foreach (RepeaterItem item in this.rptList.Items)
            {
                HiddenField hfid = item.FindControl("hfID") as HiddenField;
                CheckBox ckbDel = item.FindControl("ckbDel") as CheckBox;
                ImageButton imgbtn = item.FindControl("ImgBtnClose") as ImageButton;
                if (ckbDel.Checked)
                {
                    Guid Qid = new Guid(imgbtn.CommandName);
                    //把問題從資料庫中刪除
                    this._mgrQues.DelQues(Qid);
                }
            }

            Response.Redirect("ListPageAdmin.aspx");
        }

        #endregion
    }
}