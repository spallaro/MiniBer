using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBer
{

    [Serializable]
    public class UnexpectedEndOfStreamException : Asn1ParseException
    {
        public UnexpectedEndOfStreamException() : 
            base("Unexpected end of stream.") { }
    }
}
