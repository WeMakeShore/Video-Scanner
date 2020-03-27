
namespace VideoFilesChecker
{
    public class Drive
    {
        public double PercentageUnavailable { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long TotalSize { get; set; }
        public Drive(double percentageUnavailable, long availableFreeSpace, long totalSize)
        {
            this.PercentageUnavailable = percentageUnavailable;
            this.AvailableFreeSpace = availableFreeSpace;
            this.TotalSize = totalSize;
        }
    }
}
