/*
 * © 2026 Sebastiano Pallaro
 * Released under the terms of MIT license.
 * Please see LICENSE.md for more details.
 */
namespace MiniBer
{
    [Serializable]
    public class Asn1ParseException : Exception
    {
        public Asn1ParseException() { }

        public Asn1ParseException(
            string message) : 
            base(
                message: message) { }

        public Asn1ParseException(
            string message, 
            Exception inner) : 
            base(
                message: message, 
                innerException: inner) { }

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected Asn1ParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : 
            base(
                info: info, 
                context: context) { }
    }
}
