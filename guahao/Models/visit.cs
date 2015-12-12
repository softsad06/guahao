namespace guahao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("visit")]
    public partial class visit
    {
        [Key]
        [Column(Order = 0)]
        public DateTime date { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int doctor_id { get; set; }

        public int number { get; set; }

        public int? price { get; set; }

        public virtual doctor doctor { get; set; }
    }
}
