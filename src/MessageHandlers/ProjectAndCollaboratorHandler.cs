using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;
using Kokks.Models;
using System.Collections.Generic;

namespace Kokks.Handlers
{
    public class ProjectAndCollaboratorHandler : WebSocketHandler
    {
        public ProjectAndCollaboratorHandler(
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

        public async Task Add(long projectId)
        {
            await InvokeClientMethodToAllAsync("add", projectId);
        }

        public async Task Remove(long projectId, string name, long collaboratorId)
        {
            await InvokeClientMethodToAllAsync("delete", projectId, name, collaboratorId);
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
