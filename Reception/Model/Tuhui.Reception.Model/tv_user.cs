using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_user")]
    public partial class tv_user : BaseEntity
    {
        [Key]
        public int userid { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public int userlevel { get; set; }
        public int userstate { get; set; }
        public DateTime createtime { get; set; }
        public DateTime lastlogintime { get; set; }
        public string lastloginIP { get; set; }
    }
}
