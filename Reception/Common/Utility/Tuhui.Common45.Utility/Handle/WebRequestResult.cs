using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Net;

namespace Tuhui.Common45.Utility
{
    /// <summary>
    /// Request请求方式
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// GET请求
        /// </summary>
        GET,
        /// <summary>
        /// POST请求
        /// </summary>
        POST,
    } ;
    /// <summary>
    /// Response信息
    /// </summary>
    public class HttpResponseInfo
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpResponseInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        public HttpResponseInfo(HttpResponseInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("HttpResponseInfo");

            this.Success = info.Success;
            this.Exception = info.Exception;
            this.Response = info.Response;
            this.Request = info.Request;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 如果请求未成功,保存请求异常信息
        /// </summary>
        public System.Exception Exception { get; set; }

        /// <summary>
        /// 如果是API内部发生错误.保存此错误
        /// </summary>
        public APIHttpError Error { get; set; }

        /// <summary>
        /// 无论请求成功还是请求失败,请求结果都在这里
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// 请求的发送消息
        /// </summary>
        public HttpRequestInfo Request { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 生成API请求错误对象
        /// </summary>
        public void CreateAPIError()
        {
            if (this.Success || !(this.Exception is WebException)) return;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(APIHttpError));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(this.Response));
                this.Error = (APIHttpError)serializer.ReadObject(ms);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Response对象中添加Request信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public HttpResponseInfo AddHttpRequestInfo(string url, Method method, string data)
        {
            this.Request = new HttpRequestInfo(url, method, data);
            return this;
        }
        #endregion

    }

    /// <summary>
    /// API请求错误结果类
    /// </summary>
    public class APIHttpError
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 错误编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 错误内容
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// 错误内容(中文)
        /// </summary>
        public string error_CN { get; set; }
    }

    /// <summary>
    /// Response返回类(带JSON反序列化对象)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpResponseJsonInfo<T> : HttpResponseInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        public HttpResponseJsonInfo(HttpResponseInfo info)
            : base(info)
        {
            if (info.Success)
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(info.Response));
                    this.Result = (T)serializer.ReadObject(ms);
                }
                catch (Exception ex)
                {
                    this.Success = false;
                    this.Exception = ex;
                }
            }
        }
        /// <summary>
        /// Json反序列化对象
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// Request信息类
    /// </summary>
    public class HttpRequestInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpRequestInfo()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_method"></param>
        /// <param name="_data"></param>
        public HttpRequestInfo(string _url, Method _method, string _data)
        {
            this.Url = _url;
            this.Method = _method;
            this.data = _data;
        }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string data { get; set; }
    }
}
