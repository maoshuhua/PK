using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tuhui.Common45.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuhui.Reception.Model
{
    [Table("tv_subject")]
    public partial class tv_subject : BaseEntity
    {
        [Key]
        public int subjectid { get; set; }
        public int? parentid { get; set; }
        public string subjectname { get; set; }
        public double centerlng { get; set; }
        public double centerlat { get; set; }
        public int centerzoom { get; set; }
        public string subjectdesc { get; set; }

        public int isleaf { get; set; }

        public string lineData { get; set; }

        [NotMapped]
        public List<int> videoIds { get; set; }

        [NotMapped]
        public int treeLevel { get; set; }

        [NotMapped]
        public List<tv_subject> children { get; set; }
    }
}
