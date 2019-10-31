using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    class PrimeNumber
    {
        BigInteger rndNumber = new BigInteger();
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        public Random rnd = new Random();

        private BigInteger RndNum(int min, int max)
        {
            int minDegree = min;
            int maxDegree = rnd.Next(minDegree, max); // Степень 1-6; 9=512 ; 10=1024;
            int[] degrees = new int[maxDegree];
            degrees[0] = minDegree;
            for (int i = 1; i < degrees.Length; i++)
            {
                degrees[i] = degrees[i - 1] * 2;
            }
            int temp = rnd.Next(0, degrees.Length);
            int rndNum = degrees[temp];

            int size = rndNum;
            byte[] bytes = new byte[size];
            rng.GetBytes(bytes);
            BigInteger number = new BigInteger(bytes);
            if (number < 0) number *= (-1);

            return number;
        }
        public BigInteger GeneratePrimeNumber(int _min, int _max)
        {
            rndNumber = RndNum(_min, _max);
            for(;;)
            {
                bool Prime = false;
                Prime = MillerRabinTest(rndNumber, 100);
                if (Prime == false) rndNumber = RndNum(_min, _max);
                else if (Prime == true) break;
            }
            return rndNumber;
        }
        private bool MillerRabinTest(BigInteger n, BigInteger k)
        {
            if (n <= 1)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            BigInteger s = new BigInteger(), d = new BigInteger();
            s = 0;
            d = n - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (BigInteger i = 0; i < k; i++)
            {
                BigInteger a = new BigInteger(), x = new BigInteger();
                a = RandomInRange(rng, 2, n - 1);
                x = BigInteger.ModPow(a, d, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int j = 0; j < s - 1; j++)
                {
                    x = (x * x) % n;
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        //!! Gen Rand BigInt Number in Range !!\\
        public BigInteger RandomInRange(RandomNumberGenerator rng, BigInteger min, BigInteger max)
        {
            if (min > max)
            {
                BigInteger buff = min;
                min = max;
                max = buff;
            }

            // offset to set min = 0
            BigInteger offset = -min;
            min = 0;
            max += offset;

            BigInteger value = new BigInteger();
            value = randomInRangeFromZeroToPositive(rng, max) - offset;
            return value;
        }

        private BigInteger randomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max)
        {
            BigInteger value = new BigInteger();
            byte[] bytes = max.ToByteArray();

            // count how many bits of the most significant byte are 0
            // NOTE: sign bit is always 0 because `max` must always be positive
            byte zeroBitsMask = 0b00000000;

            byte mostSignificantByte = bytes[bytes.Length - 1];

            // we try to set to 0 as many bits as there are in the most significant byte, starting from the left (most significant bits first)
            // NOTE: `i` starts from 7 because the sign bit is always 0
            for (int i = 7; i >= 0; i--)
            {
                // we keep iterating until we find the most significant non-0 bit
                if ((mostSignificantByte & (0b1 << i)) != 0)
                {
                    int zeroBits = 7 - i;
                    zeroBitsMask = (byte)(0b11111111 >> zeroBits);
                    break;
                }
            }

            do
            {
                rng.GetBytes(bytes);

                // set most significant bits to 0 (because `value > max` if any of these bits is 1)
                bytes[bytes.Length - 1] &= zeroBitsMask;

                value = new BigInteger(bytes);

                // value > max` 50% of the times, in which case the fastest way to keep the distribution uniform is to try again
            } while (value > max);

            return value;
        }
    }
}
