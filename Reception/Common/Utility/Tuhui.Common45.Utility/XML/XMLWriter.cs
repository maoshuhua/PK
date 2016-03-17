using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：XMLWriter
    /// <summary>
    /// XML改写类
    /// </summary>
    /// <remarks>
    /// XML改写类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public class XMLWriter
    {
        #region Property
        XmlDocument document;

        #endregion

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        public XMLWriter()
        {
            document = new XmlDocument();
            XmlNode xnode = document.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xnode.InnerText += " encoding=\"GB2312\"";
            document.AppendChild(xnode);
        }

        /// <summary>
        /// 构造函数(指定修改XML文件路劲)
        /// </summary>
        /// <param name="path"></param>
        public XMLWriter(string path)
        {
            document = new XmlDocument();
            document.Load(path);
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 生成根节点
        /// </summary>
        /// <param name="rootName"></param>
        public void CreateBaseRoot(string rootName)
        {
            XmlElement _element = document.CreateElement("", rootName, "");
            document.AppendChild(_element);
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode FindNode(string xpath)
        {
            XmlNode root = document.DocumentElement;
            XmlNode node = null;
            try
            {
                node = root.SelectSingleNode(xpath);
            }
            catch
            {
            }
            return node;
        }

        #region CreateElement

        /// <summary>
        /// 创建字符串元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, string text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text);
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建GUID元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, Guid text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text.ToString());
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建int元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, int text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text.ToString());
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建short元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, short text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text.ToString());
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建decimal元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, decimal text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text.ToString());
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建datetime元素
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        public void CreateElement(string elementName, DateTime text)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            XmlText _text = document.CreateTextNode(text.ToString("yyyy-MM-dd hh:mm:ss"));
            _element.AppendChild(_text);
            document.ChildNodes[1].AppendChild(_element);
        }

        /// <summary>
        /// 创建元素集合
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="elements"></param>
        public void CreateElement(string elementName, List<XmlNode> elements)
        {
            XmlElement _element = document.CreateElement("", elementName, "");
            foreach (var item in elements)
            {
                _element.AppendChild(item);
            }
            document.ChildNodes[1].AppendChild(_element);
        }

        #endregion CreateElement
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public XmlNode CreateNode(string nodeName)
        {
            XmlNode _node = document.CreateNode(XmlNodeType.Element, nodeName, "");
            return _node;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public XmlNode CreateNode(string nodeName, string text)
        {
            XmlNode _node = document.CreateNode(XmlNodeType.Element, nodeName, "");
            _node.AppendChild(document.CreateTextNode(text));
            return _node;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<XmlNode> CreateNode(string nodeName, List<string> text)
        {
            var _returnList = new List<XmlNode>();
            foreach (var item in text)
            {
                _returnList.Add(this.CreateNode(nodeName, item));
            }
            return _returnList;
        }

        /// <summary>
        /// 保存到指定文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            document.Save(fileName);
        }

        /// <summary>
        /// 保存到文件流中
        /// </summary>
        /// <param name="s"></param>
        public void Save(Stream s)
        {
            document.Save(s);
        }
        #endregion
    }
}