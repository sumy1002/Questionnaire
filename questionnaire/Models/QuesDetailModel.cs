﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace questionnaire.Models
{
    public class QuesDetailModel
    {
        public int QuesID { get; set; }

        public int TitleID { get; set; }

        public string QuesTitle { get; set; }

        public int QuesTypeID { get; set; }

        public bool IsEnable { get; set; }
    }
}