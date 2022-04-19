namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CQ")]
    public partial class CQ
    {
        public int CQID { get; set; }

        [Required]
        [StringLength(200)]
        public string CQTitle { get; set; }

        public int QuesTypeID { get; set; }

        public string CQChoice { get; set; }

        public bool Necessary { get; set; }

        public virtual QuesType QuesType { get; set; }
    }
}
