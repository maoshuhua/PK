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
    [Table("Navigation")]
    public partial class Navigation : BaseEntity
    {
       //导航编号
       [Key]
       public string N_ID  { get; set; }
       //所属编号(暂留字段)
       public string Obj_ID  { get; set; }
       //名称
       public string Name  { get; set; }
       //路径
       public string GHLJ  { get; set; }
       //添加时间
       public DateTime AddTime  { get; set; }
    }
}
