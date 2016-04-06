using System.Collections.Generic;

namespace Tuhui.OH.Entity.JsClass
{
    public class TreeJs
    {
        public dynamic data { get; set; }
        public List<TreeJs> children { get; set; }
        public dynamic attr { get; set; }
        public dynamic metadata { get; set; }
        /// <summary>
        /// open closed leaf 
        /// </summary>
        public string state { get; set; }
    }
}