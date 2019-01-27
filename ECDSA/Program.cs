using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECDSA
{
    class Program
    {
        static void Main(string[] args)
        {
            EllepticCurve curve = new EllepticCurve(5, 34, 55);
            Console.WriteLine(curve.Sum(new Point(8,6), new Point(8,6)).ToString());
        }
    }
}
