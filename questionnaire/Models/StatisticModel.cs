using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class StatisticModel
    {
        public Guid QuestionnaireID { get; set; }
        public int QuesID { get; set; }
        public string Answer { get; set; }
        public int AnsCount { get; set; }
    }
}