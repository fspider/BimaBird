using System;
using System.Collections.Generic;
using MessageBird.Json.Converters;
using Newtonsoft.Json;
using MessageBird;
using MessageBird.Exceptions;
using MessageBird.Net.ProxyConfigurationInjector;
using MessageBird.Objects;
using MessageBird.Objects.Conversations;
using MessageBird.Resources;
using MessageBird.Resources.Conversations;

namespace BimaBird
{
    public class SendBird
    {
        private static string YourAccessKey;
        private static string ChannelId;
        private static string TemplateNameSpace;

        //var client = Client.CreateDefault(YourAccessKey);
        // client with whatsapp sandbox enabled
        static Client client;
        public static void init(string accessKey, string channelId, string templateNameSpace) {
            YourAccessKey = accessKey;
            ChannelId = channelId;
            TemplateNameSpace = templateNameSpace;
            //client = Client.CreateDefault(YourAccessKey, features: new Client.Features[] { Client.Features.EnableWhatsAppSandboxConversations });
            client = Client.CreateDefault(YourAccessKey);
        }
        public static string GetLastMessageId(string conversationId) {
            var messages = client.ListConversationMessages(conversationId, 1);
            string msgId = "";
            if(messages.Items.Count > 0)
                msgId = messages.Items[0].Id;
            return msgId;
        }
        public static ConversationStartResult StartMessage(string phonenumber, string templateName, string templateLanguage, List<string> templateParams)
        {
            //var req = new ConversationStartRequest
            //{
            //    ChannelId = ChannelId,
            //    Content = new Content
            //    {
            //        Text = "Hello! This is start!"
            //    },
            //    Type = ContentType.Text,
            //    To = phonenumber
            //};
            var TemplateParams = new List<HsmLocalizableParameter> { };
            foreach (string item in templateParams) {
                TemplateParams.Add(
                    new HsmLocalizableParameter
                    {
                        Default = item
                    }
                );
            }

            var req = new ConversationStartRequest
            {
                ChannelId = ChannelId,
                Content = new Content
                {
                    Hsm = new HsmContent { 
                        Namespace = TemplateNameSpace,
                        TemplateName = templateName,
                        Language = new HsmLanguage { 
                            Code = templateLanguage,
                            Policy = HsmLanguagePolicy.Deterministic
                        },
                        Params = TemplateParams
                    }
                },
                Type = ContentType.Hsm,
                To = phonenumber
            };

            try
            {
                var conversation = client.StartConversation(req);
                //Console.WriteLine(JsonConvert.SerializeObject(conversation));
                string msgId = SendBird.GetLastMessageId(conversation.Id);
                return new ConversationStartResult { 
                    ConversationId = conversation.Id,
                    MessageId = msgId
                };
            }
            catch (Exception e) {
                Console.WriteLine("[SendBird] [Error] : Can not start message : " + e);
                return null;
            }
        }

        public static string SendMessage(string conversationId, string dataType, string data, string captionText="")
        {
            Content content = new Content();
            MediaContent mediaContent = new MediaContent();
            mediaContent.Url = data;
            var request = new ConversationMessageSendRequest();

            switch (dataType)
            {
                case "Text":
                    content.Text = data;
                    request.Type = ContentType.Text;
                    break;
                case "Image":
                    content.Image = mediaContent;
                    content.Image.Caption = captionText;
                    request.Type = ContentType.Image;
                    break;
                case "Audio":
                    content.Audio = mediaContent;
                    content.Audio.Caption = captionText;
                    request.Type = ContentType.Audio;
                    break;
                default:
                    content.File = mediaContent;
                    content.File.Caption = captionText;
                    request.Type = ContentType.File;
                    break;
            }
            request.Content = content;
            request.ChannelId = ChannelId;
            try
            {
                var message = client.SendConversationMessage(conversationId, request);
                //Console.WriteLine(JsonConvert.SerializeObject(message));
                return message.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SendBird] [Error] : Can not send message");
                return "";
            }

        }

        public static string SendLocation(string conversationId, float latitude, float longitude) {
            Content content = new Content();
            LocationContent location = new LocationContent
            {
                Latitude = latitude,
                Longitude = longitude
            };
            content.Location = location;
            var request = new ConversationMessageSendRequest();
            request.Content = content;
            request.Type = ContentType.Location;
            request.ChannelId = ChannelId;
            try
            {
                var message = client.SendConversationMessage(conversationId, request);
                //Console.WriteLine(JsonConvert.SerializeObject(message));
                return message.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine("[SendBird] [Error] : Can not send Location");
                return "";
            }
        }

        public static bool DeleteWebHook(string hookId) {
            client.DeleteConversationWebhook(hookId);
            return true;
        }
        public static void ViewConversation(string id) {
            var conversation = client.ViewConversation(id);
            Console.WriteLine(JsonConvert.SerializeObject(conversation));
        }
        public static bool CreateWebHook(string hookUrl) {
            try
            {
                var req = new ConversationWebhook
                {
                    ChannelId = ChannelId,
                    Url = hookUrl,
                    Events = new List<ConversationWebhookEvent>
                {
                    //ConversationWebhookEvent.ConversationCreated, ConversationWebhookEvent.ConversationUpdated,
                    ConversationWebhookEvent.MessageCreated, ConversationWebhookEvent.MessageUpdated
                }
                };
                var webhook = client.CreateConversationWebhook(req);
                Console.WriteLine(JsonConvert.SerializeObject(webhook));
                return true;
            }
            catch (ErrorException e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public static void ListWebHooks() {
            var hooksList = client.ListConversationWebhooks();
            Console.WriteLine(JsonConvert.SerializeObject(hooksList));
        }

        public static ConversationMessageList ListMessages(string convId)
        {
            var messages = client.ListConversationMessages(convId);
            return messages;
        }
    }
}
