using System;
using System.Numerics;

namespace ECDSA
{
    class Program
    {
        static void Main(string[] args)
        {
            EllepticCurve curve = new EllepticCurve(
                7,
                BigInteger.Parse("43308876546767276905765904595650931995942111794451039583252968842033849580414"),
                BigInteger.Parse("57896044618658097711785492504343953926634992332820282019728792003956564821041")
                );

            ECPoint P = new ECPoint(2, BigInteger.Parse("4018974056539037503335449422937059775635739389905545080690979365213431566280"));

            BigInteger d = BigInteger.Parse("55441196065363246126355624130324183196576709222340016572108097750006097525544");

            ECPoint Q = new ECPoint(
                BigInteger.Parse("57520216126176808443631405023338071176630104906313632182896741342206604859403"),
                BigInteger.Parse("17614944419213781543809391949654080031942662045363639260709847859438286763994")
                );

            string msg = ECDSA.Generation(curve, P, d, "test");
            Console.WriteLine(msg);
            //msg = "0w" + msg;
            bool res = ECDSA.Verification(curve, P, Q, msg);
            if (res) Console.WriteLine("Подпись верна!");
            else Console.WriteLine("Подпись неверна!");
        }
    }
}
