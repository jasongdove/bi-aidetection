using System;
using System.Collections.Generic;

namespace AIDetection.Common
{
    public class Camera
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        //public string triggering_objects_as_string;
        public List<string> TriggeringObjects { get; set; }
        //public string trigger_urls_as_string;
        public List<string> TriggerUrls;
        public bool IsTelegramEnabled { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime LastTriggerTime { get; set; }
        public DateTime LastTelegramTime { get; set; }
        public TimeSpan Cooldown { get; set; }
        public TimeSpan TelegramCooldown { get; set; }
        public int ThresholdLower { get; set; }
        public int ThresholdUpper { get; set; }

        public List<string> LastDetections = new List<string>(); //stores objects that were detected last
        public List<float> LastConfidences = new List<float>(); //stores last objects confidences
        public List<string> LastPositions = new List<string>(); //stores last objects positions


        //stats
        public int AlertCount; //alert image contained relevant object counter
        public int FalseAlertCount; //alert image contained no object counter
        public int IrrelevantAlertCount; //alert image contained irrelevant object counter
    }
}
