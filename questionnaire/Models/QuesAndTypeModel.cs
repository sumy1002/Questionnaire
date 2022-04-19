using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class QuesAndTypeModel
    {
        public int QuesID { get; set; }

        public string QuesTitle { get; set; }

        public string QuesChoice { get; set; }

        public int QuesTypeID { get; set; } //問題種類ID

        public string QuesType1 { get; set; } //問題種類名稱

        public bool Necessary { get; set; }
    }
}