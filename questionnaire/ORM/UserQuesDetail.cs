namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserQuesDetail
    {
        [Key]
        public int AnsID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public int UserID { get; set; }

        public int QuesID { get; set; }

        [Required]
        public string Answer { get; set; }

        public Guid? AccountID { get; set; }

        public virtual UserInfo UserInfo { get; set; }
    }
}
