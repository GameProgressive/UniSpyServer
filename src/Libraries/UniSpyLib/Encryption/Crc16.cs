using System.Text;

namespace UniSpyLib.Encryption
{
    public enum Crc16Mode : ushort
    {
        Standard = 0xA001,
        CCITT = 4129,
        CCITTKermit = 0x8408
    }

    /// <summary>
    /// Credits go to http://www.sanity-free.com/134/standard_crc_16_in_csharp.html
    /// </summary>
    public class Crc16
    {
        /// <summary>
        /// The Crc16 Table
        /// </summary>
        public ushort[] CrcTable { get; protected set; }

        public Crc16()
        {
            // Build Standard Crc16 Table
            BuildCrcTable(0xA001);
        }

        public Crc16(ushort polynomial)
        {
            BuildCrcTable(polynomial);
        }

        public Crc16(Crc16Mode Mode)
        {
            BuildCrcTable((ushort)Mode);
        }

        /// <summary>
        /// Calculates the Checksum for the input string
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public ushort ComputeChecksum(string Input)
        {
            return ComputeChecksum(UniSpyEncoding.GetBytes(Input));
        }

        /// <summary>
        /// Calculates the Checksum for the given bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)(CrcTable[(bytes[i] ^ crc) & 0xFF] ^ (crc >> 8));
            }
            return crc;
        }

        /// <summary>
        /// Builds the Crc table programmatically with the given polynomial
        /// </summary>
        /// <param name="polynomial"></param>
        private void BuildCrcTable(ushort polynomial)
        {
            ushort value;
            ushort temp;

            // Build standard Crc16 Table
            CrcTable = new ushort[256];
            for (ushort i = 0; i < 256; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                        value = (ushort)((value >> 1) ^ polynomial);
                    else
                        value >>= 1;

                    temp >>= 1;
                }

                CrcTable[i] = value;
            }
        }
    }
}
