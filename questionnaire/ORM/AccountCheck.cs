namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccountCheck")]
    public partial class AccountCheck
    {
        [Key]
        public int CheckID { get; set; }

        public Guid AccountID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public bool Checks { get; set; }

        public virtual Account Account { get; set; }
    }
}
