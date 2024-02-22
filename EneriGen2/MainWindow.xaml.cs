
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Net.NetworkInformation;

namespace EneriGen2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] despertar = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xC3, 0x00, 0x00, 0x01, 0x27, 0x9B };
        byte[] lectura = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x03, 0x00, 0x20, 0x00, 0x5E, 0x30, 0x8C };
        byte[] saludo = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] reco = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xA0, 0xA9 };
        byte[] corte = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] numero = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xF5, 0x00, 0x03, 0x00, 0x04, 0x00, 0x08, 0x7D, 0x50 };
        public MainWindow()
        {
            InitializeComponent();
            foreach (String puerto in SerialPort.GetPortNames())
            {
                cboPuerto.Items.Add(puerto);
            }
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

        private void BtnLectura_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnLectura.IsEnabled = false;
                SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                serialPort.Close();
                serialPort.Open();
                Task.Delay(1000).Wait();
                int i = 0;
                Byte[] direccionMed = Encoding.ASCII.GetBytes(Medidor.Text);
                Array.Copy(direccionMed, 0, despertar, 10, 6);
                byte[] buf = new Byte[despertar.Length - 2];
                Array.Copy(despertar, 0, buf, 0, despertar.Length - 2);
                uint send = calc_crc(buf, buf.Length);
                Byte[] aCrc = BitConverter.GetBytes(send);
                Array.Reverse(aCrc, 0, aCrc.Length);
                Array.Copy(aCrc, 0, despertar, despertar.Length - 2, 2);
                Array.Copy(direccionMed, 0, saludo, 10, 6);
                byte[] buf1 = new Byte[saludo.Length - 2];
                Array.Copy(saludo, 0, buf1, 0, saludo.Length - 2);
                uint send1 = calc_crc(buf1, buf1.Length);
                Byte[] aCrc1 = BitConverter.GetBytes(send1);
                Array.Reverse(aCrc1, 0, aCrc1.Length);
                Array.Copy(aCrc1, 0, saludo, saludo.Length - 2, 2);
                Array.Copy(direccionMed, 0, lectura, 10, 6);
                byte[] buf2 = new Byte[lectura.Length - 2];
                Array.Copy(saludo, 0, buf2, 0, lectura.Length - 2);
                uint send2 = calc_crc(buf2, buf2.Length);
                Byte[] aCrc2 = BitConverter.GetBytes(send2);
                Array.Reverse(aCrc2, 0, aCrc2.Length);
                Array.Copy(aCrc2, 0, lectura, lectura.Length - 2, 2);
                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                do
                {
                    serialPort.ReadExisting();
                    serialPort.Write(saludo, 0, saludo.Length);
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
                    btnLectura.IsEnabled = true;
                }
                else
                {
                    intentos = 0;
                    respuestaSerial = null;
                    auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(lectura, 0, lectura.Length);
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
                }
                if (respuestaSerial == null)
                {
                    MessageBox.Show("El medidor no contesta");
                }
                else
                {
                    Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                    Array.Copy(lectura, 31, energy, 0, 4);
                    Byte[] voltaj = new Byte[] { 0x3E, 0xEB };//[2];
                    Array.Copy(lectura, 73, voltaj, 0, 2);
                    Byte[] corr = new Byte[] { 0x00, 0xE6 };//[2];
                    Array.Copy(lectura, 75, corr, 0, 2);
                    double Energia = (BitConverter.ToDouble(energy, 0)) / 1000.0D;
                    double Voltaje = (BitConverter.ToDouble(voltaj, 0)) / Math.Pow(2.0D, 7.0D);
                    double Corriente = (BitConverter.ToDouble(corr, 0)) / Math.Pow(2.0D, 9.0D);
                    Volts.Text = Voltaje.ToString();
                    Amper.Text = Corriente.ToString();
                    kWh.Text = Energia.ToString();
                    MessageBox.Show("Listo");
                }
                btnLectura.IsEnabled = true;
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnLectura.IsEnabled = true;
            }
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

        private void BtnCorte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnCorte.IsEnabled = false;
                SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                serialPort.Close();
                serialPort.Open();
                Task.Delay(1000).Wait();
                Byte[] direccionMed = Encoding.ASCII.GetBytes(Medidor.Text);
                Array.Copy(direccionMed, 0, despertar, 10, 6);
                byte[] buf = new Byte[despertar.Length - 2];
                Array.Copy(despertar, 0, buf, 0, despertar.Length - 2);
                uint send = calc_crc(buf, buf.Length);
                Byte[] aCrc = BitConverter.GetBytes(send);
                Array.Copy(aCrc, 0, despertar, despertar.Length - 2, 2);
                Array.Copy(direccionMed, 0, saludo, 10, 6);
                byte[] buf1 = new Byte[saludo.Length - 2];
                Array.Copy(saludo, 0, buf1, 0, saludo.Length - 2);
                uint send1 = calc_crc(buf1, buf1.Length);
                Byte[] aCrc1 = BitConverter.GetBytes(send1);
                Array.Copy(aCrc1, 0, saludo, saludo.Length - 2, 2);
                Array.Copy(direccionMed, 0, corte, 10, 6);
                byte[] buf2 = new Byte[corte.Length - 2];
                Array.Copy(saludo, 0, buf2, 0, corte.Length - 2);
                uint send2 = calc_crc(buf2, buf2.Length);
                Byte[] aCrc2 = BitConverter.GetBytes(send2);
                Array.Copy(aCrc2, 0, corte, corte.Length - 2, 2);
                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                do
                {
                    serialPort.ReadExisting();
                    serialPort.Write(saludo, 0, saludo.Length);
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
                    btnCorte.IsEnabled = true;
                }
                else
                {
                    intentos = 0;
                    respuestaSerial = null;
                    auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(corte, 0, corte.Length);
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
                }
                if (respuestaSerial == null)
                {
                    MessageBox.Show("El medidor no contesta");
                }
                else
                {
                    MessageBox.Show("Listo");
                }
                btnCorte.IsEnabled = true;
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnLectura.IsEnabled = true;
            }

        }

        private void BtnReconexion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnReconexion.IsEnabled = false;
                SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                serialPort.Close();
                serialPort.Open();
                Task.Delay(1000).Wait();
                Byte[] direccionMed = Encoding.ASCII.GetBytes(Medidor.Text);
                Array.Copy(direccionMed, 0, despertar, 10, 6);
                byte[] buf = new Byte[despertar.Length - 2];
                Array.Copy(despertar, 0, buf, 0, despertar.Length - 2);
                uint send = calc_crc(buf, buf.Length);
                Byte[] aCrc = BitConverter.GetBytes(send);
                Array.Copy(aCrc, 0, despertar, despertar.Length - 2, 2);
                Array.Copy(direccionMed, 0, saludo, 10, 6);
                byte[] buf1 = new Byte[saludo.Length - 2];
                Array.Copy(saludo, 0, buf1, 0, saludo.Length - 2);
                uint send1 = calc_crc(buf1, buf1.Length);
                Byte[] aCrc1 = BitConverter.GetBytes(send1);
                Array.Copy(aCrc1, 0, saludo, saludo.Length - 2, 2);
                Array.Copy(direccionMed, 0, reco, 10, 6);
                byte[] buf2 = new Byte[corte.Length - 2];
                Array.Copy(saludo, 0, buf2, 0, reco.Length - 2);
                uint send2 = calc_crc(buf2, buf2.Length);
                Byte[] aCrc2 = BitConverter.GetBytes(send2);
                Array.Copy(aCrc2, 0, reco, reco.Length - 2, 2);
                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                do
                {
                    serialPort.ReadExisting();
                    serialPort.Write(saludo, 0, saludo.Length);
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
                    btnCorte.IsEnabled = true;
                }
                else
                {
                    intentos = 0;
                    respuestaSerial = null;
                    auxiliar = null;
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(reco, 0, reco.Length);
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
                }
                if (respuestaSerial == null)
                {
                    MessageBox.Show("El medidor no contesta");
                }
                else
                {
                    MessageBox.Show("Listo");
                }
                btnCorte.IsEnabled = true;
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnLectura.IsEnabled = true;
            }

        }

        private void Btn_Med_Click(object sender, RoutedEventArgs e)
        {
            btnLectura.IsEnabled = false;
            SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
            serialPort.Close();
            serialPort.Open();
            Task.Delay(1000).Wait();
            byte[] respuestaSerial = null;
            byte[] auxiliar = null;
            int intentos = 0;
            serialPort.Write(numero, 0, numero.Length);
            Task.Delay(1000).Wait();
            do
            {
                serialPort.ReadExisting();
                serialPort.Write(saludo, 0, saludo.Length);
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
                btnLectura.IsEnabled = true;
            }
            else
            {
                intentos = 0;

                Byte[] medidor = new Byte[6] ;//[4];29
                Array.Copy(respuestaSerial, 29, medidor, 0, 6);
                Medidor.Text = medidor.ToString();
            }
        }
    }
}
