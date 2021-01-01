using System;
using System.Collections.Generic;

namespace Internal.Messages.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> GetExceptionMessages(this Exception exception)
        {
            var messages = new List<string>();

            do
            {
                messages.Add(exception.Message);
                exception = exception.InnerException;
            }
            while (exception != null);

            return messages;
        }
    }
}
