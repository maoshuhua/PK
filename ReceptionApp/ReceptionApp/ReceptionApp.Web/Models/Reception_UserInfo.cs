using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReceptionApp.Web.Models
{
    //用户表
    [Table("Reception_UserInfo")]
    public class Reception_UserInfo
    {
        //用户编号
        [Key]
        public string U_ID { get; set; }
        //用户名
        public string Name { get; set; }
        //密码
        public string Pwd { get; set; }
        //注册时间
        public DateTime RegTime { get; set; }
    }
}