using System;
using System.Linq;
using System.Numerics;
using System.Globalization;

namespace ECDSA
{
    /// <summary>
    /// Выполняет создание и проверку цифровой подписи
    /// </summary>
    public static class ECDSA
    {
        /// <summary>
        /// Создание цифровой подписи сообщения
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <param name="P">Точка эллиптической кривой</param>
        /// <param name="d">Ключ подписи</param>
        /// <param name="message">Сообщение</param>
        public static string Generation(EllepticCurve curve, ECPoint P, BigInteger d, string message)
        {
            BigInteger q = GetCyclicSubGroupOrder();

            string h = Streebog.GetHashCode(message, 512);

            //BigInteger e = BigInteger.Parse("20798893674476452017134061561508270130637142515379653289952617252661468872421");
            BigInteger e = BigInteger.Parse(h, NumberStyles.HexNumber) % q;     
            if (e == 0) e = 1;

            BigInteger r = 0;
            BigInteger s = 0;
            do
            {
                BigInteger k = 0;
                do
                {
                    do
                    {
                        k = GetMultiplier();
                    } while (k > q);

                    ECPoint C = curve.MultPoint(P, k);

                    r = C.X % q;
                } while (r == 0);

                s = (r * d + k * e) % q;
            } while (s == 0);

            string signature = ConcatParam(r, s);

            return $"{message}\n{signature}";
        }

        /// <summary>
        /// Проверка подписи сообщения
        /// </summary>
        /// <param name="curve">Эллиптическая кривая</param>
        /// <param name="P">Точка эллиптической кривой</param>
        /// <param name="Q">Ключ проверки подписи</param>
        /// <param name="message">Сообщение</param>
        public static bool Verification(EllepticCurve curve, ECPoint P, ECPoint Q, string message)
        {
            BigInteger q = GetCyclicSubGroupOrder();

            var tmp = message.Split('\n');
            string signature = tmp[tmp.Length - 1];
            message = new string(message.Take(message.Length - signature.Length - 1).ToArray());

            bool res1 = BigInteger.TryParse(signature.Substring(0, signature.Length / 2), NumberStyles.HexNumber, null, out var r);
            bool res2 = BigInteger.TryParse(signature.Substring(signature.Length / 2, signature.Length / 2), NumberStyles.HexNumber, null, out var s);
            if (!res1 || !res2 || r < 0 || r > q || s < 0 || s > q || signature.Length % 2 != 0) return false;

            string h = Streebog.GetHashCode(message, 512);

           // BigInteger e = BigInteger.Parse("20798893674476452017134061561508270130637142515379653289952617252661468872421");
            BigInteger e = BigInteger.Parse(h, NumberStyles.HexNumber) % q;    
            if (e == 0) e = 1;

            BigInteger v = PrimeNumber.GetInverse(e, q);

            BigInteger z1 = s * v % q;
            BigInteger z2 = -r * v % q;
            if (z2 < 0) z2 = z2 % q + q;

            ECPoint C = curve.SumPoint(curve.MultPoint(P, z1), curve.MultPoint(Q, z2));

            BigInteger R = C.X % q;

            return R == r;
        }

        ///// <summary>
        ///// Возвращает целое 0 < k < q
        ///// </summary>
        //private static BigInteger GetMultiplier() =>
        //    BigInteger.Parse("53854137677348463731403841147996619241504003434302020712960838528893196233395");

        /// <summary>
        /// Возвращает целое 0 < k < q
        /// </summary>
        private static BigInteger GetMultiplier() =>
           PrimeNumber.GetBigInteger(new Random().Next(0, 512));

        /// <summary>
        /// Вычисляет порядок циклической подгруппы группы точек ЭК
        /// </summary>
        private static BigInteger GetCyclicSubGroupOrder() =>
            BigInteger.Parse("57896044618658097711785492504343953927082934583725450622380973592137631069619");

        /// <summary>
        /// Соединяет параметры подписи
        /// </summary>
        private static string ConcatParam(BigInteger R, BigInteger S)
        {
            string r = R.ToString("X").ToLower();
            string s = S.ToString("X").ToLower();

            int partLength = r.Length > s.Length ? r.Length : s.Length;

            for (int i = r.Length; i < partLength; i++)
            {
                r = "0" + r;
            }

            for (int i = s.Length; i < partLength; i++)
            {
                s = "0" + s;
            }

            return r + s;
        }
    }
}
