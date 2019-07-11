using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Virgo.Net.Http
{
    /// <summary>
    /// <see cref="WebSocket"/>帮助类
    /// </summary>
    public class WebSocketsHelper
    {
        /// <summary>
        /// 客户端连接字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new ConcurrentDictionary<string, WebSocket>();

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="webSocket">WebSocket客户端</param>
        /// <returns>是否成功</returns>
        public bool AddClient(string userId, WebSocket webSocket)
        {
            if (!string.IsNullOrWhiteSpace(userId) && Sockets.ContainsKey(userId))
                return webSocket != null && Sockets.TryAdd(userId, webSocket);
            return false;
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>WebSocket客户端</returns>
        public WebSocket RemoveClient(string userId)
        {
            Sockets.TryRemove(userId ?? throw new ArgumentNullException(nameof(userId)), out var webSocket);
            return webSocket;
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>WebSocket客户端</returns>
        public WebSocket GetClient(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null;
            return Sockets.TryGetValue(userId, out var webSocket) ? webSocket : null;
        }
        /// <summary>
        /// 获取所有客户端
        /// </summary>
        /// <returns></returns>
        public List<WebSocket> All()
        {
            return Sockets.Select(s => s.Value).ToList();
        }
    }
}
