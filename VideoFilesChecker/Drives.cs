using System;
using System.IO;

namespace VideoFilesChecker
{
    public class Drives : IDrives
    {
        public Drive DockHardDrive { get; set; }
        public Drive ExternalHardDrive { get; set; }

        public Drives(Drive dockHardDrive, Drive externalHardDRive)
        {
            this.DockHardDrive = dockHardDrive;
            this.ExternalHardDrive = externalHardDRive;
        }

        public static Drives GetAvailableDriveSpace()
        {
            var dockDrive = GetDriveInfo("E");
            var externalDrive = GetDriveInfo("D");

            return new Drives(dockDrive, externalDrive);          
        }

        private static Drive GetDriveInfo(string drive)
        {
            DriveInfo dDrive = new DriveInfo(drive);
            double percentageUnavailable = 0;

            if (dDrive.IsReady)
            {
                double freeSpacePerc =
                    (dDrive.AvailableFreeSpace / (float)dDrive.TotalSize) * 100;
                percentageUnavailable = Math.Round(100 - freeSpacePerc, 2);

            } else
            {               
                return null;
            }

            return new Drive(percentageUnavailable, dDrive.AvailableFreeSpace, dDrive.TotalSize);
        }
    }
}
