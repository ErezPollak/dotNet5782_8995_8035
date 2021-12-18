using System;
using System.Runtime.Serialization;

namespace BO
{

    [Serializable]
    public class IdAlreadyExistsException : Exception
    {

        public IdAlreadyExistsException(int idNumber, string type) : base($"BL_Exception:  the id {idNumber} alredy exists in the database of {type}.")
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

        public IdAlreadyExistsException(int idNumber, string type, Exception innerException) : base($"BL_Exception:  the id {idNumber} already exists in the database of {type}.", innerException)
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
        //public BL_ConstaractorException()
        //{
        //}

        public BL_ConstaractorException(string message) : base(" BL_ConstaractorException: " + message)
        {
        }

        public BL_ConstaractorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        //protected BL_ConstaractorException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }


    [Serializable]
    public class UnableToAssignParcelToTheDroneException : Exception
    {

        //public UnableToAssignParcelToTheDroneException()
        //{
        //}

        public UnableToAssignParcelToTheDroneException(int droneId, string massage) : base($"BL_Exception: not able to assign any parcel to the drone {droneId} due to: " + massage)
        {
        }

        //public UnableToAssignParcelToTheDroneException(string message) : base(message)
        //{
        //}

        public UnableToAssignParcelToTheDroneException(int droneId, string message, Exception innerException) : base($"BL_Exception: not able to assign any parcel to the drone {droneId} due to: " + message, innerException)
        {
        }

        //protected UnableToAssignParcelToTheDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        //{
        //}
    }

    /// <summary>
    /// exaption for the function of  delivering parccel that is carried by a drone.
    /// </summary>
    [Serializable]
    public class UnableToDeliverParcelFromTheDroneException : Exception
    {
        public UnableToDeliverParcelFromTheDroneException()
        {
        }

        public UnableToDeliverParcelFromTheDroneException(int droneId, string message) : base($"unable to deliver aparcel from the  drone {droneId} due to: " + message)
        {
        }

        public UnableToDeliverParcelFromTheDroneException(int droneId, string message, Exception innerException) : base($"BL_Exception: not able to deliver the parcel from the drone {droneId} due to: " + message, innerException)
        {
        }

        protected UnableToDeliverParcelFromTheDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// exception that descrine the reasons why a drone cannot be charged.
    /// </summary>
    [Serializable]
    internal class UnAbleToSendDroneToChargeException : Exception
    {

        public UnAbleToSendDroneToChargeException()
        {
        }

        public UnAbleToSendDroneToChargeException(string message) : base("BL_Exception: Not Able To Send The Drone To Charge Due To: " + message)
        {
        }

        public UnAbleToSendDroneToChargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnAbleToSendDroneToChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    /// exception that describes drone cannot be released from charge.
    /// </summary>
    [Serializable]
    internal class UnAbleToReleaseDroneFromChargeException : Exception
    {
        public UnAbleToReleaseDroneFromChargeException()
        {
        }

        public UnAbleToReleaseDroneFromChargeException(int droneId, string message) : base($"BL_Exception: Unable to release the drone {droneId} from charging  Due to: " + message)
        {
        }

        public UnAbleToReleaseDroneFromChargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnAbleToReleaseDroneFromChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }



    [Serializable]
    public class WrongInoutException : Exception
    {
        public WrongInoutException()
        {
        }

        public WrongInoutException(string message) : base("BL_Exception: the input is wrong because: " + message)
        {
        }

        public WrongInoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongInoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class UnableToAddDroneException : Exception
    {

        public UnableToAddDroneException()
        {
        }

        public UnableToAddDroneException(string message) : base(message)
        {
        }

        public UnableToAddDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToAddDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class ModelEmptyException : Exception
    {
        public ModelEmptyException():base("BL_Exception: the name that was provided was empty.")
        {
        }

        public ModelEmptyException(string message) : base(message)
        {
        }

        public ModelEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }



}
