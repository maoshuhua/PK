using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_log")]
    public partial class tv_log : BaseEntity
    {
        [Key]
        public int logid { get; set; }
        public int logtype { get; set; }
        public string logcontent { get; set; }
        public DateTime logtime { get; set; }
    }
}
