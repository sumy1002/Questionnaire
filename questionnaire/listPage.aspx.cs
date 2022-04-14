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
            //if(!string.IsNullOrWhiteSpace(this.txtTitle.Text))
            //{
            //    var quesList = this._mgrQuesContents.GetQuesContentsList(this.txtTitle.Text);
            //    this.rptQues.DataSource = quesList;
            //    this.rptQues.DataBind();
            //}
            string title = this.txtTitle.Text;
            var quesList = this._mgrQuesContents.GetQuesContentsList(title);
            this.rptQues.DataSource = quesList;
            this.rptQues.DataBind();
            
            string url = this.Request.Url.LocalPath + "?Caption=" + this.txtTitle.Text;
            this.Response.Redirect(url);

            // 日期搜尋
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text) && 
                !string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                var quesList2 = this._mgrQuesContents.GetQuesContentsList(this.txtStartDate.Text, this.txtEndDate.Text);
                this.rptQues.DataSource = quesList2;
                this.rptQues.DataBind();

                string url2 = this.Request.Url.LocalPath + "?StartDate=" + this.txtStartDate.Text + "&EndDate=" + this.txtEndDate.Text;
                this.Response.Redirect(url2);
            }
        }
    }
}