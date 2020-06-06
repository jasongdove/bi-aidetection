using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp2.Legacy
{
    public class LegacySettings
    {
        private string _telegramChatId;

        public LegacySettings()
        {
            //image input path
            InputPath = Properties.Settings.Default.input_path;

            //deepstack url
            DeepstackUrl = Properties.Settings.Default.deepstack_url;

            //save every action sent to Log() into the log file?
            LogEverything = Properties.Settings.Default.log_everything;

            //send error messages to Telegram?
            SendErrors = Properties.Settings.Default.send_errors;

            //telegram chat id
            TelegramChatId = Properties.Settings.Default.telegram_chatid;

            //telegram chat id
            TelegramToken = Properties.Settings.Default.telegram_token;
        }

        public string InputPath { get; set; }
        public string DeepstackUrl { get; set; }
        public string TelegramChatId
        {
            get
            {
                return _telegramChatId;
            }
            set
            {
                _telegramChatId = value;
                TelegramChatIds = _telegramChatId.Replace(" ", "").Split(',').ToList();
            }
        }
        public List<string> TelegramChatIds { get; private set; }
        public string TelegramToken { get; set; }
        public bool LogEverything { get; set; }
        public bool SendErrors { get; set; }

        public void Save()
        {
            Properties.Settings.Default.input_path = InputPath;
            Properties.Settings.Default.deepstack_url = DeepstackUrl;
            Properties.Settings.Default.telegram_chatid = TelegramChatId;
            Properties.Settings.Default.telegram_token = TelegramToken;
            Properties.Settings.Default.log_everything = LogEverything;
            Properties.Settings.Default.send_errors = SendErrors;

            Properties.Settings.Default.Save();
        }
    }
}
