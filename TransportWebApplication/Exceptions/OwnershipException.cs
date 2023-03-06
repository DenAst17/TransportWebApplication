using System;

namespace TransportWebApplication.Exceptions
{
    public class OwnershipException:Exception
    {
        public OwnershipException()
        {
        }

        public OwnershipException(string message)
            : base(message)
        {
        }

        public OwnershipException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
