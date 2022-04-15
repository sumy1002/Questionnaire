using questionnaire.Helpers;
using questionnaire.Models;
using questionnaire.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Managers
{
    public class QuesDetailManager
    {
        #region "增刪修"
        /// <summary>
        /// 新增問題
        /// </summary>
        /// <param name="ques"></param>
        public void CreateQuesDetail(QuesDetailModel ques)
        {
            try
            {
                //新增資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //建立新問卷
                    var newQuesDetail = new QuesDetail
                    {
                        QuesID = ques.QuesID,
                        TitleID = ques.TitleID,
                        QuesTitle = ques.QuesTitle,
                        QuesTypeID = ques.QuesTypeID,
                        IsEnable = ques.IsEnable
                    };

                    //將新資料插入EF的集合中
                    contextModel.QuesDetails.Add(newQuesDetail);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.CreateQuesDetail", ex);
                throw;
            }
        }

        /// <summary>
        /// 修改問題
        /// </summary>
        /// <param name="ques"></param>
        public void UpdateQuesDetail(QuesDetailModel ques)
        {
            try
            {
                //編輯資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.QuesDetails.Where(item => item.TitleID == ques.TitleID);

                    //取得資料
                    var updateQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (updateQues != null)
                    {
                        updateQues.QuesID = ques.QuesID;
                        updateQues.TitleID = ques.TitleID;
                        updateQues.QuesTitle = ques.QuesTitle;
                        updateQues.QuesTypeID = ques.QuesTypeID;
                        updateQues.IsEnable = ques.IsEnable;
                    }
                    else
                        throw new Exception("此問卷不存在");

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.UpdateQuesDetail", ex);
                throw;
            }
        }

        /// <summary>
        /// 刪除問題
        /// </summary>
        /// <param name="id"></param>
        public void DeleteQuesDetail(int id)
        {
            try
            {
                //刪除資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.QuesDetails.Where(item => item.TitleID == id);

                    //取得資料
                    var deleteQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (deleteQues != null)
                    {
                        deleteQues.IsEnable = false;
                    }
                    //contextModel.Contents.Remove(deleteQues);

                    //確定存檔
                    contextModel.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.DeleteQuesDetail", ex);
                throw;
            }
        }
        #endregion
    }
}