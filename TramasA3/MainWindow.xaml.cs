using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.IO.Ports;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Collections;
using Ejemplo_Elster;

namespace TramasA3
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        [DllImport("LibreriaElster2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GeneraLLave")]
        public static extern void GeneraLlave(int[] CadenaE, int[] password);
        SerialPort serialPort = new SerialPort();

        Byte[] tramaM0 = new Byte[] { 0x06 };
        Byte[] tramaM1 = new Byte[] { 0x55 };

        Byte[] tramaM2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };

        Byte[] tramaM3 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x04, 0x00, 0xff, 0x06, 0xce, 0x5a };

        Byte[] tramaM4 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x41, 0x64, 0x6d, 0x69, 0x6e, 0x69, 0x73, 0x74, 0x72, 0x61, 0xf9, 0x04 };

        Byte[] tramaM5 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xb6, 0xa5 };
        Byte[] tramaM44 = new Byte[20] { 0x06, 0xEE, 0x02, 0x20, 0x00, 0x00, 0x0B, 0x53, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        Byte[] tramaM6 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x1C, 0xB2, 0xA5 }; // insta
        Byte[] tramaM7 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        Byte[] tramaM8 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        Byte[] tramaM9 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x06, 0xea, 0x79 };//medidor
        Byte[] tramaM10 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x17, 0xE2, 0x78 };//acumulado
        Byte[] tramaM20 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00 };

        Byte[] MED = new Byte[6];
        Byte[] VoltajeA = new Byte[6];
        Byte[] VoltajeC = new Byte[6];
        Byte[] VoltajeB = new Byte[6];
        Byte[] AnguloVoltajeA = new Byte[6];
        Byte[] AnguloVoltajeC = new Byte[6];
        Byte[] AnguloVoltajeB = new Byte[6];
        Byte[] CorrienteA = new Byte[6];
        Byte[] CorrienteC = new Byte[6];
        Byte[] CorrienteB = new Byte[6];
        Byte[] AnguloCorrienteA = new Byte[6];
        Byte[] AnguloCorrienteC = new Byte[6];
        Byte[] AnguloCorrienteB = new Byte[6];
        Byte[] fKey = new Byte [8];
        int[] password = null;
        string medio = "";
        byte[] respuestaSerial = null;
        byte[] respuestaSerial2 = null;
        byte[] auxiliar = null;
        byte[] respuestaSerial3 = null;
        byte[] auxiliar2 = null;
        byte[] auxiliar3 = null;
        byte[] respuestaSerial4 = null;
        byte[] auxiliar4 = null;

        public MainWindow()
        {
            InitializeComponent();
            cbo_Puerto.Text = " ";
            foreach (String puerto in SerialPort.GetPortNames())
            {
                cbo_Puerto.Items.Add(puerto);
            }
            MessageBox.Show("Ingrese el puerto COM del lector optico");
            cbo_Puerto.Focus();
            Tipo.Text = " ";
            Tipo.Items.Add("Optico");
            Tipo.Items.Add("Interno");

        }

        // arreglo 64 bits

        byte[] EntregaLLave(byte[] fKey, int[] password)
        {
            int[] cadenaE = new int[64];

            BitArray bits = new BitArray(fKey.Reverse().ToArray());


            for (int x = 0; x < bits.Count; x++)
            {
                cadenaE[x] = (bits.Get(x) == true) ? 1 : 0;
            }

            Array.Reverse(cadenaE);

            GeneraLlave(cadenaE, password);

            StringBuilder sb = new StringBuilder(cadenaE.Length / 4);

            for (int y = 0; y < cadenaE.Length; y += 4)
            {
                int v = (cadenaE[y] == 1 ? 8 : 0) |
                        (cadenaE[y + 1] == 1 ? 4 : 0) |
                        (cadenaE[y + 2] == 1 ? 2 : 0) |
                        (cadenaE[y + 3] == 1 ? 1 : 0);

                sb.Append(v.ToString("x1")); // Or "X1"
            }

            byte[] respuestaKey = new byte[sb.ToString().Length / 2];
            for (int z = 0; z < sb.ToString().Length; z += 2)
            {
                respuestaKey[z / 2] = Convert.ToByte(sb.ToString().Substring(z, 2), 16);
            }
            return respuestaKey;
        }

        
        
        
        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Iniciar.IsEnabled = false;
                medio = Tipo.Items.GetItemAt(Tipo.SelectedIndex).ToString();
                
                int intentos = 0;
                Task.Delay(500).Wait();
                serialPort.Write(tramaM1, 0, tramaM1.Length);
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
                Bandeja.Text = Bandeja.Text + tramaM1 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial + "\r\n";
                respuestaSerial = null;
                serialPort.Write(tramaM2, 0, tramaM2.Length);
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
                    Bandeja.Text = Bandeja.Text + tramaM2 + "\r\n";
                    Bandeja.Text = Bandeja.Text + respuestaSerial + "\r\n";
                    respuestaSerial = null;
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                    serialPort.Write(tramaM3, 0, tramaM3.Length);
                    Task.Delay(500).Wait();
                    while (serialPort.BytesToRead > 0)
                    {
                        if (respuestaSerial2 == null)
                        {
                            respuestaSerial2 = new byte[serialPort.BytesToRead];
                            serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                        }
                        else
                        {
                            auxiliar2 = new byte[respuestaSerial2.Length];
                            auxiliar2 = respuestaSerial2;
                            byte[] bytes2 = new byte[serialPort.BytesToRead];
                            respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                            Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                            serialPort.Read(bytes2, 0, bytes2.Length);
                            Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                        }
                    }
                    Bandeja.Text = Bandeja.Text + tramaM3 + "\r\n";
                    Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                    respuestaSerial2 = null;
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM4, 0, tramaM4.Length);
                    Task.Delay(500).Wait();
                    while (serialPort.BytesToRead > 0)
                    {
                        if (respuestaSerial2 == null)
                        {
                            respuestaSerial2 = new byte[serialPort.BytesToRead];
                            serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                        }
                        else
                        {
                            auxiliar2 = new byte[respuestaSerial2.Length];
                            auxiliar2 = respuestaSerial2;
                            byte[] bytes2 = new byte[serialPort.BytesToRead];
                            respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                            Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                            serialPort.Read(bytes2, 0, bytes2.Length);
                            Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                        }
                    }
                    Bandeja.Text = Bandeja.Text + tramaM4 + "\r\n";
                    Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                    respuestaSerial2 = null;
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";

                    if (medio == "Optico")
                    {
                        respuestaSerial = null;
                        serialPort.Write(tramaM5, 0, tramaM5.Length);
                        Task.Delay(500).Wait();
                        Bandeja.Text = Bandeja.Text + tramaM5 + "\r\n";
                    }
                    else if((medio == "Interno"))
                    {
                        Array.Copy(respuestaSerial, 15, fKey, 0, 8);
                        byte[] respuestaKey = EntregaLLave(fKey, password);
                        Byte[] ChkCRC = new Byte[17];
                        Array.Copy(respuestaKey, 0, tramaM44, 10, 8);
                        Array.Copy(tramaM44, 1, ChkCRC, 0, 17);
                        Crc16Ccitt crc = new Crc16Ccitt();
                        ushort cr = crc.CCITT_CRC16(ChkCRC);
                        Byte[] aCrc = BitConverter.GetBytes(cr);
                        Array.Reverse(aCrc);
                        Array.Copy(aCrc, 0, tramaM44, 18, 2);
                        respuestaSerial = null;
                        serialPort.Write(tramaM44, 0, tramaM44.Length);
                        Task.Delay(500).Wait();
                        Bandeja.Text = Bandeja.Text + tramaM44 + "\r\n";
                    }
                   
                    
                    
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
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    Bandeja.Text = Bandeja.Text + respuestaSerial + "\r\n";
                    Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                    int a = 0;
                    for (int i = 0; i < respuestaSerial.Length; i++)
                    {
                        if (respuestaSerial[i] == 0xee && respuestaSerial[i + 1] == 0x00)
                        {
                            a = i;
                            break;
                        }
                    }
                    
                    if (respuestaSerial[a + 5] == 0x01 && respuestaSerial[a + 6] == 0x00)
                    {
                        MessageBox.Show("Ya entro");
                    }
                    else
                    {
                        MessageBox.Show("La contraseña no es");
                    }
                    respuestaSerial = null;
                }
                Iniciar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Iniciar.IsEnabled = true;
            }

        }

        public void Lectura_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort.Write(tramaM10, 0, tramaM10.Length);
                Task.Delay(500).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial2 == null)
                    {
                        respuestaSerial2 = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                    }
                    else
                    {
                        auxiliar2 = new byte[respuestaSerial2.Length];
                        auxiliar2 = respuestaSerial2;
                        byte[] bytes2 = new byte[serialPort.BytesToRead];
                        respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                        serialPort.Read(bytes2, 0, bytes2.Length);
                        Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                    }
                }
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                Bandeja.Text = Bandeja.Text + tramaM10 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                respuestaSerial2 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Iniciar.IsEnabled = true;
            }
        }

        public void Instantaneos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort.Write(tramaM6, 0, tramaM6.Length);
                Task.Delay(500).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial2 == null)
                    {
                        respuestaSerial2 = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                    }
                    else
                    {
                        auxiliar2 = new byte[respuestaSerial2.Length];
                        auxiliar2 = respuestaSerial2;
                        byte[] bytes2 = new byte[serialPort.BytesToRead];
                        respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                        serialPort.Read(bytes2, 0, bytes2.Length);
                        Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                    }
                }
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                Bandeja.Text = Bandeja.Text + tramaM6 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                respuestaSerial2 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Iniciar.IsEnabled = true;
            }
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort.Write(tramaM9, 0, tramaM9.Length);
                Task.Delay(500).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial2 == null)
                    {
                        respuestaSerial2 = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                    }
                    else
                    {
                        auxiliar2 = new byte[respuestaSerial2.Length];
                        auxiliar2 = respuestaSerial2;
                        byte[] bytes2 = new byte[serialPort.BytesToRead];
                        respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                        serialPort.Read(bytes2, 0, bytes2.Length);
                        Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                    }
                }
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                Bandeja.Text = Bandeja.Text + tramaM9 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                respuestaSerial2 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Iniciar.IsEnabled = true;
            }
        }

        public void Abrir_Click(object sender, RoutedEventArgs e)
        {
            SerialPort serialPort = new SerialPort(cbo_Puerto.Text, 9600, Parity.None, 8, StopBits.One);
            serialPort.Close();
            serialPort.Open();

        }

        public void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Close();
        }

       

        public void Enviar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cbo_Puerto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tipo.Focus();
        }

        private void Tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Iniciar.Focus();
        }

        private void Fin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort.Write(tramaM7, 0, tramaM7.Length);
                Task.Delay(500).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial2 == null)
                    {
                        respuestaSerial2 = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                    }
                    else
                    {
                        auxiliar2 = new byte[respuestaSerial2.Length];
                        auxiliar2 = respuestaSerial2;
                        byte[] bytes2 = new byte[serialPort.BytesToRead];
                        respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                        serialPort.Read(bytes2, 0, bytes2.Length);
                        Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                    }
                }
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                Bandeja.Text = Bandeja.Text + tramaM7 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                respuestaSerial2 = null;
                serialPort.Write(tramaM8, 0, tramaM8.Length);
                Task.Delay(500).Wait();
                while (serialPort.BytesToRead > 0)
                {
                    if (respuestaSerial2 == null)
                    {
                        respuestaSerial2 = new byte[serialPort.BytesToRead];
                        serialPort.Read(respuestaSerial2, 0, respuestaSerial2.Length);
                    }
                    else
                    {
                        auxiliar2 = new byte[respuestaSerial2.Length];
                        auxiliar2 = respuestaSerial2;
                        byte[] bytes2 = new byte[serialPort.BytesToRead];
                        respuestaSerial2 = new byte[auxiliar2.Length + bytes2.Length];
                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                        serialPort.Read(bytes2, 0, bytes2.Length);
                        Array.Copy(bytes2, 0, respuestaSerial2, auxiliar2.Length, bytes2.Length);
                    }
                }
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                Bandeja.Text = Bandeja.Text + tramaM8 + "\r\n";
                Bandeja.Text = Bandeja.Text + respuestaSerial2 + "\r\n";
                Bandeja.Text = Bandeja.Text + tramaM0 + "\r\n";
                respuestaSerial2 = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Iniciar.IsEnabled = true;
            }
        }
    }
}
