using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuhui.Common45.Framework;

namespace Tuhui.Reception.Model
{
    //
    [Table("Reception_ResourceType")]
    public partial class Reception_ResourceType : BaseEntity
    {
           //
           [Key]
           public string RT_ID  { get; set; }
           //
           public string Name  { get; set; }
           //
           public string ImgPath  { get; set; }
           //
           public DateTime CreateTime  { get; set; }
    }
}
