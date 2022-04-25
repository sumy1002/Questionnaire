using questionnaire.Manager;
using questionnaire.Managers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace questionnaire
{
    public partial class statisticPage : System.Web.UI.Page
    {
        private QuesContentsManager _mgrContent = new QuesContentsManager();
        private QuesDetailManager _mgrQuesDetail = new QuesDetailManager();
        private UserQuesDetailManager _mgrUQDetail = new UserQuesDetailManager();
        private StatisticManager _mgrSta = new StatisticManager();
        private string[] arrAns;
        //private static Guid _questionnaireID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            this._mgrSta.GetStasticList(id);

            //if (Guid.TryParse(IDstring, out _questionnaireID))
            //{
            //取得問卷資訊
            ORM.Content questionnaire = _mgrContent.GetQuesContent(id);
            this.ltlTitle.Text = questionnaire.Title;
            this.ltlContent.Text = questionnaire.Body;

            //過濾狀態
            if (questionnaire.StartDate > DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('問卷尚未開始。');location.href='listPage.aspx';", true);
            }
            else if (questionnaire.EndDate < DateTime.Now)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('問卷已截止。');location.href='listPage.aspx';", true);
            }

            //取得該問卷的問題細節
            List<QuesDetail> questionList = _mgrQuesDetail.GetQuesDetailList(id);

            //List<UserQuesDetail> UserAns = _mgrUQDetail.GetUserInfo();
            List<StatisticModel> staList = _mgrSta.GetStasticList(id);

            foreach (QuesDetail question in questionList)
            {
                string q = $"<br/>{question.QuesID}";
                if (question.Necessary)
                    q += "(*必填)";
                Literal ltlQuestion = new Literal();
                ltlQuestion.Text = q + "<br/>";
                this.plcDynamic.Controls.Add(ltlQuestion);

                if (question.QuesTypeID != 1)
                {
                    List<StatisticModel> NoList = staList.FindAll(x => x.QuesID == question.QuesID);
                    int total = 0;


                    foreach (StatisticModel item in NoList)
                    {
                        item.AnsCount = NoList.Count;
                        total = item.AnsCount;
                    }

                    if (total == 0)
                    {
                        Literal ltlNoAns = new Literal();
                        ltlNoAns.Text = "尚無資料<br/>";
                        this.plcDynamic.Controls.Add(ltlNoAns);
                    }
                    else
                    {
                        //取問題的答案
                        string[] arrQue = question.QuesChoice.Split(';');
                        int ckbAns = 0;
                        for (int i = 0; i < arrQue.Length; i++)
                        {
                            ckbAns = 0;
                            int ansCount = 0;
                            string ans = arrQue[i].ToString();
                            foreach (var s in NoList)
                            {
                                var ansTrim = s.Answer.TrimEnd(';');
                                string[] sAns = ansTrim.Split(';');
                                ckbAns += sAns.Length; //問題
                                foreach (string sans in sAns)
                                {
                                    if (sans == ans)
                                    {
                                        ansCount++;
                                    }
                                }
                            }

                            StatisticModel stastic = NoList.Find(x => x.Answer == arrQue[i].ToString());

                            switch (question.QuesTypeID)
                            {
                                case 2:
                                    Literal ltlSelection = new Literal();
                                    ltlSelection.Text = $"{arrQue[i]} : {ansCount * 100 / total}% ({ansCount})";
                                    this.plcDynamic.Controls.Add(ltlSelection);
                                    break;
                                case 3:
                                    double aaa = Convert.ToInt32(((double)ansCount / ckbAns) * 100);
                                    Literal ltlSelection2 = new Literal();
                                    ltlSelection2.Text = $"{arrQue[i]} : {aaa}% ({ansCount})";
                                    this.plcDynamic.Controls.Add(ltlSelection2);
                                    break;
                            }

                            HtmlGenericControl outsideDiv = new HtmlGenericControl("div");
                            outsideDiv.Style.Value = "width:100%;height:20px;border:1px solid black;";
                            this.plcDynamic.Controls.Add(outsideDiv);
                            HtmlGenericControl colorDiv = new HtmlGenericControl("div");
                            colorDiv.Style.Value = $"width:{ansCount * 100 / ckbAns}%;height:18px;background-color:cadetblue;color:white;font-//weight:bold;";
                            outsideDiv.Controls.Add(colorDiv);
                        }
                    }
                }
                else
                {
                    Literal ltlSelection = new Literal();
                    ltlSelection.Text = "-<br/>";
                    this.plcDynamic.Controls.Add(ltlSelection);
                }
            }
            //}
            //else
            //Response.Redirect("List.aspx");
        }
    }
}