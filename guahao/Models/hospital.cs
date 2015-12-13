namespace guahao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hospital")]
    public partial class hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hospital()
        {
            appointment = new HashSet<appointment>();
            department = new HashSet<department>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [Column(TypeName = "ntext")]
        public string introduction { get; set; }

        public int? rank { get; set; }

        [Column(TypeName = "ntext")]
        public string address { get; set; }

        [StringLength(50)]
        public string tel { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(10)]
        public string city { get; set; }

        [StringLength(80)]
        public string url { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointment { get; set; }

        public virtual city city1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<department> department { get; set; }
    }
}
