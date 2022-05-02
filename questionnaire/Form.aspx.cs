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
        #region Manager&變數

        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        int i = 1;
        bool isEdit = false;
        int ansCount = 0;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //如果沒有帶ID，跳回列表頁
            if (Request.QueryString["ID"] == null)
                Response.Redirect($"listPage.aspx");

            //判斷是不是編輯狀態
            bool checkEdit = Request.QueryString["Edit"] == "1";
            if (checkEdit)
                isEdit = true;

            bool _n = String.IsNullOrWhiteSpace(this.txtName.Text);
            bool _p = String.IsNullOrWhiteSpace(this.txtPhone.Text);
            bool _e = String.IsNullOrWhiteSpace(this.txtEmail.Text);
            bool _a = String.IsNullOrWhiteSpace(this.txtAge.Text);

            if (isEdit && _n && _p && _e && _a)
            {
                var name = GetSession("Name");
                var phone = GetSession("Phone");
                var email = GetSession("Email");
                var age = GetSession("Age");

                this.txtName.Text = name;
                this.txtPhone.Text = phone;
                this.txtEmail.Text = email;
                this.txtAge.Text = age;
            }

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

            //生成控制項
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

                //判斷種類類型
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
                    this.ltlVote.Text = "投票中";

                this.ltlTime.Text = $"{Ques.strStartTime} ~ {Ques.strEndTime}";

                string count = questionList.Count.ToString();
                this.ltlQCount.Text = "共 " + count + " 個問題";
            }

            //重製一下Session
            Session.Remove("Name");
            Session.Remove("Phone");
            Session.Remove("Email");
            Session.Remove("Age");
        }

        #region 建立問題

        //建立單選問題
        private void CreateRdb(QuesDetail q)
        {
            //不是編輯
            if (!isEdit)
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
            //編輯
            else
            {
                RadioButtonList radioButtonList = new RadioButtonList();
                radioButtonList.ID = "Q" + q.QuesID;
                this.plcDynamic.Controls.Add(radioButtonList);

                //取ID
                string ID = Request.QueryString["ID"];
                Guid id = new Guid(ID);
                List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
                List<UserQuesDetailModel> answerList = GetSessionList("Answer");

                string[] arrQ = q.QuesChoice.Split(';');

                for (int i = 0; i < arrQ.Length; i++)
                {
                    RadioButton item = new RadioButton();
                    item.Text = arrQ[i].ToString();
                    item.ID = q.QuesID + i.ToString();
                    item.GroupName = "group" + q.QuesID;

                    if (item.Text == answerList[ansCount].Answer.TrimEnd(';'))
                        item.Checked = true;

                    this.plcDynamic.Controls.Add(item);
                    this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
                }
                ansCount++;
            }
        }

        //建立複選問題
        private void CreateCkb(QuesDetail q)
        {
            //不是編輯
            if (!isEdit)
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
            //編輯
            else
            {
                //取ID
                string ID = Request.QueryString["ID"];
                Guid id = new Guid(ID);
                List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);
                List<UserQuesDetailModel> answerList = GetSessionList("Answer");

                string[] arrQ = q.QuesChoice.Split(';');

                for (int i = 0; i < arrQ.Length; i++)
                {
                    CheckBox item = new CheckBox();
                    item.Text = arrQ[i].ToString();
                    item.ID = q.QuesID + i.ToString();

                    var ans1 = answerList[ansCount].Answer.TrimEnd(';');
                    var ans2 = ans1.Split(';');

                    foreach (var item2 in ans2)
                    {
                        if (item.Text == item2)
                        {
                            item.Checked = true;
                        }
                    }

                    this.plcDynamic.Controls.Add(item);
                    this.plcDynamic.Controls.Add(new LiteralControl("&nbsp&nbsp&nbsp&nbsp&nbsp"));
                }
                ansCount++;
            }
        }

        //建立文字問題
        private void CreateTxt(QuesDetail q)
        {
            //不是編輯
            if (!isEdit)
            {
                TextBox textBox = new TextBox();
                textBox.ID = "Q" + q.QuesID.ToString();
                this.plcDynamic.Controls.Add(textBox);
            }
            //編輯
            else
            {
                List<UserQuesDetailModel> answerList = GetSessionList("Answer");

                foreach (var Q in answerList)
                {
                    if (Q.QuesID == answerList[ansCount].QuesID)
                    {
                        TextBox txt = new TextBox();
                        txt.ID = "Q" + Q.QuesID;
                        txt.Text = answerList[ansCount].Answer.TrimEnd(';');
                        this.plcDynamic.Controls.Add(txt);
                    }
                }
                ansCount++;
            }
        }

        #endregion

        #region 送出

        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region 個人資料防呆
            bool isNameRight = false;
            bool isPhoneRight = false;
            bool isEmailRight = false;
            bool isAgeRight = false;

            bool telCheck = Regex.IsMatch(this.txtPhone.Text.Trim(), @"^09[0-9]{8}$");

            bool gmail = Regex.IsMatch(this.txtEmail.Text.Trim(), @"@gmail.com$");
            bool hotmail = Regex.IsMatch(this.txtEmail.Text.Trim(), @"@hotmail.com$");
            bool yahoo = Regex.IsMatch(this.txtEmail.Text.Trim(), @"@yahoo.com$");
            bool emailCheck = false;

            if (gmail || hotmail || yahoo)
                emailCheck = true;

            if (!string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                isNameRight = true;
                this.lblName.Visible = false;
            }
            else
                this.lblName.Visible = true;

            if (!string.IsNullOrWhiteSpace(this.txtPhone.Text) && telCheck)
            {
                isPhoneRight = true;
                this.lblPhone1.Visible = false;
                this.lblPhone2.Visible = false;
            }
            else if (!telCheck)
                this.lblPhone2.Visible = true;
            else if (string.IsNullOrWhiteSpace(this.txtPhone.Text))
                this.lblPhone1.Visible = true;

            if (!string.IsNullOrWhiteSpace(this.txtEmail.Text) && emailCheck)
            {
                isEmailRight = true;
                this.lblEmail1.Visible = false;
                this.plcEmail.Visible = false;
            }
            else if (!emailCheck)
                this.plcEmail.Visible = true;
            else if (string.IsNullOrWhiteSpace(this.txtEmail.Text))
                this.lblEmail1.Visible = true;

            if (!string.IsNullOrWhiteSpace(this.txtAge.Text))
            {
                isAgeRight = true;
                this.lblAge.Visible = false;
            }
            else
                this.lblAge.Visible = true;

            //if (isNameRight && isPhoneRight && isEmailRight && isAgeRight)
            //{
            //    //this.Session["Name"] = this.txtName.Text;
            //    //this.Session["Phone"] = this.txtPhone.Text;
            //    //this.Session["Email"] = this.txtEmail.Text;
            //    //this.Session["Age"] = this.txtAge.Text;
            //}
            //else
            //{
            //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('查無資料。');", true);
            //}
            #endregion

            //取ID
            string ID2 = Request.QueryString["ID"];
            Guid ID3 = new Guid(ID2);

            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(ID3);
            bool ansCheck = true; ;

            #region 檢查有沒有填必選問題
            for (var i = 0; i < questionList.Count; i++)
            {
                ansCheck = false;

                var q = _mgrQuesDetail.GetOneQuesDetail(questionList[i].QuesID);
                if (questionList[i].Necessary == true)
                {
                    switch (questionList[i].QuesTypeID)
                    {
                        //單選
                        case 2:
                            for (int j = 0; j < q.QuesChoice.Split(';').Length; j++)
                            {
                                foreach (var rdb in q.QuesChoice.Split(';'))
                                {
                                    RadioButton ansRdb = this.plcDynamic.FindControl($"{questionList[i].QuesID}{j}") as RadioButton;
                                    if (ansRdb.Checked == true)
                                    {
                                        ansCheck = true;
                                        break;
                                    }
                                }

                            }
                            break;
                        //複選
                        case 3:
                            for (int j = 0; j < q.QuesChoice.Split(';').Length; j++)
                            {
                                foreach (var rdb in q.QuesChoice.Split(';'))
                                {
                                    CheckBox ansCdb = this.plcDynamic.FindControl($"{questionList[i].QuesID}{j}") as CheckBox;
                                    if (ansCdb.Checked == true)
                                    {
                                        ansCheck = true;
                                        break;
                                    }
                                }
                            }
                            break;
                        //文字
                        case 1:
                            foreach (var item in this.plcDynamic.Controls)
                            {
                                TextBox txtCdb = this.plcDynamic.FindControl($"Q{questionList[i].QuesID}") as TextBox;
                                if (!String.IsNullOrWhiteSpace(txtCdb.Text))
                                {
                                    ansCheck = true;
                                    break;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    ansCheck = true;
                }
            }
            #endregion

            if (ansCheck && isNameRight && isPhoneRight && isEmailRight && isAgeRight)
            {
                this.Session["Name"] = this.txtName.Text;
                this.Session["Phone"] = this.txtPhone.Text;
                this.Session["Email"] = this.txtEmail.Text;
                this.Session["Age"] = this.txtAge.Text;

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
                        //單選 //問題
                        case 2:
                            for (var j = -1; j < i; j++)
                            {
                                //判斷一下有沒有找到答案
                                int check = 0;

                                //把選項拆進陣列中
                                string[] arrQ = questionList[i].QuesChoice.Split(';');

                                for (var k = 0; k < arrQ.Length; k++)
                                {
                                    RadioButton rdb = (RadioButton)this.plcDynamic.FindControl($"{questionList[i].QuesID}{k}");

                                    //將答案寫入清單
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
                            //判斷一下有沒有找到答案
                            int check2 = 0;

                            //把選項拆進陣列中
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
                            if (txb.Text != null)
                            {
                                Ans.Answer = txb.Text + ";";
                                answerList.Add(Ans);
                                break;
                            }
                            else
                            {
                                Ans.Answer = " " + ";";
                                answerList.Add(Ans);
                                break;
                            }

                    }
                }

                //寫進Session
                Session["Answer"] = answerList;

                //取ID
                string ID = Request.QueryString["ID"];
                Guid id = new Guid(ID);

                Response.Redirect($"checkPage.aspx?ID={ID}");
            }
            else
            {
                this.lblMsg.Visible = true;
            }
        }

        #endregion

        #region Session

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

        #endregion

        #region 取消

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("listPage.aspx");
        }

        #endregion

        protected void txt_TextChanged(object sender, EventArgs e)
        {
            if (isEdit)
            {
                Session.Remove("Name");
                Session.Remove("Phone");
                Session.Remove("Email");
                Session.Remove("Age");
            }
        }
    }
}