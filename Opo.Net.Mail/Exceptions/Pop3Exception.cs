using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opo.Net.Mail
{
    /// <summary>
    /// Pop3Exception is the default exception which is raised on any error in the Pop3Client
    /// </summary>
    public class Pop3Exception : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        public Pop3Exception() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public Pop3Exception(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop3Exception"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public Pop3Exception(string message, Exception innerException) : base(message, innerException) { }
    }
}
