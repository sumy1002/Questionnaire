using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class QuesContentsModel
    {
        public Guid QuestionnaireID { get; set; }

        public int TitleID { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string strStartTime { get; set; }

        public string strEndTime { get; set; }

        public string State1 { get; set; }
        public string State2 { get; set; }
        public string State3 { get; set; }

        public bool IsEnable { get; set; }
        public string strIsEnable { get; set; }

        public string Content
        {
            get { return this.Body; }
            set { this.Body = value; }
        }
    }
}