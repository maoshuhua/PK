using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    public class zTreeModel<T> : zTreeModel where T : class
    {
        public T detail { get; set; }
    }

    public class zTreeModel
    {
        public string id { get; set; }

        public string name { get; set; }

        public bool @checked { get; set; }

        public bool isParent { get; set; }

        public bool open { get; set; }

        public string pId { get; set; }

        public string icon { get; set; }

        public string iconOpen { get; set; }

        public string iconClose { get; set; }
    }
}
