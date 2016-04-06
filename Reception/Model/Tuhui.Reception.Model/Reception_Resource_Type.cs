using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuhui.Reception.Model
{
    public class Reception_Resource_Type
    {
        //资源编号
        public string R_ID { get; set; }

        //资源分类编号
        public string RT_ID { get; set; }

        //资源名称
        public string R_Name { get; set; }

        //资源分类名称
        public string RT_Name { get; set; }

        //经度
        public string Long { get; set; }

        //纬度
        public string Lat { get; set; }

        //责任单位
        public string RRDW { get; set; }

        //施方单位
        public string SFDW { get; set; }

        //开工时间
        public DateTime StartTime { get; set; }

        //计划完成时间
        public DateTime EndTime { get; set; }

        //项目状态(1、未开工 2、已开工 3、已投产)
        public string RStatus { get; set; }

        //所属街道
        public string SSJD { get; set; }
    }
}
