using System;
using BimaBird;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BimaTest0.Message
{
    class CreateMessage
    {
        static string hookResult;
        static string accessKey = "SdmWOdut2nLlI3zBgu1HVbWtE";
        static string channelId = "72d19ef4f5a14316a489242a8043bba7";
        static string templateNameSpace = "0d4193e3_da2d_4e5e_a332_6b99f0bea48c";

        static void testRcvMessage() {
            // Read Test Data from JSON File
            // Replace this part with WebHook Request Data
            // Type Json String
            hookResult = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "../../../hookResult.json"));
            Console.WriteLine(hookResult);
            ////////////////////////////////

            // Read Necessarry Fields.
            var msgContent = RecvBird.recvMessage(hookResult);

            Console.WriteLine("Direction : " + msgContent.Direction);
            Console.WriteLine("From : " + msgContent.From);
            Console.WriteLine("ConversationId : " + msgContent.ConversationId);
            Console.WriteLine("ChannelId : " + msgContent.ChannelId);
            Console.WriteLine("MessageId : " + msgContent.MessageId);
            Console.WriteLine("Status : " + msgContent.Status);
            Console.WriteLine("CreatedDatetime : " + msgContent.CreatedDatetime);
            Console.WriteLine("UpdatedDatetime : " + msgContent.UpdatedDatetime);


            Console.WriteLine("DataType : " + msgContent.DataType);
            Console.WriteLine("Text : " + msgContent.Text);
            Console.WriteLine("TemplateName : " + msgContent.TemplateName);
            Console.WriteLine("URL : " + msgContent.Url);
            Console.WriteLine("FileName : " + msgContent.Filename);
            Console.WriteLine("Extension : " + msgContent.Extension);
            Console.WriteLine("Caption : " + msgContent.Caption);
            Console.WriteLine("DataLen : " + (msgContent.Data==null? "0" :msgContent.Data.Length.ToString()));
            if (msgContent.DataType == "Location") {
                Console.WriteLine("Location : ");
                Console.WriteLine("[latitude] = " + msgContent.Location.Latitude);
                Console.WriteLine("[longitude = " + msgContent.Location.Longitude);
            }
            if(msgContent.Error != null)
            {
                Console.WriteLine("Error Code : " + msgContent.Error.Code);
                Console.WriteLine("Error Description : " + msgContent.Error.Description);

                switch (msgContent.Error.Code)
                {
                    case 301:
                        Console.WriteLine("[ERROR] Message is Invalid");
                        break;
                    case 302:
                        Console.WriteLine("[ERROR] Non registered Contact");
                        break;
                    case 470:
                        Console.WriteLine("[ERROR] Please send Template MSG");
                        break;
                }

            }
        }
        static void testSendMessage() {
            // Initialize Send Bird with accessKey, channelId
            SendBird.init(accessKey, channelId, templateNameSpace);

            //string phonenumber = "+8618043352122";
            //string conversationId = "57347aa945cd4d4680241d9c12dfa01a";
            string phonenumber = "+8618943739660";
            string conversationId = "f4a528b72cee4530aee4ed8cb7b8139d";
            //var msgs = SendBird.ListMessages(conversationId);
            //Console.WriteLine(JsonConvert.SerializeObject(msgs));
            //SendBird.ViewConversation(conversationId);
            //return;

            /*******Start SEND Template Message Here*******/
            // Input Information for template messages
            string templateName = "auto_compelete_en";
            string templateLanguage = "en";
            List<string> templateParams = new List<string> { "Huhu", "https://messagebird.com" };

            //This function should be called when start conversation with new customer.
            //var convStartResult = SendBird.StartMessage(phonenumber, templateName, templateLanguage, templateParams);
            //Console.WriteLine("ConversationId = " + convStartResult.ConversationId + " MessageId = " + convStartResult.MessageId);
            //return;
            /*******End SEND Template Message *******/


            /*******Start SEND Normal Message Here*******/
            //SendBird.SendMessage(conversationId, "Text", "Really?");
            //SendBird.SendMessage(conversationId, "Image", "https://www.gstatic.com/webp/gallery/4.sm.jpg", "captionText");
            //SendBird.SendMessage(conversationId, "File", "https://www.radiantmediaplayer.com/media/big-buck-bunny-360p.mp4", "captionText");
            /*******Start SEND Normal Message Here*******/

            SendBird.SendLocation(conversationId, (float)23.579419, (float)58.411768);
        }

        static void changeWebHookUrl() {
            // Initialize Send Bird with accessKey, channelId
            SendBird.init(accessKey, channelId, templateNameSpace);

            /*******IGNORE this part*******/
            SendBird.DeleteWebHook("f62f1349a73d4dcc85c5f3534c836f74");
            SendBird.CreateWebHook("https://bima.om/api/Notifications/ReceiveWhatsAppMsg");
            SendBird.ListWebHooks();
        }

        static void Main(string[] args)
        {
            testRcvMessage();
            //testSendMessage();
            //changeWebHookUrl();
            return;
        }
    }
}
