using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：SerializerUtility
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    /// <remarks>
    /// 序列化帮助类
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public static class SerializerUtility
    {
        /// <summary>
        /// 将一个json字符串反序列化为指定格式的对象
        /// </summary>
        /// <typeparam name="T">指定格式泛型</typeparam>
        /// <param name="json">需要被反序列化的json字符串</param>
        /// <returns>反序列化结果</returns>
        public static SerializerResult<T> DeSerializer<T>(string json)
        {
            
            var returnValue = new SerializerResult<T>();
            returnValue.Source = json;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                returnValue.Result = (T)serializer.ReadObject(ms);
                returnValue.State = true;
            }
            catch (Exception ex)
            {
                returnValue.State = false;
                returnValue.Exception = ex;
            }
            return returnValue;
        }


        /// <summary>
        /// 将请求序列化成xml
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string SerializeToXml<T>( T request )
        {

            string xmlRequest = "";
            MemoryStream stream = new MemoryStream();
            try {
                XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                serializer.Serialize( stream, request );
                stream.Position = 0;
                StreamReader reader = new StreamReader( stream );
                xmlRequest = reader.ReadToEnd();
            }
            finally {
                stream.Close();
            }
            return xmlRequest;
        }

        /// <summary>
        /// 将回执的xml反序列化
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static T DeSerializeXml<T>( string xmlStr )
        {
            StringReader reader = new StringReader( xmlStr );
            try {
                XmlSerializer serializer = new XmlSerializer( typeof( T ) );
                T result = (T) serializer.Deserialize( reader );
                return result;
            }
            finally {
                reader.Close();
            }
        }  

    }

    /// <summary>
    /// 反序列化结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SerializerResult<T>
    {
        /// <summary>
        /// 反序列化是否成功
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 失败时候的异常信息
        /// </summary>
        public System.Exception Exception { get; set; }

        /// <summary>
        /// 原始Json字符串
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 反序列化成功时对应的输出对象
        /// </summary>
        public T Result { get; set; }
    }



}
