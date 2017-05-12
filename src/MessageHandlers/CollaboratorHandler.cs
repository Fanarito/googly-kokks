using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;
using Kokks.Models;

namespace Kokks.Handlers
{
    public class CollaboratorHandler : WebSocketHandler
    {
        public CollaboratorHandler(
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

        public async Task Add(Collaborator project)
        {
            await InvokeClientMethodToAllAsync("add", project);
        }

        public async Task Remove(Collaborator project)
        {
            await InvokeClientMethodToAllAsync("delete", project);
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
