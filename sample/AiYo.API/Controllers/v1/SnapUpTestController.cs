using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;
using Virgo.AspNetCore.ApiResults;
using Virgo.Redis;

namespace Virgo.UserInterface.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class SnapUpTestController : ApplicationController
    {
        private readonly IDatabase _redis;
        private readonly ILock _redisLock;
        private const string CommodityKey = "CommodityKey";
        private const string LockKey = $"Lock:{CommodityKey}";
        private const string RushBuySuccessUser = "RushBuySuccess";
        private const int LockKeyOutTime = 30;

        public SnapUpTestController(ILock redisLock, IRedisCacheProvider redisCacheProvider)
        {
            _redisLock = redisLock;
            _redis = redisCacheProvider.GetDatabase();
        }
        /// <summary>
        /// 模拟抢购
        /// </summary>
        [HttpGet]
        public void SnapUpTest()
        {
           
            Console.WriteLine($"抢购开始——————:{DateTime.Now.ToLongTimeString()}");
            Parallel.For(0, 1000, _ => //模拟N个人，多线程异步
            {
                for (int i = 0; i < 10; i++) //模拟一个人点击N次，每次间隔50毫秒
                {
                    AsyncContext.Run(SnapUpCommodity);
                    Thread.Sleep(50);
                }
            });
            Console.WriteLine($"抢购结束——————:{DateTime.Now.ToLongTimeString()}");
        }
        /// <summary>
        /// 设置Redis
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        [HttpPost]
        public string SendRedis([FromBody] JObject jObject)
        {
            if (!jObject.HasValues)
            {
                return "数据错误！";
            }
            string key = jObject.Value<string>("key") ?? "Test1";
            string value = jObject.Value<string>("value") ?? "Value1";
            _redis.StringSet(key, value);
            return _redis.StringGet(key, CommandFlags.PreferSlave);
        }
        /// <summary>
        /// 抢购商品
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private async Task SnapUpCommodity()
        {
            string guid = Guid.NewGuid().ToString();//设置唯一key，互斥锁，只能解锁自己上的锁
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString(); //线程id 模拟UserID
            var commoditySum = int.Parse(await _redis.StringGetAsync(CommodityKey, CommandFlags.PreferSlave)); //模拟库存库存 如果库存不足，则直接返回
            if (commoditySum <= 0 || _redis.HashExists(RushBuySuccessUser, threadId, CommandFlags.PreferSlave))   //根据业务，防止超抢情况，如果抢购成功则直接返回
            {
                return;
            }
            var lockResult = await _redisLock.LockTakeAsync(LockKey, guid, LockKeyOutTime);//获取锁，设置定期时间，一般为30秒，防止执行过程中服务器宕机导致死锁
            if (lockResult) //如果获取成功则执行业务
            {
                try
                {
                    _redisLock.LockWatchDogStart(LockKey, guid, LockKeyOutTime);//开启看门狗
                    Thread.Sleep(1000);//模拟耗时操作
                    await RushBuySuccess();//模拟抢购成功，减库存
                }
                finally
                {
                    _redisLock.LockWatchDogStop();//关闭看门狗
                    _redis.HashIncrement(RushBuySuccessUser, threadId);//如果抢购成功则存入hashkey，避免超抢
                    await _redisLock.LockReleaseAsync(LockKey, guid);//抢购成功则释放锁
                    Console.WriteLine($"用户：{threadId}已抢到商品，时间为:{DateTime.Now}");
                }
            }
        }

        /// <summary>
        /// 抢购商品
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private async Task RushBuySuccess()
        {
            await _redis.StringDecrementAsync(CommodityKey);
        }
    }
}
