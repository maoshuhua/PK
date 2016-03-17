using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Tuhui.Common45.Utility
{
    /// <summary>
    /// WebRequest请求类
    /// </summary>
    public static class WebRequestHandle
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">数据</param>
        /// <returns>GET请求结果</returns>
        public static HttpResponseInfo Get(string url, object data = null)
        {
            string queryString = CreateQueryString(data);
            if (!string.IsNullOrEmpty(queryString))
            {
                url = url.Contains('?') ? url + "&" + queryString : url + "?" + queryString;
            }
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "GET";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            return GetHttpWebResponse(httpWebRequest)
                .AddHttpRequestInfo(url, Method.GET, queryString);
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <typeparam name="T">返回结果类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="data">参数</param>
        /// <returns>请求返回数据</returns>
        public static HttpResponseJsonInfo<T> Get<T>(string url, object data = null)
        {
            return new HttpResponseJsonInfo<T>(Get(url, data));
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <returns>返回结果</returns>
        public static HttpResponseInfo Post(string url, object data = null)
        {
            string queryString = CreateQueryString(data);
            var httpWebRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            if (!string.IsNullOrEmpty(queryString))
            {
                var requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(queryString);
                }
                finally
                {
                    requestWriter.Close();
                }
            }

            return GetHttpWebResponse(httpWebRequest)
                .AddHttpRequestInfo(url, Method.POST, queryString);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <returns>请求返回结果</returns>
        public static HttpResponseJsonInfo<T> Post<T>(string url, object data = null)
        {
            return new HttpResponseJsonInfo<T>(Post(url, data));
        }

        /// <summary>
        /// Request请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">GET还是POST方式</param>
        /// <param name="data">请求参数</param>
        /// <returns>返回内容</returns>
        public static HttpResponseInfo Request(string url, Method method, object data = null)
        {
            if (method == Method.GET) return Get(url, data);
            if (method == Method.POST) return Post(url, data);
            return null;
        }

        /// <summary>
        /// Request请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="method">GET还是POST方式</param>
        /// <param name="data">请求参数</param>
        /// <returns>返回内容</returns>
        public static HttpResponseJsonInfo<T> Request<T>(string url, Method method, object data = null)
        {
            if (method == Method.GET) return Get<T>(url, data);
            if (method == Method.POST) return Post<T>(url, data);
            return null;
        }

        private static string CreateQueryString(object data)
        {
            if (data == null) return string.Empty;
            if (data is string) return data as string;
            var dic = ObjectUtility.AnonymousObjectToHtmlAttributes(data);
            if (dic == null || dic.Count == 0) return string.Empty;
            return string.Join("&", dic.Select(p => p.Key
                + "="
                + (p.Value == null ? string.Empty : p.Value.ToString())));
        }

        private static HttpResponseInfo GetHttpWebResponse(HttpWebRequest webRequest)
        {
            HttpResponseInfo info;
            StreamReader responseReader = null;
            WebResponse webresponse = null;
            Stream stream = null;
            try
            {
                webresponse = webRequest.GetResponse();
                stream = webresponse.GetResponseStream();
                responseReader = new StreamReader(stream);
                info = new HttpResponseInfo
                {
                    Success = true,
                    Response = responseReader.ReadToEnd()
                };
            }
            catch (WebException ex)
            {
                stream = ex.Response.GetResponseStream();
                responseReader = new StreamReader(stream);
                info = new HttpResponseInfo
                {
                    Success = false,
                    Exception = ex,
                    Response = responseReader.ReadToEnd()
                };
                info.CreateAPIError();
            }
            catch (Exception ex)
            {
                info = new HttpResponseInfo
                {
                    Success = false,
                    Exception = ex
                };
            }
            finally
            {
                if (responseReader != null) responseReader.Close();
                if (stream != null) stream.Close();
                if (webresponse != null) webresponse.Close();
            }

            return info;
        }

        /// <summary>
        /// 清空缓存地址
        /// </summary>
        /// <param name="url"></param>
        public static void Request_ClearCacheUrl(List<string> url)
        {
            if (url != null)
            {
                url.ForEach(p =>
                {
                    try
                    {
                        Get(p);
                    }
                    catch { }
                });
            }
        }
    }
}
