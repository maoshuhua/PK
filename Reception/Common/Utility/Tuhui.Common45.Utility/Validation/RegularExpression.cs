namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 结构体名：RegularExpression
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    /// <remarks>
    /// 常用正则表达式
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        叶斌成           新建
    /// =======================================================================
    public struct RegularExpression
    {
        /// <summary>
        /// 匹配空白行
        /// </summary>
        public const string Empty = "\n\\s*\r";

        /// <summary>
        /// 匹配中文表达式
        /// </summary>
        public const string Chinese = "[\u4e00-\u9fa5]";

        /// <summary>
        /// 匹配Email地址的正则表达式
        /// </summary>
        public const string Email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// 匹配网址URL的正则表达式
        /// </summary>
        public const string Url = @"^([hH][tT][tT][pP]|[hH][tT][tT][pP][sS])\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)?((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.[a-zA-Z]{2,4})(\:[0-9]+)?(/[^/][a-zA-Z0-9\.\,\?\'\\/\+&amp;%\$#\=~_\-@]*)*$";

        /// <summary>
        /// 匹配价格的正则表达式(格式0.00)
        /// </summary>
        public const string Price = @"^(([1-9]\d*)|0)(\.\d{1,2})?$";

        /// <summary>
        /// 匹配手机号码的正则表达式
        /// </summary>
        //public const string Mobile = @"^(13[0-9]|15[0|3|6|7|8|9]|18[6|7|8|9])\d{8}$";
        public const string Mobile = @"^(1(([35][0-9])|(45)|(47)|[89][012356789]))\d{8}$";

        /// <summary>
        /// 匹配联通手机号码的正则表达式
        /// </summary> 
        public const string UnicomMobile = @"^1(3[0-2]|5[56]|8[56])\d{8}$";

        /// <summary>
        /// 电信手机号码
        /// </summary>
        public const string TelecomMobile = @"^(180|181|189|133|153)\d{8}$";

        /// <summary>
        /// 匹配中国邮政编码
        /// </summary>
        public const string Postal = "[1-9]\\d{5}(?!\\d)";

        /// <summary>
        /// 匹配身份证
        /// </summary>
        public const string Identity = "\\d{15}|\\d{18}";

        /// <summary>
        /// 匹配ip地址
        /// </summary>
        public const string IP = "\\d+\\.\\d+\\.\\d+\\.\\d+";

        /// <summary>
        /// 匹配整数
        /// </summary>
        public const string Integer = "^-?[1-9]\\d*$";

        /// <summary>
        /// 匹配正整数
        /// </summary>
        public const string Integer_Positive = "^[1-9]\\d*$";

        /// <summary>
        /// 匹配负整数
        /// </summary>
        public const string Integer_Negative = "^-[1-9]\\d*$";

        /// <summary>
        /// 匹配非负整数(即正整数+0)
        /// </summary>
        public const string Integer_Positive_Zero = "^[1-9]\\d*|0$";

        /// <summary>
        /// 匹配非正整数(即负整数+0)
        /// </summary>
        public const string Integer_Negative_Zero = "^-[1-9]\\d*|0$";

        /// <summary>
        /// 验证日期
        /// </summary>
        public const string Date = "^((([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})"
            + "[\\/](((0[13578]|1[02])[\\/](0[1-9]|[12][0-9]|3[01]))|((0[469]|11)[\\/](0[1-9]|[12][0-9]|30))|"
            + "(02[\\/](0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|"
            + "[3579][26])00))[\\/]02[\\/]29))$";

        /// <summary>
        /// 验证QQ
        /// </summary>
        public const string QQ = @"(^[1-9](\d){4,12}$)|(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";

        /// <summary>
        /// 验证MSN
        /// </summary>
        public const string MSN = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// 登录名正则验证
        /// </summary>
        //public const string UserName = @"^\w{2,20}$";
        //public const string UserName = @"^(?!\d*$)[A-Za-z0-9_]{2,20}$";
        public const string UserName = @"^[a-zA-Z_\u4e00-\u9fa5][a-zA-Z0-9_\u4e00-\u9fa5]{1,19}$";

        /// <summary>
        /// 密码正则验证
        /// </summary>
        public const string PassWord = @"^[\x21-\x7f]{6,16}$";

        /// <summary>
        /// 昵称正则验证
        /// </summary>
        public const string NickName = @"^[\u4E00-\u9FA5A-Za-z0-9]{2,30}$";

        /// <summary>
        /// 姓名正则验证
        /// </summary>
        public const string Name = @"^[\u4e00-\u9fa5A-Za-z0-9]{1,30}(?:\u00B7[\u4e00-\u9fa5A-Za-z0-9]{2,30}){0,1}$";

        /// <summary>
        /// 电话号码
        /// </summary>
        public const string Telephone = @"^(\d{11})|^((\d{7,8})|(\d{4}|\d{3})[-_－—](\d{7,8})|(\d{4}|\d{3})[-_－—](\d{7,8})[-_－—](\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})[-_－—](\d{4}|\d{3}|\d{2}|\d{1}))$";

        /// <summary>
        /// 教育卡
        /// </summary>
        public const string CardNo= @"^\d{19}$";

        /// <summary>
        /// 物理卡号
        /// </summary>
        public const string CardPNo = @"^\d{10}";     
    }
}