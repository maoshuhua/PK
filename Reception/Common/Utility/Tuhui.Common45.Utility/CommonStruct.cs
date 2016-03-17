using System;
using System.Xml.Linq;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：CommonCode
    /// <summary>
    /// 全局常量及枚举定义
    /// </summary>
    /// <remarks>
    /// 全局常量及枚举定义
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        李根华           新建
    /// =======================================================================
    public partial class CommonCode
    {
        /// <summary>
        /// 全局常量
        /// </summary>
        public struct GlobalConstant
        {
            /// <summary>
            /// 登录用户Session键
            /// </summary>
            public const string UserSessionKey = "LoginUser";

            /// <summary>
            /// 用户存储rp的cookie
            /// </summary>
            public const string SiteCookieName = "JSESTSSiteCookie";

            /// <summary>
            ///被动认证STS名称 
            /// </summary>
            public const string IssuerName = "JSEPassiveSTS";

            /// <summary>
            ///JSESecurityTokenService存放于httpcontext中的key 
            /// </summary>
            public const string JSESecurityTokenServiceConfigurationKey = "JSESecurityTokenServiceConfigurationKey";

            /// <summary>
            /// Oauth登录用户SessionKey
            /// </summary>
            public const string OauthUserSessionKey = "SessionKey_OauthUser";

            /// <summary>
            /// Oauth授权应用信息
            /// </summary>
            public const string OauthClientInfo = "SessionKey_OauthClient";

            /// <summary>
            /// 请求Token Session键
            /// </summary>
            public const string Request_Token_Session = "Request_Token";

            /// <summary>
            /// 请求Token密钥 Session键
            /// </summary>
            public const string Request_TokenSecret_Session = "Request_TokenSecret";

            /// <summary>
            /// 授权Token Session键
            /// </summary>
            public const string Access_Token_Session = "Access_Token";

            /// <summary>
            /// 授权Token密钥Session键
            /// </summary>
            public const string Access_TokenSecret_Session = "Access_TokenSecret";

            /// <summary>
            /// MessageID的长度
            /// </summary>
            public const int MessageIDLength = 7;

            /// <summary>
            /// 平台应用的提供商代码
            /// </summary>
            public const string PlatformProvider = "0000000000";

            /// <summary>
            /// 平台应用的提供商名称
            /// </summary>
            public const string PlatformProviderName = "教育平台";

            /// <summary>
            /// 从不同系统进入后共通样式（此cookie记录从何系统进入），无值时用默认样式
            /// </summary>
            public const string Logon_Style_Cookie = "jse_logon_style";
        }

        public struct ConfigName {

            public const string NLogConfig = "Config/NLog.config";

            public const string EnvironmentGlobal = "Config/Environment_Global.config";

            public const string EnvironmentNameValue = "Config/Environment_NameValue.config";

            public const string EnvironmentCache = "Config/Environment_Cache.config";


        }


        /// <summary>
        /// 验证常量
        /// </summary>
        public struct ValidConstant
        {
            /// <summary>
            /// 身份证验证常量
            /// </summary>
            public static string[] IdenityNum = new string[] { 
                "11", "12", "13", "14", "15",
                "21", "22", "23",
                "31", "32", "33", "34", "35", "36", "37",
                "41", "42", "43","44","45","46",
                "50","51","52","53","54",
                "61","62","63","64","65",
                "71",
                "81","82"};

            /// <summary>
            /// 17位数对应的加权因子，自左至右
            /// </summary>
            public static int[] WI = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

            /// <summary>
            /// 余数对应的校验码
            /// </summary>
            public static string[] VI = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
        }

        /// <summary>
        /// 全局消息常量
        /// </summary>
        public struct GlobalConstantMessage
        {
            /// <summary>
            /// 重新加载环境配置文件成功
            /// </summary>
            public const string ReloadEnvironmentSuccess = "重新加载环境配置文件成功";

            /// <summary>
            /// 缓存清除成功
            /// </summary>
            public const string ClearCacheSuccess = "缓存清除成功";

            /// <summary>
            /// 缓存清除失败,详细请见网站错误日志
            /// </summary>
            public const string ClearCacheFail = "缓存清除失败,详细请见网站错误日志";
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        public struct IDType
        {
            /// <summary>
            /// 01 居民身份证
            /// </summary>
            public const string IDCard = "01";
            /// <summary>
            /// 02 临时身份证
            /// </summary>
            public const string TempIDCard = "02";
            /// <summary>
            /// 03 护照
            /// </summary>
            public const string Passport = "03";
            /// <summary>
            /// 04 户口簿
            /// </summary>
            public const string Account = "04";
            /// <summary>
            /// 05 军人身份证
            /// </summary>
            public const string MilitaryID = "05";
            /// <summary>
            /// 06 武装警察身份证
            /// </summary>
            public const string PoliceID = "06";
            /// <summary>
            /// 07 港澳台居民往来内地通行证
            /// </summary>
            public const string HKID = "07";
            /// <summary>
            /// 08 外交人员身份证
            /// </summary>
            public const string DiplomatID = "08";
            /// <summary>
            /// 09 外国人居留许可证
            /// </summary>
            public const string ForeignerID = "09";
            /// <summary>
            /// 10 边民出入境通行证
            /// </summary>
            public const string BorderID = "10";
            /// <summary>
            /// 其它
            /// </summary>
            public const string Other = "11";
        }

        /// <summary>
        /// 登录使用的证书
        /// </summary>
        public struct SSOCert
        {
            /// <summary>
            /// 证书subject
            /// </summary>
            public const string Subject = "CN=JSESTSCERT";

            /// <summary>
            /// 证书rawdata
            /// </summary>
            public const string RawDataString =
                "MIIKLwIBAzCCCesGCSqGSIb3DQEHAaCCCdwEggnYMIIJ1DCCBhUGCSqGSIb3DQEHAaCCBgYEggYCMIIF/jCCBfoGCyqGSIb3DQEMCgECoIIE/jCCBPowHAYKKoZIhvcN" +
                "AQwBAzAOBAh0GRfUOVOxXAICB9AEggTYRZE2sr0NG9pHJfyHtLlWDzwMckL4AXEwsFYcjCRhJm5djbV9WYOYLuGWL4AxAr+1wtyDdP5byz+vI+pCIyEwpiTaNpIK2Plv" +
                "WiufKSmVUA2LP8GzL9twBfXm5bK5u7yvm9u7kaSMSZDr4sy1yBvZ/v6gNo0VaioaMIQTj10EvUZN5j3XkjRBsBjI+Jfe5aSQQag1YaoP7ZGxP6MT2u141xGV9Vw4BCgH" +
                "BT0Jxb/UOHgKCqB0LC/kA2I6yaUSo7Z2xMc7bRzMyJs7HtTOyxuFFSDxjjenbti00E4ApMiQ7MPzx9kiRHIGhUKwoptLtJZ2WiGWwz65gaVoac4MumKvWZ4nV5yYCQsi" +
                "etmzEuFKhhu50mkbwng4HMtlQqEL6zSSjcgY2M+8S98PMBxdy9i3dIKicTjpTUQyI6jb/tQzl8No3taYvzzLlz8IBWXSiT1PsOj3ebF7tmz6bWbWDu275ZlAFtVbQZva" +
                "1TyqYpdQJxseZOvctrJ7y+d51sY7vhKPGbJw8wEjoTOFyMN4glPbs70h58DG/P98hCjQLxE/5+Ptw7V2ajVHzlqyje+NKoz0cKmTXzhU8dUU3K/G8hi4ITMNtqzbH4yI" +
                "gIzCB/rS5y5IokxhRG1us5MKVpzOJh+l1HTfIH8JfIM0PjmhrZbZpfotDTjpJv+8N31dKfvULr9tfV2I5F/5p4HAuRzkcwRTggW/WgJJ/uFb0l7iJKkQnbQfUqnOjXMy" +
                "AjlTsTItEmmn/uAFDfgo0Tg+TuEb+X4yAOayqxEUQVZy7SDVhR3JQQVdQlZM0FRVqhPXfnEdeyoDMHlIgV3+LGSMq6STe4IqA478DaXmPDCvvY6+9fVn0HNZcP1yKpaT" +
                "l4IEJJdQXu7yHGh9+4x8ENFTjRLvF/+42lA8QApB3th2TgGZY0xdTC6JbERvIDb9DBm03G0T6xciQZfyjlcVZyQIEId18Knh97GRxkYSwiUV0j+XLV0l2m84O5mqa/mx" +
                "/tv+RaiF7JxixFH58N35gMqMGdHB/tnnIZ4fxwhrS0BdJ2aSTOhozCv/Qcuk1QpMw3J41+S+n/HVRdMwNgDay9SF0+5B5al3dcGug6E1rObYrSdQzPu5uMZK9YC1SvHC" +
                "FoXsHsAlcejOLvLF2LTiVM0CSxuHo2hnfaUEaNdGBDCHGCBPwDIRV/4SwGeAmzFwwzxWqiv2fH+iDsq5ViGiH0l7faThtO+wS6JcH6ivD/U6US7lnzd5+Bv7IcOt7/zH" +
                "x9jp+SXmIQ+Gh9cygZGOOM8a1Jbc+1ORRsKT8epbYtcBy45kxSmYV/obbWiqvVuoBzC8prJIxYpwmA4OO8IRnGEMRwtCP9pK2FZjzRBpNNYNWC5NbYuS3p4feYwzsFP9" +
                "xbTCkYLyK65XzAq+5OjtKukUoxw2nQ7LYf97xvhIqn7caC91+TvQ/ZQoQpP23NHGxGH19aeE9oafFScC4PkDWS6rhJRP5+MAsGazoUK3AD22Q6+h2uxXItTGO7VVRoVB" +
                "CdJpqadNNhY/s5qABlOLdg5MhOuTyYAiBIupsdZOCMz1rZ8pxPZfUytwgIaCsTRcHpczHhAw15x2qhYudIy7I8iEwirpJlAwXKhCz3ZSdrh6r60bm+Fq6nOBa76Vvvhi" +
                "8blU7zpw5hdXYySalzC+RzGB6DANBgkrBgEEAYI3EQIxADATBgkqhkiG9w0BCRUxBgQEAQAAADBdBgkqhkiG9w0BCRQxUB5OAGwAcAAtADIAMgBhADYAMgBiAGEAYQAt" +
                "ADUAYQBkAGQALQA0ADYAYgBjAC0AOQAyADkAMQAtADMAMAAzADIAMAAwAGIAMgA5AGEAYwAyMGMGCSsGAQQBgjcRATFWHlQATQBpAGMAcgBvAHMAbwBmAHQAIABCAGEA" +
                "cwBlACAAQwByAHkAcAB0AG8AZwByAGEAcABoAGkAYwAgAFAAcgBvAHYAaQBkAGUAcgAgAHYAMQAuADAwggO3BgkqhkiG9w0BBwagggOoMIIDpAIBADCCA50GCSqGSIb3" +
                "DQEHATAcBgoqhkiG9w0BDAEGMA4ECPpaKNnSq6ZAAgIH0ICCA3C7NNbjSji82NheXARfF/W8p6/sqJCvR8cQOrkxjK1WO1bBau1kLBsfXlEWCJ05Si2PaIUYQ3IFUY2A" +
                "ZDMDHmr1MrBwSRozY37yFoPzjQcyY1uA23SjY4Rn+wgcIcLElO4+x0+Spvz0bM8MHaP5WwKpzRga52+Gzn2WWWbfjQMHq3eF5pAeS9aBldNCyceTXdA+//JXONi75fRo" +
                "qbPFD233Knfy2M11+L936tMg6irwA8ouZIFSJ7kVJnWeWRukAR2e5L2CTCVvpseGGqs7qldSY06eM3h910KNnF1xXzOM0sR+fsUEhcpJopOH6B9qS7teoIGM/6iDD1kR" +
                "zjyqSga/66VD9Xlr6XXFLE1qQbQ2uVUC/qFtxvJIVzzI45owfLdRx+9z2D1IDMGZKsKjYES1ghAqs1ARaCh0GLl96puSmcExbYnyPMjafE5I96hld8oIIJwpyYLp5Rzx" +
                "3hm2nwmyFRdv7PkQWNheOxFI/iRlEZ/MOzdG4jcL2wm/KJ4hpVM5DquMDrjgy61m2EovcqrAFHCkIOrnukVwdaeRmeSIRyPHp604LCxGhnsqefBro1Fu53MB/hUsLJgD" +
                "BoA5JbVjMfwipEEHbOZnGiqANRT/GvQIXFU5ezz5ow5kDsF97jptRMZLZlMVoEhAP+KuCKw0+qMx9QslzA0nDrk9j64A79yU7cdtxZ9y/i3MUt0Bl6YKnDKp1PCmQhDa" +
                "frCIASnfM3ZWz5C+BDZl3Ty3o8VDWUw/vFiiv+E9OQY+A3Zedj7ozbDqLxMSa2IKsB4tllji4stxHiTRaImsNv/IBQZMHjju7KvkkfNA2EGWGeTHB9TzK+Gg6CpiKOTI" +
                "eIRMj85p1p6hS1FmzXYHuag65LxkUhtEl82Mba3LhAYuaKFkz4mKFxdnSuJZtI0Hxvn3TFl05faUoibhBHJekP6tSG+hzfMXsNSPqOI25vMrpywiOpXX/HaMsMzIsc/R" +
                "egqaEf3pmKY9J10N1SmaYNRtl3Osti29DSUae4oGa2eF6MonEPyUxTmB2P65boIjfppZ+PK6ms59TzLZJ0zUgNKzkP0WcBJeSSMTbRJ47+qTCBlSwszW9YDIvwSOl67e" +
                "BRm+cCTBW+oxDYdkNgMhQjhlgjN9Z0WDYPS2ts9OPLJzrfgpb3COQp3TqCvdwJPdKqsfxolXMDswHzAHBgUrDgMCGgQUouRURzEfGa4b20U8/oZDYdHpbmgEFD8o9y6/" +
                "wFX99Lm3AKqjOKURrVTpAgIH0A==";

            /// <summary>
            /// 证书指纹
            /// </summary>
            public const string Thumbprint = "C9205AFD1EC47327DA48F17737ABE45F30690EF8";


            /// <summary>
            /// rp使用的加密秘钥
            /// </summary>
            public const string SymmetricKey = "+zqf97FD/xyzzyplugh42ploverFeeFieFoeFooxqjE=";
        }

        /// <summary>
        /// 静态文件对应的路径
        /// </summary>
        public struct JSECommResource
        {
            /// <summary>
            /// 共通css;虚拟路径~/content/jsecomm
            /// </summary>
            public const string CommContent = "css/jsecomm";

            /// <summary>
            /// bootstrap脚本；虚拟路径~/bundles/bootstrap
            /// </summary>
            public const string Bootstrap = "bundles/bootstrap";

            /// <summary>
            /// angular脚本；虚拟路径~/bundles/angular
            /// </summary>
            public const string Angular = "bundles/angular";

            /// <summary>
            /// jquery脚本；虚拟路径~/bundles/jquery
            /// </summary>
            public const string Jquery = "bundles/jquery";

            /// <summary>
            /// json2脚本；虚拟路径~/bundles/json2
            /// </summary>
            public const string Json2 = "bundles/json2";

            /// <summary>
            /// 共通脚本；虚拟路径~/bundles/jsecomm
            /// </summary>
            public const string CommJs = "bundles/jsecomm";

            /// <summary>
            /// 共通插件；虚拟路径~/bundles/jse-ui
            /// </summary>
            public const string JseUI = "bundles/jse-ui";

            /// <summary>
            /// 百度社会服务脚本；虚拟路径~/bundle/frontia
            /// </summary>
            public const string Frontia = "bundles/frontia";

            /// <summary>
            /// UpLoad脚本；虚拟路径~/bundle/upload
            /// </summary>
            public const string UpLoad = "bundles/upload";


        }
    }
}