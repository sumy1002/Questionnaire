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
        /// 新增帳戶
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
                        TitleID = member.TitleID,
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