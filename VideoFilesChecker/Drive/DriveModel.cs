using VideoFilesChecker.Drive;

namespace VideoFilesChecker
{
    public class DriveModel : IDriveModel
    {
        public string DriveLabel { get; set; }
        public string VolumeLabel { get; set; }
        public double PercentageUnavailable { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long TotalSize { get; set; }

        public DriveModel(string driveLabel, string volumeLabel, double percentageUnavailable, long availableFreeSpace, long totalSize)
        {
            this.DriveLabel = driveLabel;
            this.VolumeLabel = volumeLabel;
            this.PercentageUnavailable = percentageUnavailable;
            this.AvailableFreeSpace = availableFreeSpace;
            this.TotalSize = totalSize;
        }
    }
}
