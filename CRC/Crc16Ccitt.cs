using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace CRC
{
    public enum InitialCrcValue { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F }

    public enum Crc16Mode : ushort { Standard = 0xA001, CcittKermit = 0x8408 }

    public class Crc16Ccitt
    {
        public ushort CCITT_CRC16(byte[] bytes)
        {
            ushort data;
            ushort crc = 0xFFFF;

            for (int j = 0; j < bytes.Length; j++)
            {
                crc = (ushort)(crc ^ bytes[j]);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x0001) == 1)
                        crc = (ushort)((crc >> 1) ^ 0x8408);
                    else
                        crc >>= 1;
                }
            }
            crc = (ushort)~crc;
            data = crc;
            crc = (ushort)((crc << 8) ^ (data >> 8 & 0xFF));
            return crc;
        }

        private byte[] GetBytesFromHexString(string strInput)
        {
            Byte[] bytArOutput = new Byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return bytArOutput;
        }
    }
}
