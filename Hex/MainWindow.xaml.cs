using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hex
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            timer.Elapsed += new ElapsedEventHandler(activaTimeOut);
            timer.Interval = 10000;

            foreach (string s in SerialPort.GetPortNames())
            {
                cboPuertos.Items.Add(s);
            }
        }

        const short pagina = 1024;
        const byte registro = 255;

        static System.Timers.Timer timer = new System.Timers.Timer();
        static bool timerTimeOut;

        byte[] respuestaSerial = null;
        byte[] auxiliar = null;
        int bytesLeidos = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cboPuertos.Text != "")
            {
                OpenFileDialog dg = new OpenFileDialog();
                dg.Title = "Select a Hex file";
                dg.DefaultExt = "*.hex";
                dg.Filter = "HEX Files|*.hex";

                dg.ShowDialog();

                string[] lines = File.ReadAllLines(dg.FileName);
                List<RegistroHex> registros = new List<RegistroHex>();

                foreach (String line in lines)
                {
                    RegistroHex registro = new RegistroHex();
                    registro.contador = new Byte[1];
                    registro.contador[0] = Byte.Parse(line.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    String hex = line.Substring(3, 4).Substring(0, 2) + line.Substring(3, 4).Substring(2, 2);
                    registro.direccion = Convert.ToInt32(hex, 16);
                    registro.tipoRegistro = new Byte[1];
                    registro.tipoRegistro[0] = Byte.Parse(line.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                    registro.data = new Byte[registro.contador[0]];
                    for (int i = 0; i / 2 < registro.contador[0]; i += 2)
                    {
                        registro.data[i / 2] = Byte.Parse(line.Substring(9 + i, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                    registro.checksum = new Byte[1];
                    registro.checksum[0] = Byte.Parse(line.Substring(9 + (registro.contador[0] * 2), 2), System.Globalization.NumberStyles.HexNumber);
                    registros.Add(registro);
                }

                List<Paquetes> paquetes = new List<Paquetes>();
                short contadorPaquete = 0;
                Paquetes paquete = null;
                int direccionMemoria = 0;
                bool afterOffset = false;
                int offset = 0;

//                foreach (RegistroHex registro in registros)
                for (int k = 0; k < registros.Count; k++)
                {
                    if (registros[k].tipoRegistro[0] == 0x02)
                    {
                        Array.Reverse(registros[k].data);
                        offset = BitConverter.ToInt16(registros[k].data, 0);
                        offset = (offset * 0x10);
                        float division = 1;
                        float bloqueFinOffset = (float)direccionMemoria / 256;

                        if (bloqueFinOffset % 1 != 0)
                            bloqueFinOffset = (int)bloqueFinOffset + 1;

                        while (division != 0)
                        {
                            division = bloqueFinOffset % 4;
                            if (division != 0)
                                bloqueFinOffset += 1;
                        }

                        while (direccionMemoria < ((bloqueFinOffset * 256) + 1))
                        {
                            if (contadorPaquete == 256)
                            {
                                paquetes.Add(paquete);
                                contadorPaquete = 0;
                            }

                            if (contadorPaquete == 0)
                            {
                                paquete = new Paquetes();
                                paquete.paquete = new Byte[256];
                                if (direccionMemoria == 0x1000)
                                    direccionMemoria += 0x1000;
                                paquete.direccion = (direccionMemoria < 0x1000) ? direccionMemoria + 0x1000 : direccionMemoria;
                                paquete.bloque = (direccionMemoria / 1024) < 5 ? (direccionMemoria / 1024) + 4 : direccionMemoria / 1024;
                            }
                            paquete.paquete[contadorPaquete] = 0xFF;
                            contadorPaquete += 1;
                            direccionMemoria += 1;
                        }
                        contadorPaquete -= 1;
                        direccionMemoria = offset;
                        afterOffset = true;
                    }
                    else
                    {
                        if (afterOffset)
                        {
                            direccionMemoria = direccionMemoria + registros[k].direccion;
                            afterOffset = false;
                        }

                        if ((registros[k].direccion + offset) != direccionMemoria)
                        {
                            for (int i = direccionMemoria; i < (registros[k].direccion + offset); i++)
                            {
                                if (contadorPaquete == 256)
                                {
                                    paquetes.Add(paquete);
                                    contadorPaquete = 0;
                                }

                                if (contadorPaquete == 0)
                                {
                                    paquete = new Paquetes();
                                    paquete.paquete = new Byte[256];
                                    if (direccionMemoria == 0x1000)
                                    {
                                        direccionMemoria += 0x1000;
                                        i += 0x1000;
                                    }
                                    paquete.direccion = (direccionMemoria < 0x1000) ? direccionMemoria + 0x1000 : direccionMemoria;
                                    paquete.bloque = (direccionMemoria / 1024) < 5 ? (direccionMemoria / 1024) + 4 : direccionMemoria / 1024;
                                }
                                paquete.paquete[contadorPaquete] = 0xFF;
                                contadorPaquete += 1;
                                direccionMemoria += 1;
                            }

                            foreach (Byte dataArray in registros[k].data)
                            {
                                if (contadorPaquete == 256)
                                {
                                    paquetes.Add(paquete);
                                    contadorPaquete = 0;
                                }

                                if (contadorPaquete == 0)
                                {
                                    paquete = new Paquetes();
                                    paquete.paquete = new Byte[256];
                                    if (direccionMemoria == 0x1000)
                                        direccionMemoria += 0x1000;
                                    paquete.direccion = (direccionMemoria < 0x1000) ? direccionMemoria + 0x1000 : direccionMemoria;
                                    paquete.bloque = (direccionMemoria / 1024) < 5 ? (direccionMemoria / 1024) + 4 : direccionMemoria / 1024;
                                }

                                paquete.paquete[contadorPaquete] = dataArray;
                                contadorPaquete += 1;
                                direccionMemoria += 1;
                            }
                        }
                        else
                        {
                            foreach (Byte dataArray in registros[k].data)
                            {
                                if (contadorPaquete == 256)
                                {
                                    paquetes.Add(paquete);
                                    contadorPaquete = 0;
                                }

                                if (contadorPaquete == 0)
                                {
                                    paquete = new Paquetes();
                                    paquete.paquete = new Byte[256];
                                    if (direccionMemoria == 0x1000)
                                        direccionMemoria += 0x1000;
                                    paquete.direccion = (direccionMemoria < 0x1000) ? direccionMemoria + 0x1000 : direccionMemoria;
                                    paquete.bloque = (direccionMemoria / 1024) < 5 ? (direccionMemoria / 1024) + 4 : direccionMemoria / 1024;
                                 }

                                paquete.paquete[contadorPaquete] = dataArray;
                                contadorPaquete += 1;
                                direccionMemoria += 1;
                            }
                        }
                    }
                }

                SerialPort serial = new SerialPort();
                serial.PortName = cboPuertos.Text;
                serial.Parity = Parity.None;
                serial.StopBits = StopBits.One;
                serial.BaudRate = 9600;
                serial.ReadTimeout = 1800000;
                serial.WriteTimeout = 1800000;
                serial.Close();
                serial.Open();

                int cantidadPaquetes = 4;
                byte[] startCode = { 0x01 };
                short dataLength = 0x0008;
                byte[] command = new byte[1];
                byte[] data = null;
                long chk;
                byte[] checksum = new byte[1];
                try
                {
                    foreach (Paquetes paq in paquetes)
                    {
                        byte[] add = null;
                        byte[] si = null;
                        byte[] block = BitConverter.GetBytes(paq.bloque);
                        short size = 0x0400;

                        if (cantidadPaquetes == 4)
                        {
                            command[0] = 0x05;

                            dataLength = 0x0008;
                            data = BitConverter.GetBytes(dataLength);
                            add = BitConverter.GetBytes(paq.direccion);
                            si = BitConverter.GetBytes(size);
                            Array.Reverse(data);
                            Array.Reverse(add);
                            Array.Reverse(si);

                            chk = startCode[0];
                            foreach (byte d in data)
                            {
                                chk = chk + d;
                            }

                            chk = chk + command[0];
                            chk = chk + block[0];

                            foreach (byte a in add)
                            {
                                chk = chk + a;
                            }

                            foreach (byte s in si)
                            {
                                chk = chk + s;
                            }

                            checksum[0] = BitConverter.GetBytes(chk)[0];

                            serial.Write(startCode, 0, 1);
                            serial.Write(data, 0, data.Length);
                            serial.Write(command, 0, 1);
                            serial.Write(block, 0, 1);
                            serial.Write(add, 1, 3);
                            serial.Write(si, 0, si.Length);
                            serial.Write(checksum, 0, checksum.Length);

                            respuestaSerial = null;
                            auxiliar = null;
                            bytesLeidos = 0;
                            timer.Enabled = true;
                            timerTimeOut = false;

                            while (!timerTimeOut && bytesLeidos != 6)
                            {
                                while (serial.BytesToRead > 0)
                                {
                                    int bytesToRead = serial.BytesToRead;
                                    bytesLeidos = bytesLeidos + bytesToRead;

                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[bytesToRead];
                                        serial.Read(respuestaSerial, 0, bytesToRead);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        respuestaSerial = new byte[auxiliar.Length + bytesToRead];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        byte[] bytes = new byte[bytesToRead];
                                        serial.Read(bytes, 0, bytesToRead);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytesToRead);
                                    }
                                }
                            }

                            if (respuestaSerial != null)
                            {
                                if (respuestaSerial[4] != 0x0)
                                {
                                    MessageBox.Show("Respuesta Erronea");
                                }
                            }
                            else
                                MessageBox.Show("No se envió DATA_REV");

                            timer.Enabled = false;
                            Thread.Sleep(500);
                        }


                        dataLength = 0x0102;
                        data = BitConverter.GetBytes(dataLength);
                        Array.Reverse(data);
                        command[0] = 0x06;
                        chk = startCode[0];
                        foreach (byte d in data)
                        {
                            chk = chk + d;
                        }

                        chk = chk + command[0];

                        foreach (byte b in paq.paquete)
                        {
                            chk = chk + b;
                        }

                        checksum[0] = BitConverter.GetBytes(chk)[0];

                        serial.Write(startCode, 0, 1);
                        serial.Write(data, 0, data.Length);
                        serial.Write(command, 0, 1);
                        serial.Write(paq.paquete, 0, paq.paquete.Length);
                        serial.Write(checksum, 0, checksum.Length);

                        respuestaSerial = null;
                        auxiliar = null;
                        bytesLeidos = 0;
                        timer.Enabled = true;
                        timerTimeOut = false;

                        while (!timerTimeOut && bytesLeidos != 6)
                        {
                            while (serial.BytesToRead > 0)
                            {
                                int bytesToRead = serial.BytesToRead;
                                bytesLeidos = bytesLeidos + bytesToRead;

                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[bytesToRead];
                                    serial.Read(respuestaSerial, 0, bytesToRead);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + bytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[bytesToRead];
                                    serial.Read(bytes, 0, bytesToRead);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytesToRead);
                                }
                            }
                        }

                        if (respuestaSerial != null)
                        {
                            if (respuestaSerial[4] != 0x0)
                            {
                                MessageBox.Show("Respuesta Erronea");
                            }
                        }
                        else
                            MessageBox.Show("No se envió DATA_REV");

                        timer.Enabled = false;

                        cantidadPaquetes -= 1;
                        Thread.Sleep(500);

                        if (cantidadPaquetes == 0)
                        {
                            dataLength = 0x0003;
                            data = BitConverter.GetBytes(dataLength);
                            Array.Reverse(data);
                            command[0] = 0x0B;
                            chk = startCode[0];
                            foreach (byte d in data)
                            {
                                chk = chk + d;
                            }

                            chk = chk + command[0];
                            chk = chk + block[0];
                            checksum[0] = BitConverter.GetBytes(chk)[0];

                            serial.Write(startCode, 0, 1);
                            serial.Write(data, 0, data.Length);
                            serial.Write(command, 0, 1);
                            serial.Write(block, 0, 1);
                            serial.Write(checksum, 0, checksum.Length);

                            respuestaSerial = null;
                            auxiliar = null;
                            bytesLeidos = 0;
                            timer.Enabled = true;
                            timerTimeOut = false;

                            while (!timerTimeOut && bytesLeidos != 6)
                            {
                                while (serial.BytesToRead > 0)
                                {
                                    int bytesToRead = serial.BytesToRead;
                                    bytesLeidos = bytesLeidos + bytesToRead;

                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[bytesToRead];
                                        serial.Read(respuestaSerial, 0, bytesToRead);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        respuestaSerial = new byte[auxiliar.Length + bytesToRead];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        byte[] bytes = new byte[bytesToRead];
                                        serial.Read(bytes, 0, bytesToRead);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytesToRead);
                                    }
                                }
                            }

                            if (respuestaSerial != null)
                            {
                                if (respuestaSerial[4] != 0x0)
                                {
                                    MessageBox.Show("Respuesta Erronea");
                                }
                            }
                            else
                                MessageBox.Show("No se envió DATA_REV");

                            timer.Enabled = false;
                            cantidadPaquetes = 4;
                        }

                        Thread.Sleep(500);

                    }

                    dataLength = 0x0002;
                    data = BitConverter.GetBytes(dataLength);
                    Array.Reverse(data);
                    command[0] = 0x08;

                    chk = startCode[0];
                    foreach (byte d in data)
                    {
                        chk = chk + d;
                    }

                    chk = chk + command[0];
                    checksum[0] = BitConverter.GetBytes(chk)[0];

                    serial.Write(startCode, 0, 1);
                    serial.Write(data, 0, data.Length);
                    serial.Write(command, 0, 1);
                    serial.Write(checksum, 0, checksum.Length);

                    respuestaSerial = null;
                    auxiliar = null;
                    bytesLeidos = 0;
                    timer.Enabled = true;
                    timerTimeOut = false;

                    while (!timerTimeOut && bytesLeidos != 6)
                    {
                        while (serial.BytesToRead > 0)
                        {
                            int bytesToRead = serial.BytesToRead;
                            bytesLeidos = bytesLeidos + bytesToRead;

                            if (respuestaSerial == null)
                            {
                                respuestaSerial = new byte[bytesToRead];
                                serial.Read(respuestaSerial, 0, bytesToRead);
                            }
                            else
                            {
                                auxiliar = new byte[respuestaSerial.Length];
                                auxiliar = respuestaSerial;
                                respuestaSerial = new byte[auxiliar.Length + bytesToRead];
                                Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                byte[] bytes = new byte[bytesToRead];
                                serial.Read(bytes, 0, bytesToRead);
                                Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytesToRead);
                            }
                        }
                    }

                    if (respuestaSerial != null)
                    {
                        if (respuestaSerial[4] != 0x0)
                        {
                            MessageBox.Show("Respuesta Erronea");
                        }
                    }
                    else
                        MessageBox.Show("No se envió DATA_REV");

                    serial.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                }
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un puerto COM");
            }
        }

        private static void activaTimeOut(object source, ElapsedEventArgs e)
        {
            timerTimeOut = true;
            timer.Enabled = false;
        }
    }
}
