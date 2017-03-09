using Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.DAL.Identity;

namespace Data
{
    public class DataContext: IdentityDbContext<AppUser>
    {
        public DataContext() : base("DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        DbSet<Team> Teams { get; set; }
    }
}
