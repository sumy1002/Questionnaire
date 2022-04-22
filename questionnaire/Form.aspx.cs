using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace questionnaire
{
    public partial class mainPage : System.Web.UI.Page
    {
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        int i = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    title += "(*必填)";
                i = i + 1;
                Literal ltlQuestion = new Literal();
                ltlQuestion.Text =title + "<br/>";

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

                this.ltlVote.Text = Ques.State.ToString();
                this.lvlTime.Text = Ques.strStartTime.ToString();

                string count = questionList.Count.ToString();
                this.ltlQCount.Text = "共 " + count + " 個問題";
            }
        }

        private void CreateRdb(QuesDetail question)
        {
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(radioButtonList);

            //
            string[] arrQ = question.QuesChoice.Split(';');
            for (int i = 0; i < arrQ.Length; i++)
            {
                ListItem item = new ListItem(arrQ[i], i.ToString());
                radioButtonList.Items.Add(item);
            }
        }
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
        private void CreateTxt(QuesDetail question)
        {
            TextBox textBox = new TextBox();
            textBox.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(textBox);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("listPage.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            bool isNameRight = false;
            bool isPhoneRight = false;
            bool isEmailRight = false;
            bool isAgeRight = false;

            bool telCheck = Regex.IsMatch(this.txtPhone.Text.Trim(), @"^09[0-9]{8}$");
            bool emailCheck = Regex.IsMatch(this.txtEmail.Text.Trim(), @"@gmail.com$");

            if (!string.IsNullOrWhiteSpace(this.txtName.Text))
                isNameRight = true;
            if (!string.IsNullOrWhiteSpace(this.txtPhone.Text) && telCheck)
                isPhoneRight = true;
            if (!string.IsNullOrWhiteSpace(this.txtEmail.Text) && emailCheck)
                isEmailRight = true;
            if (!string.IsNullOrWhiteSpace(this.txtAge.Text))
                isAgeRight = true;

            if (isNameRight && isPhoneRight && isEmailRight && isAgeRight)
            {
                this.Session["Name"] = this.txtName.Text;
                this.Session["Phone"] = this.txtPhone.Text;
                this.Session["Email"] = this.txtEmail.Text;
                this.Session["Age"] = this.txtAge.Text;

                Response.Redirect("checkPage.aspx");
            }
        }
    }
}