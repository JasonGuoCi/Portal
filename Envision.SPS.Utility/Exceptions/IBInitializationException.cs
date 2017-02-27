using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Envision.SPS.Utility.Exceptions
{
    [Serializable]
    public class IBInitializationException : IBException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public IBInitializationException()
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public IBInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public IBInitializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public IBInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
