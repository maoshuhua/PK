using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_face")]
    public partial class tv_face : BaseEntity
    {
        [Key]
        public int faceid { get; set; }
        public string facecontent { get; set; }
        public int facetype { get; set; }
        public int facesize { get; set; }
        public int sortno { get; set; }
       
    }
}
