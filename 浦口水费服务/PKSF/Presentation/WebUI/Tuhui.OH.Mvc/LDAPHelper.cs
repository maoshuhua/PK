using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tuhui.Common.Mvc;
using Tuhui.Common.Environment;
using System.Web.Security;
using System.DirectoryServices;
using System.Configuration;
using System.Collections;

using System.Reflection;
using System.IO;


namespace Tuhui.OH.Mvc
{
    public class LDAPHelper
    {

        //public bool VerifyUser(string strUserName, string strPwd)
        //{
        //    DirectoryEntry root = null;
        //    string domain = ConfigurationManager.AppSettings["IP"];
        //    string usergroup = ConfigurationManager.AppSettings["ldap_usergroup"];
        //    string ldap_username = ConfigurationManager.AppSettings["ldap_UserName"];
        //    string ldap_password = ConfigurationManager.AppSettings["ldap_PassWord"];
        //    string dn = ConfigurationManager.AppSettings["dn"];
        //    try
        //    {
        //        root = new DirectoryEntry(domain+"/"+dn, strUserName, strPwd, AuthenticationTypes.None);
        //        string strName = root.Name;//失败，会抛出异常  
        //        root.Close();
        //        root = null;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public bool VerifyUser(string strUserName, string strPwd)
        {
            DirectoryEntry root = null;
            string domain = ConfigurationManager.AppSettings["IP"];
            string usergroup = ConfigurationManager.AppSettings["ldap_usergroup"];
            string ldap_username = ConfigurationManager.AppSettings["ldap_UserName"];
            string ldap_password = ConfigurationManager.AppSettings["ldap_PassWord"];
            string dn = ConfigurationManager.AppSettings["dn"];
            string ldapPath = domain;
            string domainAndUsername = strUserName;
            DirectoryEntry entry = new DirectoryEntry(ldapPath, domainAndUsername, strPwd);

            DirectorySearcher search = new DirectorySearcher(entry);

            try
            {
                SearchResult result = search.FindOne();

                return true;
            }
            catch (Exception ex)
            {
                string path =System.Web.HttpContext.Current.Server.MapPath("~/");
                WriteFile(ex.Message + "\n" + ex.Source+"\n"+ex.StackTrace, path  +"\\ex.txt");
                return false;
            }

        }

        /// <summary>
        /// 写入文件 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="FileSavePath"></param>
        public static void WriteFile(string Content, string FileSavePath)
        {
            if (File.Exists(FileSavePath))
            {
                File.Delete(FileSavePath);
            }
            FileStream fs = File.Create(FileSavePath);
            Byte[] bContent = System.Text.Encoding.UTF8.GetBytes(Content);
            fs.Write(bContent, 0, bContent.Length);
            fs.Close();
            fs.Dispose();
        }
        /*
        public bool VerifyUser(string strUserName, string strPwd)
        {
            string domain = ConfigurationManager.AppSettings["IP"];
            string usergroup = ConfigurationManager.AppSettings["ldap_usergroup"];
            string ldap_username = ConfigurationManager.AppSettings["ldap_UserName"];
            string ldap_password = ConfigurationManager.AppSettings["ldap_PassWord"];
            string dn = ConfigurationManager.AppSettings["dn"];

            DirectoryEntry entry = new DirectoryEntry(domain + "/" + dn, ldap_username, ldap_password, AuthenticationTypes.None);

            try
            {
                object native = entry.NativeObject;
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = "(cn=" + strUserName + ")";
                searcher.PropertiesToLoad.Add("cn");
                SearchResultCollection ret = searcher.FindAll();
                foreach (SearchResult sr in ret)
                {
                    if (sr != null)
                    {
                        string strPath = sr.Path;
                        int nIndex = strPath.LastIndexOf("/");
                        if (nIndex > 0)
                        {
                            strPath = strPath.Substring(nIndex + 1, strPath.Length - nIndex - 1);
                            entry = new DirectoryEntry(sr.Path, strPath, strPwd, AuthenticationTypes.None);
                            try
                            {
                                object native1 = entry.NativeObject;
                                return true;
                            }
                            catch (System.Exception ex)
                            {
                                return false;
                            }
                        }
                    }
                }

                if (ret.Count > 0)
                    return false; //invalid password
            }
            catch (System.Exception ex)
            {
                throw new Exception("Error authenticating user." + ex.Message);
            }
            return false; //invalid user
        }
         */


        //public List<string> FindUsers()
        //{
        //    string domain = ConfigurationManager.AppSettings["IP"];
        //    string usergroup = ConfigurationManager.AppSettings["ldap_usergroup"];
        //    string username = ConfigurationManager.AppSettings["ldap_UserName"];
        //    string password = ConfigurationManager.AppSettings["ldap_PassWord"];
        //    try
        //    {
        //        List<string> list = new List<string>();
        //        if (username == null || password.Trim().Length == 0)
        //        {
        //            return null;
        //        }

        //        DirectoryEntry de = new DirectoryEntry(domain, username, password);
        //        DirectorySearcher ds = new DirectorySearcher(de);

        //        ds.Filter = "(&(objectClass=group)(SAMAccountName=" + usergroup + "))";

        //        SearchResult results = ds.FindOne();
        //        if (results != null)
        //        {
        //            DirectoryEntry dirEntry = new DirectoryEntry(results.Path, username, password);
        //            PropertyCollection propertyCollection = dirEntry.Properties;
        //            int count = propertyCollection["member"].Count;
        //            for (int i = 0; i < count; i++)
        //            {
        //                string respath = results.Path;
        //                string[] pathnavigate = respath.Split("CN".ToCharArray());
        //                respath = pathnavigate[0];
        //                string objpath = propertyCollection["member"][i].ToString();
        //                string path = respath + objpath;
        //                DirectoryEntry user = new DirectoryEntry(path, username, password);
        //                if (user.Properties.Contains("Name"))
        //                {
        //                    var ss = user.Properties["Name"][0].ToString();
        //                    list.Add(ss);
        //                }
        //            }
        //        }
        //        return list;

        //        //object obj = System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(entry.NativeObject);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// 查找所有ad用户
        /// </summary>
        /// <returns></returns>
        public List<string> FindUsers()
        {
            string domain = ConfigurationManager.AppSettings["IP"];
            string usergroup = ConfigurationManager.AppSettings["ldap_usergroup"];
            string username = ConfigurationManager.AppSettings["ldap_UserName"];
            string password = ConfigurationManager.AppSettings["ldap_PassWord"];
            string dn1 = ConfigurationManager.AppSettings["dn1"];
            List<string> lst = new List<string>();
            //DirectoryEntry entry = new DirectoryEntry(domain + "/" + dn, username, password, AuthenticationTypes.None);
            DirectoryEntry entry = new DirectoryEntry(domain, username, password, AuthenticationTypes.None);
            try
            {
                object native = entry.NativeObject;
                DirectorySearcher searcher = new DirectorySearcher(entry);
                //searcher.Filter = "(objectClass=user)";
                searcher.Filter = "(&(objectCategory=person)(memberof=" + dn1 + "))";
                SearchResultCollection ret = searcher.FindAll();




                //searcher.Filter = "(objectClass=Users)";

                //searcher.SearchScope = SearchScope.Subtree;


                //searcher.Filter = "((objectCategory=person))";
                ////过滤从User组中得到人名
                //searcher.PropertiesToLoad.Add("name");//添加中文用户名
                //searcher.PropertiesToLoad.Add("samaccountname");//添加登陆名


                //SearchResultCollection ret = searcher.FindAll();
                foreach (SearchResult result in ret)
                {
                    ResultPropertyCollection props = result.Properties;
                    string name = "";
                    string accountname = "";
                    string memberof = "";
                    foreach (string propName in props.PropertyNames)
                    {
                        bool flag = false;
                        if (propName == "name")
                        {
                            name = props[propName][0].ToString();
                        }
                        if (propName == "samaccountname")
                        {
                            accountname = props[propName][0].ToString();

                        }
                        if (propName == "memberof")
                        {
                            memberof = props[propName][0].ToString();
                            flag = true;
                        }


                    }
                    lst.Add(accountname);

                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("错误信息：" + ex.Message);
            }
            return lst;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="usergroup"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void getGroupInfo(string domain, string usergroup, string username, string password)
        {
            DirectoryEntry de = new DirectoryEntry(usergroup, username, password);
            DirectorySearcher directorySearch = new DirectorySearcher(de);
            directorySearch.Filter = "(&(objectClass=group)(SAMAccountName=" + usergroup + "))";
            SearchResult results = directorySearch.FindOne();
            if (results != null)
            {
                DirectoryEntry dirEntry = new DirectoryEntry(results.Path, username, password);
                PropertyCollection propertyCollection = dirEntry.Properties;
                int count = propertyCollection["member"].Count;
                for (int i = 0; i < count; i++)
                {
                    string respath = results.Path;
                    string[] pathnavigate = respath.Split("CN".ToCharArray());
                    respath = pathnavigate[0];
                    string objpath = propertyCollection["member"][i].ToString();
                    string path = respath + objpath;
                    DirectoryEntry user = new DirectoryEntry(path, username, password);
                }
            }

        }


        /// <summary>
        /// 根据本地用户组获得组里的用户名数组
        /// </summary>
        /// <param name="localGroup">本地用户组</param>
        /// <returns>用户名数组</returns>
        static ArrayList GetUsersArrayList(DirectoryEntry directoryEntry)
        {
            ArrayList arrUsers = new ArrayList();

            try
            {

                foreach (object member in (IEnumerable)directoryEntry.Invoke("Members"))
                {
                    DirectoryEntry dirmem = new DirectoryEntry(member);
                    arrUsers.Add(dirmem.Name);
                }
                return arrUsers;
            }
            catch { return arrUsers; }

        }
    }
}