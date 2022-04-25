using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class UserInfoModel
    {
        public Guid UserID { get; set; }

        public Guid? AccountID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public DateTime CreateDate { get; set; }

        public string strDate { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Age { get; set; }

        public string Email { get; set; }
    }
}