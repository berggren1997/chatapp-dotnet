using ChatApp.Api.Services.Conversations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs
{
    [Authorize]
    public class ConversationHub : Hub
    {
        private readonly IConversationService _conversationService;

        public ConversationHub(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public async Task NewConversationEvent(Guid conversationId)
        {
            var conversation = await _conversationService.GetConversation(conversationId);
            
            if (conversation != null)
            {
                await Clients.All.SendAsync("NewConversationEvent", conversation);
            }
        }
    }
}
