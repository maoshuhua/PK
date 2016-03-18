using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.Data.Entity;

namespace Tuhui.Reception.Model
{
    public class DbReceptionContext : BaseContext
    {
        public DbReceptionContext()
            : base("DbReception")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TUHUI_AQI");
        }

        public DbSet<Reception_UserInfo> Reception_UserInfo { get; set; }
        public DbSet<Reception_ResourceType> Reception_ResourceType { get; set; }
        public DbSet<Reception_Resource> Reception_Resource { get; set; }
    }
}
