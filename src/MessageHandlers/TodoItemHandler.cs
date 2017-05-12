using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketManager;
using WebSocketManager.Common;
using Kokks.Models;

namespace Kokks.Handlers
{
    public class TodoItemHandler : WebSocketHandler
    {
        public TodoItemHandler(
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

        public async Task AddTodo(int todoId, string name, long projectId)
        {
            await InvokeClientMethodToAllAsync("add", todoId, name, projectId);
        }

        public async Task DeleteTodo(int todoId, long projectId)
        {
            await InvokeClientMethodToAllAsync("delete", todoId, projectId);
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
