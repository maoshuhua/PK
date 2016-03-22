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
    [Table("Video")]
    public partial class Video : BaseEntity
    {
       //视频编号
       [Key]
       public string V_ID  { get; set; }
       //编号
       public string Obj_ID  { get; set; }
       //视频名称
       public string Name  { get; set; }
       //视频路径
       public string VideoPath  { get; set; }
       //添加时间
       public DateTime AddTime  { get; set; }
       //视频来源(1、资源 2、大事件)
       public string VideoSource  { get; set; }
    }
}
