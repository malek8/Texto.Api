using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Texto.Api.Requests
{
    public class SendMessageRequest
    {
        public string ToNumber { get; set; }
        public string Message { get; set; }
    }
}
