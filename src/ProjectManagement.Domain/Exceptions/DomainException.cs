using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Domain.Exceptions
{
    public class DomainException:Exception
    {
        public DomainException(string exceptionMessage)
        : base(exceptionMessage) { }
    }
}
