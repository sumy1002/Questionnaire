using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace questionnaire.Helpers
{
    public class Logger
    {
        private const string _savePath = @"D:\\Logs";
        private const string _saveFile = @"D:\\Logs\\logs.log";

        /// <summary> 紀錄錯誤 </summary>
        /// <param name="moduleName"></param>
        /// <param name="ex"></param>
        public static void WriteLog(string moduleName, Exception ex)
        {
            // -----
            // yyyy/MM/dd HH:mm:ss
            //   Module Name
            //   Error Content
            // -----

            string content =
$@"-----
{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}
    {moduleName}
    {ex.ToString()}
-----
";
            //確認路徑是否存在
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            if (!File.Exists(_saveFile))
            {
                File.Create(_saveFile);
            }

            File.AppendAllText(Logger._saveFile, content);
        }
    }
}