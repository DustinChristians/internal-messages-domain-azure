using System;
using System.Collections.Generic;
using System.Text;

namespace Internal.Messages.WebApi.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public T Payload { get; set; }
        public string Error { get; set; }
    }
}
