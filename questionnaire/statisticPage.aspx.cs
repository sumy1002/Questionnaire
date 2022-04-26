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

        protected void Page_Load(object sender, EventArgs e)
        {
            //取ID
            string ID = Request.QueryString["ID"];
            Guid id = new Guid(ID);

            this._mgrSta.GetStasticList(id);

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
                string q = $"<br/>{question.QuesTitle}";
                Label lblNess = new Label();

                //必填的部分
                if (question.Necessary)
                {
                    lblNess.Text = "(*必填)<br />";
                    lblNess.ForeColor= System.Drawing.Color.Red;
                    lblNess.Style.Value = "font-size:10px;";
                }
                Label lblQues = new Label();
                lblQues.Text = q;
                lblQues.Style.Value = "font-weight:bold";
                this.plcDynamic.Controls.Add(lblQues);
                this.plcDynamic.Controls.Add(lblNess);


                //複選&單選
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
                                ckbAns += sAns.Length;
                                foreach (string sans in sAns)
                                {
                                    if (sans == ans)
                                    {
                                        ansCount++;
                                    }
                                }
                            }

                            //篩選一下問題種類
                            switch (question.QuesTypeID)
                            {
                                //單選
                                case 2:
                                    Literal ltlAns = new Literal();
                                    ltlAns.Text = $"{arrQue[i]} : {ansCount * 100 / total}% ({ansCount})";
                                    this.plcDynamic.Controls.Add(ltlAns);
                                    break;
                                //複選
                                case 3:
                                    double aaa = Convert.ToInt32(((double)ansCount / ckbAns) * 100);
                                    Literal ltlAns2 = new Literal();
                                    ltlAns2.Text = $"{arrQue[i]} : {aaa}% ({ansCount})";
                                    this.plcDynamic.Controls.Add(ltlAns2);
                                    break;
                            }

                            //長條圖外圈
                            HtmlGenericControl whiteDiv = new HtmlGenericControl("div");
                            whiteDiv.Style.Value = "border:1px solid black;width:100%;height:20px;";
                            this.plcDynamic.Controls.Add(whiteDiv);

                            //長條圖內圈
                            HtmlGenericControl colorDiv = new HtmlGenericControl("div");
                            colorDiv.Style.Value = $"width:{ansCount * 100 / ckbAns}%;height:18px;background-color:cadetblue;";
                            whiteDiv.Controls.Add(colorDiv);
                        }
                    }
                }
                //文字選項
                else
                {
                    Literal ltlSelection = new Literal();
                    ltlSelection.Text = "<br />無資料，回答為文字輸入<br/>";
                    this.plcDynamic.Controls.Add(ltlSelection);
                }
            }
            //}
            //else
            //Response.Redirect("List.aspx");
        }
    }
}