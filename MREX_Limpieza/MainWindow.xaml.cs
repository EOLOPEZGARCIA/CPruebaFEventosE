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

namespace MREX_Limpieza
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] T0 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x05, 0xb0, 0x40 }; //SALUDO
        byte[] T1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x20, 0x62, 0x3a }; //NUMERO MEDIDOR
        byte[] T2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x01, 0xc8, 0xa6 }; //TIPO DE LECTURA
        byte[] T3_1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x29, 0x82, 0x0b };// LECTURA normal
        byte[] T3_2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x34, 0xe6, 0xc0 };// LECTURA bidi
        byte[] T4 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 }; //Estado
        byte[] T5 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x09, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x26, 0x23, 0xdb };// Validacion reco
        byte[] T7 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x80, 0x00, 0x3e, 0x04 };//reconexion
        byte[] T8 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x40, 0x00, 0x07, 0x00, 0x03, 0x00, 0x03, 0x00, 0x33, 0xcd };//reset
        byte[] T9 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x34, 0x00, 0x16, 0x39, 0x27 };//pre
        bool bidi = false;

        byte[] E2despertar = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xC3, 0x00, 0x00, 0x01, 0x27, 0x9B };
        byte[] E2preguntar = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xf5, 0x00, 0x03, 0x00, 0x04, 0x00, 0x08, 0x7d, 0x50 };
        byte[] E2despertar3 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x06, 0x00, 0xc5, 0x00, 0x45, 0x94, 0x3a };

        byte[] E2lectura = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x03, 0x00, 0x20, 0x00, 0x5E, 0x30, 0x8C };
        //  byte[] E2saludo = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x60, 0x68 };
        byte[] E2reco = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x38, 0x30, 0x54, 0x55, 0x42, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xA0, 0xA9 };
        byte[] E2reset1 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x39, 0x38, 0x54, 0x55, 0x52, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xc3, 0x00, 0x00, 0x20, 0x37, 0xc7 };
        byte[] E2reset2 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x39, 0x38, 0x54, 0x55, 0x52, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x00, 0x20, 0x70, 0x34 };
        byte[] E2reset3 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x39, 0x38, 0x54, 0x55, 0x52, 0x00, 0x10, 0x00, 0x17, 0x00, 0x02, 0xc3, 0x00, 0x01, 0x00, 0x37, 0xc7 };
        byte[] E2reset4 = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x35, 0x39, 0x38, 0x54, 0x55, 0x52, 0x00, 0x10, 0x00, 0x28, 0x00, 0x02, 0x00, 0x00, 0x01, 0x00, 0x70, 0x34 };




        byte[] respuestaSerial = null;
        byte[] auxiliar = null;
        byte[] respuestaSerial1 = null;
        byte[] auxiliar1 = null;
        byte[] respuestaSerial2 = null;
        byte[] auxiliar2 = null;
        byte[] respuestaSerial3 = null;
        byte[] auxiliar3 = null;
        byte[] Medidor = new byte[6];
        byte[] Act = new byte[3];
        byte[] React = new byte[3];
        string Rel = "";


        string[] mac = { "846993047C7E", "2CF05D501246", "2CF05D50123B", "64006A6DE24B", "2CF05D4FF3D0", "D8F883526FE3", "CC3D826CC57D", "4CEB42C8E74F","2CF05D4FF3D0", "34CFF6157291","5CC5D4C24250", "64006A6DE24B", "2CF05D4FF3D0", "001D098B6A4E", "001AA062C81A", "2CF05D5011FD", "2CF05D501246", "2CF05D50123B", "4CEB42C8D047", "CC3D826CC57D", "CC3D826CC249", "34CFF6148E35", "4CEB42AD3789", "34CFF60FF032" };
        String COMPU = "";
        int ok;
        int tiempo = 300;
        int tiempoeneri = 200;
        int tiempo2 = 2000;
        DateTime Quincena = new DateTime(2024, 05, 10, 0, 0, 0);

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

            if (MARCA == "nk151")
            {
                try
                {
                    btnAccion.IsEnabled = false;
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(tiempoeneri).Wait();
                    int i = 0;
                    int a = 0;
                    int intentos = 0;
                    byte[] respuestaSerial = null;
                    byte[] auxiliar = null;


                    if(Txt_MED.Text == "")
                    {
                        do
                        {
                            serialPort.ReadExisting();
                            serialPort.Write(E2preguntar, 0, E2preguntar.Length);
                            Task.Delay(tiempoeneri).Wait();
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

                        if (respuestaSerial == null )
                        {
                            MessageBox.Show("El medidor no contesta solicitar numero de medidor");
                            btnAccion.IsEnabled = true;
                            serialPort.Close();
                            MessageBox.Show("Ingrese Numero Medidor manualmente");
                            Txt_MED.Focus();
                        }
                        else
                        {
                            a = 0;
                            for (int k = 0; k < respuestaSerial.Length; k++)
                            {
                                if (respuestaSerial[k] == 0x03 && respuestaSerial[k + 1] == 0x10)
                                {
                                    a = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 6; j++)
                            {
                                Medidor[j] = respuestaSerial[a + 12 + j];
                            }
                            Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        }
                    }
                    if (Txt_MED.Text != "")
                    {
                        Byte[] direccionMed = Encoding.ASCII.GetBytes(Txt_MED.Text);
                        Array.Copy(direccionMed, 0, E2despertar, 10, 6);
                        byte[] buf = new Byte[E2despertar.Length - 2];
                        Array.Copy(E2despertar, 0, buf, 0, E2despertar.Length - 2);
                        uint send = calc_crc(buf, buf.Length);
                        Byte[] aCrc = BitConverter.GetBytes(send);
                        Array.Reverse(aCrc, 0, aCrc.Length);
                        Array.Copy(aCrc, 2, E2despertar, E2despertar.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2lectura, 10, 6);
                        byte[] buf2 = new Byte[E2lectura.Length - 2];
                        Array.Copy(E2lectura, 0, buf2, 0, E2lectura.Length - 2);
                        uint send2 = calc_crc(buf2, buf2.Length);
                        Byte[] aCrc2 = BitConverter.GetBytes(send2);
                        Array.Reverse(aCrc2, 0, aCrc2.Length);
                        Array.Copy(aCrc2, 2, E2lectura, E2lectura.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2reco, 10, 6);
                        byte[] buf4 = new Byte[E2reco.Length - 2];
                        Array.Copy(E2reco, 0, buf4, 0, E2reco.Length - 2);
                        uint send4 = calc_crc(buf4, buf4.Length);
                        Byte[] aCrc4 = BitConverter.GetBytes(send4);
                        Array.Reverse(aCrc4, 0, aCrc4.Length);
                        Array.Copy(aCrc4, 2, E2reco, E2reco.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2reset1, 10, 6);
                        byte[] buf3 = new Byte[E2reset1.Length - 2];
                        Array.Copy(E2reset1, 0, buf3, 0, E2reset1.Length - 2);
                        uint send3 = calc_crc(buf3, buf3.Length);
                        Byte[] aCrc3 = BitConverter.GetBytes(send3);
                        Array.Reverse(aCrc3, 0, aCrc3.Length);
                        Array.Copy(aCrc3, 2, E2reset1, E2reset1.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2reset2, 10, 6);
                        byte[] buf5 = new Byte[E2reset2.Length - 2];
                        Array.Copy(E2reset2, 0, buf5, 0, E2reset2.Length - 2);
                        uint send5 = calc_crc(buf5, buf5.Length);
                        Byte[] aCrc5 = BitConverter.GetBytes(send5);
                        Array.Reverse(aCrc5, 0, aCrc5.Length);
                        Array.Copy(aCrc5, 2, E2reset2, E2reset2.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2reset3, 10, 6);
                        byte[] buf6 = new Byte[E2reset3.Length - 2];
                        Array.Copy(E2reset3, 0, buf6, 0, E2reset3.Length - 2);
                        uint send6 = calc_crc(buf6, buf6.Length);
                        Byte[] aCrc6 = BitConverter.GetBytes(send6);
                        Array.Reverse(aCrc6, 0, aCrc6.Length);
                        Array.Copy(aCrc6, 2, E2reset3, E2reset3.Length - 2, 2);
                        Array.Copy(direccionMed, 0, E2reset4, 10, 6);
                        byte[] buf7 = new Byte[E2reset4.Length - 2];
                        Array.Copy(E2reset4, 0, buf7, 0, E2reset4.Length - 2);
                        uint send7 = calc_crc(buf7, buf7.Length);
                        Byte[] aCrc7 = BitConverter.GetBytes(send7);
                        Array.Reverse(aCrc7, 0, aCrc7.Length);
                        Array.Copy(aCrc7, 2, E2reset4, E2reset4.Length - 2, 2);
                        do
                        {
                            serialPort.ReadExisting();
                            serialPort.Write(E2despertar, 0, E2despertar.Length);
                            Task.Delay(tiempoeneri).Wait();
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
                            MessageBox.Show("El medidor no contesta el despertar");
                            btnAccion.IsEnabled = true;
                            serialPort.Close();
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
                                Task.Delay(tiempoeneri).Wait();
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
                                MessageBox.Show("El medidor no contesta primera lectura");
                                serialPort.Close();
                            }
                            else
                            {
                                Byte[] energy = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                                Array.Copy(respuestaSerial, 31, energy, 0, 4);
                                Array.Reverse(energy);
                                double Energia = (BitConverter.ToSingle(energy, 0)) / 1000.0D;
                                Txt_ACT.Text = Energia.ToString();
                                intentos = 0;
                                respuestaSerial = null;
                                auxiliar = null;
                                do
                                {
                                    serialPort.ReadExisting();
                                    serialPort.Write(E2reset1, 0, E2reset1.Length);
                                    Task.Delay(tiempoeneri).Wait();
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
                                    MessageBox.Show("El medidor no contesta reset");
                                    serialPort.Close();
                                }
                                else
                                {
                                    intentos = 0;
                                    respuestaSerial = null;
                                    auxiliar = null;
                                    do
                                    {
                                        serialPort.ReadExisting();
                                        serialPort.Write(E2reset2, 0, E2reset2.Length);
                                        Task.Delay(tiempoeneri).Wait();
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
                                        MessageBox.Show("El medidor no contesta reset acumulados");
                                        serialPort.Close();
                                    }
                                    else
                                    {
                                        intentos = 0;
                                        respuestaSerial = null;
                                        auxiliar = null;
                                        do
                                        {
                                            serialPort.ReadExisting();
                                            serialPort.Write(E2reset3, 0, E2reset3.Length);
                                            Task.Delay(tiempoeneri).Wait();
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
                                            MessageBox.Show("El medidor no contesta reset demanda maxima");
                                            serialPort.Close();
                                        }
                                        else
                                        {
                                            intentos = 0;
                                            respuestaSerial = null;
                                            auxiliar = null;
                                            do
                                            {
                                                serialPort.ReadExisting();
                                                serialPort.Write(E2reset4, 0, E2reset4.Length);
                                                Task.Delay(tiempoeneri).Wait();
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
                                                MessageBox.Show("El medidor no contesta reset demanda acumulada");
                                                serialPort.Close();
                                            }
                                            else
                                            {
                                                intentos = 0;
                                                respuestaSerial = null;
                                                auxiliar = null;
                                                do
                                                {
                                                    serialPort.ReadExisting();
                                                    serialPort.Write(E2reco, 0, E2reco.Length);
                                                    Task.Delay(tiempoeneri).Wait();
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
                                                    MessageBox.Show("Error:El relevador NO se puede cerrar es muy pronto");
                                                    Task.Delay(tiempo).Wait();
                                                    do
                                                    {
                                                        serialPort.ReadExisting();
                                                        serialPort.Write(E2reco, 0, E2reco.Length);
                                                        Task.Delay(tiempoeneri).Wait();
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
                                                        MessageBox.Show("Error El relevador esta abierto");
                                                        Rel = "Abierto";
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Se reconectó");
                                                        Rel = "Cerrado";
                                                    }

                                                    serialPort.Close();
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
                                                        serialPort.Write(E2lectura, 0, E2lectura.Length);
                                                        Task.Delay(tiempoeneri).Wait();
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
                                                        MessageBox.Show("El medidor no contesta segunda lectura");
                                                        serialPort.Close();
                                                    }
                                                    else
                                                    {
                                                        Byte[] energy2 = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];
                                                        Array.Copy(respuestaSerial, 31, energy2, 0, 4);
                                                        Array.Reverse(energy2);
                                                        double Energia2 = (BitConverter.ToSingle(energy2, 0)) / 1000.0D;
                                                        Txt_REAC.Text = Energia2.ToString();
                                                    }
                                                    serialPort.Close();
                                                    MessageBox.Show("Se reconectó");
                                                    Rel = "Cerrado";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Txt_REL.Text = Rel;
                        Archivo2(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text, Txt_REL.Text);
                        btnAccion.IsEnabled = true;
                        serialPort.Close();
                        MessageBox.Show("Listo");
                        Txt_MED.Text = "";
                        Txt_ACT.Text = "";
                        Txt_REAC.Text = "";
                        Txt_REL.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    btnAccion.IsEnabled = true;
                }
            }
            else
            {
                try
                {
                    btnAccion.IsEnabled = false;
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 4800, Parity.None, 8, StopBits.One);
                    serialPort.Close();
                    serialPort.Open();
                    int intentos = 0;
                  
                    int a = 0, b = 0, c = 0, d = 0;
                    Task.Delay(tiempo).Wait();
                    do
                    {
                        serialPort.ReadExisting();
                        serialPort.Write(T0, 0, T0.Length);
                        Task.Delay(tiempo).Wait();
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
                        btnAccion.IsEnabled = true;
                        serialPort.Close();
                    }
                    else
                    {
                        intentos = 0;
                        respuestaSerial = null;
                        auxiliar = null;
                        serialPort.Write(T1, 0, T1.Length);
                        Task.Delay(tiempo).Wait();
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
                        for (int k = 0; k < respuestaSerial.Length; k++)
                        {
                            if (respuestaSerial[k] == 0xb4 && respuestaSerial[k + 1] == 0x00)
                            {
                                a = k;
                                break;
                            }
                        }
                        for (int j = 0; j < 6; j++)
                        {
                            Medidor[j] = respuestaSerial[a + 26 + j];
                        }
                        if (MARCA == "rex")
                        {
                            serialPort.Write(T2, 0, T2.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial1 == null)
                                {
                                    respuestaSerial1 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                                }
                                else
                                {
                                    auxiliar1 = new byte[respuestaSerial1.Length];
                                    auxiliar1 = respuestaSerial1;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                    Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial1.Length; k++)
                            {
                                if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                                {
                                    b = k;
                                    break;
                                }
                            }
                            if (respuestaSerial1[b + 10] == 0x08)
                            {
                                bidi = false;
                            }
                            else
                            {
                                bidi = true;
                            }


                            if (bidi == false)
                            {
                                serialPort.Write(T3_1, 0, T3_1.Length);
                                Task.Delay(tiempo).Wait();
                                Task.Delay(tiempo).Wait();
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
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar1.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial2.Length; k++)
                                {
                                    if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                    {
                                        c = k;
                                        break;
                                    }
                                }
                                for (int j = 0; j < 3; j++)
                                {
                                    Act[j] = respuestaSerial2[c + 11 + j];

                                }
                            }
                            else if (bidi == true)
                            {
                                serialPort.Write(T3_2, 0, T3_2.Length);
                                Task.Delay(tiempo).Wait();
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
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial2.Length; k++)
                                {
                                    if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                    {
                                        c = k;
                                        break;
                                    }
                                }
                                for (int j = 0; j < 3; j++)
                                {
                                    Act[j] = respuestaSerial2[c + 17 + j];
                                }
                            }
                        }

                        else
                        {
                            serialPort.Write(T3_2, 0, T3_2.Length);
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
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
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                    Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial2.Length; k++)
                            {
                                if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                Act[j] = respuestaSerial2[c + 17 + j];
                            }
                        }

                        auxiliar1 = null;
                        respuestaSerial1 = null;
                        serialPort.Write(T9, 0, T9.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial1 == null)
                            {
                                respuestaSerial1 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                            }
                            else
                            {
                                auxiliar1 = new byte[respuestaSerial1.Length];
                                auxiliar1 = respuestaSerial1;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                            }
                        }
                        auxiliar1 = null;
                        respuestaSerial1 = null;
                        serialPort.Write(T8, 0, T8.Length);
                        Task.Delay(tiempo2).Wait();
                        do
                        {
                            Task.Delay(tiempo).Wait();
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial1 == null)
                                {
                                    respuestaSerial1 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                                }
                                else
                                {
                                    auxiliar1 = new byte[respuestaSerial1.Length];
                                    auxiliar1 = respuestaSerial1;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                    Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                                }
                            }

                        } while (respuestaSerial == null);
                        auxiliar1 = null;
                        respuestaSerial1 = null;
                        Task.Delay(tiempo).Wait();
                        if (MARCA == "rex")
                        {
                            serialPort.Write(T2, 0, T2.Length);
                            do
                            {
                                Task.Delay(tiempo).Wait();

                                Task.Delay(tiempo).Wait();
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial1 == null)
                                    {
                                        respuestaSerial1 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                                    }
                                    else
                                    {
                                        auxiliar1 = new byte[respuestaSerial1.Length];
                                        auxiliar1 = respuestaSerial1;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                        Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                                    }
                                }
                            } while (respuestaSerial == null);


                            for (int k = 0; k < respuestaSerial1.Length; k++)
                            {
                                if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                                {
                                    b = k;
                                    break;
                                }
                            }
                            if (respuestaSerial1[b + 10] == 0x08)
                            {
                                bidi = false;
                            }
                            else
                            {
                                bidi = true;
                            }
                            if (bidi == false)
                            {
                                serialPort.Write(T3_1, 0, T3_1.Length);
                                Task.Delay(tiempo).Wait();
                                Task.Delay(tiempo).Wait();
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
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial2.Length; k++)
                                {
                                    if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                    {
                                        c = k;
                                        break;
                                    }
                                }
                                for (int j = 0; j < 3; j++)
                                {
                                    Act[j] = respuestaSerial2[c + 11 + j];

                                }
                            }
                            else if (bidi == true)
                            {
                                serialPort.Write(T3_2, 0, T3_2.Length);
                                Task.Delay(tiempo).Wait();
                                Task.Delay(tiempo).Wait();
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
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial2 = new byte[auxiliar2.Length + bytes.Length];
                                        Array.Copy(auxiliar2, respuestaSerial2, auxiliar2.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial2, auxiliar2.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial2.Length; k++)
                                {
                                    if (respuestaSerial2[k] == 0xb4 && respuestaSerial2[k + 1] == 0x00)
                                    {
                                        c = k;
                                        break;
                                    }
                                }
                                for (int j = 0; j < 3; j++)
                                {
                                    Act[j] = respuestaSerial2[c + 17 + j];
                                }
                            }
                        }

                        else
                        {
                            serialPort.Write(T2, 0, T2.Length);

                            do
                            {
                                Task.Delay(tiempo).Wait();
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial1 == null)
                                    {
                                        respuestaSerial1 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                                    }
                                    else
                                    {
                                        auxiliar1 = new byte[respuestaSerial1.Length];
                                        auxiliar1 = respuestaSerial1;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                        Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                                    }
                                }
                                auxiliar1 = null;
                                respuestaSerial1 = null;

                                serialPort.Write(T3_2, 0, T3_2.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial1 == null)
                                    {
                                        respuestaSerial1 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial1, 0, respuestaSerial1.Length);
                                    }
                                    else
                                    {
                                        auxiliar1 = new byte[respuestaSerial1.Length];
                                        auxiliar1 = respuestaSerial1;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial1 = new byte[auxiliar1.Length + bytes.Length];
                                        Array.Copy(auxiliar1, respuestaSerial1, auxiliar1.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial1, auxiliar1.Length, bytes.Length);
                                    }
                                }

                            } while (respuestaSerial == null);
                            for (int k = 0; k < respuestaSerial1.Length; k++)
                            {
                                if (respuestaSerial1[k] == 0xb4 && respuestaSerial1[k + 1] == 0x00)
                                {
                                    c = k;
                                    break;
                                }
                            }
                            for (int j = 0; j < 3; j++)
                            {
                                React[j] = respuestaSerial1[c + 17 + j];
                            }
                        }
                        serialPort.Write(T4, 0, T4.Length);
                        Task.Delay(tiempo).Wait();
                        while (serialPort.BytesToRead > 0)
                        {
                            if (respuestaSerial3 == null)
                            {
                                respuestaSerial3 = new byte[serialPort.BytesToRead];
                                serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                            }
                            else
                            {
                                auxiliar3 = new byte[respuestaSerial3.Length];
                                auxiliar3 = respuestaSerial3;
                                byte[] bytes = new byte[serialPort.BytesToRead];
                                respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                serialPort.Read(bytes, 0, bytes.Length);
                                Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                            }
                        }
                        for (int k = 0; k < respuestaSerial3.Length; k++)
                        {
                            if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                            {
                                d = k;
                                break;
                            }
                        }
                        if (respuestaSerial3[d + 15] == 0x01)
                        {
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T5, 0, T5.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T7, 0, T7.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            respuestaSerial3 = null;
                            auxiliar3 = null;
                            serialPort.Write(T4, 0, T4.Length);
                            Task.Delay(tiempo).Wait();
                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial3 == null)
                                {
                                    respuestaSerial3 = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                }
                                else
                                {
                                    auxiliar3 = new byte[respuestaSerial3.Length];
                                    auxiliar3 = respuestaSerial3;
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                    Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                    serialPort.Read(bytes, 0, bytes.Length);
                                    Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                }
                            }
                            for (int k = 0; k < respuestaSerial3.Length; k++)
                            {
                                if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                {
                                    d = k;
                                    break;
                                }
                            }
                            if (respuestaSerial3[d + 15] == 0x01)
                            {
                                MessageBox.Show("Error:El relevador NO se puede cerrar es muy pronto");
                                Task.Delay(tiempo).Wait();
                                respuestaSerial3 = null;
                                Task.Delay(tiempo).Wait();
                                auxiliar3 = null;
                                Task.Delay(tiempo).Wait();
                                serialPort.Write(T7, 0, T7.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                respuestaSerial3 = null;
                                auxiliar3 = null;
                                serialPort.Write(T4, 0, T4.Length);
                                Task.Delay(tiempo).Wait();
                                while (serialPort.BytesToRead > 0)
                                {
                                    if (respuestaSerial3 == null)
                                    {
                                        respuestaSerial3 = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial3, 0, respuestaSerial3.Length);
                                    }
                                    else
                                    {
                                        auxiliar3 = new byte[respuestaSerial3.Length];
                                        auxiliar3 = respuestaSerial3;
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        respuestaSerial3 = new byte[auxiliar3.Length + bytes.Length];
                                        Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                        serialPort.Read(bytes, 0, bytes.Length);
                                        Array.Copy(bytes, 0, respuestaSerial3, auxiliar3.Length, bytes.Length);
                                    }
                                }
                                for (int k = 0; k < respuestaSerial3.Length; k++)
                                {
                                    if (respuestaSerial3[k] == 0xb4 && respuestaSerial3[k + 1] == 0x00)
                                    {
                                        d = k;
                                        break;
                                    }
                                }
                                if (respuestaSerial3[d + 15] == 0x01)
                                {
                                    MessageBox.Show("Error El relevador esta abierto");
                                    Rel = "Abierto";
                                }
                                else if (respuestaSerial3[d + 15] == 0x00)
                                {
                                    MessageBox.Show("Se reconectó");
                                    Rel = "Cerrado";
                                }
                            }
                            else if (respuestaSerial3[d + 15] == 0x00)
                            {
                                MessageBox.Show("Se reconectó");
                                Rel = "Cerrado";
                            }

                        }
                        else if (respuestaSerial3[d + 15] == 0x00)
                        {
                            MessageBox.Show("El relevador ya esta cerrado");
                            Rel = "Cerrado";
                        }
                        Txt_MED.Text = Encoding.ASCII.GetString(Medidor);
                        Txt_ACT.Text = BitConverter.ToString(Act).Replace("-", "");
                        Txt_REAC.Text = BitConverter.ToString(React).Replace("-", "");
                        Txt_REL.Text = Rel;
                        Archivo2(Txt_MED.Text, Txt_ACT.Text, Txt_REAC.Text, Txt_REL.Text);
                        btnAccion.IsEnabled = true;
                        serialPort.Close();
                        MessageBox.Show("Listo");
                        Txt_MED.Text = "";
                        Txt_ACT.Text = "";
                        Txt_REAC.Text = "";
                        Txt_REL.Text = "";

                    }
                    btnAccion.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    btnAccion.IsEnabled = true;
                }


            }

        }

        public void Archivo2(string med, string ACT, string REACT,string REL)
        {
            string fecha = DateTime.Now.ToString("F");
            string fecha1 = DateTime.Now.ToString("yyyyMMdd");
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = filepath + ".\\_LEC_" + COMPU;
            string path2 = path + ".\\_LEC_" + fecha1;
            string linea0 = med + ",ANTIGUA," + ACT + ",NUEVA, " + REACT + ",RELEVADOR," + REL + "," + fecha;
            bool result = File.Exists(path2);
            if (result == true)
            {
                TextWriter sw = new StreamWriter(path2, true);
                sw.WriteLine(linea0);
                sw.Close();
            }
            else
            {
                DirectoryInfo Folder = Directory.CreateDirectory(path);
                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(linea0);
                }
            }
        }

        private void RadioBtn_mrex_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "mrex";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
        }

        private void RadioBtn_Rex_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "rex";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
        }

        private void RadioBtn_nk151_Checked(object sender, RoutedEventArgs e)
        {
            MARCA = "nk151";
            Txt_ACT.Text = "";
            Txt_REAC.Text = "";
            Txt_REL.Text = "";
            Txt_MED.Text = "";
           
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
    }
}
