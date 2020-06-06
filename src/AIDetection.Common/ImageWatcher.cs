using System;
using System.IO;

namespace AIDetection.Common
{
    public class ImageWatcher
    {
        private readonly FileSystemWatcher _fileSystemWatcher;

        public event EventHandler<string> OnImageCreatedAsync;
        public event EventHandler<string> OnImageRenamed;
        public event EventHandler<string> OnImageDeleted;

        public ImageWatcher(string inputPath)
        {
            //fswatcher checking the input folder for new images
            _fileSystemWatcher = new FileSystemWatcher();

            //configure fswatcher to checks input_path for new images, images deleted and renamed images
            try
            {
                _fileSystemWatcher = new FileSystemWatcher
                {
                    Path = inputPath,
                    Filter = "*.jpg"
                };

                //fswatcher events
                _fileSystemWatcher.Created += new FileSystemEventHandler(OnCreated);
                _fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);
                _fileSystemWatcher.Deleted += new FileSystemEventHandler(OnDeleted);

                //enable fswatcher
                _fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch
            {
                // TODO: Fix logging

                //if (inputPath == "")
                //{
                //    Log("ATTENTION: No input folder defined.");
                //}
                //else
                //{
                //    Log($"ERROR: Can't access input folder '{inputPath}'.");
                //}
            }
        }

        public void UpdateInputPath(string inputPath)
        {
            try
            {
                _fileSystemWatcher.Path = inputPath;
            }
            catch
            {
                // TODO: Fix logging

                //if (inputPath == "")
                //{
                //    Log("ATTENTION: No input folder defined.");
                //}
                //else
                //{
                //    Log($"ERROR: Can't access input folder '{inputPath}'.");
                //}
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            OnImageCreatedAsync?.Invoke(source, e.Name);
        }

        //event: image in input_path renamed
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            OnImageRenamed?.Invoke(source, e.OldName);
        }

        //event: image in input path deleted
        void OnDeleted(object source, FileSystemEventArgs e)
        {
            OnImageDeleted?.Invoke(source, e.Name);
        }
    }
}
