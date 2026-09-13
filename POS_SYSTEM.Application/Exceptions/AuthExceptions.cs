using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Application.Exceptions
{
    public class UnauthorizedAppException : Exception
    {
        public UnauthorizedAppException(string message) : base(message) { }
    }

    public class ForbiddenAppException : Exception
    {
        public ForbiddenAppException(string message) : base(message) { }
    }

    public class NotFoundAppException : Exception
    {
        public NotFoundAppException(string message) : base(message) { }
    }

    public class BadRequestAppException : Exception
    {
        public BadRequestAppException(string message) : base(message) { }
    }
}
