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
    [Table("Reception_ResourceEvent")]
    public partial class Reception_ResourceEvent : BaseEntity
    {
       //资源大事件编号
       [Key]
       public string RE_ID  { get; set; }
       //资源编号
       public string R_ID  { get; set; }
       //大事件名称
       public string Name  { get; set; }
       //大事件内容
       public string Content  { get; set; }
       //添加时间
       public DateTime AddTime  { get; set; }
    }
}
