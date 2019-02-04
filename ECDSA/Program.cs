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

            ECPoint point = new ECPoint(
                 2,
                 BigInteger.Parse("4018974056539037503335449422937059775635739389905545080690979365213431566280")
                );
            BigInteger k = BigInteger.Parse("53854137677348463731403841147996619241504003434302020712960838528893196233395");
            BigInteger m = BigInteger.Parse("57896044618658097711785492504343953927082934583725450622380973592137631069619");
            BigInteger q = BigInteger.Parse("57896044618658097711785492504343953927082934583725450622380973592137631069619");
            //ECPoint sumPoint = point;
            //for (int i = 0; i < 1401; i++)
            //{
            //    sumPoint = curve.SumPoint(sumPoint, point);
            //}
            //Console.WriteLine(sumPoint);
            ECPoint multPoint = curve.MultPoint(point, k);
            Console.WriteLine(multPoint);
        }
    }
}
