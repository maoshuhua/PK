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
    //用户表
    [Table("Reception_ResourceType")]
    public class Reception_ResourceType : BaseEntity
    {
        //资源分类编号
        [Key]
        public string RT_ID { get; set; }
        //资源分类名称
        public string Name { get; set; }
        //资源分类图片
        public string ImgPath { get; set; }
        //创建时间
        public DateTime CreateTime { get; set; }
    }
}
