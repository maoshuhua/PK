using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuhui.Common45.Utility
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class JsonError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_e_type"></param>
        public JsonError(string _e_type)
        {
            this.e_type = _e_type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_e_type"></param>
        /// <param name="_e_content"></param>
        public JsonError(string _e_type, object _e_content)
        {
            this.e_type = _e_type;
            this.e_content = _e_content;
        }

        /// <summary>
        /// "modelstate","exception","userexception"
        /// </summary>
        public string e_type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object e_content { get; set; }
    }

    //public class JsonError_Exception
    //{
    //    public string message { get; set; }
    //}

    //public class JsonError_UserException
    //{
    //    public string message { get; set; }
    //}

    //public class JsonError_ModelState
    //{
    //    public JsonError_ModelState(Dictionary<string,string> _messages)
    //    {
    //        this.messages = _messages;
    //    }

    //    public Dictionary<string, string> messages { get; set; }
    //}
}
