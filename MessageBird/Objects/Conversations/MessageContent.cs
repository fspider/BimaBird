using Newtonsoft.Json;
using System;

namespace MessageBird.Objects.Conversations
{
    public class MessageContent
    {

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("conversationId")]
        public string ConversationId { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("filename")]
        public string Filename { get; set; }
        
        [JsonProperty("extension")]
        public string Extension { get; set; }


        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("templateName")]
        public string TemplateName { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("location")]
        public LocationContent Location { get; set; }


        [JsonProperty("data")]
        public byte[] Data { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("entityCode")]
        public string EntityCode { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public ConversationMessageError Error { get; set; }

        [JsonProperty("createdDatetime")]
        public DateTime? CreatedDatetime { get; set; }

        [JsonProperty("updatedDatetime")]
        public DateTime? UpdatedDatetime { get; set; }
    }
}
