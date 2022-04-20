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
        //private List<QuesAndTypeModel> _quesList = new List<QuesAndTypeModel>();

        //public List<QuesAndTypeModel> GetQuesList(QuesAndTypeModel ques)
        //{
        //    _quesList.Add(ques);

        //    return _quesList;
        //}

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
                        QuestionnaireID = ques.QuestionnaireID,
                        QuesID = ques.QuesID,
                        //TitleID = ques.TitleID,
                        QuesTitle = ques.QuesTitle,
                        QuesTypeID = ques.QuesTypeID,
                        Necessary = ques.Necessary
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
                    var query = contextModel.QuesDetails.Where(item => item.QuestionnaireID == ques.QuestionnaireID);

                    //取得資料
                    var updateQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (updateQues != null)
                    {
                        updateQues.QuesID = ques.QuesID;
                        updateQues.QuestionnaireID = ques.QuestionnaireID;
                        updateQues.TitleID = ques.TitleID;
                        updateQues.QuesTitle = ques.QuesTitle;
                        updateQues.QuesTypeID = ques.QuesTypeID;
                        updateQues.Necessary = ques.Necessary;
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
        public void DeleteQuesDetail(Guid id)
        {
            try
            {
                //刪除資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.QuesDetails.Where(item => item.QuestionnaireID == id);

                    //取得資料
                    var deleteQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (deleteQues != null)
                    {
                        deleteQues.Necessary = false;
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

        /// <summary>
        /// 輸入字串，做字串切割，輸出成列表
        /// </summary>
        /// <param name="ques"></param>
        /// <returns></returns>
        public List<QuesAndTypeModel> GetQuesList(string ques)
        {
            ques = ques.TrimEnd('$');
            string[] Q = ques.Split('$');

            List<QuesAndTypeModel> quesList = new List<QuesAndTypeModel>();
            foreach (string item in Q)
            {
                string[] QD = item.Split('&');

                QuesAndTypeModel Ques = new QuesAndTypeModel();
                Ques.QuesTitle = QD[0];
                Ques.QuesChoice = QD[1];
                Ques.QuesTypeID = Convert.ToInt32(QD[2]);
                Ques.QuesType1 = QD[3];
                Ques.Necessary = Convert.ToBoolean(QD[4]);

                quesList.Add(Ques);
            }
            return quesList;
        }
    }
}