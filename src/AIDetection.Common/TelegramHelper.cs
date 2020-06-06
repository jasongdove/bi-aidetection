using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace AIDetection.Common
{
    public class TelegramHelper
    {
        private string _token;
        private List<string> _chatIds;

        public TelegramHelper(string token, List<string> chatIds)
        {
            Configure(token, chatIds);
        }

        public void Configure(string token, List<string> chatIds)
        {
            _token = token;
            _chatIds = chatIds;
        }

        //send image to Telegram
        public async Task Upload(string image_path)
        {
            if (_chatIds.Count > 0  && _token != "")
            {
                //telegram upload sometimes fails
                try
                {
                    using (var image_telegram = File.OpenRead(image_path))
                    {
                        var bot = new TelegramBotClient(_token);

                        //upload image to Telegram servers and send to first chat
                        // TODO: fix logging
                        //Log($"      uploading image to chat \"{_settings.TelegramChatIds[0]}\"");
                        var message = await bot.SendPhotoAsync(_chatIds[0], new InputOnlineFile(image_telegram, "image.jpg"));
                        string file_id = message.Photo[0].FileId; //get file_id of uploaded image

                        //share uploaded image with all remaining telegram chats (if multiple chat_ids given) using file_id 
                        foreach (string chatid in _chatIds.Skip(1))
                        {
                            // TODO: fix logging
                            //Log($"      uploading image to chat \"{chatid}\"");
                            await bot.SendPhotoAsync(chatid, file_id);
                        }
                    }
                }
                catch
                {
                    // TODO: fix logging
                    //Log($"ERROR: Could not upload image {image_path} to Telegram.");
                    //store image that caused an error in ./errors/
                    if (!Directory.Exists("./errors/")) //if folder does not exist, create the folder
                    {
                        //create folder
                        DirectoryInfo di = Directory.CreateDirectory("./errors");
                        // TODO: fix logging
                        //Log("./errors/" + " dir created.");
                    }

                    //save error image
                    File.Move(image_path, "./errors/" + "TELEGRAM-ERROR-" + Path.GetFileName(image_path) + ".jpg");
                }
            }
        }

        //send text to Telegram
        public async Task Text(string text)
        {
            if (_chatIds.Count > 0 && _token != "")
            {
                //telegram upload sometimes fails
                try
                {
                    var bot = new TelegramBotClient(_token);
                    foreach (string chatid in _chatIds)
                    {
                        await bot.SendTextMessageAsync(chatid, text);
                    }

                }
                catch
                {
                    // TODO: fix this error flow after fixing logging
                    //if (_settings.SendErrors == true && text.Contains("ERROR") || text.Contains("WARNING")) //if Error message originating from Log() methods can't be uploaded
                    //{
                    //    _settings.SendErrors = false; //shortly disable send_errors to ensure that the Log() does not try to send the 'Telegram upload failed' message via Telegram again (causing a loop)
                    //    // TODO: fix logging
                    //    //Log($"ERROR: Could not send text \"{text}\" to Telegram.");
                    //    _settings.SendErrors = true;

                    //    //inform on main tab that Telegram upload failed
                    //    MethodInvoker LabelUpdate = delegate { lbl_errors.Text = "Can't upload error message to Telegram!"; };
                    //    Invoke(LabelUpdate);
                    //}
                    //else
                    //{
                    //    // TODO: fix logging
                    //    //Log($"ERROR: Could not send text \"{text}\" to Telegram.");
                    //}
                }
            }
        }
    }
}
