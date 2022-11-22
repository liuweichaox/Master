namespace Virgo.IP
{
    /// <summary>
    /// IP设置
    /// </summary>
    public class IpSettings
    {
        /// <summary>
        /// 默认语言代码，例如:zh-CN，en
        /// </summary>
        public static string DefaultLanguage = "zh-CN";

        /// <summary>
        /// 只有同时应用IPTools.International和IPTools.China才会生效。
        /// </summary>
        public static IpSearcherType DefalutSearcherType = IpSearcherType.China;

        /// <summary>
        /// 它可以使查询速度加倍。 只有使用IPTools.International才会生效。
        /// </summary>
        public static bool LoadInternationalDbToMemory = false;
    }
}
