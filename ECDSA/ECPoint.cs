using System.Collections.Generic;
using System.Numerics;

namespace ECDSA
{
    public class ECPoint
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
        public bool IsO { get; set; }

        /// <summary>
        /// Инициализирует точку О
        /// </summary>
        public ECPoint()
        {
            IsO = true;
        }

        /// <summary>
        /// Инициализирует точку с координатами х, у
        /// </summary>
        public ECPoint(BigInteger x, BigInteger y)
        {
            X = x;
            Y = y;
            IsO = false;
        }

        public static bool operator ==(ECPoint a, ECPoint b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(ECPoint a, ECPoint b) => a.X != b.X || a.Y != b.Y;

        public override string ToString() => IsO ? "O" : $"({X:D2},{Y:D2})";

        public override bool Equals(object obj)
        {
            var point = obj as ECPoint;
            return point != null &&
                   X.Equals(point.X) &&
                   Y.Equals(point.Y);
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(X);
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(Y);
            return hashCode;
        }
    }
}
