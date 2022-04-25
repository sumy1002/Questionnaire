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
                        //item.Answer = item.Answer.Split(';');
                        //this._mgrSta.CreateStatistic(item);
                    }

                    if (total == 0)
                    {
                        Literal ltlNoAns = new Literal();
                        ltlNoAns.Text = "尚無資料<br/>";
                        this.plcDynamic.Controls.Add(ltlNoAns);
                    }
                    else
                    {
                        string[] arrQue = question.QuesChoice.Split(';');
                        //string[] arrAns = NoList.
                        for (int i = 0; i < arrQue.Length; i++)
                        {
                            int ansCount = 0;
                            string ans = arrQue[i].ToString();
                            foreach (var s in NoList)
                            {
                                var ansTrim = s.Answer.TrimEnd(';');
                                string[] sAns = ansTrim.Split(';');
                                foreach(string sans in sAns)
                                {
                                    if(sans == ans)
                                        ansCount++;
                                }
                            }

                            StatisticModel stastic = NoList.Find(x => x.Answer == arrQue[i].ToString());

                            Literal ltlSelection = new Literal();
                            ltlSelection.Text = $"{arrQue[i]} : {ansCount * 100 / total}% ({ansCount})";
                            this.plcDynamic.Controls.Add(ltlSelection);

                            HtmlGenericControl outterDiv = new HtmlGenericControl("div");
                            outterDiv.Style.Value = "width:100%;height:20px;border:1px solid black;";
                            this.plcDynamic.Controls.Add(outterDiv);
                            HtmlGenericControl innerDiv = new HtmlGenericControl("div");
                            innerDiv.Style.Value = $"width:{ansCount * 100 / total}%;height:20px;background-color:gray;color:white;font-//weight:bold;";
                            outterDiv.Controls.Add(innerDiv);
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