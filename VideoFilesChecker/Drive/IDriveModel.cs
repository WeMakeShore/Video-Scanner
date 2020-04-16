namespace VideoFilesChecker.Drive
{
    interface IDriveModel
    {
        string DriveLabel { get; set; }
        double PercentageUnavailable { get; set; }
        long AvailableFreeSpace { get; set; }
        long TotalSize { get; set; }
    }
}
