using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class QuesTypeManager
    {
        /// <summary>
        /// 取得所有或附加查詢條件的問卷，及其所有資料
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<QuesType> GetQuesTypesList()
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得所有或加查詢條件的問卷
                    IQueryable<QuesType> query;

                    query =
                        from item in contextModel.QuesTypes
                        select item;


                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesTypeManager.GetQuesTypesList", ex);
                throw;
            }
        }

        /// <summary>
        /// 輸入ID取得常用問題
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CQAndTypeModel GetCQType(int id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var query =
                        from item in contextModel.CQs
                        join item2 in contextModel.QuesTypes
                        on item.QuesTypeID equals item2.QuesTypeID
                        where item.CQID == id
                        select new CQAndTypeModel
                        {
                            CQID = item.CQID,
                            CQTitle = item.CQTitle,
                            CQChoice = item.CQChoice,
                            QuesTypeID = item2.QuesTypeID,
                            QuesType1 = item2.QuesType1,
                            Necessary = item.Necessary
                        };

                    var memberInfo = query.FirstOrDefault();

                    //檢查是否存在
                    if (memberInfo != null)
                        return memberInfo;

                    return null;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("ReservationManager.UpdateMember", ex);
                throw;
            }

        }
    }
}