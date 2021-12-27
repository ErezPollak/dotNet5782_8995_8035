using System;
using System.Runtime.Serialization;

namespace PL
{
    [Serializable]
    internal class TheListIsEmptyException : Exception
    {
        public TheListIsEmptyException()
        {
        }

        public TheListIsEmptyException(string message) : base(message)
        {
        }

        public TheListIsEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TheListIsEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}