using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tuhui.Common45.Utility
{
    /// =======================================================================
    /// 类名：ValidatorUtility
    /// <summary>
    /// 验证方法
    /// </summary>
    /// <remarks>
    /// 验证方法
    /// </remarks>
    /// =======================================================================
    /// 更新履历
    /// 序号          修改日期          责任人           更新内容
    /// 001           2011/11/05        叶斌成           新建
    /// =======================================================================
    public static class ValidatorUtility
    {
        /// <summary>
        /// 判断是否是合法汉字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsChinese(string value)
        {
            return new Regex(RegularExpression.Chinese).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法Email地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            //return Regex.IsMatch(value,RegularExpression.Email,RegexOptions.ECMAScript);
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (value.Contains(" ")) return false;
            if (value.Length > 255) return false;
            return new Regex(RegularExpression.Email).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法请求Url
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(string value)
        {
            return new Regex(RegularExpression.Url).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法邮编
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPostal(string value)
        {
            return new Regex(RegularExpression.Postal).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法IP地址
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIP(string value)
        {
            return new Regex(RegularExpression.IP).Match(value).Success;

        }

        /// <summary>
        /// 判断是否是合法整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger(string value)
        {
            long _item = 0;
            return long.TryParse(value, out _item);
        }

        /// <summary>
        /// 判断是否是合法正整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger_Positive(string value)
        {
            long _item = 0;
            if (long.TryParse(value, out _item))
            {
                return _item > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是合法非负整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger_Positive_Zero(string value)
        {
            long _item = 0;
            if (long.TryParse(value, out _item))
            {
                return _item >= 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是合法负整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger_Negative(string value)
        {
            long _item = 0;
            if (long.TryParse(value, out _item))
            {
                return _item < 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是合法非正整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger_Negative_Zero(string value)
        {
            long _item = 0;
            if (long.TryParse(value, out _item))
            {
                return _item >= 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否是合法手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMobile(string value)
        {
            return new Regex(RegularExpression.Mobile).Match(value).Success;
        }

        /// <summary>
        ///是否为电信号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTelecomMobile(string value)
        {
            return new Regex(RegularExpression.TelecomMobile).Match(value).Success;
        }

       /// <summary>
        /// 判断是否是联通合法手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUnicomMobile(string value)
        {
            return new Regex(RegularExpression.UnicomMobile).Match(value).Success;
        }



        /// <summary>
        /// 判断是否是合法教育卡号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCardNo(string value)
        {
            var result = new Regex(RegularExpression.CardNo).Match(value).Success;

            if (!result)
                return result;

            int year;

            // 判断年份是否正确
            if (int.TryParse(value.Substring(6,4),out year))
            {
                if ( year <1900 || year >2999)
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }

            // 判断身份是否正确,0:学生，1：老师
            if (value.Substring(10,1) != "0" &&  value.Substring(10,1) != "1")
            {
                return false;
            }

            // 判断性别是否正确，1：男，2：女
            if ( value.Substring( 11, 1 ) != "1" && value.Substring( 11, 1 ) != "2" )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断物理卡号格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static  bool IsCardPNo(string value)
        {
           return new Regex( RegularExpression.CardNo ).Match( value ).Success;
        }

        ///// <summary>
        ///// 判断是否是合法电话号码
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static bool IsTelephone(string value)
        //{
        //    return new Regex(RegularExpression.Telephone).Match(value).Success;
        //}

 /// <summary>
/// 判断是否是合法身份证号
 /// </summary>
        /// <param name="_value">身份证号</param>
 /// <param name="isValidVerifyCode">是否验证标志位，默认验证</param>
 /// <returns></returns>
        public static bool IsIdenity(string _value,bool isValidVerifyCode=true)
        {
            if (string.IsNullOrEmpty(_value)) return true;

            if (_value.Length == 15) _value = convertToEighteen(_value);

            if (_value.Length != 18) return false;

            _value = _value.ToUpper();

            try
            {
                //var regex = new Regex( @"/^\d{15}$|^\d{17}[\d|X|x]$/" );
                //var match = regex.Match( _value );
                //if ( !match.Success ) return false;

                if (!validCity(_value.Substring(0, 2))) return false;

                var birth = _value.Substring(6, 8);
                var year = birth.Substring(0, 4);
                var month = birth.Substring(4, 2);
                var day = birth.Substring(6, 2);

                if (!validDate(year, month, day)) return false;

                if(isValidVerifyCode)
                if (_value.Substring(17, 1) != validVerifyCode(_value)) return false;
            }
            catch
            {

                return false;
            }
            return true;
        }
               
        /// <summary>
        /// 判断是否是合法用户名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUserName(string value)
        {
            return new Regex(RegularExpression.UserName).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法姓名
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsName(string value)
        {
            return new Regex(RegularExpression.Name).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法昵称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNickName(string value)
        {
            return new Regex(RegularExpression.NickName).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法密码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPassword(string value)
        {
            return new Regex(RegularExpression.PassWord).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法QQ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsQQ(string value) {
            return new Regex(RegularExpression.QQ).Match(value).Success;
        }

        /// <summary>
        /// 判断是否是合法MSN
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMSN(string value)
        {
            return new Regex(RegularExpression.MSN).Match(value).Success;
        }

        #region 身份证验证
        private static string convertToEighteen(string _value)
        {
            var eightten = "";
            eightten = _value.Substring(0, 6) + "19" + _value.Substring(6);
            eightten += validVerifyCode(eightten);
            return eightten;
        }

        private static bool validCity(string code)
        {
            return CommonCode.ValidConstant.IdenityNum.Contains(code);
        }

        private static bool validDate(string year, string month, string day)
        {
            var dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            if (dt >= DateTime.Today) return false;
            if (dt.Year < 1900 || dt.Year > 2999) return false;
            if (dt.Month < 1 || dt.Month > 12) return false;
            if (dt.Day < 1 || dt.Day > 31) return false;
            return true;
        }

        private static string validVerifyCode(string idenityCode)
        {
            var checkCode = "";
            var checkCodeIndex = 0;
            try
            {
                idenityCode = idenityCode.Substring(0, 17);
                var sum = 0;
                var _char = idenityCode.ToCharArray();
                for (int i = 0; i < 17; i++)
                {
                    sum = sum + (CommonCode.ValidConstant.WI[i] * (int.Parse(_char[i].ToString())));
                }
                checkCodeIndex = sum % 11;
                checkCode = CommonCode.ValidConstant.VI[checkCodeIndex];
            }
            catch (System.Exception)
            {

                checkCode = "";
            }
            return checkCode;
        }
        #endregion
    }
}
