using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Manager
{
    public class StatisticManager
    {
        public List<StatisticModel> GetStasticList(Guid id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得所有或加查詢條件的帳戶
                    var query =
                          from item in contextModel.UserQuesDetails
                          where item.QuestionnaireID == id
                          select new StatisticModel
                          {
                              QuestionnaireID = item.QuestionnaireID,
                              QuesID = item.QuesID,
                              Answer = item.Answer,
                          };

                    //組合，並取回結果
                    var list = query.ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserInfoManager.GetAccountInfoList", ex);
                throw;
            }
        }

        /// <summary>
        /// 新增Statistic
        /// </summary>
        /// <param name="member"></param>
        public void CreateStatistic(StatisticModel statistic)
        {
            try
            {
                //新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //建立要新增的帳戶資料
                    Statistic newSta = new Statistic()
                    {
                        QuestionnaireID = statistic.QuestionnaireID,
                        QuesID = statistic.QuesID,
                        Answer = statistic.Answer,
                    };
        
                    //將新資料插入EF的集合中
                    contextModel.Statistics.Add(newSta);
        
                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("StatisticManager.CreateStatistic", ex);
                throw;
            }
        }
    }
}