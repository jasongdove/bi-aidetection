using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]


        static void Main()
        {
            // TODO: send errors to telegram if configured
            //    if(_settings.SendErrors == true && text.Contains("ERROR") || text.Contains("WARNING"))
            //    {
            //        await _telegramHelper.Text($"[{time}]: {text}"); //upload text to Telegram
            //    }

            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddConsole()
                       .AddFile("./log.txt", LogLevel.Warning);
                }
            );

            ILogger logger = loggerFactory.CreateLogger("WindowsFormsApp2");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Shell(logger));
        }
    }
}
