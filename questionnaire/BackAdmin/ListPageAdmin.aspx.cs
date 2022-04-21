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
    public partial class listPageA : System.Web.UI.Page
    {
        private QuesContentsManager _mgrQues = new QuesContentsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //列表資料繫結
            string a = string.Empty;
            var quesList = this._mgrQues.GetQuesContentsList(a);
            this.rptList.DataSource = quesList;
            this.rptList.DataBind();
        }

        //新建問卷按鈕
        protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            //去新建
            Response.Redirect("~/BackAdmin/NewQues.aspx");
        }

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
                }
            }

            //this.txtTitle.Text = String.Empty;
            //this.txtStartDate.Text = String.Empty;
            //this.txtEndDate.Text = String.Empty;
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
    }
}