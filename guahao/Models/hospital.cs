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
    
    public partial class hospital
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hospital()
        {
            this.appointment = new HashSet<appointment>();
            this.department = new HashSet<department>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string introduction { get; set; }
        public string rank { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> city { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointment> appointment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<department> department { get; set; }
    }
}
