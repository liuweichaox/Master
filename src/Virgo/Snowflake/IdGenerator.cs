namespace Virgo.Snowflake
{
    /// <summary>
    /// 雪花ID生成器
    /// </summary>
    public class IdGenerator
    {
        IdGenerator() { }

        public static IdWorker Instance = new IdWorker(1, 1);
    }
}
