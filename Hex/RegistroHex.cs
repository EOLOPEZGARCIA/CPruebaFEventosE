using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hex
{
    public class RegistroHex
    {
        public Byte[] contador { get; set; }
        public int direccion { get; set; }
        public Byte[] tipoRegistro { get; set; }
        public Byte[] data { get; set; }
        public Byte[] checksum { get; set; }
    }
}
