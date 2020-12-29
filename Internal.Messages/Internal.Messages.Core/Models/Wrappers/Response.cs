using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.Messages.Core.Models.Wrappers
{
    public class Response<T>
    {
        public T Payload { get; set; }
        public string Error { get; set; }
    }
}
