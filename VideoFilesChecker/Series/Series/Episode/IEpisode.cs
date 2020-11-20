using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoFilesChecker.Drive;

namespace VideoFilesChecker
{
    interface IEpisode : IDriveModel
    {
       string Title { get; set; }
       long Size { get; set; }
       DateTime CreationTime { get; set; }
    }
}
