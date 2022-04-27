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
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private UserInfoManager _mgrUserInfo = new UserInfoManager();
        private UserQuesDetailManager _mgrUserDetail = new UserQuesDetailManager();
        int i = 1;
        int ansCount = 0;
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
                string title = $"<br /><br />{i}. {question.QuesTitle}";
                if (question.Necessary)
                    title += "(*)";
                i = i + 1;
                Literal ltlQuestion = new Literal();
                ltlQuestion.Text = title + "<br/>";

                this.plcDynamic.Controls.Add(ltlQuestion);

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

        //取Session值
        public static string GetSession(string key)
        {
            if (key.Length == 0)
                return string.Empty;
            return HttpContext.Current.Session[key] as string;
        }

        //取SessionList
        public List<UserQuesDetailModel> GetSessionList(string key)
        {
            if (key.Length == 0)
                return null;
            return HttpContext.Current.Session[key] as List<UserQuesDetailModel>;
        }

        //建立單選問題
        private void CreateRdb(QuesDetail question)
        {
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "Q" + question.QuesID;
            this.plcDynamic.Controls.Add(radioButtonList);

            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);
            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
            List<UserQuesDetailModel> answerList = GetSessionList("Answer");

            for (; ansCount < questionList.Count; ansCount++)
            {
                if (answerList[ansCount].QuesID == questionList[ansCount].QuesID)
                {
                    Label lbl = new Label();
                    lbl.ID = "Q" + question.QuesID;
                    lbl.Text = answerList[ansCount].Answer.TrimEnd(';');
                    this.plcDynamic.Controls.Add(lbl);
                    ansCount++;
                    break;
                }
            }
        }


        //建立複選問題
        private void CreateCkb(QuesDetail question)
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);
            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
            List<UserQuesDetailModel> answerList = GetSessionList("Answer");

            foreach (var q in answerList)
            {
                if (q.QuesID == question.QuesID)
                {
                    Label lbl = new Label();
                    lbl.ID = "Q" + question.QuesID;
                    lbl.Text = q.Answer.TrimEnd(';');
                    this.plcDynamic.Controls.Add(lbl);
                }
            }
        }

        //建立文字問題
        private void CreateTxt(QuesDetail question)
        {
            List<UserQuesDetailModel> answerList = GetSessionList("Answer");

            foreach (var q in answerList)
            {
                if (q.QuesID == question.QuesID)
                {
                    Label lbl = new Label();
                    lbl.ID = "Q" + question.QuesID;
                    lbl.Text = q.Answer.TrimEnd(';');
                    this.plcDynamic.Controls.Add(lbl);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var name = GetSession("Name");
            var phone = GetSession("Phone");
            var email = GetSession("Email");
            var age = GetSession("Age");

            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問卷及問題列表
            var Ques = this._mgrContent.GetQuesContent2(id);
            var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);
            //var QuesDetail2 = this._mgrQuesDetail.GetOneQuesDetail(ID);

            Guid userid = Guid.NewGuid();

            //個人資訊存入資料庫
            UserInfoModel user = new UserInfoModel()
            {
                QuestionnaireID = id,
                CreateDate = DateTime.Now,
                UserID = userid,
                Name = name,
                Phone = phone,
                Email = email,
                Age = age,
            };

            this._mgrUserInfo.CreateUserInfo(user);

            List<UserQuesDetailModel> answerList = GetSessionList("Answer");


            UserQuesDetailModel userAns = new UserQuesDetailModel()
            {
                QuestionnaireID = id,
                UserID = userid,
            };

            for (int i = 0; i < QuesDetail.Count; i++)
            {
                foreach (var q in answerList)
                {
                    if (q.QuesID == QuesDetail[i].QuesID)
                    {
                        userAns.QuesID = QuesDetail[i].QuesID;
                        userAns.Answer = q.Answer;
                        userAns.QuesTypeID = QuesDetail[i].QuesTypeID;

                        this._mgrUserDetail.CreateUserQuesDetail(userAns);
                    }
                }
            }

            //Response.Redirect($"listPage.aspx");
            Response.Redirect($"statisticPage.aspx?ID={ID}");
        }
    }
}