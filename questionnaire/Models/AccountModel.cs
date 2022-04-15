using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class AccountModel
    {
        public Guid AccountID { get; set; }

        /// <summary> 帳號 </summary>
        public string Account { get; set; }

        /// <summary> 密碼 </summary>
        public string PWD { get; set; }

        /// <summary> 使用者權限 </summary>
        public int UserLevel { get; set; }

        public bool IsEnable { get; set; }

        public DateTime CreateDate { get; set; }
    }
}