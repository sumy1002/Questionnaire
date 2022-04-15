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
    public partial class checkPage : System.Web.UI.Page
    {
        private QuesContentsManager _mgrQues = new QuesContentsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            string idText = this.Request.QueryString["TitleID"];

            // 如果沒有帶 id ，跳回列表頁
            if (string.IsNullOrWhiteSpace(idText))
                this.BackToListPage();

            int id;
            if (!int.TryParse(idText, out id))
                this.BackToListPage();

            // 查資料
            ORM.Content quesContent = this._mgrQues.GetQuesContent(id);
            if (quesContent == null)
                this.BackToListPage();

            // 不開放前台顯示
            if (!quesContent.IsEnable)
                this.BackToListPage();


            string name = this.Session["Name"] as string;
            string phone = this.Session["Phone"] as string;
            string email = this.Session["Email"] as string;
            string age = this.Session["Age"] as string;

            if (!string.IsNullOrWhiteSpace(name))
                this.ltlNameAns.Text = name.ToString();
            else
                this.ltlNameAns.Text = "No Session";

            if (!string.IsNullOrWhiteSpace(phone))
                this.ltlPhoneAns.Text = phone.ToString();
            if (!string.IsNullOrWhiteSpace(email))
                this.ltlEmailAns.Text = email.ToString();
            if (!string.IsNullOrWhiteSpace(age))
                this.ltlAgeAns.Text = age.ToString();
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('確定要返回嗎？')", true);
            Response.Redirect("mainPage.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('資料都正確嗎？確定要送出囉？')", true);
        }

        private void BackToListPage()
        {
            this.Response.Redirect("listPage.aspx", true);
        }
    }
}