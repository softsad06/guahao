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
    
    public partial class appointment
    {
        public int id { get; set; }
        public Nullable<int> hospital_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> doctor_id { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public Nullable<byte> status { get; set; }
        public Nullable<int> price { get; set; }
    
        public virtual doctor doctor { get; set; }
        public virtual hospital hospital { get; set; }
        public virtual user user { get; set; }
    }
}
