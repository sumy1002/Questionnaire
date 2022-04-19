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
            string a = string.Empty;
            var quesList = this._mgrQues.GetQuesContentsList(a);
            this.rptList.DataSource = quesList;
            this.rptList.DataBind();
        }

        protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            //去新建
            Response.Redirect("~/BackAdmin/NewQues.aspx");
        }
    }
}