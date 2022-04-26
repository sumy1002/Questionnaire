using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class UserInfoManager
    {
        /// <summary>
        /// 取得所有或附加查詢條件的UserInfo，及其所有資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoList(Guid Qid)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var id = Qid.ToString();

                    //取得所有或加查詢條件的帳戶
                    IQueryable<UserInfo> query;
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        query =
                            from item in contextModel.UserInfos
                            where item.QuestionnaireID == Qid
                            select item;
                    }
                    else
                    {
                        query =
                            from item in contextModel.UserInfos
                            select item;
                    }

                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserInfoManager.GetUserInfoList", ex);
                throw;
            }
        }


        /// <summary>
        /// 取得所有或附加查詢條件的UserInfo，及其所有資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfoList2(Guid Userid)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var id = Userid.ToString();

                    //取得所有或加查詢條件的帳戶
                    IQueryable<UserInfo> query;
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        query =
                            from item in contextModel.UserInfos
                            where item.UserID == Userid
                            select item;
                    }
                    else
                    {
                        query =
                            from item in contextModel.UserInfos
                            select item;
                    }

                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserInfoManager.GetUserInfoList", ex);
                throw;
            }
        }

        /// <summary>
        /// 取得指定帳戶的UserInfo列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<UserInfo> GetAccountInfoList(Guid id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得所有或加查詢條件的帳戶
                    var query =
                          from item in contextModel.UserInfos
                          where item.AccountID == id
                          select item;

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
        /// 新增UserInfo
        /// </summary>
        /// <param name="member"></param>
        public void CreateUserInfo(UserInfoModel member)
        {
            try
            {
                //新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //建立要新增的帳戶資料
                    UserInfo newInfo = new UserInfo()
                    {
                        UserID = member.UserID,
                        AccountID = member.AccountID,
                        QuestionnaireID = member.QuestionnaireID,
                        CreateDate = DateTime.Now,
                        Name = member.Name,
                        Phone = member.Phone,
                        Age = member.Age,
                        Email = member.Email,
                    };

                    //將新資料插入EF的集合中
                    contextModel.UserInfos.Add(newInfo);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserInfoManager.CreateUserInfo", ex);
                throw;
            }
        }
    }
}