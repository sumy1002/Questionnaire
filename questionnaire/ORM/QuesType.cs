namespace questionnaire.ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuesType")]
    public partial class QuesType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuesType()
        {
            CQs = new HashSet<CQ>();
            QuesDetails = new HashSet<QuesDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuesTypeID { get; set; }

        [Column("QuesType")]
        [Required]
        [StringLength(10)]
        public string QuesType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CQ> CQs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuesDetail> QuesDetails { get; set; }
    }
}
