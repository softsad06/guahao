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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public DateTime date { get; set; }

        public int doctor_id { get; set; }

        public int? number { get; set; }

        public int? price { get; set; }
    }
}
