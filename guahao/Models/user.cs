namespace guahao.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Cryptography;
    using System.Text;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            appointment = new HashSet<appointment>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }


        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string real_name { get; set; }

        [StringLength(50)]
        public string social_id { get; set; }

        [StringLength(50)]
        public string tel { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        public byte? is_activated { get; set; }

        public short? credict_rank { get; set; }

        [StringLength(100)]
        public string picture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointment { get; set; }

        public string Md5Helper(string password)
        {

            byte[] result = Encoding.Default.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            password = BitConverter.ToString(output).Replace("-", "");
            return password;
        }
    }
}
