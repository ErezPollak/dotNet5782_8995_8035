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
           
            public IdAlreadyExistsException() : base() 
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
            public IdAlreadyExistsException(int idNumber , string type) : base($"DAL_Exception: the id {idNumber} dont exists in the database of {type}.")
            {
            }

        }



        /// <summary>
        /// the Exception that accours when the user tries to update an Item with serial number that does not exist in the database.
        /// </summary>
        [Serializable]
        public class IdDontExistsException : Exception
        {
         
            public IdDontExistsException() : base()
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
            public IdDontExistsException(int idNumber, string type) : base($"DAL_Exception: the id {idNumber} already exists in the database of {type}.")
            {
            }
        }

    }
   
}
