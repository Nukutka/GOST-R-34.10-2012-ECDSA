using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ECDSA
{
    public class EllepticCurve
    {
        public BigInteger A { get; }
        public BigInteger B { get; }
        public BigInteger P { get; }
        //public BigInteger M { get; }
        //public List<ECPoint> PointList { get; }

        /// <summary>
        /// Задает базовые параметры кривой
        /// </summary>
        public EllepticCurve(BigInteger a, BigInteger b, BigInteger p)
        {
            A = a;
            B = b;
            P = p;
            //PointList = ComputePointList();
            //M = PointList.Count;
        }

        ///// <summary>
        ///// Создает список всех точек, удовлетворяющих уравнению кривой
        ///// </summary>
        ///// <returns></returns>
        //private List<ECPoint> ComputePointList()
        //{
        //    List<ECPoint> points = new List<ECPoint>();
        //    for (int x = 0; x < P; x++)
        //    {
        //        for (int y = 0; y < P; y++)
        //        {
        //            if (y * y % P == (x * x * x + A * x + B) % P)
        //            {
        //                points.Add(new ECPoint(x, y));
        //            }
        //        }
        //    }
        //    return points;
        //}

        /// <summary>
        /// Выполняет умножение точки на число
        /// </summary>
        /// <param name="point">Точка эллиптической кривой</param>
        /// <param name="n">Число, на которое производится умножение</param>
        public ECPoint MultPoint(ECPoint point, BigInteger n)
        {
            ECPoint res = new ECPoint();
            ECPoint doublePoint = point;
            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    res = SumPoint(res, doublePoint);
                }
                doublePoint = SumPoint(doublePoint, doublePoint);
                n /= 2;
            }
            return res;
        }

        /// <summary>
        /// Вычисляют сумму двух точек P + Q = -R
        /// </summary>
        /// <param name="a">Первая точка</param>
        /// <param name="b">Вторая точка</param>
        public ECPoint SumPoint(ECPoint a, ECPoint b)
        {
            if (a.IsO || b.IsO) return a.IsO ? b.IsO ? a : b : a; // костыль для О ))
            ECPoint res = new ECPoint();
            BigInteger tg = GetTg(a, b);
            if (tg != -1)
            {
                res.IsO = false;
                BigInteger x = (tg * tg - a.X - b.X) % P;
                BigInteger y = (tg * (a.X - x) - a.Y) % P;
                res.X = x < 0 ? x + P : x;
                res.Y = y < 0 ? y + P : y;
            }
            else
            {
                res.IsO = true;
            }
            return res;
        }

        /// <summary>
        /// Вычисляет угол наклона касательной 
        /// </summary>
        /// <param name="a">Первая точка</param>
        /// <param name="b">Вторая точка</param>
        private BigInteger GetTg(ECPoint a, ECPoint b)
        {
            BigInteger tg;
            if (a.X == b.X && a.Y == b.Y)
            {
                if (a.Y == 0) return -1;
                tg = (3 * a.X * a.X + A) * PrimeNumber.GetInverse(2 * a.Y, P) % P;
            }
            else
            {
                if (a.X - b.X == 0) return -1;
                tg = (a.Y - b.Y) * PrimeNumber.GetInverse(a.X - b.X, P) % P;
            }
            return tg < 0 ? tg % P + P : tg;
        }
    }
}
