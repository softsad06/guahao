﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class guahaoEntities : DbContext
    {
        public guahaoEntities()
            : base("name=guahaoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<administrator> administrator { get; set; }
        public virtual DbSet<appointment> appointment { get; set; }
        public virtual DbSet<department> department { get; set; }
        public virtual DbSet<doctor> doctor { get; set; }
        public virtual DbSet<hospital> hospital { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<visit> visit { get; set; }
    }
}
