using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Virgo.Extensions;

namespace Virgo.Web.Sample.Middlewares
{
    /// <summary>
    /// Chat WebSocket中间件
    /// </summary>
    public class ChatWebSocketMiddleware
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

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
            CancellationToken ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            var socketId = context.Request.Query["UserId"].ToString();
            if (!string.IsNullOrWhiteSpace(socketId))
            {
                _sockets.AddOrUpdate(HttpUtility.UrlDecode(socketId), currentSocket, (key, websocket) => currentSocket);
            }
            while (!currentSocket.CloseStatus.HasValue)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                string response = await currentSocket.ReceiveStringAsync(ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    continue;
                }
                MsgTemplate msg = JsonConvert.DeserializeObject<MsgTemplate>(response);
                foreach (var socket in _sockets)
                {
                    if (socket.Value.State != WebSocketState.Open)
                    {
                        continue;
                    }

                    if ((string.IsNullOrWhiteSpace(msg.ReceiverID) && socket.Key != socketId) || socket.Key == msg.ReceiverID)
                    {
                        await socket.Value.SendStringAsync(JsonConvert.SerializeObject(msg), ct);
                    }
                }
            }
            _sockets.TryRemove(socketId, out var webSocket);
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
