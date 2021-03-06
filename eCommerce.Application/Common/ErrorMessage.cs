using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.Application.Common
{
    public class ErrorMessage : Exception
    {
        public ErrorMessage()
        {
        }
        public ErrorMessage(string message)
            : base(message)
        {
        }

        public ErrorMessage(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
