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

namespace ReconexionSiemens
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Byte[] S0 = new Byte[] { 0x06 };
        Byte[] S1 = new Byte[] { 0x55 };
        Byte[] S2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        Byte[] S3 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x20, 0x20, 0x20, 0x20, 0x4b, 0x38, 0x31, 0x31, 0x4c, 0x50, 0x34, 0xb7 };
        Byte[] S4 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x15, 0x51, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xaf, 0xa3 };
        Byte[] S5 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x05, 0x00, 0x38, 0x00, 0x00, 0x02, 0xc6, 0xec, 0x57 };//reco
        Byte[] S6 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x52, 0x86, 0x40 };
        Byte[] S7 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x01, 0x21, 0x0b, 0x61 };

        string[] mac = { "60EB695D004D", "34CFF60FF032" };
        String COMPU = "";
        int ok;
        int tiempo = 300;

        int tiempo2 = 2000;
        DateTime Quincena = new DateTime(2021, 08, 30, 0, 0, 0);

        string MARCA = "";

        public MainWindow()
        {
            ok = GetMacAddressgood();

            int fecha2 = DateTime.Compare(DateTime.Now, Quincena);
            if (ok == 2 && fecha2 < 1)
            {
                InitializeComponent();
                foreach (String puerto in SerialPort.GetPortNames())
                {
                    cboPuerto.Items.Add(puerto);
                }

            }
            else
            {
                MessageBox.Show("Error: Contacte a Jefe de Laboratorio");
                this.Close();
            }
        }

        private int GetMacAddressgood()
        {
            // string macAddresses = string.Empty;
            //string macAddresses2 = string.Empty;
            int a = 0;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                int i;
                for (i = 0; i < mac.Length; i++)
                {
                    if (nic.GetPhysicalAddress().ToString() == mac[i])
                    {
                        COMPU = mac[i];
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

        private void btnAccion_Click(object sender, RoutedEventArgs e)
        {
            SerialPort serialPort = new SerialPort(cboPuerto.Text, 9600, Parity.None, 8, StopBits.One);

            try
            {
                btnAccion.IsEnabled = false;

                serialPort.Close();
                serialPort.Open();
                int intentos = 0;
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                int a = 0, b = 0, c = 0, d = 0;
                Task.Delay(tiempo).Wait();
                serialPort.Write(S1, 0, S1.Length);
                Task.Delay(500).Wait();
                serialPort.Write(S2, 0, S2.Length);
                Task.Delay(500).Wait();
                do
                {
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
                    serialPort.Close();
                }
                else
                {
                    respuestaSerial = null;
                    serialPort.Write(S0, 0, S0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(S3, 0, S3.Length);
                    Task.Delay(500).Wait();
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
                    respuestaSerial = null;
                    serialPort.Write(S0, 0, S0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(S4, 0, S4.Length);
                    Task.Delay(500).Wait();
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
                    respuestaSerial = null;
                    serialPort.Write(S0, 0, S0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(S5, 0, S5.Length);
                    Task.Delay(500).Wait();
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
                    respuestaSerial = null;
                    serialPort.Write(S0, 0, S0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(S6, 0, S6.Length);
                    Task.Delay(500).Wait();
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
                    respuestaSerial = null;
                    serialPort.Write(S0, 0, S0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(S7, 0, S7.Length);
                    Task.Delay(500).Wait();
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

                   
                }
                serialPort.Close();
                btnAccion.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnAccion.IsEnabled = true;
                serialPort.Close();
            }
        }
    }
}