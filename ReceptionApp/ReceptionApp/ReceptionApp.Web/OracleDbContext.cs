using ReceptionApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReceptionApp.Web
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext()
            : base("OracleDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<OracleDbContext>());
        }

        public DbSet<Reception_UserInfo> Reception_UserInfo { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TUHUI_AQI");
        }
    }
}