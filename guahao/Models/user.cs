//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace guahao.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.appointment = new HashSet<appointment>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string real_name { get; set; }
        public string social_id { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public Nullable<byte> is_activated { get; set; }
        public Nullable<short> credict_rank { get; set; }
        public string picture { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointment { get; set; }
    }
}
