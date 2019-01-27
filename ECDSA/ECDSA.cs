using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ECDSA
{
    public static class ECDSA
    {
        public static string Generation(EllepticCurve curve, string message)
        {
            string h = Streebog.GetHashCode(message, 512);
           // BigInteger e = BigInteger.Parse(h) % 
            return "";
        }

        public static string Verification(EllepticCurve curve, string message)
        {
            return "";
        }
    }
}
