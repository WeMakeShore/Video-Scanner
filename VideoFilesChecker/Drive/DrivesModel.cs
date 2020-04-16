using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VideoChecking;

namespace VideoFilesChecker.Drive
{
    public class DrivesModel
    {
        private static List<DriveModel> Items = new List<DriveModel>();
        private static List<string> driveLabels = new List<string>();

        public static List<DriveModel> GetDrives()
        {
            foreach (var vp in Program.settings.VideoPaths)
            {
                driveLabels.Add(Path.GetPathRoot(vp));
            }

            driveLabels = driveLabels.Distinct().ToList();

            ScanDrives();

            return Items;
        }

        private static void ScanDrives()
        {
            for (int i = 0; i < driveLabels.Count; i++)
            {
                Items.Add(GetDriveInfo(driveLabels[i]));
            }
        }

        private static DriveModel GetDriveInfo(string drive)
        {
            DriveInfo dDrive = new DriveInfo(drive);
            double percentageUnavailable = 0;

            if (dDrive.IsReady)
            {
                double freeSpacePerc =
                    (dDrive.AvailableFreeSpace / (float)dDrive.TotalSize) * 100;
                percentageUnavailable = Math.Round(100 - freeSpacePerc, 2);

            }

            return new DriveModel(drive, percentageUnavailable, dDrive.AvailableFreeSpace, dDrive.TotalSize);
        }
    }
}
