using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ECDSA
{
    public class Point
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }

        public Point() { }

        public Point(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Point a, Point b) => a.X != b.X || a.Y != b.Y;

        public override string ToString() => $"({X:D2},{Y:D2})";
    }
}
