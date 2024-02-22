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

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] despertar = new byte[] { 0x00, 0x10, 0x00, 0x12, 0x00, 0x04, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4f, 0x42 };
        byte[] lectura = new byte[] { 0x66, 0x03, 0x00, 0x05, 0x00, 0x08, 0x5c, 0x1a };
        byte[] saludo = new byte[] { 0x66, 0x06, 0x00, 0x1c, 0xc3, 0x01, 0xd1, 0x2b };
        byte[] reco = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x00, 0x41, 0xde };
        byte[] corte = new byte[] { 0x66, 0x06, 0x00, 0x0c, 0x00, 0x01, 0x80, 0x1e };
        
        byte[] buf2 = new Byte[6];
        byte[] buf3 = new Byte[6];
        byte[] buffer_out = new byte[26];
        byte[] buffer_in = new byte[26];
        string[] mac = { "34CFF60FF032", "482AE30D0517", "482AE30D0518", "482AE30D0519", "482AE30D0521", "34CFF61565DA", "482AE30D0511", "5CC5D4C24250" };

       // string mac = "482AE30D0511";
        String[] COMPU = new string[10];
        int ok;
        double voltaje = 0;
        double corriente = 0;
        double lectur = 0;
        public MainWindow()
        {
            ok = GetMacAddressgood();
            if (ok == 2)
            {
                InitializeComponent();
                foreach (String puerto in SerialPort.GetPortNames())
                {
                    cboPuerto.Items.Add(puerto);
                }

            }
            else
            {
                MessageBox.Show("Equipo de Computo invalido");
                this.Close();
            }
              //  System.Windows.Application.Current.Shutdown();

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

        private int GetMacAddressgood()
        {
           // string macAddresses = string.Empty;
            //string macAddresses2 = string.Empty;
            int a = 0;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                int i;
                for( i=0; i<mac.Length;i++)
                {
                    if (nic.GetPhysicalAddress().ToString() == mac[i])
                    {
                        a = 2;
                        break;
                    }
                }
                i = 0;
                if (a == 2)
                {
                    break;

                }
            }

            return a;
        }
        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
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
                serialPort.Write(despertar,0,despertar.Length);
                Byte[] direccionMed = StringToByteArray(Medidor.Text);
                Array.Copy(direccionMed, 0, lectura, 0, 1);

                Crc16Ccitt crc5 = new Crc16Ccitt();
                Array.Copy(lectura, 0, buf2, 0, 6);
                /*
                ushort send = crc5.CCITT_CRC16(lectura);
                Byte [] aCrc3 = BitConverter.GetBytes(send);
                */

                uint send = calc_crc(buf2, buf2.Length);
                Byte[] aCrc3 = BitConverter.GetBytes(send);
                //Array.Reverse(aCrc3);
                Array.Copy(aCrc3, 0, lectura, 6, 2);

                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                Array.Copy(direccionMed, 0, saludo, 0, 1);
                Array.Copy(saludo, 0, buf3, 0, 6);
                uint send2 = calc_crc(buf3, buf3.Length);
                Byte[] aCrc4 = BitConverter.GetBytes(send2);
                Array.Copy(aCrc4, 0, saludo, 6, 2);
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
                

                    voltaje = respuestaSerial[7] + respuestaSerial[8] * 0.003921569;
                    corriente = respuestaSerial[9] + respuestaSerial[10] * 0.003921569;
                    lectur = respuestaSerial[3] * 1024.0 + respuestaSerial[4] * 4.0 + respuestaSerial[5] * (1.0 / 64.0) + ((respuestaSerial[5] & 63) * (1.0 / byte.MaxValue) + respuestaSerial[6] * 1.52590218966964E-05);

                    Volts.Text = voltaje.ToString();
                    Amper.Text = corriente.ToString();
                    kWh.Text = lectur.ToString();
                    MessageBox.Show("Listo");
                }
                serialPort.Close();
                btnLectura.IsEnabled = true;
                
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
        private void BtnCorte_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnCorte.IsEnabled = false;
                SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                serialPort.Close();
                serialPort.Open(); 
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                Byte[] direccionMed = StringToByteArray(Medidor.Text);

                Array.Copy(direccionMed, 0, saludo, 0, 1);
                Array.Copy(saludo, 0, buf3, 0, 6);
                uint send2 = calc_crc(buf3, buf3.Length);
                Byte[] aCrc4 = BitConverter.GetBytes(send2);
                Array.Copy(aCrc4, 0, saludo, 6, 2);

                Array.Copy(direccionMed, 0, corte, 0, 1);
                Array.Copy(corte, 0, buf2, 0, 6);
                uint send = calc_crc(buf2, buf2.Length);
                Byte[] aCrc3 = BitConverter.GetBytes(send);
                Array.Copy(aCrc3, 0, corte, 6, 2);

                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;

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
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnCorte.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Listo");
                        btnCorte.IsEnabled = true;
                    }
                }
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnCorte.IsEnabled = true;
               
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
                serialPort.Write(despertar, 0, despertar.Length);
                Task.Delay(1000).Wait();
                Byte[] direccionMed = StringToByteArray(Medidor.Text);

                Array.Copy(direccionMed, 0, saludo, 0, 1);
                Array.Copy(saludo, 0, buf3, 0, 6);
                uint send2 = calc_crc(buf3, buf3.Length);
                Byte[] aCrc4 = BitConverter.GetBytes(send2);
                Array.Copy(aCrc4, 0, saludo, 6, 2);

                Array.Copy(direccionMed, 0, reco, 0, 1);
                Array.Copy(reco, 0, buf2, 0, 6);
                uint send = calc_crc(buf2, buf2.Length);
                Byte[] aCrc3 = BitConverter.GetBytes(send);
                Array.Copy(aCrc3, 0, reco, 6, 2);

                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;

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
                    btnReconexion.IsEnabled = true;
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
                    if (respuestaSerial == null)
                    {
                        MessageBox.Show("El medidor no contesta");
                        btnReconexion.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Listo");
                        btnReconexion.IsEnabled = true;
                    }
                }
                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnReconexion.IsEnabled = true;
               
            }
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
