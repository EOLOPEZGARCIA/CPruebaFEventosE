using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Eneri_Traductor
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] E1despertar = new byte[] { 0x00, 0x10, 0x00, 0x12, 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4f, 0x42 };
        byte[] E1lectura = new byte[] { 0x66, 0x03, 0x00, 0x05, 0x00, 0x08, 0x5c, 0x1a };
        byte[] E1saludo = new byte[] { 0x66, 0x06, 0x00, 0x1c, 0xc3, 0x01, 0xd1, 0x2b };
        byte[] E1reco = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x00, 0x41, 0xde };
        byte[] E1corte = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x01, 0x80, 0x1e };
        byte[] E1med = new byte[] { 0x66, 0x03, 0x00, 0x0e, 0x00, 0x03, 0x67, 0x2f };
        double lectur = 0;

        byte[] E2despertar = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xC3, 0x00, 0x00, 0x01, 0x27, 0x9B };
        byte[] E2despertar2 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x06, 0x00, 0xc4, 0x00, 0x34, 0x70, 0xab };
        byte[] E2despertar3 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x06, 0x00, 0xc5, 0x00, 0x45, 0x94, 0x3a };
        byte[] E2lectura = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x03, 0x00, 0x20, 0x00, 0x5E, 0x30, 0x8C };
        //  byte[] E2saludo = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] E2reco = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xA0, 0xA9 };
        byte[] E2corte = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] E2numero = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xF5, 0x00, 0x03, 0x00, 0x04, 0x00, 0x08, 0x7D, 0x50 };
        int ok;
        int tiempo = 300;
        byte[] buf = new Byte[6];
        byte[] buf2 = new Byte[6];
        byte[] buf3 = new Byte[6];
        string salida = "";
        string medidor = "";
        string cadena = "";



        public MainWindow()
        {
            InitializeComponent();
            foreach (String puerto in SerialPort.GetPortNames())
            {
                cboPuerto.Items.Add(puerto);
            }

        }

        private void Btn_Accionar_Click(object sender, RoutedEventArgs e)
        {
            Btn_Accionar.IsEnabled = false;
            salida = "";
            cadena = Txt_Input.Text.Replace(" ", "");
            Byte[] respuestaserial = StringToByteArray(cadena);
            if (respuestaserial.Length < 34)
            {
                MessageBox.Show("La cadena es mas corta a la esperada");
                Btn_Accionar.IsEnabled = true;
            }
            else
            {
                Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                Array.Copy(respuestaserial, 31, energy, 0, 4);
                Array.Reverse(energy);
                medidor = Encoding.ASCII.GetString(respuestaserial, 10, 6);
                double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                Byte[] voltaj = new Byte[] { 0x3E, 0xEB };//[2];
                Array.Copy(respuestaserial, 73, voltaj, 0, 2);
                Byte[] corr = new Byte[] { 0x00, 0xE6 };//[2];
                Array.Copy(respuestaserial, 75, corr, 0, 2);
                double Voltaje = (BitConverter.ToDouble(voltaj, 0)) / Math.Pow(2.0D, 7.0D);
                double Corriente = (BitConverter.ToDouble(corr, 0)) / Math.Pow(2.0D, 9.0D);
                //Volts.Text = Voltaje.ToString();
                //Amper.Text = Corriente.ToString();
                salida = medidor+ "  Voltaje: " + Voltaje.ToString() + ",  Corriente: " + Corriente.ToString() + ",  Energia: " + Energia.ToString();
                Txt_Output.Text = salida;
                MessageBox.Show("Listo");
            }
        
        Btn_Accionar.IsEnabled = true;

        }


        private void Txt_Input_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Btn_Lectura_1_Click(object sender, RoutedEventArgs e)
        {
            SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
            serialPort.Close();
            serialPort.Open();
            Task.Delay(1000).Wait();
            Btn_Lectura_1.IsEnabled = false;
            Byte[] direccionMed = StringToByteArray(Txt_id.Text);

            Array.Copy(direccionMed, 0, E1lectura, 0, 1);
            Array.Copy(E1lectura, 0, buf2, 0, 6);
            uint send = calc_crc(buf2, buf2.Length);
            Byte[] aCrc3 = BitConverter.GetBytes(send);
            Array.Copy(aCrc3, 0, E1lectura, 6, 2);

            Array.Copy(direccionMed, 0, E1med, 0, 1);
            Array.Copy(E1med, 0, buf3, 0, 6);
            uint send4 = calc_crc(buf3, buf3.Length);
            Byte[] aCrc4 = BitConverter.GetBytes(send4);
            Array.Copy(aCrc4, 0, E1med, 6, 2);

            int intentos = 0;
            byte[] respuestaSerial = null;
            byte[] auxiliar = null;
            serialPort.Write(E1despertar, 0, E1despertar.Length);
            Task.Delay(1000).Wait();

            Array.Copy(direccionMed, 0, E1saludo, 0, 1);
            Array.Copy(E1saludo, 0, buf3, 0, 6);
            uint send5 = calc_crc(buf3, buf3.Length);
            Byte[] aCrc5 = BitConverter.GetBytes(send5);
            Array.Copy(aCrc5, 0, E1saludo, 6, 2);

            do
            {
                serialPort.ReadExisting();
                serialPort.Write(E1saludo, 0, E1saludo.Length);
                Task.Delay(1000).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial == null)
                    {
                        respuestaSerial = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                    }
                    else
                    {
                        auxiliar = new byte[respuestaSerial.Length];
                        auxiliar = respuestaSerial;
                        byte[] bytes = new byte[serialPort.BytesToRead];
                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                        serialPort.Read(bytes, 0, bytes.Length);
                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                    }
                }
                intentos += 1;
            } while (intentos < 3 && respuestaSerial == null);

            if (respuestaSerial == null)
            {
                MessageBox.Show("El medidor no contesta");
                Btn_Lectura_1.IsEnabled = true;
            }
            else
            {
                intentos = 0;
                do
                {
                    serialPort.ReadExisting();
                    respuestaSerial = null;
                    auxiliar = null;
                    serialPort.Write(E1med, 0, E1med.Length);
                    Task.Delay(1000).Wait();

                    while (serialPort.BytesToRead > 0)
                    {
                        if (respuestaSerial == null)
                        {
                            respuestaSerial = new byte[serialPort.BytesToRead];
                            serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                        }
                        else
                        {
                            auxiliar = new byte[respuestaSerial.Length];
                            auxiliar = respuestaSerial;
                            byte[] bytes = new byte[serialPort.BytesToRead];
                            respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                            Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                            serialPort.Read(bytes, 0, bytes.Length);
                            Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                        }
                    }
                    intentos += 1;
                } while (intentos < 3 && respuestaSerial == null);

                if (respuestaSerial == null)
                {
                    MessageBox.Show("El medidor no contesta");
                    Btn_Lectura_1.IsEnabled = true;
                }
                else
                {
                    byte[] MED = new byte[6];
                    for (int i = 0; i < 6; i++)
                    {
                        MED[i] = respuestaSerial[i + 3];
                    }
                    intentos = 0;
                    respuestaSerial = null;
                    auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(E1lectura, 0, E1lectura.Length);
                        Task.Delay(1000).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                            }
                        }
                        intentos += 1;
                    } while (intentos < 3 && respuestaSerial == null);
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");

                    }
                    else
                    {
                        // voltaje = respuestaSerial[7] + respuestaSerial[8] * 0.003921569;
                        //corriente = respuestaSerial[9] + respuestaSerial[10] * 0.003921569;

                        Txt_Input.Text = BitConverter.ToString(respuestaSerial);
                        lectur = respuestaSerial[3] * 1024.0 + respuestaSerial[4] * 4.0 + respuestaSerial[5] * (1.0 / 64.0) + ((respuestaSerial[5] & 63) * (1.0 / byte.MaxValue) + respuestaSerial[6] * 1.52590218966964E-05);

                        // Volts.Text = voltaje.ToString();
                        // Amper.Text = corriente.ToString();
                        Txt_Output.Text = lectur.ToString();
                        MessageBox.Show("Listo");

                    }

                }
            }
            serialPort.Close();
            Btn_Lectura_1.IsEnabled = true;


        }

        private void Btn_Lectura_2_Click(object sender, RoutedEventArgs e)
        {
            SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
            serialPort.Close();
            serialPort.Open();
            Task.Delay(1000).Wait();
            Btn_Lectura_2.IsEnabled = false;
            Byte[] direccionMed = Encoding.ASCII.GetBytes(Txt_id.Text);
            Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
            byte[] buf = new Byte[E2despertar2.Length - 2];
            Array.Copy(E2despertar2, 0, buf, 0, E2despertar2.Length - 2);
            uint send = calc_crc(buf, buf.Length);
            Byte[] aCrc = BitConverter.GetBytes(send);
            Array.Reverse(aCrc, 0, aCrc.Length);
            Array.Copy(aCrc, 2, E2despertar2, E2despertar2.Length - 2, 2);
            Array.Copy(direccionMed, 0, E2despertar2, 10, 6);
            byte[] buf1 = new Byte[E2despertar2.Length - 2];
            Array.Copy(E2despertar2, 0, buf1, 0, E2despertar2.Length - 2);
            uint send1 = calc_crc(buf1, buf1.Length);
            Byte[] aCrc1 = BitConverter.GetBytes(send1);
            Array.Reverse(aCrc1, 0, aCrc1.Length);
            Array.Copy(aCrc1, 2, E2despertar2, E2despertar2.Length - 2, 2);
            Array.Copy(direccionMed, 0, E2lectura, 10, 6);
            byte[] buf2 = new Byte[E2lectura.Length - 2];
            Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
            uint send2 = calc_crc(buf2, buf2.Length);
            Byte[] aCrc2 = BitConverter.GetBytes(send2);
            Array.Reverse(aCrc2, 0, aCrc2.Length);
            Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
            int intentos = 0;
            byte[] respuestaSerial = null;
            byte[] auxiliar = null;
            serialPort.Write(E2despertar2, 0, E2despertar2.Length);
            Task.Delay(1000).Wait();
            do
            {
                serialPort.ReadExisting();
                serialPort.Write(E2despertar2, 0, E2despertar2.Length);
                Task.Delay(1000).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial == null)
                    {
                        respuestaSerial = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                    }
                    else
                    {
                        auxiliar = new byte[respuestaSerial.Length];
                        auxiliar = respuestaSerial;
                        byte[] bytes = new byte[serialPort.BytesToRead];
                        respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                        serialPort.Read(bytes, 0, bytes.Length);
                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                    }
                }
                intentos += 1;
            } while (intentos < 3 && respuestaSerial == null);

            if (respuestaSerial == null)
            {
                MessageBox.Show("El medidor no contesta");
                Btn_Lectura_2.IsEnabled = true;
            }
            else
            {
                intentos = 0;
                respuestaSerial = null;
                auxiliar = null;
                do
                {
                    serialPort.ReadExisting();
                    serialPort.Write(E2lectura, 0, E2lectura.Length);
                    Task.Delay(1000).Wait();
                    while (serialPort.BytesToRead > 0)
                    {
                        if (respuestaSerial == null)
                        {
                            respuestaSerial = new byte[serialPort.BytesToRead];
                            serialPort.Read(respuestaSerial, 0, respuestaSerial.Length);
                        }
                        else
                        {
                            auxiliar = new byte[respuestaSerial.Length];
                            auxiliar = respuestaSerial;
                            byte[] bytes = new byte[serialPort.BytesToRead];
                            respuestaSerial = new byte[auxiliar.Length + bytes.Length];
                            Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                            serialPort.Read(bytes, 0, bytes.Length);
                            Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                        }
                    }
                    intentos += 1;
                } while (intentos < 3 && respuestaSerial == null);
                if (respuestaSerial == null)
                {
                    MessageBox.Show("El medidor no contesta");
                    Btn_Lectura_2.IsEnabled = true;
                }
                else
                {
                    Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                    Txt_Input.Text = BitConverter.ToString(respuestaSerial);
                    Array.Copy(respuestaSerial, 31, energy, 0, 4);
                    Array.Reverse(energy);
                    double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                    Txt_Output.Text = Energia.ToString();

                    MessageBox.Show("Listo");
                }
            }
            serialPort.Close();
            Btn_Lectura_2.IsEnabled = true;


        }
        public static byte[] fromStringAsciiToByteArray(string input)
        {
            byte[] bytes = new byte[input.Length];
            char[] values = input.ToCharArray();
            for (int i = 0; i < values.Length; i++)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(values[i]);
                // Convert the decimal value to a byte value
                bytes[i] = (byte)value;
            }
            return bytes;
        }
        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        uint calc_crc(byte[] ptbuf, int num)
        {
            uint crc16 = 0xffff;
            uint temp;
            uint flag;

            for (int i = 0; i < num; i++)
            {
                temp = (uint)ptbuf[i]; // temp has the first byte 
                temp &= 0x00ff; // mask the MSB 
                crc16 = crc16 ^ temp; //crc16 XOR with temp 
                for (uint c = 0; c < 8; c++)
                {
                    flag = crc16 & 0x01; // LSBit di crc16 is mantained
                    crc16 = crc16 >> 1; // Lsbit di crc16 is lost 
                    if (flag != 0)
                        crc16 = crc16 ^ 0x0a001; // crc16 XOR with 0x0a001 
                }
            }
            //crc16 = (crc16 >> 8) | (crc16 << 8); // LSB is exchanged with MSB
            return (crc16);
        }
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
}
