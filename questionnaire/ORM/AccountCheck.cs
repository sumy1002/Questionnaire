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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CheckID { get; set; }

        public Guid AccountID { get; set; }

        public int TitleID { get; set; }

        public virtual Account Account { get; set; }

        public virtual Content Content { get; set; }
    }
}
