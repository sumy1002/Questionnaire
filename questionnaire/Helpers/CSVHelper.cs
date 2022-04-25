using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace questionnaire.Helpers
{
    public class CSVHelper
    {
        /// <summary>
        /// 儲存csv檔
        /// </summary>
        /// <param name="fillFoldPath">目標路徑</param>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void CSVGenerator(string fillFoldPath, string filePath, List<string> data)
        {
            if (!Directory.Exists(fillFoldPath))
            {
                Directory.CreateDirectory(fillFoldPath);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            using (var file = new StreamWriter(filePath))
            {
                foreach (var i in data)
                {
                    file.WriteLineAsync(i);
                }
            }
        }
    }
}