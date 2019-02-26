using System;
using System.Linq;
using System.Numerics;

namespace ECDSA
{
    /// <summary>
    /// Класс для работы с простыми числами
    /// </summary>
    public static class PrimeNumber
    {
        private static Random random;

        static PrimeNumber()
        {
            random = new Random();
        }

        /// <summary>
        /// Поиск обратного элемента с помощью расширенного алгоритма Евклида
        /// </summary>
        /// <param name="a">Числе, для которого необходимо найти обратное</param>
        /// <param name="b">Модуль</param>
        /// <param name="x">Обратное для числа</param>
        /// <param name="y">Обратное для модуля</param>
        private static void ExtendedEuclideanAlgorithm(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return;
            }
            ExtendedEuclideanAlgorithm(b % a, a, out BigInteger x1, out BigInteger y1);
            x = y1 - (b / a) * x1;
            y = x1;
        }

        /// <summary>
        /// Возвращает обратный элемент для заданного числа и модуля
        /// </summary>
        /// <param name="a">Число</param>
        /// <param name="mod">Модуль</param>
        public static BigInteger GetInverse(BigInteger a, BigInteger mod)
        {
            if (a < 0)
            {
                a = a % mod + mod;
            }
            ExtendedEuclideanAlgorithm(a, mod, out BigInteger x, out BigInteger y);
            return x;
        }

        /// <summary>
        /// Проверяет число на простоту с помощью теста Миллера-Рабина
        /// </summary>
        /// <param name="n">Число</param>
        /// <param name="bits">Разрядность числа</param>
        private static bool MillerRabinTest(BigInteger n, int bits)
        {
            if (n <= 1 || n % 2 == 0)
                return false;
            if (n == 2)
                return true;

            BigInteger s = 0, d = n - 1;
            while (d % 2 == 0) // n - 1 = 2^s * d
            {
                d /= 2;
                s++;
            }

            var r = new Random();

            for (int i = 0; i < 256; i++)
            {
                byte[] buff = new byte[bits / 8];
                random.NextBytes(buff);
                buff[0] = buff[0] < 100 ? (byte)random.Next(100, 256) : buff[0];
                BigInteger a = BigInteger.Parse(string.Join("", buff.Select(x => x.ToString("D3"))));

                BigInteger number = BigInteger.ModPow(a, d, n);
                if (number == 1 || number == n - 1) // a ^ d == 1
                    continue;

                for (int j = 0; j < s - 1; j++) // a ^ (2 ^ r) * d == -1
                {
                    number = (number * number) % n;
                    if (number == 1)
                        return false;
                    if (number == n - 1)
                        break;
                }
                if (number != n - 1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает случайное число заданной разрядности
        /// </summary>
        /// <param name="bits">Количество бит числа</param>
        public static BigInteger GetBigInteger(int bits)
        {
            BigInteger bigNumber = 0;
            for (int i = 0; i < bits - 1; i++)
            {
                bigNumber += random.Next(0, 2) > 0 ? BigInteger.Pow(2, i) : 0;
            }
            bigNumber += BigInteger.Pow(2, bits - 1);
            return bigNumber;
        }

        /// <summary>
        /// Возвращает случайное простое число заданной разрядности
        /// </summary>
        /// <param name="bits">Количество бит числа</param>
        public static BigInteger GetPrimeNumber(int bits)
        {
            BigInteger primeNumber = GetBigInteger(bits);
            if (primeNumber % 2 == 0) primeNumber++;
            do
            {
                primeNumber += 2;
            } while (!MillerRabinTest(primeNumber, bits));
            return primeNumber;
        }
    }
}