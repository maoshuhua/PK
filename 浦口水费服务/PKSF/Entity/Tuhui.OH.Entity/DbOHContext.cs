using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common.Framework;
using System.Data.Entity;

namespace Tuhui.OH.Entity
{
    public class DbOHContext : BaseContext
    {
        public DbOHContext()
            : base("name=DbOH")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
