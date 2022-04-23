using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class AccountCheckManager
    {
        /// <summary>
        /// 取得所有或附加查詢條件的帳戶的填寫紀錄，及其所有資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<AccountCheck> GetAccountCheckList(Guid id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    string strid = id.ToString();

                    //取得所有或加查詢條件的帳戶
                    IQueryable<AccountCheck> query;
                    if (!string.IsNullOrWhiteSpace(strid))
                    {
                        query =
                            from item in contextModel.AccountChecks
                            where item.AccountID == id
                            select item;
                    }
                    else
                    {
                        query =
                            from item in contextModel.AccountChecks
                            select item;
                    }

                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountCheckManager.GetAccountCheckList", ex);
                throw;
            }
        }

        /// <summary>
        /// 新增Check
        /// </summary>
        /// <param name="member"></param>
        public void CreateCheck(AccountCheckModel member)
        {
            try
            {
                //新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //建立要新增的帳戶資料
                    var newAccountCheck = new AccountCheck()
                    {
                        CheckID = member.CheckID,
                        AccountID = member.AccountID,
                        QuestionnaireID = member.QuestionnaireID,
                        Checks = member.Checks,
                    };

                    //將新資料插入EF的集合中
                    contextModel.AccountChecks.Add(newAccountCheck);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountCheckManager.CreateCheck", ex);
                throw;
            }
        }
    }
}