﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PolKaramnov
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
  

    public partial class KaramnovPolEntities3 : DbContext
    {
        private static KaramnovPolEntities3 _context;
        public KaramnovPolEntities3()
            : base("name=KaramnovPolEntities3")
        {
        }
        public static KaramnovPolEntities3 GetContext()
        {
            if (_context == null)
                _context = new KaramnovPolEntities3();
            return _context;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<PartnerProducts> PartnerProducts { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TypePartner> TypePartner { get; set; }

        
    }
}
