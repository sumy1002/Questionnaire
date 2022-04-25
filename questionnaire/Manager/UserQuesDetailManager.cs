using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class UserQuesDetailManager
    {
        private UserInfoManager _mgrUInfo = new UserInfoManager();
        private AccountManager _mgrAcc = new AccountManager();

        ///// <summary>
        ///// 取得所有或附加查詢條件的問卷回答，及其所有資料
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public List<UserQuesDetail> GetUserInfoList(int userID)
        //{
        //    try
        //    {
        //        using (ContextModel contextModel = new ContextModel())
        //        {
        //            var id = userID.ToString();

        //            //取得所有或加查詢條件的帳戶
        //            IQueryable<UserQuesDetail> query;
        //            if (!string.IsNullOrWhiteSpace(id))
        //            {
        //                query =
        //                    from item in contextModel.UserQuesDetails
        //                    where item.UserID == userID
        //                    select item;
        //            }
        //            else
        //            {
        //                query =
        //                    from item in contextModel.UserQuesDetails
        //                    select item;
        //            }

        //            //組合，並取回結果
        //            var list = query.ToList();
        //            return list;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("UserQuesDetailManager.GetUserInfoList", ex);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 取得指定UserID的問卷回答，及其所有資料 一筆
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserQuesDetail GetUserInfo(Guid userID)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var id = userID.ToString();

                    //取得所有或加查詢條件的帳戶
                    var query =
                           from item in contextModel.UserQuesDetails
                           where item.UserID == userID
                           select item;

                    //取得Account所有資料
                    var userInfo = query.FirstOrDefault();

                    //檢查是否存在
                    if (userInfo != null)
                        return userInfo;

                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserInfoManager.GetUserInfoList", ex);
                throw;
            }
        }



        /// <summary>
        /// 新增新的問卷回答
        /// </summary>
        /// <param name="member"></param>
        public void CreateUserQuesDetail(UserQuesDetailModel member)
        {
            try
            {
                //新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //建立要新增的帳戶資料
                    var newInfo = new UserQuesDetail()
                    {
                        AnsID = member.AnsID,
                        UserID = member.UserID,
                        QuestionnaireID = member.QuestionnaireID,
                        QuesID = member.QuesID,
                        Answer = member.Answer,
                        AccountID = member.AccountID,
                    };

                    //int i = Convert.ToInt32(string.Empty);
                    //var list = this.GetUserInfoList(i);
                    //foreach (UserInfo item in list)
                    //{
                    //    if (!(newInfo.AccountID == item.AccountID &&
                    //        newInfo.QuestionnaireID == item.QuestionnaireID))
                    //    {
                    //        //將新資料插入EF的集合中
                    //        contextModel.UserInfos.Add(newInfo);
                    //    }
                    //}

                    //將新資料插入EF的集合中
                    contextModel.UserQuesDetails.Add(newInfo);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserQuesDetailManager.CreateUserQuesDetail", ex);
                throw;
            }
        }

    }
}