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
            //针对Oracle数据库权限问题
            modelBuilder.HasDefaultSchema("TUHUI_AQI");
        }
        
        //
        public DbSet<Reception_Resource> Reception_Resource { get; set; }
        //
        public DbSet<Reception_ResourceType> Reception_ResourceType { get; set; }
        //
        public DbSet<Reception_UserInfo> Reception_UserInfo { get; set; }
        //
        public DbSet<Image> Image { get; set; }
        //
        public DbSet<Video> Video { get; set; }
        //
        public DbSet<Reception_ResourceEvent> Reception_ResourceEvent { get; set; }
        //
        public DbSet<Navigation> Navigation { get; set; }
    }
}