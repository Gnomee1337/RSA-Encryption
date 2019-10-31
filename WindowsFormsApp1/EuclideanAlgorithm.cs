using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace WindowsFormsApp1
{
    class EuclideanAlgorithm
    {
        public BigInteger ClassicEuclidean(BigInteger val1, BigInteger val2)
        {
            if (val2 == 0)
                return val1;
            else
                return ClassicEuclidean(val2, val1 % val2);
        }
        public BigInteger ExtendEuclidean(BigInteger val1, BigInteger val2, out BigInteger x, out BigInteger y)
        {
            if (val1 == 0)
            {
                x = 0;
                y = 1;
                return val2;
            }
            BigInteger x1 = new BigInteger(), y1 = new BigInteger();
            BigInteger GCD = new BigInteger();
            GCD = ExtendEuclidean(val2 % val1, val1, out x1, out y1);
            x = y1 - (val2 / val1) * x1;
            y = x1;
            return GCD;
        }
    }
}
