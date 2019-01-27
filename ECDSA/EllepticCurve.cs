using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ECDSA
{
    public class EllepticCurve
    {
        public BigInteger A { get; }
        public BigInteger B { get; }
        public BigInteger P { get; }
        public BigInteger M { get; }
        public List<Point> PointList { get; }

        /// <summary>
        /// Задает базовые параметры кривой
        /// </summary>
        public EllepticCurve(int a, int b, int p)
        {
            A = a;
            B = b;
            P = p;
            PointList = ComputePointList();
            M = PointList.Count;
        }

        /// <summary>
        /// Создает список всех точек, удовлетворяющих уравнению кривой
        /// </summary>
        /// <returns></returns>
        private List<Point> ComputePointList()
        {
            List<Point> points = new List<Point>();
            for (int x = 0; x < P; x++)
            {
                for (int y = 0; y < P; y++)
                {
                    if (y * y % P == (x * x * x + A * x + B) % P)
                    {
                        points.Add(new Point(x, y));
                    }
                }
            }
            return points;
        }

        private Point Sum(Point a, Point b)
        {
            Point res = new Point();
            BigInteger tg = GetTg(a, b);
            BigInteger x = (tg * tg - a.X - b.X) % P;
            BigInteger y = (tg * (a.X - x) - a.Y) % P;
            res.X = x < 0 ? x + P : x;
            res.Y = y < 0 ? y + P : y;
            return res;
        }

        private BigInteger GetTg(Point a, Point b)
        {
            BigInteger tg;
            if (a.X == b.X && a.Y == b.Y)
            {
                tg = (3 * a.X * a.X + A) * PrimeNumber.GetInverse(2 * a.Y, P) % P;
            }
            else
            {
                tg = (a.Y - b.Y) * PrimeNumber.GetInverse(a.X - b.X, P) % P;
            }
            return tg < 0 ? tg % P + P : tg;
        }
    }
}
