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
    public partial class checkPage : System.Web.UI.Page
    {
        //private QuesContentsManager _mgrQues = new QuesContentsManager();
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private static List<QuesDetailModel> _answerList;
        int i = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 1
            //string idText = this.Request.QueryString["TitleID"];
            //
            //// 如果沒有帶 id ，跳回列表頁
            //if (string.IsNullOrWhiteSpace(idText))
            //    this.BackToListPage();
            //
            //Guid id;
            //if (!Guid.TryParse(idText, out id))
            //    this.BackToListPage();
            //
            //// 查資料
            //ORM.Content quesContent = this._mgrQues.GetQuesContent(id);
            //if (quesContent == null)
            //    this.BackToListPage();
            //
            //// 不開放前台顯示
            //if (!quesContent.IsEnable)
            //    this.BackToListPage();
            //
            //
            //string name = this.Session["Name"] as string;
            //string phone = this.Session["Phone"] as string;
            //string email = this.Session["Email"] as string;
            //string age = this.Session["Age"] as string;
            //
            //if (!string.IsNullOrWhiteSpace(name))
            //    this.ltlNameAns.Text = name.ToString();
            //else
            //    this.ltlNameAns.Text = "No Session";
            //
            //if (!string.IsNullOrWhiteSpace(phone))
            //    this.ltlPhoneAns.Text = phone.ToString();
            //if (!string.IsNullOrWhiteSpace(email))
            //    this.ltlEmailAns.Text = email.ToString();
            //if (!string.IsNullOrWhiteSpace(age))
            //    this.ltlAgeAns.Text = age.ToString();
            #endregion
            var name = GetSession("Name");
            var phone = GetSession("Phone");
            var email = GetSession("Email");
            var age = GetSession("Age");

            this.txtName.Text = name;
            this.txtPhone.Text = phone;
            this.txtEmail.Text = email;
            this.txtAge.Text = age;

            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問卷及問題列表
            var Ques = this._mgrContent.GetQuesContent2(id);
            var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);

            this.hfID.Value = Ques.QuestionnaireID.ToString();
            this.ltlTitle.Text = Ques.Title;
            this.ltlContent.Text = Ques.Body;

            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
            foreach (QuesDetail question in questionList)
            {
                //string q = $"<br/>{question.TitleID}. {question.QuesID}";
                string title = $"<br /><br />{i}. {question.QuesTitle}";
                if (question.Necessary)
                    title += "(*)";
                i = i + 1;
                Literal ltlQuestion = new Literal();
                ltlQuestion.Text = title + "<br/>";

                this.plcDynamic.Controls.Add(ltlQuestion);

                //if (isEditMode)
                //{
                //    switch (question.Type)
                //    {
                //        case QuestionType.單選方塊:
                //            EditRdb(question);
                //            break;
                //        case QuestionType.複選方塊:
                //            EditCkb(question);
                //            break;
                //        case QuestionType.文字:
                //            EditTxt(question);
                //            break;
                //    }
                //}
                //else
                //{
                switch (question.QuesTypeID)
                {
                    case 1:
                        CreateTxt(question);
                        break;
                    case 2:
                        CreateRdb(question);
                        break;
                    case 3:
                        CreateCkb(question);
                        break;
                }

                this.ltlVote.Text = Ques.IsEnable.ToString();
                this.lvlTime.Text = Ques.strStartTime.ToString();

                string count = questionList.Count.ToString();
                this.ltlQCount.Text = "共 " + count + " 個問題";
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('確定要返回嗎？')", true);
            //Response.Redirect("mainPage.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('資料都正確嗎？確定要送出囉？')", true);
        }

        private void BackToListPage()
        {
            //this.Response.Redirect("listPage.aspx", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click1(object sender, EventArgs e)
        {

        }
        public static string GetSession(string key)
        {
            if (key.Length == 0)
                return string.Empty;
            return HttpContext.Current.Session[key] as string;

        }

        //建立單選問題
        private void CreateRdb(QuesDetail question)
        {
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(radioButtonList);

            //
            string[] arrQ = question.QuesChoice.Split(';');

            //for (int i = 0; i < arrQ.Length; i++)
            //{
            //    ListItem item = new ListItem(arrQ[i], i.ToString());
            //    radioButtonList.Items.Add(item);
            //}

            for (int i = 0; i < arrQ.Length; i++)
            {
                RadioButton item = new RadioButton();
                item.Text = arrQ[i].ToString();
                item.GroupName = "group" + question.QuesID;
                this.plcDynamic.Controls.Add(item);
                this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
            }
        }

        //建立複選問題
        private void CreateCkb(QuesDetail question)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(checkBoxList);
            string[] arrQ = question.QuesChoice.Split(';');
            for (int i = 0; i < arrQ.Length; i++)
            {
                ListItem item = new ListItem(arrQ[i], i.ToString());
                checkBoxList.Items.Add(item);
            }
        }

        //建立文字問題
        private void CreateTxt(QuesDetail question)
        {
            TextBox textBox = new TextBox();
            textBox.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(textBox);
        }
    }
}