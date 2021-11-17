using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IBAL
{
    namespace BO
    {

        [Serializable]
        public class IdAlreadyExistsException : Exception
        {

            public IdAlreadyExistsException(int idNumber , string type) : base($"BL_Exception:  the id {idNumber} alredy exists in the database of {type}.")
            {
            }

            public IdAlreadyExistsException()
            {
            }

            public IdAlreadyExistsException(string message) : base(message)
            {
            }

            public IdAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
            {
            }

            public IdAlreadyExistsException(int idNumber ,string type, Exception innerException) : base($"BL_Exception:  the id {idNumber} already exists in the database of {type}.", innerException)
            {
            }

            protected IdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class IdDontExistsException : Exception
        {

            public IdDontExistsException(int idNumber, string type) : base($"BL_Exception:  the id {idNumber} dont exists in the database of {type}.")
            {
            }

            public IdDontExistsException()
            {
            }

            public IdDontExistsException(string message) : base(message)
            {
            }

            public IdDontExistsException(int idNumber, string type, Exception innerException) : base($"BL_Exception: the id {idNumber} dont exists in the database of {type}.", innerException)
            {
            }

            protected IdDontExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        /// <summary>
        /// describes all the exceptions that happen in the bl ctor.
        /// </summary>
        [Serializable]
        public class BL_ConstaractorException : Exception
        {
            public BL_ConstaractorException()
            {
            }

            public BL_ConstaractorException(string message) : base(" BL_ConstaractorException: " + message)
            {
            }

            public BL_ConstaractorException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected BL_ConstaractorException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


        [Serializable]
        internal class NotAbleToSendDroneToChargeException : Exception
        {

            public NotAbleToSendDroneToChargeException()
            {
            }
             
            public NotAbleToSendDroneToChargeException(string message) : base("Not Able To Send The Drone To Charge Due To: "+message)
            {
            }

            public NotAbleToSendDroneToChargeException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotAbleToSendDroneToChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


        [Serializable]
        internal class NotAbleToFreeDroneFromChargeException : Exception
        {
            public NotAbleToFreeDroneFromChargeException()
            {
            }

            public NotAbleToFreeDroneFromChargeException(string message) : base(message)
            {
            }

            public NotAbleToFreeDroneFromChargeException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotAbleToFreeDroneFromChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


        [Serializable]
        public class UnableToAssignParcelToTheDroneException : Exception
        {
            private int droneId;

            public UnableToAssignParcelToTheDroneException()
            {
            }

            public UnableToAssignParcelToTheDroneException(int droneId)
            {
                this.droneId = droneId;
            }

            public UnableToAssignParcelToTheDroneException(string message) : base(message)
            {
            }

            public UnableToAssignParcelToTheDroneException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UnableToAssignParcelToTheDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        public class UnableToDeliverParcelToTheDroneException : Exception
        {
            private int droneId;

            public UnableToDeliverParcelToTheDroneException()
            {
            }

            public UnableToDeliverParcelToTheDroneException(int droneId)
            {
                this.droneId = droneId;
            }

            public UnableToDeliverParcelToTheDroneException(string message) : base(message)
            {
            }

            public UnableToDeliverParcelToTheDroneException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UnableToDeliverParcelToTheDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


    }
}
