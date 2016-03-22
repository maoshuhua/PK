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
    [Table("Image")]
    public partial class Image : BaseEntity
    {
       //图片编号
       [Key]
       public string I_ID  { get; set; }
       //编号
       public string Obj_ID  { get; set; }
       //图片
       public string ImagePath  { get; set; }
       //图片来源(1、资源 2、大事件)
       public string ImageSource  { get; set; }
       //添加时间
       public DateTime AddTime  { get; set; }
    }
}
