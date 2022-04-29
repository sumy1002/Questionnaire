using questionnaire.Managers;
using questionnaire.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire
{
    public partial class listPage : System.Web.UI.Page
    {
        private QuesContentsManager _mgrQues = new QuesContentsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //列表資料繫結
                string a = string.Empty;
                var quesList = this._mgrQues.GetContentsList(a);
                this.rptQues.DataSource = quesList;
                this.rptQues.DataBind();

                foreach (RepeaterItem item in this.rptQues.Items)
                {
                    Literal ltl1 = item.FindControl("ltlState") as Literal;
                    HiddenField hfID = item.FindControl("hfID") as HiddenField;
                    HiddenField hfSta = item.FindControl("hfSta") as HiddenField;
                    HiddenField hfEnd = item.FindControl("hfEnd") as HiddenField;

                    var SDT = Convert.ToDateTime(hfSta.Value);
                    var EDT = Convert.ToDateTime(hfEnd.Value);
                    if (EDT < DateTime.Now && int.TryParse(hfID.Value, out int titleID))
                    {
                        ltl1.Text = "已截止";
                    }
                    else if(SDT > DateTime.Now && int.TryParse(hfID.Value, out int titleID2))
                    {
                        ltl1.Text = "尚未開始";
                    }
                }
            }
        }

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
                this.rptQues.DataSource = quesList;
                this.rptQues.DataBind();

                this.txtStartDate.Enabled = true;
                this.txtEndDate.Enabled = true;
                this.txtTitle.Text = String.Empty;

                //查無資料
                if (quesList.Count == 0 || quesList == null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='listPage.aspx';", true);
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
                    this.rptQues.DataSource = quesList;
                    this.rptQues.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='listPage.aspx';", true);
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
                    this.rptQues.DataSource = quesList;
                    this.rptQues.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='listPage.aspx';", true);
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
                    this.rptQues.DataSource = quesList;
                    this.rptQues.DataBind();

                    this.txtTitle.Enabled = true;
                    this.txtStartDate.Text = String.Empty;
                    this.txtEndDate.Text = String.Empty;

                    //查無資料
                    if (quesList.Count == 0 || quesList == null)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');location.href='listPage.aspx';", true);
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
                this.rptQues.DataSource = quesList;
                this.rptQues.DataBind();

                this.ltlMsg.Visible = false;
            }

            //判斷一下狀態
            foreach (RepeaterItem item in this.rptQues.Items)
            {
                Literal ltl1 = item.FindControl("ltlState") as Literal;
                HiddenField hfID = item.FindControl("hfID") as HiddenField;
                HiddenField hfSta = item.FindControl("hfSta") as HiddenField;
                HiddenField hfEnd = item.FindControl("hfEnd") as HiddenField;

                var SDT = Convert.ToDateTime(hfSta.Value);
                var EDT = Convert.ToDateTime(hfEnd.Value);
                if (EDT < DateTime.Now && int.TryParse(hfID.Value, out int titleID))
                {
                    ltl1.Text = "已截止";
                }
                else if (SDT > DateTime.Now && int.TryParse(hfID.Value, out int titleID2))
                {
                    ltl1.Text = "尚未開始";
                }
            }
        }

        #region 日期搜尋
        //以標題搜尋
        private List<QuesContentsModel> TitleSearch(string id)
        {
            var List = this._mgrQues.GetContentsList(id);
            return List;
        }
        //以起始日搜尋
        private List<QuesContentsModel> OneDateSearch1(DateTime Sdt)
        {
            var List = this._mgrQues.GetContentsList_DateStart(Sdt);
            return List;
        }
        //以結束日搜尋
        private List<QuesContentsModel> OneDateSearch2(DateTime Edt)
        {
            var List = this._mgrQues.GetContentsList_DateEnd(Edt);
            return List;
        }
        //以起始日+結束日搜尋
        private List<QuesContentsModel> BothDateSearch(DateTime Sdt, DateTime Edt)
        {
            var List = this._mgrQues.GetsContentsList_Date2(Sdt, Edt);
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

        //登入
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}