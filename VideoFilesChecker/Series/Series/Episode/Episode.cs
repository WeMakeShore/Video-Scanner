using System;

namespace VideoFilesChecker
{
    public class Episode
    {
        public string Title { get; set; }
        public long Size { get; set; }
        public DateTime CreationTime { get; set; }

        public Episode(string title, long size, DateTime CreationTime)
        {
            this.Title = title;
            this.Size = size;
            this.CreationTime = CreationTime;
        }
    }
}
