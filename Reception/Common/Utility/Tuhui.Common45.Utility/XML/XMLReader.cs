using System;
using System.IO;
using System.Xml;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：XMLReader
    /// <summary>
    /// XML文件读取类
    /// </summary>
    /// <remarks>
    /// XML文件读取类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public class XMLReader
    {
        #region Init

        /// <summary>
        ///    xml文档
        /// </summary>
        public XmlDocument document { get; 
            private set; }
        
        /// <summary>
        /// xml节点
        /// </summary>
        private XmlElement root = null;

        #endregion Init

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public XMLReader()
        {
            document = new XmlDocument();
        }
         

        /// <summary>
        /// 使用数据流实例化对象
        /// </summary>
        /// <param name="stream"></param>
        public XMLReader(Stream stream)
        {
            document = new XmlDocument();
            try
            {
                document.Load(stream);
            }
            catch (XmlException e)
            {
                throw e;
            }
            root = document.DocumentElement;
        }

        /// <summary>
        /// 使用XML字符串实例化对象
        /// </summary>
        /// <param name="xml"></param>
        public XMLReader(string xml)
        {
            document = new XmlDocument();
            try
            {
                document.LoadXml(xml);
            }
            catch (XmlException e)
            {
                throw e;
            }
            root = document.DocumentElement;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// 加载一个指定xml文件
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFile(string fileName)
        {
            document.Load(fileName);
            root = document.DocumentElement;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点）， 查找的起始节点为document的根节点
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>查找到的节点对象</returns>
        public XmlNode FindNode(string xpath)
        {
            return XMLReader.FindNode(root, xpath);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应的一组节点， 查找的起始节点为document的根节点
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>查找到的一组节点对象</returns>
        public XmlNodeList FindNodes(string xpath)
        {
            return XMLReader.FindNodes(root, xpath);
        }

        /// <summary>
        /// 寻找第一个子节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode FindFirstChildNode(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return XMLReader.FindFirstChildNode(root);
            }
            else
            {
                var _item = XMLReader.FindNode(root, xpath);
                if (_item == null) return null;
                return XMLReader.FindFirstChildNode(_item);
            }
        }

        /// <summary>
        /// 寻找最后一个子节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode FindLastChildNode(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return XMLReader.FindLastChildNode(root);
            }
            else
            {
                var _item = XMLReader.FindNode(root, xpath);
                if (_item == null) return null;
                return XMLReader.FindLastChildNode(_item);
            }
        }

        /// <summary>
        /// 寻找子节点集合
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNodeList FindChildNodes(string xpath)
        {
            if (string.IsNullOrEmpty(xpath))
            {
                return XMLReader.FindChildNodes(root);
            }
            else
            {
                var _item = XMLReader.FindNode(root, xpath);
                if (_item == null) return null;
                return XMLReader.FindChildNodes(_item);
            }
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点）， 查找的起始节点为document的根节点，并返回该节点的文本值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="HasCDATA">是否包含CDATA符</param>
        /// <returns>查找到的节点对象的文本值</returns>
        public string FindValue(string xpath, bool HasCDATA = false)
        {
            return XMLReader.FindValue(root, xpath, HasCDATA);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为document的根节点，并返回该节点的文本值，如文本值为空，
        /// 则返回指定的默认值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>该节点的文本值，如文本值为空，则返回指定的默认值</returns>
        public string FindValue(string xpath, string defaultValue)
        {
            return XMLReader.FindValue(root, xpath, defaultValue);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        ///  查找的起始节点为document的根节点，并将该节点的文本值转换为int型数据返回，
        ///  如文本值无法转换成int，则返回指定的默认值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为int型数据返回，如文本值无法转换成int，则返回指定的默认值</returns>
        public int FindValueAsInt(string xpath, int defaultValue)
        {
            return XMLReader.FindValueAsInt(root, xpath, defaultValue);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        ///  查找的起始节点为document的根节点，并将该节点的文本值转换为double型数据返回，
        ///  如文本值无法转换成double，则返回指定的默认值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为double型数据返回，如文本值无法转换成int，则返回指定的默认值</returns>
        public double FindValueAsDouble(string xpath, double defaultValue)
        {
            return XMLReader.FindValueAsDouble(root, xpath, defaultValue);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        ///  查找的起始节点为document的根节点，并将该节点的文本值转换为bool型数据返回，
        ///  如文本值无法转换成bool，则返回指定的默认值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为bool型数据返回，如文本值无法转换成bool，则返回指定的默认值</returns>
        public bool FindValueAsBool(string xpath, bool defaultValue)
        {
            return XMLReader.FindValueAsBool(root, xpath, defaultValue);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        ///  查找的起始节点为document的根节点，并将该节点的文本值转换为DateTime型数据返回，
        ///  如文本值无法转换成DateTime，则返回指定的默认值
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为DateTime型数据返回，如文本值无法转换成DateTime，则返回指定的默认值</returns>
        public DateTime FindValueAsDatetime(string xpath, DateTime defaultValue)
        {
            return XMLReader.FindValueAsDatetime(root, xpath, defaultValue);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应的一组节点， 查找的起始节点为document的根节点,
        /// 并将这一组节点的文本值作为一个字符串数组一起返回
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>文本值数组</returns>
        public string[] FindValues(string xpath)
        {
            return XMLReader.FindValues(root, xpath);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应的一组节点， 查找的起始节点为document的根节点,
        /// 并将这一组节点的文本值作为一个字符串数组一起返回
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>文本值数组</returns>
        public String[] FindSubNodeValues(String xpath)
        {
            return XMLReader.FindSubNodeValues(root, xpath);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        ///   查找的起始节点为document的根节点,同时返回该节点所包含的所有xml报文内容
        /// </summary>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>节点所包含的所有xml报文内容</returns>
        public string FindXmlContents(string xpath)
        {
            return XMLReader.FindXmlContents(root, xpath);
        }

        /// <summary>
        /// 寻找根节点下的子节点集合
        /// </summary>
        /// <returns></returns>
        public XmlNodeList FindChildNodes()
        {
            return FindChildNodes(root);
        }

        #region 静态方法

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点）， 查找的起始节点为指定的节点
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>查找到的节点对象</returns>
        public static XmlNode FindNode(XmlNode prefix, string xpath)
        {
            XmlNode node = null;

            if (prefix != null)
            {
                try
                {
                    node = prefix.SelectSingleNode(xpath);
                }
                catch
                {
                }
            }
            return node;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应的一组节点， 查找的起始节点为指定的节点
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>查找到的一组节点对象</returns>
        public static XmlNodeList FindNodes(XmlNode prefix, string xpath)
        {
            XmlNodeList nodeList = null;
            if (prefix != null)
            {
                try
                {
                    nodeList = prefix.SelectNodes(xpath);
                }
                catch
                {
                }
            }
            return nodeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static XmlNode FindFirstChildNode(XmlNode prefix)
        {
            XmlNode node = null;
            if (prefix != null)
            {
                try
                {
                    node = prefix.FirstChild;
                }
                catch
                {
                }
            }
            return node;
        }

        /// <summary>
        /// 查询指定节点的最后一个子节点
        /// </summary>
        /// <param name="prefix">指定节点</param>
        /// <returns>最后一个子节点</returns>
        public static XmlNode FindLastChildNode(XmlNode prefix)
        {
            XmlNode node = null;
            if (prefix != null)
            {
                try
                {
                    node = prefix.LastChild;
                }
                catch
                {
                }
            }
            return node;
        }

        /// <summary>
        /// 查询子节点
        /// </summary>
        /// <param name="prefix">指定节点</param>
        /// <returns>子节点集合</returns>
        public static XmlNodeList FindChildNodes(XmlNode prefix)
        {
            XmlNodeList node = null;
            if (prefix != null)
            {
                try
                {
                    node = prefix.ChildNodes;
                }
                catch
                {
                }
            }
            return node;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并返回该节点的文本值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="hasCDATA">节点内容是否包含CDATA,默认不包含</param>
        /// <returns>查找到的节点对象的文本值</returns>
        public static string FindValue(XmlNode prefix, string xpath, bool hasCDATA = false)
        {
            string result = null;
            XmlNode node = FindNode(prefix, xpath);
            if (node != null)
            {
                result = getTextContents(node, hasCDATA);
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并返回该节点的文本值，如文本值为空，
        /// 则返回指定的默认值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>该节点的文本值，如文本值为空，则返回指定的默认值</returns>
        public static string FindValue(XmlNode prefix, string xpath, string defaultValue)
        {
            string result = FindValue(prefix, xpath);
            if (result == string.Empty)
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并将该节点的文本值转换为int型数据返回，
        /// 如文本值无法转换成int，则返回指定的默认值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为int型数据返回，如文本值无法转换成int，则返回指定的默认值</returns>
        public static int FindValueAsInt(XmlNode prefix, string xpath, int defaultValue)
        {
            int result = 0;
            string strValue = FindValue(prefix, xpath);
            try
            {
                result = int.Parse(strValue);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并将该节点的文本值转换为double型数据返回，
        /// 如文本值无法转换成double，则返回指定的默认值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为double型数据返回，如文本值无法转换成double，则返回指定的默认值</returns>
        public static double FindValueAsDouble(XmlNode prefix, string xpath, double defaultValue)
        {
            double result = 0;
            string strValue = FindValue(prefix, xpath);
            try
            {
                result = double.Parse(strValue);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并将该节点的文本值转换为bool型数据返回，
        /// 如文本值无法转换成bool，则返回指定的默认值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为bool型数据返回，如文本值无法转换成bool，则返回指定的默认值</returns>
        public static bool FindValueAsBool(XmlNode prefix, string xpath, bool defaultValue)
        {
            bool result = false;
            string strValue = FindValue(prefix, xpath);
            try
            {
                result = bool.Parse(strValue);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 查询一个节点的某一个属性
        /// </summary>
        /// <param name="node">被查询节点</param>
        /// <param name="attName">属性名</param>
        /// <returns>属性值</returns>
        public static string GetAttribute(XmlNode node, string attName)
        {
            if (node == null)
            {
                return null;
            }

            string result = null;
            try
            {
                result = node.Attributes[attName].Value;
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// 查询节点属性值
        /// </summary>
        /// <param name="prefix">节点</param>
        /// <param name="xpath">节点路劲</param>
        /// <param name="attName">属性名</param>
        /// <returns>属性值</returns>
        public static string FindAttribute(XmlNode prefix, string xpath, string attName)
        {
            XmlNode node = FindNode(prefix, xpath);

            return GetAttribute(node, attName);
        }

        /// <summary>
        /// 查询节点属性值
        /// </summary>
        /// <param name="xpath">节点路劲</param>
        /// <param name="attName">属性名</param>
        /// <returns>属性值</returns>
        public string FindAttribute(string xpath, string attName)
        {
            XmlNode node = this.FindNode(xpath);

            return GetAttribute(node, attName);
        }

        /// <summary>
        /// 查询节点属性值
        /// </summary>
        /// <param name="prefix">节点</param>
        /// <param name="xpath">节点路劲</param>
        /// <param name="attName">属性名</param>
        /// <returns>各个属性值的集合</returns>
        public static string[] FindAttributes(XmlNode prefix, string xpath, string attName)
        {
            XmlNodeList nodelist = FindNodes(prefix, xpath);
            string[] result = null;
            if (nodelist != null)
            {
                int length = nodelist.Count;
                result = new string[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = GetAttribute(nodelist[i], attName);
                }
            }

            return result;
        }

        /// <summary>
        /// 查询节点属性值
        /// </summary>
        /// <param name="xpath">路劲</param>
        /// <param name="attName">属性名</param>
        /// <returns>各个属性值的集合</returns>
        public string[] FindAttributes(string xpath, string attName)
        {
            return FindAttributes(root, xpath, attName);
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，并将该节点的文本值转换为DateTime型数据返回，
        /// 如文本值无法转换成DateTime，则返回指定的默认值
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>将该节点的文本值转换为DateTime型数据返回，如文本值无法转换成DateTime，则返回指定的默认值</returns>
        public static DateTime FindValueAsDatetime(XmlNode prefix, string xpath, DateTime defaultValue)
        {
            DateTime result = DateTime.MinValue;
            string strValue = FindValue(prefix, xpath);
            try
            {
                result = DateTime.Parse(strValue);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应的一组节点， 查找的起始节点为指定的节点，
        /// 并将这一组节点的文本值作为一个字符串数组一起返回
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>文本值数组</returns>
        public static string[] FindValues(XmlNode prefix, string xpath)
        {
            string[] result = null;
            XmlNodeList nodeList = FindNodes(prefix, xpath);
            if (nodeList != null)
            {
                result = new string[nodeList.Count];
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList.Item(i);
                    result[i] = getTextContents(node);
                }
            }
            return result;
        }

        /// <summary>
        /// 查询下属节点的value值
        /// </summary>
        /// <param name="prefix">指定节点</param>
        /// <param name="xpath">路劲</param>
        /// <returns>value值集合</returns>
        public static String[] FindSubNodeValues(XmlNode prefix, String xpath)
        {
            String[] result = null;

            int lastIndex = xpath.LastIndexOf('/');
            string strTemp = xpath.Substring(0, lastIndex);
            XmlNodeList nodeList = FindNodes(prefix, strTemp);

            if (nodeList != null)
            {
                result = new String[nodeList.Count];
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList.Item(i);

                    string strSubPath = "." + xpath.Substring(lastIndex, xpath.Length - lastIndex);
                    result[i] = XMLReader.FindValue(node, strSubPath);
                }
            }
            return result;
        }

        /// <summary>
        /// 采用xpath查询语句查找相应节点（与查询语句匹配的第一个节点），
        /// 查找的起始节点为指定的节点，同时返回该节点所包含的所有xml报文内容
        /// </summary>
        /// <param name="prefix">查找的起始节点</param>
        /// <param name="xpath">xpath查询语句，参见微软帮助</param>
        /// <returns>节点所包含的所有xml报文内容</returns>
        public static string FindXmlContents(XmlNode prefix, string xpath)
        {
            string result = null;
            XmlNode node = FindNode(prefix, xpath);
            if (node != null)
            {
                result = node.InnerXml;
            }
            return result;
        }

        private static string getTextContents(XmlNode node, bool hasCDATA = false)
        {
            XmlNodeList childNodes;
            string result = "";

            if (node == null)
            {
                return null;
            }
            childNodes = node.ChildNodes;
            for (int i = 0; i < childNodes.Count; i++)
            {
                if (!hasCDATA)
                {
                    if (childNodes.Item(i).NodeType == XmlNodeType.Text)
                    {
                        string nodeValue = childNodes.Item(i).Value;

                        result += nodeValue;
                    }
                }
                else
                {
                    if (childNodes.Item(i).NodeType == XmlNodeType.Text || childNodes.Item(i).NodeType == XmlNodeType.CDATA)
                    {
                        string nodeValue = childNodes.Item(i).Value;

                        result += nodeValue;
                    }
                }
            }
            return result.Trim();
        }

        #endregion 静态方法

        #endregion Method
    }
}