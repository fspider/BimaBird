using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MessageBird.Objects.Conversations
{
    public class ConverstationWebhookResult
    {
        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("conversation")]
        public Conversation Conversation { get; set; }

        [JsonProperty("message")]
        public ConversationMessage Message { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
