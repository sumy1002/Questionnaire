namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Statistic")]
    public partial class Statistic
    {
        [Key]
        public Guid QuestionnaireID { get; set; }

        public int QuesID { get; set; }

        public string Answer { get; set; }

        public int? AnsCount { get; set; }

        public virtual QuesDetail QuesDetail { get; set; }
    }
}
