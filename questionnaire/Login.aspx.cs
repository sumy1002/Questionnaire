using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire
{
    public partial class Login : System.Web.UI.Page
    {
        private AccountManager _mgrAccount = new AccountManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this._mgrAccount.IsLogined())
            {
                this.plcUserPage.Visible = true;
                this.plcLogin.Visible = false;

                Account account = this._mgrAccount.GetCurrentUser();
                this.ltlAccount.Text = account.Account1;
            }
            else
            {
                this.plcLogin.Visible = true;
                this.plcUserPage.Visible = false;
            }
        }

        //確定登入
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string account = this.txtAccount.Text.Trim();
            string pwd = this.txtPassword.Text.Trim();

            if (this._mgrAccount.TryLogin(account, pwd))
            {
                Response.Redirect(Request.RawUrl);
            }
            //帳號密碼錯誤
            else
            {
                this.txtAccount.Text = String.Empty;
                this.txtPassword.Text = String.Empty;
                this.lblMessage.Visible = true;
            }
        }

        //取消登入
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("listPage.aspx");
        }

        
    }
}