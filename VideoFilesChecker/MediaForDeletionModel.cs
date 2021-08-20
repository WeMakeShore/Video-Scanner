using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoChecking;

namespace VideoFilesChecker
{
    class MediaForDeletionModel
    {
        public DataModel MediaForDeletion;

        public MediaForDeletionModel(DataModel getMediaForDeletion)
        {
            this.MediaForDeletion = getMediaForDeletion;
        }
    }
}
