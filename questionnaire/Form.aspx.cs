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
        //private static List<UserQuesDetailModel> _answerList;
        int i = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            //尋找該ID的問卷及問題列表
            var Ques = this._mgrContent.GetQuesContent2(id);
            var QuesDetail = this._mgrQuesDetail.GetQuesDetailList(id);

            //過濾問卷狀態
            if (Ques.StartDate > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('問卷尚未開始。');location.href='listPage.aspx';", true);
            }
            else if (Ques.EndDate < DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('問卷已截止。');location.href='listPage.aspx';", true);
            }

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
                if (this.ltlVote.Text == "True")
                    this.ltlVote.Text = "開放中";

                this.lvlTime.Text = $"{Ques.strStartTime} ~ {Ques.strEndTime}";

                string count = questionList.Count.ToString();
                this.ltlQCount.Text = "共 " + count + " 個問題";
            }
        }

        //建立單選問題
        private void CreateRdb(QuesDetail q)
        {
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "Q" + q.QuesID;
            this.plcDynamic.Controls.Add(radioButtonList);

            string[] arrQ = q.QuesChoice.Split(';');

            for (int i = 0; i < arrQ.Length; i++)
            {
                RadioButton item = new RadioButton();
                item.Text = arrQ[i].ToString();
                item.ID = q.QuesID + i.ToString();
                item.GroupName = "group" + q.QuesID;
                this.plcDynamic.Controls.Add(item);
                this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
            }
            this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
        }

        //建立複選問題
        private void CreateCkb(QuesDetail q)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "Q" + q.QuesID;
            this.plcDynamic.Controls.Add(checkBoxList);

            string[] arrQ = q.QuesChoice.Split(';');

            for (int i = 0; i < arrQ.Length; i++)
            {
                CheckBox item = new CheckBox();
                item.Text = arrQ[i].ToString();
                item.ID = q.QuesID + i.ToString();
                this.plcDynamic.Controls.Add(item);
                this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
            }
        }

        //建立文字問題
        private void CreateTxt(QuesDetail q)
        {
            TextBox textBox = new TextBox();
            textBox.ID = "Q" + q.QuesID.ToString();
            this.plcDynamic.Controls.Add(textBox);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("listPage.aspx");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region 個人資料防呆
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
            }
            else
            {

            }
            #endregion

            //取ID
            string ID2 = Request.QueryString["ID"];
            Guid ID3 = new Guid(ID2);

            var QID = this._mgrQuesDetail.GetQuesDetailList(ID3);
            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(ID3);

            //取得動態控制項的值
            List<UserQuesDetailModel> answerList = new List<UserQuesDetailModel>();
            for (var i = 0; i < questionList.Count; i++)
            {
                var q = _mgrQuesDetail.GetOneQuesDetail(questionList[i].QuesID);
                UserQuesDetailModel Ans = new UserQuesDetailModel()
                {
                    QuestionnaireID = q.QuestionnaireID,
                    QuesID = q.QuesID,
                    QuesTypeID = q.QuesTypeID,
                };

                //判斷一下問題種類
                switch (questionList[i].QuesTypeID)
                {
                    //單選 //有問題
                    case 2:
                        for (var j = -1; j < i; j++)
                        {
                            int check = 0;
                            string[] arrQ = questionList[i].QuesChoice.Split(';');
                            for (var k = 0; k < arrQ.Length; k++)
                            {
                                RadioButton rdb = (RadioButton)this.plcDynamic.FindControl($"{questionList[i].QuesID}{k}");
                                if (rdb.Checked == true)
                                {
                                    Ans.Answer = rdb.Text + ";";
                                    answerList.Add(Ans);
                                    check = 1;
                                    break;
                                }
                            }
                            if (check == 1)
                                break;
                        }
                        break;
                    //複選
                    case 3:
                        int check2 = 0;
                        CheckBoxList ckblist = (CheckBoxList)this.plcDynamic.FindControl($"Q{questionList[i].QuesID}");
                        string[] arrQ2 = questionList[i].QuesChoice.Split(';');
                        for (var j = 0; j < arrQ2.Length; j++)
                        {
                            for (var k = 0; k < arrQ2.Length; k++)
                            {
                                CheckBox ckb = (CheckBox)this.plcDynamic.FindControl($"{questionList[i].QuesID}{k}");
                                if (ckb.Checked == true)
                                {
                                    Ans.Answer += ckb.Text + ";";
                                    check2++;
                                }
                                else if (ckb.Checked == false)
                                    check2++;

                                if (check2 == arrQ2.Length)
                                {
                                    answerList.Add(Ans);
                                    break;
                                }
                            }
                            break;
                        }
                        break;
                    //文字
                    case 1:
                        TextBox txb = (TextBox)this.plcDynamic.FindControl($"Q{questionList[i].QuesID}");
                        Ans.Answer = txb.Text + ";";
                        answerList.Add(Ans);
                        break;
                }
            }

            //寫進Session
            Session["Answer"] = answerList;

            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            Response.Redirect($"checkPage.aspx?ID={ID}");
        }
    }
}