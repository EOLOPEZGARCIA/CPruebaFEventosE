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

namespace Cambio_Hora
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Byte[] tramaM1 = new Byte[] { 0x55 };
        Byte[] tramaM2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        Byte[] tramaM3 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x04, 0x00, 0xff, 0x06, 0xce, 0x5a };
        Byte[] tramaM4 = new Byte[] { 0x06, 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x41, 0x64, 0x6d, 0x69, 0x6e, 0x69, 0x73, 0x74, 0x72, 0x61, 0xf9, 0x04 };
        Byte[] tramaM5 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x11, 0x52 };
        Byte[] tramaM6 = new Byte[] { 0x06, 0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x01, 0x55, 0x0d };
        Byte[] tramaM7 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x0c, 0x40, 0x00, 0x35, 0x00, 0x06, 0x02, 0x00, 0x00, 0x3c, 0x98, 0xfe, 0x2c, 0x8d, 0x0a };
        Byte[] tramaM8 = new Byte[] { 0x06, 0xee, 0x00, 0x00, 0x00, 0x00, 0x11, 0x40, 0x00, 0x07, 0x00, 0x0b, 0x0a, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x0c, 0x29, 0x31, 0x60, 0x2b, 0xcc, 0x7c };
        Byte[] tramaM9 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x17, 0xf3 };
        Byte[] tramaM10 = new Byte[] { 0x06, 0xee, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x03, 0x00, 0x00, 0x01, 0x00, 0x02, 0x29, 0x7e };
        Byte[] tramaM11 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x09, 0x40, 0x00, 0x07, 0x00, 0x03, 0x0b, 0x00, 0x00, 0xf5, 0x22, 0x90 };
        Byte[] tramaM12 = new Byte[] { 0x15 };
        Byte[] tramaM13 = new Byte[] { 0x06, 0xee, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x08, 0x94, 0x90 };
        Byte[] tramaM14 = new Byte[] { 0x06, 0xee, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x08, 0x03, 0x04, 0x83 };






        Byte [] unresviej = new Byte[] { 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31 };

        public MainWindow()
        {
            InitializeComponent();
            foreach (String puerto in SerialPort.GetPortNames())
            {
                cboPuerto.Items.Add(puerto);
            }
        }

        public byte Checksum(byte[] bytes)
        {
            int chkSum = bytes.Aggregate(0, (s, b) => s += b) & 0xff;
            chkSum = (0x100 - chkSum) & 0xff;

            return (Byte)chkSum; // <-- D9
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnLectura.IsEnabled = false;
                SerialPort serialPort = new SerialPort();
                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                Byte[] ChkCRC5 = new Byte[27];
                Byte[] ChkCRC16 = new Byte[78];
                Byte[] Chk8s16 = new Byte[76];
                Byte[] ChkCRC17 = new Byte[39];
                Byte[] Chk8s17 = new Byte[24];
                Array.Copy(unresviej, 0, tramaM5, 8, 20);
                Array.Copy(tramaM5, 1, ChkCRC5, 0, 27);
                Crc16Ccitt crc5 = new Crc16Ccitt();
                ushort a5 = crc5.CCITT_CRC16(ChkCRC5);
                Byte[] aCrc5 = BitConverter.GetBytes(a5);
                Array.Reverse(aCrc5);
                Array.Copy(aCrc5, 0, tramaM5, 28, 2);
                List<Byte[]> tramasGM = new List<Byte[]>();
                tramasGM.Add(tramaM1);
                tramasGM.Add(tramaM2);
                tramasGM.Add(tramaM3);
                tramasGM.Add(tramaM4);
                tramasGM.Add(tramaM5);
                tramasGM.Add(tramaM6);
                tramasGM.Add(tramaM7);
                tramasGM.Add(tramaM8);
                tramasGM.Add(tramaM9);
                tramasGM.Add(tramaM10);
                tramasGM.Add(tramaM11);
                tramasGM.Add(tramaM12);
                tramasGM.Add(tramaM13);
                tramasGM.Add(tramaM14);
                serialPort.Close();
                serialPort.PortName = cboPuerto.Text;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.Open();

                foreach (Byte[] trama in tramasGM)
                {
                    serialPort.Write(trama, 0, trama.Length);

                    serialPort.ReadExisting();
                    Thread.Sleep(1000);

                }

                MessageBox.Show("Actualizacion Finalizada");
                btnLectura.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnLectura.IsEnabled = true;

            }
        }
    }
}
