using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;
using Kokks.Models;

namespace Kokks.Handlers
{
    public class FileHandler : WebSocketHandler
    {
        public FileHandler(
            WebSocketConnectionManager webSocketConnectionManager,
            ICollaboratorRepository collaboratorRepository
            ) : base(webSocketConnectionManager)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} is now connected"
            };

            await SendMessageToAllAsync(message);
        }

        public async Task Add(long fileId, string name, string content, Syntax syntax, long parentId, long projectId)
        {
            await InvokeClientMethodToAllAsync("add", fileId, name, content, syntax.ToString(), parentId, projectId);
        }

        public async Task Remove(long fileId, long parentId, long projectId)
        {
            await InvokeClientMethodToAllAsync("delete", fileId, parentId, projectId);
        }

        public async Task Change(long fileId, long parentId, long projectId, string userId, object change)
        {
            await InvokeClientMethodToAllAsync("change", fileId, parentId, projectId, userId, change);
        }

        public async Task Disconnect(string socketId)
        {
            var socket = WebSocketConnectionManager.GetSocketById(socketId);
            await OnDisconnected(socket);
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            await base.OnDisconnected(socket);

            var message = new Message()
            {
                MessageType = MessageType.Text,
                Data = $"{socketId} disconnected"
            };
            await SendMessageToAllAsync(message);
        }
    }
}
