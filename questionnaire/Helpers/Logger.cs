using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace questionnaire.Helpers
{
    public class Logger
    {
        private const string _savePath = "D:\\ccc\\Logs\\log.log";

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

            File.AppendAllText(Logger._savePath, content);
        }
    }
}