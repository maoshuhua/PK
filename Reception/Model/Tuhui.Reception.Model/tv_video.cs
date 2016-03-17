using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_video")]
    public partial class tv_video : BaseEntity
    {
        [Key]
        public int videoid { get; set; }
        public string videoname { get; set; }
        public double videolng { get; set; }
        public double videolat { get; set; }
        public int videostatus { get; set; }
        public string videounit { get; set; }
        public int videosource { get; set; }
        public string videoaccess { get; set; }
        public int enable { get; set; }
        public string videodesc { get; set; }
        public string thirdid { get; set; }

    }
}
