using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronizer.Core.ApiCommunication.Files
{
    public class AddFileResponse
    {
        public IList<string> PreSignedUrl { get; set; }
    }
}
