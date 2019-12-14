using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronizer.Core.ApiCommunication.Files
{
    public class FileOverviewResponse
    {
        public string BucketName { get; set; }
        public string Key { get; set; }
        public string Owner { get; set; }
        public long Size { get; set; }
    }
}
