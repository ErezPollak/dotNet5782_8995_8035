﻿using System;
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

            public IdAlreadyExistsException(int idNumber , string type) : base($"the id {idNumber} alredy exists in the database of {type}.")
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

            protected IdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


        

        [Serializable]
        public class IdDontExistsException : Exception
        {

            public IdDontExistsException(int idNumber, string type) : base($"the id {idNumber} dont exists in the database of {type}.")
            {
            }

            public IdDontExistsException()
            {
            }

            public IdDontExistsException(string message) : base(message)
            {
            }

            public IdDontExistsException(int idNumber, string type, Exception innerException) : base($"the id {idNumber} dont exists in the database of {type}.", innerException)
            {
            }

            protected IdDontExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


        [Serializable]
        internal class NotEnoughRangeException : Exception
        {
            public NotEnoughRangeException()
            {
            }

            public NotEnoughRangeException(string message) : base(message)
            {
            }

            public NotEnoughRangeException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotEnoughRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        internal class NotAbleToSendDroneToChargeException : Exception
        {

            static string message = "the drone cant make it to the station, the maximum distance is: ";

            public NotAbleToSendDroneToChargeException()
            {
            }
             
            public NotAbleToSendDroneToChargeException(string message) : base(message)
            {
            }

            public NotAbleToSendDroneToChargeException(int distance) : base(message + distance.ToString())
            {
            }

            public NotAbleToSendDroneToChargeException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected NotAbleToSendDroneToChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }


    }
}
