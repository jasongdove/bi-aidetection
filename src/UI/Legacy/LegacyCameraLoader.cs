using System;
using System.IO;
using System.Linq;
using AIDetection.Common;

namespace WindowsFormsApp2
{
    public static class LegacyCameraLoader
    {
        public static Camera FromFile(string configPath)
        {
            Camera camera = new Camera();

            //retrieve whole config file content
            string[] content = File.ReadAllLines(configPath);

            //import config data into variables, cut out relevant data between " "
            camera.Name = Path.GetFileNameWithoutExtension(configPath);
            camera.Prefix = content[2].Split('"')[1];

            //read triggering objects
            var triggeringObjectsAsString = content[1].Split('"')[1].Replace(" ", ""); //take the second line, split it between every ", take the part after the first ", remove every " " in this part
            camera.TriggeringObjects = triggeringObjectsAsString.Split(',').ToList(); //split the row of triggering objects between every ','

            //read trigger urls
            var triggerUrlsAsString = content[0].Split('"')[1]; //takes the first line, cuts out everything between the first and the second " marker; all trigger urls in one string, ! still contains possible spaces etc.
            camera.TriggerUrls = triggerUrlsAsString.Replace(" ", "").Split(',')
                .Where(x => !string.IsNullOrWhiteSpace(x)) //remove empty entries
                .ToList(); //all trigger urls in a list

            //read telegram enabled
            camera.IsTelegramEnabled = content[3].Split('"')[1].Replace(" ", "") == "yes";

            //read enabled
            camera.IsEnabled = content[4].Split('"')[1].Replace(" ", "") == "yes";

            Double.TryParse(content[5].Split('"')[1], out double cooldownMinutes); //read cooldown time
            camera.Cooldown = TimeSpan.FromMinutes(cooldownMinutes);

            //read lower and upper threshold. Only load if line containing threshold values already exists (>version 1.58).
            if (!string.IsNullOrWhiteSpace(content[6]))
            {
                //read lower threshold
                if (Int32.TryParse(content[6].Split('"')[1].Split(',')[0], out int thresholdLower))
                {
                    camera.ThresholdLower = thresholdLower;
                }

                //read upper threshold
                if (Int32.TryParse(content[6].Split('"')[1].Split(',')[1], out int thresholdUpper))
                {
                    camera.ThresholdUpper = thresholdUpper;
                }
            }
            else //if config file from older version, set values to 0% and 100%
            {
                camera.ThresholdLower = 0;
                camera.ThresholdUpper = 100;
            }


            //read stats
            //bedeutet: Zeile 7 (6+1), aufgetrennt an ", 2tes (1+1) Resultat, aufgeteilt an ',', davon 1. Resultat  
            if (Int32.TryParse(content[7].Split('"')[1].Split(',')[0], out int alertCount))
            {
                camera.AlertCount = alertCount;
            }
            if (Int32.TryParse(content[7].Split('"')[1].Split(',')[1], out int irrelevantAlertCount))
            {
                camera.IrrelevantAlertCount = irrelevantAlertCount;
            }
            if (Int32.TryParse(content[7].Split('"')[1].Split(',')[2], out int falseAlertCount))
            {
                camera.FalseAlertCount = falseAlertCount;
            }

            //read telegram cooldown time
            if (content.Length >= 9 && Double.TryParse(content[8].Split('"')[1], out double telegramCooldownMinutes))
            {
                camera.TelegramCooldown = TimeSpan.FromMinutes(telegramCooldownMinutes);
            }

            return camera;
        }

        public static void WriteFile(Camera camera)
        {
            using (StreamWriter sw = File.CreateText(GetFileName(camera)))
            {
                sw.WriteLine($"Trigger URL(s): \"{string.Join(", ", camera.TriggerUrls)}\" (input one or multiple urls, leave empty to disable; format: \"url, url, url\", example: \"http://192.168.1.133:80/admin?trigger&camera=frontyard&user=admin&pw=secretpassword, http://google.com\")");
                sw.WriteLine($"Relevant objects: \"{string.Join(", ", camera.TriggeringObjects)}\" (format: \"object, object, ...\", options: see below, example: \"person, bicycle, car\")");
                sw.WriteLine($"Input file begins with: \"{camera.Prefix}\" (only analyze images which names start with this text, leave empty to disable the feature, example: \"backyardcam\")");
                sw.WriteLine($"Send images to Telegram: \"{(camera.IsTelegramEnabled ? "yes" : "no")}\"(options: yes, no)");
                sw.WriteLine($"ai detection enabled?: \"{(camera.IsEnabled ? "yes" : "no")}\"(options: yes, no)");
                sw.WriteLine($"Cooldown time: \"{camera.Cooldown.TotalMinutes}\" minutes (How many minutes must have passed since the last detection. Used to separate event to ensure that every event only causes one alert.)");
                sw.WriteLine($"Certainty threshold: \"{camera.ThresholdLower},{camera.ThresholdUpper}\" (format: \"lower % limit, upper % limit\")");
                sw.WriteLine($"STATS: alerts,irrelevant alerts,false alerts: \"{camera.AlertCount}, {camera.IrrelevantAlertCount}, {camera.FalseAlertCount}\" ");
                sw.WriteLine($"Telegram cooldown time: \"{camera.TelegramCooldown.TotalMinutes}\" minutes (How many minutes must have passed since the last detection. Used to separate event to ensure that every event only causes one telegram message.)");
            }
        }

        public static void DeleteFile(Camera camera)
        {
            File.Delete(GetFileName(camera));
        }

        private static string GetFileName(Camera camera)
        {
            return AppDomain.CurrentDomain.BaseDirectory + $"/cameras/{ camera.Name }.txt";
        }
    }
}
