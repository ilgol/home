﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class homeEntities : DbContext
    {
        public homeEntities()
            : base("name=homeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<OverallHashRateHistory> OverallHashRateHistories { get; set; }
        public virtual DbSet<PartialHashRateHistory> PartialHashRateHistories { get; set; }
        public virtual DbSet<PayoutHistory> PayoutHistories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Notify> Notifies { get; set; }
    }
}
