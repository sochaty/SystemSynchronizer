using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronizer.Core.ApiCommunication.Files
{
    public class GetJsonObjectResponse
    {
        public Guid Id { get; set; }
        public DateTime TimeSent { get; set; }
        public string Data { get; set; }
    }
}
