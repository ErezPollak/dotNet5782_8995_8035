using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// the Exception that accours when the user tries to add an Item with serial number that already exists in the database.
        /// </summary>
        [Serializable]
        public class IdAlreadyExistsException : Exception
        {
            //the number that the user tried to add even though it is already in the list.
            int number;

            //the recommended constractors for all the Exeption classes
            public IdAlreadyExistsException() : base() { }
            public IdAlreadyExistsException(string message) : base(message) { }
            public IdAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
            protected IdAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //Special constractor for the needs of the excption
            public IdAlreadyExistsException(int number) : base()
            {
                this.number = number;
            }

            /// <summary>
            /// the function returns a string with the Details of the excption.
            /// </summary>
            /// <returns></returns>
            override public string ToString()
            {
                return "SerialNumberExistsExceptions: the number: " + number + " already exists in the list.";
            }
        }



        /// <summary>
        /// the Exception that accours when the user tries to update an Item with serial number that does not exist in the database.
        /// </summary>
        [Serializable]
        public class IdDontExistsException : Exception
        {
            //the number that the user tried to add even though it is not in the list.
            int number;
            //represents the type to Item witch is missing from the database.
            string type;

            //the recommended constractors for all the Exeption classes
            public IdDontExistsException() : base() { }
            public IdDontExistsException(string message) : base(message) { }
            public IdDontExistsException(string message, Exception inner) : base(message, inner) { }
            protected IdDontExistsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //Special constractor for the needs of the excption
            public IdDontExistsException(int number, string type) : base()
            {
                this.number = number;
                this.type = type;
            }

            /// <summary>
            /// the function returns a string with the Details of the excption.
            /// </summary>
            /// <returns></returns>
            override public string ToString()
            {
                return "SerialNumberWasNotFoundExceptions: the number: " + this.number + " does not exist in the list of the "  + this.type+ "s.";
            }
        }

    }
   
}
