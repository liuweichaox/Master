using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Virgo.Extensions;

namespace Virgo.UserInterface.Middlewares
{
    /// <summary>
    /// Chat WebSocket中间件
    /// </summary>
    public class ChatWebSocketMiddleware
    {
        private static readonly ConcurrentDictionary<string, WebSocket> Sockets = new ConcurrentDictionary<string, WebSocket>();

        private readonly RequestDelegate _next;

        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            var ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            var socketId = context.Request.Query["UserId"].ToString();
            if (!string.IsNullOrWhiteSpace(socketId))
            {
                Sockets.AddOrUpdate(HttpUtility.UrlDecode(socketId), currentSocket, (key, websocket) => currentSocket);
            }
            while (!currentSocket.CloseStatus.HasValue)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                var response = await currentSocket.ReceiveStringAsync(ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    continue;
                }
                var msg = JsonConvert.DeserializeObject<MsgTemplate>(response);
                foreach (var (key, value) in Sockets)
                {
                    if (value.State != WebSocketState.Open)
                    {
                        continue;
                    }

                    if ((string.IsNullOrWhiteSpace(msg.ReceiverID) && key != socketId) || key == msg.ReceiverID)
                    {
                        await value.SendStringAsync(JsonConvert.SerializeObject(msg), ct);
                    }
                }
            }
            Sockets.TryRemove(socketId, out var webSocket);
            if (webSocket.CloseStatus != null)
                await currentSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, ct);
            currentSocket?.Dispose();
        }
    }
    /// <summary>
    /// 将中间件添加到HTTP请求管道
    /// </summary>
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseChatWebSocketMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ChatWebSocketMiddleware>();
        }
    }
    /// <summary>
    /// 消息模型
    /// </summary>
    public class MsgTemplate
    {
        /// <summary>
        /// 发送者ID
        /// </summary>
        public string SenderID { get; set; }
        /// <summary>
        /// 接收者ID
        /// </summary>
        public string ReceiverID { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
    }
}
