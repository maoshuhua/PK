using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_subject_video")]
    public partial class tv_subject_video : BaseEntity
    {
        [Key]
        [Column(Order=1)]
        public int subjectid { get; set; }

        [Key]
        [Column(Order = 2)]
        public int videoid { get; set; }
    }
}
