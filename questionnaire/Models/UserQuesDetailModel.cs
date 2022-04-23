using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class UserQuesDetailModel
    {
        public int AnsID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public int UserID { get; set; }

        public int QuesID { get; set; }


        public string Answer { get; set; }

        public int QuesTypeID { get; set; }

        public Guid? AccountID { get; set; }
    }
}