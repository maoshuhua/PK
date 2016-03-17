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
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Reception_UserInfo> Reception_UserInfo { get; set; }

    }
}
