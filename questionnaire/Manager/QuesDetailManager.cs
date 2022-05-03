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
        /// <summary>
        /// 輸入問卷id取得問題清單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesDetail> GetQuesDetailList(Guid id)
        {
            string idText = id.ToString();

            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得加查詢條件的問題
                    IQueryable<QuesDetail> query;
                    if (!string.IsNullOrWhiteSpace(idText))
                    {
                        query =
                        from item in contextModel.QuesDetails
                        where item.QuestionnaireID == id
                        select item;
                    }
                    else
                    {
                        query =
                            from item in contextModel.QuesDetails
                            select item;
                    }

                    //組合，並取回結果
                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.GetQuesDetailList", ex);
                throw;
            }
        }

        /// <summary>
        /// 輸入問題id取得該筆問題
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuesDetail GetOneQuesDetail(int id)
        {
            string idText = id.ToString();

            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得加查詢條件的問題
                    var query =
                    from item in contextModel.QuesDetails
                    where item.QuesID == id
                    select item;

                    var quesDetail = query.FirstOrDefault();

                    if (quesDetail != null)
                        return quesDetail;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.GetOneQuesDetail", ex);
                throw;
            }
        }

        /// <summary>
        /// 輸入問題title取得該筆問題
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuesDetail GetTitleQuesDetail(string title)
        {
            string titleText = title.ToString();

            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    //取得加查詢條件的問題
                    var query =
                    from item in contextModel.QuesDetails
                    where item.QuesTitle == title
                    select item;

                    var quesDetail = query.FirstOrDefault();

                    if (quesDetail != null)
                        return quesDetail;

                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("QuesDetailManager.GetTitleQuesDetail", ex);
                throw;
            }
        }

        /// <summary>
        /// 輸入GUID取得問題及問題種類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesAndTypeModel> GetQuesDetailAndTypeList(Guid id)
        {
            try
            {
                using (ContextModel contextModel = new ContextModel())
                {
                    var query =
                        from item in contextModel.QuesDetails
                        join item2 in contextModel.QuesTypes
                        on item.QuesTypeID equals item2.QuesTypeID
                        where item.QuestionnaireID == id
                        select new QuesAndTypeModel
                        {
                            QuesID = item.QuesID,
                            QuesTitle = item.QuesTitle,
                            QuesChoice = item.QuesChoice,
                            QuesTypeID = item2.QuesTypeID,
                            QuesType1 = item2.QuesType1,
                            Necessary = item.Necessary
                        };

                    ////組合，並取回結果
                    //var list = query.ToList();
                    //return list;

                    //組合，並取回結果
                    var list = query.ToList();
                    var Qlist = new List<QuesAndTypeModel>();
                    foreach (var item in list)
                    {
                        var Q = new QuesAndTypeModel()
                        {
                            QuesID = item.QuesID,
                            QuesTitle = item.QuesTitle,
                            QuesChoice = item.QuesChoice,
                            QuesTypeID = item.QuesTypeID,
                            QuesType1 = item.QuesType1,
                            Necessary = item.Necessary
                        };
                        Qlist.Add(Q);
                    }
                    return Qlist;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("ReservationManager.UpdateMember", ex);
                throw;
            }

        }

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
                        QuesChoice = ques.QuesChoice,
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
                    var query = contextModel.QuesDetails.Where(item => item.QuesID == ques.QuesID);

                    //取得資料
                    var updateQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (updateQues != null)
                    {
                        updateQues.QuesID = ques.QuesID;
                        updateQues.QuestionnaireID = ques.QuestionnaireID;
                        updateQues.TitleID = ques.TitleID;
                        updateQues.QuesTitle = ques.QuesTitle;
                        updateQues.QuesChoice = ques.QuesChoice;
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
        public void DeleteQuesDetail(int id)
        {
            try
            {
                //刪除資料
                using (ContextModel contextModel = new ContextModel())
                {
                    //組查詢條件
                    var query = contextModel.QuesDetails.Where(item => item.QuesID == id);

                    //取得資料
                    var deleteQues = query.FirstOrDefault();

                    //檢查是否存在
                    if (deleteQues != null)
                    {
                        contextModel.QuesDetails.Remove(deleteQues);
                    }

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

        /// <summary>
        /// 輸入字串，做字串切割，輸出成列表
        /// </summary>
        /// <param name="ques"></param>
        /// <returns></returns>
        public QuesAndTypeModel GetOneQues(string ques)
        {
            ques = ques.TrimEnd('$');
            string[] Q = ques.Split('$');

            QuesAndTypeModel quesList = new QuesAndTypeModel();
            foreach (string item in Q)
            {
                string[] QD = item.Split('&');

                QuesAndTypeModel Ques = new QuesAndTypeModel();
                Ques.QuesTitle = QD[0];
                Ques.QuesChoice = QD[1];
                Ques.QuesTypeID = Convert.ToInt32(QD[2]);
                Ques.QuesType1 = QD[3];
                Ques.Necessary = Convert.ToBoolean(QD[4]);

                quesList = Ques;
            }
            return quesList;
        }

        /// <summary>
        /// 輸入字串，做字串切割，輸出成列表
        /// </summary>
        /// <param name="ques"></param>
        /// <returns></returns>
        public List<QuesAndTypeModel> DelQuesList(string ques, List<QuesAndTypeModel> queslist)
        {
            int count = queslist.Count;

            for (int i = 0; i < count; i++)
            {
                //foreach (var item in queslist)
                //{
                if(i < queslist.Count)
                {
                    if (queslist[i].QuesTitle == ques)
                    {
                        queslist.Remove(queslist[i]);
                        i--;
                    }
                }
                    
               // }
            }

            return queslist;
        }
    }
}