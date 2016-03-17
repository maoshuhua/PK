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

        public DbSet<tv_subject> tv_subject { get; set; }
        public DbSet<tv_subject_video> tv_subject_video { get; set; }
        public DbSet<tv_face> tv_face { get; set; }
        public DbSet<tv_user> tv_user { get; set; }
        public DbSet<tv_log> tv_log { get; set; }
        public DbSet<tv_video> tv_video { get; set; }

        public DbSet<tv_busLine> tv_busLine { get; set; }

    }
}
