using questionnaire.Managers;
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
        private QuesContentsManager _mgrQuesContents = new QuesContentsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string str = string.Empty;
                var quesList = this._mgrQuesContents.GetQuesContentsList(str);
                this.rptQues.DataSource = quesList;
                this.rptQues.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //輸入Title搜尋
            string title = this.txtTitle.Text;
            var quesList = this._mgrQuesContents.GetQuesContentsList(title);
            this.rptQues.DataSource = quesList;
            this.rptQues.DataBind();
            
            string url = this.Request.Url.LocalPath + "?Caption=" + this.txtTitle.Text;
            this.Response.Redirect(url);

            // 輸入日期搜尋
            // String轉DateTime
            var startDate = this.txtStartDate.Text;
            var endDate = this.txtEndDate.Text;
            var strDT = Convert.ToDateTime(startDate);
            var endDT = Convert.ToDateTime(endDate);

            //過濾搜尋條件(起始or結束、起始+結束或全部)
            if (string.IsNullOrWhiteSpace(startDate) || 
                string.IsNullOrWhiteSpace(endDate))
            {
                var quesList2 = this._mgrQuesContents.GetQuesContentsList_Date(strDT, endDT);
                this.rptQues.DataSource = quesList2;
                this.rptQues.DataBind();

                string url2 = this.Request.Url.LocalPath + "?StartDate=" + startDate + "&EndDate=" + endDate;
                this.Response.Redirect(url2);
            }
            else if(!string.IsNullOrWhiteSpace(startDate) &&
                !string.IsNullOrWhiteSpace(endDate))
            {
                var quesList2 = this._mgrQuesContents.GetQuesContentsList_Date2(strDT, endDT);
                this.rptQues.DataSource = quesList2;
                this.rptQues.DataBind();

                string url2 = this.Request.Url.LocalPath + "?StartDate=" + startDate + "&EndDate=" + endDate;
                this.Response.Redirect(url2);
            }
            else
            {
                var quesList2 = this._mgrQuesContents.GetQuesContentsList_Date(strDT, endDT);
                this.rptQues.DataSource = quesList2;
                this.rptQues.DataBind();

                string url2 = this.Request.Url.LocalPath + "?StartDate=" + startDate + "&EndDate=" + endDate;
                this.Response.Redirect(url2);
            }
        }
    }
}