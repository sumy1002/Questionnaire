using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class AccountCheckModel
    {
        public int CheckID { get; set; }

        public Guid AccountID { get; set; }

        public int TitleID { get; set; }
    }
}