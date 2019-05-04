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
            string socketId = context.Request.Cookies["UserId"]?.ToString();
            if (!string.IsNullOrWhiteSpace(socketId))
            {
                _sockets.AddOrUpdate(socketId, currentSocket, (key, websocket) => { websocket = currentSocket; return websocket; });
            }
            while (!currentSocket.CloseStatus.HasValue)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                string response = await ReceiveStringAsync(currentSocket, ct);
                MsgTemplate msg = JsonConvert.DeserializeObject<MsgTemplate>(response);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }
                    continue;
                }

                foreach (var socket in _sockets)
                {
                    if (socket.Value.State != WebSocketState.Open)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(msg.ReceiverID) || socket.Key == msg.ReceiverID)
                    {
                        await SendStringAsync(socket.Value, JsonConvert.SerializeObject(msg), ct);
                    }
                }
            }
            _sockets.TryRemove(socketId, out var webSocket);
            webSocket.Dispose();
            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
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
