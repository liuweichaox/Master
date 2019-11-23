using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Virgo.Extensions
{
    /// <summary>
    /// <see cref="String"/>的扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// 判断字符串是否为空，或仅包含空格字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        public static string ToMd5(this string str)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(str);
            var hashBytes = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }

        #region 匹配Email

        /// <summary>
        /// 匹配Email
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Match MatchEmail(this string s, out bool isMatch)
        {
            Match match = Regex.Match(s, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            isMatch = match.Success;
            return isMatch ? match : null;
        }

        /// <summary>
        /// 匹配Email
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchEmail(this string s)
        {
            MatchEmail(s, out bool success);
            return success;
        }

        #endregion

        #region 匹配完整的URL

        /// <summary>
        /// 匹配完整格式的URL
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Uri MatchUrl(this string s, out bool isMatch)
        {
            try
            {
                isMatch = true;
                return new Uri(s);
            }
            catch (Exception e)
            {
                isMatch = false;
                return null;
            }
        }

        /// <summary>
        /// 匹配完整格式的URL
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchUrl(this string s)
        {
            MatchUrl(s, out var isMatch);
            return isMatch;
        }

        #endregion

        #region 权威校验身份证号码

        /// <summary>
        /// 根据GB11643-1999标准权威校验中国身份证号码的合法性
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchIdentifyCard(this string s)
        {
            if (s.Length == 18)
            {
                if (long.TryParse(s.Remove(17), out var n) == false || n < Math.Pow(10, 16) || long.TryParse(s.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false; //数字验证  
                }

                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(s.Remove(2), StringComparison.Ordinal) == -1)
                {
                    return false; //省份验证  
                }

                string birth = s.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time;
                if (!DateTime.TryParse(birth, out time))
                {
                    return false; //生日验证  
                }

                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] ai = s.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += wi[i].ToInt32() * ai[i].ToString().ToInt32();
                }

                int y;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != s.Substring(17, 1).ToLower())
                {
                    return false; //校验码验证  
                }

                return true; //符合GB11643-1999标准  
            }

            if (s.Length == 15)
            {
                if (long.TryParse(s, out var n) == false || n < Math.Pow(10, 14))
                {
                    return false; //数字验证  
                }

                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(s.Remove(2), StringComparison.Ordinal) == -1)
                {
                    return false; //省份验证  
                }

                string birth = s.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                if (DateTime.TryParse(birth, out _) == false)
                {
                    return false; //生日验证  
                }

                return true;
            }

            return false;
        }

        #endregion

        #region 校验IP地址的合法性

        /// <summary>
        /// 校验IP地址的正确性，同时支持IPv4和IPv6
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Match MatchInetAddress(this string s, out bool isMatch)
        {
            Match match;
            if (s.Contains(":"))
            {
                //IPv6
                match = Regex.Match(s, @"^([\da-fA-F]{0,4}:){1,7}[\da-fA-F]{1,4}$");
                isMatch = match.Success;
            }
            else
            {
                //IPv4
                match = Regex.Match(s, @"^(\d+)\.(\d+)\.(\d+)\.(\d+)$");
                isMatch = match.Success;
                foreach (Group m in match.Groups)
                {
                    if (m.Value.ToInt32() < 0 || m.Value.ToInt32() > 255)
                    {
                        isMatch = false;
                        break;
                    }
                }
            }

            return isMatch ? match : null;
        }

        /// <summary>
        /// 校验IP地址的正确性，同时支持IPv4和IPv6
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchInetAddress(this string s)
        {
            MatchInetAddress(s, out bool success);
            return success;
        }

        #endregion

        #region 校验手机号码的正确性

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <param name="isMatch">是否匹配成功，若返回true，则会得到一个Match对象，否则为null</param>
        /// <returns>匹配对象</returns>
        public static Match MatchPhoneNumber(this string s, out bool isMatch)
        {
            Match match = Regex.Match(s, @"^((1[3,5,8][0-9])|(14[5,7])|(17[0,1,3,6,7,8])|(19[8,9]))\d{8}$");
            isMatch = match.Success;
            return isMatch ? match : null;
        }

        /// <summary>
        /// 匹配手机号码
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchPhoneNumber(this string s)
        {
            MatchPhoneNumber(s, out bool success);
            return success;
        }

        #endregion

        /// <summary>
        /// 根据正则替换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex">正则表达式</param>
        /// <param name="replacement">新内容</param>
        /// <returns></returns>
        public static string Replace(this string input, Regex regex, string replacement)
        {
            return regex.Replace(input, replacement);
        }

        /// <summary>
        /// 严格比较两个对象是否是同一对象
        /// </summary>
        /// <param name="_this">自己</param>
        /// <param name="o">需要比较的对象</param>
        /// <returns>是否同一对象</returns>
        public new static bool ReferenceEquals(this object _this, object o) => object.ReferenceEquals(_this, o);
    }
}
