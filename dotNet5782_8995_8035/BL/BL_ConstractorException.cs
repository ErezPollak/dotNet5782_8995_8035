using System;
using System.Runtime.Serialization;

namespace IBAL
{
    namespace BO
    {
        [Serializable]
        public class BL_ConstractorException : Exception
        {
            public BL_ConstractorException()
            {
            }

            public BL_ConstractorException(string message) : base(message)
            {
            }

            public BL_ConstractorException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected BL_ConstractorException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


    }
}
