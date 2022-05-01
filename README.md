## Questionnaire

#### 基本功能
* 前台可以作答、搜尋問卷及觀看統計數據
* 登入後台後可設計問卷、管理問卷(增刪修)及觀看統計數據
* 後台可以查看所有作答的詳細記錄，並可匯出為csv檔
* 後台可以管理常用問題(增刪修)，並可將常用問題引用至問卷設計中

#### 資料庫(MSSQL)
* 結構與描述 `Questionnaire.sql`
* 資料 `QuestionnaireData.sql`
* 備份 `Questionnaire.bak`

#### 環境依賴
* asp.net framework4.8

#### 目錄結構描述 `in questionnaire.sln`
> 前台
>> listPage.aspx `列表頁`
>> 
>> Form.aspx `問卷頁`
>> 
>> checkPage.aspx `確認頁`
>> 
>> statistic.aspx `統計數據頁`
>> 
>> Login.aspx `登入頁(往後台)`

> BackAdmin
>> ListPageAdmin.aspx `列表管理頁`
>> 
>> EditQues.aspx `問卷編輯頁`
>> 
>> NewQues.aspx `新增問卷頁`
>> 
>> CommonQuesPageA.aspx `常用問題管理頁`

> Models
>> QuesContentsModel.cs `問卷Model`
>> 
>> QuesDetailModel.cs `問題Model`
>> 
>> QuesTypeModel.cs `問題種類Model`
>> 
>> UserInfoModel.cs `填寫個人資訊Model`
>> 
>> UserQuesDetailModel.cs `填寫答案資訊Model`
>> 
>> StatisticModel.cs `統計Model`
>> 
>> CSVModel.cs `CSVModel`
>> 
>> CQModel.cs `常用問題Model`

> Manager(與資料庫溝通的所有方法)
>> AccountManager.cs `帳號管理Manager`
>> 
>> QuesContentsManager.cs `問卷管理Manager`
>> 
>> QuesDetailManager.cs `問題管理Manager`
>> 
>> QuesTypeManager.cs `問題種類管理Manager`
>> 
>> StatisticManager.cs `統計管理Manager`
>> 
>> UserInfoManager.cs `填寫個人資訊管理Manager`
>> 
>> UserQuesDetailManager.cs `填寫答案管理Manager`
>> 
>> CQManager.cs `常用問題管理Manager`
>> 


####  後台登入帳密
* 帳號: Admin
* 密碼: 12345678
