using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class CQModel
    {
        public int CQID { get; set; }

        public string CQTitle { get; set; }

        public int QuesTypeID { get; set; }

        public string CQChoice { get; set; }

        public bool Necessary { get; set; }
    }
}