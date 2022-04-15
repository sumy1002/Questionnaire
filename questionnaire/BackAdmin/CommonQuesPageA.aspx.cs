using questionnaire.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire.BackAdmin
{
    public partial class CommonQuesPageA : System.Web.UI.Page
    {
        private CQManager _mgrCQ = new CQManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            var quesList = this._mgrCQ.GetCQsList();
            this.rptCQ.DataSource = quesList;
            this.rptCQ.DataBind();
        }
    }
}