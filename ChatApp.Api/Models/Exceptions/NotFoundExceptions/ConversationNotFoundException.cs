namespace ChatApp.Api.Models.Exceptions.NotFoundExceptions
{
    public class ConversationNotFoundException : NotFoundException
    {
        public ConversationNotFoundException(Guid conversationId) : 
            base ($"Conversation with id: {conversationId} does not exist.")
        {
            
        }
    }
}
