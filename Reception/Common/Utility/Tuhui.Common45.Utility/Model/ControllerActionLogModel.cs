using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    public class ControllerActionLogModel
    {
        public DateTime OccurTime { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public string UserHostName { get; set; }

        public string UserHostAddress { get; set; }

        public string Browser { get; set; }

        public string Form { get; set; }

        public string QueryString { get; set; }

        public override string ToString()
        {
            StringBuilder _builder = new StringBuilder();
            _builder.Append("{");
            _builder.AppendFormat("'{0}':'{1}',", "OccurTime", this.OccurTime.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            _builder.AppendFormat("'{0}':'{1}',", "ActionName", this.ActionName);
            _builder.AppendFormat("'{0}':'{1}',", "ControllerName", this.ControllerName);
            _builder.AppendFormat("'{0}':'{1}',", "Method", this.Method);
            _builder.AppendFormat("'{0}':'{1}',", "Url", this.Url);
            _builder.AppendFormat("'{0}':'{1}',", "UserHostName", this.UserHostName);
            _builder.AppendFormat("'{0}':'{1}',", "UserHostAddress", this.UserHostAddress);
            _builder.AppendFormat("'{0}':'{1}',", "Browser", this.Browser);
            _builder.AppendFormat("'{0}':{1},", "Form", this.Form);
            _builder.AppendFormat("'{0}':{1}", "QueryString", this.QueryString);
            _builder.Append("}");
            return _builder.ToString();
        }

        public string ToShortString() {
            StringBuilder _builder = new StringBuilder();
            _builder.Append("{");
            _builder.AppendFormat("'{0}':'{1}',", "OccurTime", this.OccurTime.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            _builder.AppendFormat("'{0}':'{1}',", "ActionName", this.ActionName);
            _builder.AppendFormat("'{0}':'{1}',", "ControllerName", this.ControllerName);
            _builder.Append("}");
            return _builder.ToString();
        }
    }
}
