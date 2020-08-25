using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MessageBird.Objects.Conversations
{
    public class ConversationStartResult
    {
        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }
        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
}
