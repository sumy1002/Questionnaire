namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuesDetail")]
    public partial class QuesDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuesDetail()
        {
            Statistics = new HashSet<Statistic>();
            UserQuesDetails = new HashSet<UserQuesDetail>();
        }

        [Key]
        public int QuesID { get; set; }

        public Guid QuestionnaireID { get; set; }

        public int TitleID { get; set; }

        [Required]
        [StringLength(200)]
        public string QuesTitle { get; set; }

        public string QuesChoice { get; set; }

        public int QuesTypeID { get; set; }

        public bool Necessary { get; set; }

        public int? Count { get; set; }  

        public virtual Content Content { get; set; }

        public virtual QuesType QuesType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Statistic> Statistics { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserQuesDetail> UserQuesDetails { get; set; }
    }
}
