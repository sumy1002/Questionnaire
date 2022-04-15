using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class AccountManager
    {
        public bool TryLogin(string account, string password)
        {
            bool isAccountRight = false;
            bool isPasswordRight = false;

            Account member = this.GetAccount(account);

            if (member == null) // 找不到就代表登入失敗
                return false;

            if (string.Compare(member.Account1, account, true) == 0)
                isAccountRight = true;

            if (member.PWD == password)
                isPasswordRight = true;

            // 檢查帳號密碼是否正確
            bool result = (isAccountRight && isPasswordRight);

            // 帳密正確：把值寫入 Session
            // 為避免任何漏洞導致 session 流出，先把密碼清除
            if (result)
            {
                member.PWD = null;
                HttpContext.Current.Session["MemberAccount"] = member;
            }

            return result;
        }

        /// <summary>
        /// 取得目前使用者是否登入
        /// </summary>
        /// <returns></returns>
        public bool IsLogined()
        {
            Account account = GetCurrentUser();
            return (account != null);
        }

        /// <summary>
        /// 取得目前使用者
        /// </summary>
        /// <returns></returns>
        public Account GetCurrentUser()
        {
            Account account = HttpContext.Current.Session["MemberAccount"] as Account;
            return account;
        }

        public void Logout()
        {
            HttpContext.Current.Session.Remove("MemberAccount");
        }

        #region "增刪修查"
        /// <summary>
        /// 取得所有或附加查詢條件的帳戶，及其所有資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Account> GetAccountList(string keyword)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得所有或加查詢條件的帳戶
                    IQueryable<Account> query;
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query =
                            from item in contextModel.Accounts
                            where item.Account1.Contains(keyword)
                            select item;
                    }
                    else
                    {
                        query =
                            from item in contextModel.Accounts
                            select item;
                    }

                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccountList", ex);
                throw;
            }
        }

        /// <summary>
        /// 取得帳戶資料(ID,Account,PWD,UserLevel,IsEnable,CreateDate)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Account GetAccount(string account)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var query =
                        from item in contextModel.Accounts
                        where item.Account1 == account
                        select item;

                    //取得Account所有資料
                    var memberAccount = query.FirstOrDefault();

                    //檢查是否存在
                    if (memberAccount != null)
                        return memberAccount;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccount", ex);
                throw;
            }
        }

        /// <summary>
        /// 輸入ID取得帳戶所有資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Account GetAccount(Guid id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var query =
                        from item in contextModel.Accounts
                        where item.AccountID == id
                        select item;

                    //取得Account所有資料
                    var memberAccount = query.FirstOrDefault();

                    //檢查是否存在
                    if (memberAccount != null)
                        return memberAccount;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.GetAccount", ex);
                throw;
            }
        }

        /// <summary>
        /// 新增帳戶
        /// </summary>
        /// <param name="member"></param>
        public void CreateAccount(AccountModel member)
        {
            // 1. 判斷資料庫是否有相同的 Account
            if (this.GetAccount(member.Account) != null)
                throw new Exception("已存在相同的帳號");

            try
            {
                // 2. 新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    member.AccountID = Guid.NewGuid();

                    //建立要新增的帳戶資料
                    var newAccount = new Account()
                    {
                        AccountID = member.AccountID,
                        Account1 = member.Account,
                        PWD = member.PWD,
                        UserLevel = member.UserLevel,
                        IsEnable = member.IsEnable,
                        CreateDate = DateTime.Now
                    };

                    //將新資料插入EF的集合中
                    contextModel.Accounts.Add(newAccount);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.CreateAccount", ex);
                throw;
            }
        }

        /// <summary>
        /// 修改帳戶
        /// </summary>
        /// <param name="member"></param>
        public void UpdateAccount(AccountModel member)
        {
            // 1. 判斷資料庫是否有相同的 Account
            if (this.GetAccount(member.Account) == null)
                throw new Exception("帳號不存在：" + member.Account);

            try
            {
                // 2. 編輯資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.Accounts.Where(item => item.AccountID == member.AccountID);

                    //取得資料
                    var memberAccount = query.FirstOrDefault();

                    //檢查是否存在
                    if (memberAccount != null)
                        memberAccount.Account1 = member.Account;
                    else
                        throw new Exception("此帳號不存在");

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.UpdateAccount", ex);
                throw;
            }
        }

        /// <summary>
        /// 將帳戶從資料庫移除
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAccount(Guid id)
        {
            // 1. 判斷是否有傳入 id
            if (id == null)
                throw new Exception("需指定 id");

            try
            {
                // 2. 刪除資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.Accounts.Where(item => item.AccountID == id);

                    //取得資料
                    var deleteAccount = query.FirstOrDefault();

                    //檢查是否存在
                    if (deleteAccount != null)
                    {
                        deleteAccount.IsEnable = false;
                    }
                    //contextModel.Accounts.Remove(deleteAccount);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountManager.DeleteAccount", ex);
                throw;
            }
        }
        #endregion
}
}