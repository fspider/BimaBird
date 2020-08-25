using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MessageBird.Objects.Conversations;
using System.Net;

namespace BimaBird
{
    public class RecvBird
    {
        public static string getFileNameFromUrl(string url) {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string disposition = response.Headers["Content-Disposition"];
            if (disposition == null) return null;
            string filename = disposition.Substring(disposition.IndexOf("filename=") + 10).Replace("\"", "");
            response.Close();
            return filename;
        }

        public static MessageContent recvMessage(string hookResult) {
            var msg= JsonConvert.DeserializeObject<ConverstationWebhookResult> (hookResult);
            MessageContent msgContent = new MessageContent();

            msgContent.Direction = msg.Message.Direction.ToString();
            msgContent.From = msg.Contact.Msisdn.ToString();
            msgContent.ConversationId = msg.Message.ConversationId.ToString();
            msgContent.ChannelId = msg.Message.ChannelId.ToString();
            msgContent.MessageId = msg.Message.Id;
            msgContent.Status = msg.Message.Status.ToString();
            if (msg.Message.Error != null)
            {
                msgContent.Error = new ConversationMessageError
                {
                    Code = msg.Message.Error.Code,
                    Description = msg.Message.Error.Description
                };
            }
            msgContent.CreatedDatetime = msg.Message.CreatedDatetime;
            msgContent.UpdatedDatetime = msg.Message.UpdatedDatetime;

            string dataType = msg.Message.Type.ToString();
            msgContent.DataType = dataType;

            var content = msg.Message.Content;
            string url = "";
            byte[] data;
            string dataCaption = "";

            switch (dataType)
            {
                case "Text":
                    msgContent.Text = content.Text;
                    break;
                case "Hsm":
                    msgContent.TemplateName = content.Hsm.TemplateName;
                    break;
                case "Image":
                    url = content.Image.Url;
                    dataCaption = content.Image.Caption;
                    break;
                case "Audio":
                    url = content.Audio.Url;
                    dataCaption = content.Audio.Caption;
                    break;
                case "File":
                    url = content.File.Url;
                    dataCaption = content.File.Caption;
                    break;
                case "Video":
                    url = content.Video.Url;
                    dataCaption = content.Video.Caption;
                    break;
                case "Location":
                    msgContent.Location = content.Location;
                    break;
            }
            // Done Read Necessary Fields.
            if (dataType != "Text" && dataType != "Hsm" && dataType != "Location") {
                msgContent.Url = url;
                msgContent.Data= new System.Net.WebClient().DownloadData(url);
                msgContent.Caption = dataCaption;
                string fullname = getFileNameFromUrl(url);
                string filename="", extension="";
                if(fullname != null)
                {
                    filename = System.IO.Path.GetFileName(fullname);
                    extension = System.IO.Path.GetExtension(fullname).Replace(".", "");
                }
                msgContent.Filename = filename;
                msgContent.Extension = extension;
            }

            return msgContent;
        }
    }
}
