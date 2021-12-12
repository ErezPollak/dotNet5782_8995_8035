using System;
using System.Runtime.Serialization;

namespace DO
{
    /// <summary>
    /// the Exception that accours when the user tries to add an Item with serial number that already exists in the database.
    /// </summary>
    [Serializable]
    public class IdAlreadyExistsException : Exception
    {

        public IdAlreadyExistsException()
        {
        }

        public IdAlreadyExistsException(string message) : base(message)
        {
        }


        public IdAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected IdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        //Special constractor for the needs of the excption
        public IdAlreadyExistsException(int idNumber, string type) : base($"DAL_Exception: the id {idNumber} alreay exists in the database of {type}.")
        {
        }

    }



    /// <summary>
    /// the Exception that accours when the user tries to update an Item with serial number that does not exist in the database.
    /// </summary>
    [Serializable]
    public class IdDontExistsException : Exception
    {

        public IdDontExistsException()
        {
        }

        public IdDontExistsException(string message) : base(message)
        {
        }


        public IdDontExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected IdDontExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        //Special constractor for the needs of the excption
        public IdDontExistsException(int idNumber, string type) : base($"DAL_Exception: the id {idNumber} don't exists in the database of {type}.")
        {
        }
    }


    [Serializable]
    internal class NotSuchDataTypeException : Exception
    {
        public NotSuchDataTypeException()
        {
        }

        public NotSuchDataTypeException(string message) : base(message)
        {
        }

        public NotSuchDataTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotSuchDataTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}

