using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace GoAhead
{
    public class SubSets
    {
        public IEnumerable<List<T>> GetAllSubSets<T>(List<T> source)
        {
            DateTime last = DateTime.Now;

            BigInteger size = BigInteger.Pow(2, source.Count);
            BigInteger i = 0;

            for (i = 0; i < size; i++)
            {
                string binary = i.ToBinaryString();

                List<T> combination = new List<T>();

                for (int j = 0; j < binary.Length; j++)
                {
                    if (binary[j] == '1')
                    {
                        combination.Add(source[j]);
                    }
                    /*
                    if ((i & (1 << (source.Count - j - 1))) != 0)
                    {
                        combination.Add(source[j]);
                    }*/
                }

                yield return combination;

            }
        }

        public IEnumerable<List<T>> GetAllSubSets<T>(List<T> source, int size)
        {
            DateTime last = DateTime.Now;

            BigInteger i = 0;

            for (i = 0; i <= BigInteger.Pow(2, source.Count)-1; i++)
            {
                if (i == 1048583)
                {
                }

                string binary = i.ToBinaryString();
                if (binary.Count(c => c == '1') != size)
                {
                    continue;
                }
                //Console.WriteLine(binary.ToString());

                List<T> combination = new List<T>();

                for (int j = 0; j < binary.Length; j++)
                {
                    if (binary[j] == '1')
                    {                       
                        combination.Add(source[j]);
                    }
                    /*
                    if ((i & (1 << (source.Count - j - 1))) != 0)
                    {
                        combination.Add(source[j]);
                    }*/
                }

                yield return combination;

            }
        }
    }

    /// <summary>
    /// Extension methods to convert <see cref="System.Numerics.BigInteger"/>
    /// instances to hexadecimal, octal, and binary strings.
    /// </summary>
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Converts a <see cref="BigInteger"/> to a binary string.
        /// </summary>
        /// <param name="bigint">A <see cref="BigInteger"/>.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing a binary
        /// representation of the supplied <see cref="BigInteger"/>.
        /// </returns>
        public static string ToBinaryString(this BigInteger bigint)
        {
            byte[] bytes = bigint.ToByteArray();
            int idx = bytes.Length - 1;

            // Convert first byte to binary.
            string binary = Convert.ToString(bytes[idx], 2);            

            // Create a StringBuilder having appropriate capacity.
            var base2 = new StringBuilder(bytes.Length * 8);

            // Append binary string to StringBuilder.
            base2.Append(binary);

            // Convert remaining bytes adding leading zeros.
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            return base2.ToString();
        }

        /// <summary>
        /// Converts a <see cref="BigInteger"/> to a hexadecimal string.
        /// </summary>
        /// <param name="bigint">A <see cref="BigInteger"/>.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing a hexadecimal
        /// representation of the supplied <see cref="BigInteger"/>.
        /// </returns>
        public static string ToHexadecimalString(this BigInteger bigint)
        {
            return bigint.ToString("X");
        }

        /// <summary>
        /// Converts a <see cref="BigInteger"/> to a octal string.
        /// </summary>
        /// <param name="bigint">A <see cref="BigInteger"/>.</param>
        /// <returns>
        /// A <see cref="System.String"/> containing an octal
        /// representation of the supplied <see cref="BigInteger"/>.
        /// </returns>
        public static string ToOctalString(this BigInteger bigint)
        {
            var bytes = bigint.ToByteArray();
            var idx = bytes.Length - 1;

            // Create a StringBuilder having appropriate capacity.
            var base8 = new StringBuilder(((bytes.Length / 3) + 1) * 8);

            // Calculate how many bytes are extra when byte array is split
            // into three-byte (24-bit) chunks.
            var extra = bytes.Length % 3;

            // If no bytes are extra, use three bytes for first chunk.
            if (extra == 0)
            {
                extra = 3;
            }

            // Convert first chunk (24-bits) to integer value.
            int int24 = 0;
            for (; extra != 0; extra--)
            {
                int24 <<= 8;
                int24 += bytes[idx--];
            }

            // Convert 24-bit integer to octal without adding leading zeros.
            var octal = Convert.ToString(int24, 8);

            // Ensure leading zero exists if value is positive.
            if (octal[0] != '0' && bigint.Sign == 1)
            {
                base8.Append('0');
            }

            // Append first converted chunk to StringBuilder.
            base8.Append(octal);

            // Convert remaining 24-bit chunks, adding leading zeros.
            for (; idx >= 0; idx -= 3)
            {
                int24 = (bytes[idx] << 16) + (bytes[idx - 1] << 8) + bytes[idx - 2];
                base8.Append(Convert.ToString(int24, 8).PadLeft(8, '0'));
            }

            return base8.ToString();
        }
    }
}
