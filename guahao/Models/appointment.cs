namespace guahao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("appointment")]
    public partial class appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int? hospital_id { get; set; }

        public int? user_id { get; set; }

        public int? doctor_id { get; set; }

        public DateTime? time { get; set; }

        public byte? status { get; set; }

        public int? price { get; set; }

        public virtual doctor doctor { get; set; }

        public virtual hospital hospital { get; set; }

        public virtual user user { get; set; }
    }
}
