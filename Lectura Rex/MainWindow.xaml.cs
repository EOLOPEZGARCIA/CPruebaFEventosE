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

namespace Lectura_Rex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        byte[] T0 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xca, 0x00, 0x00, 0x00, 0x00, 0x05, 0xb0, 0x40 }; //SALUDO
        byte[] T1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x20, 0x62, 0x3a }; //NUMERO MEDIDOR

        byte[] T2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x01, 0xc8, 0xa6 }; //TIPO DE LECTURA

        byte[] t3_1 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x29, 0x82, 0x0b };// LECTURA

        byte[] t3_2 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x00, 0x54, 0x00, 0x00, 0x00, 0x00, 0x34, 0xe6, 0xc0 };// LECTURA

        byte[] t4 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x00, 0x00, 0x00, 0x1e, 0x84 }; //Estado

        byte[] t5 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x3f, 0x09, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x26, 0x23, 0xdb };// Validacion reco

        byte[] t6 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x00, 0x00, 0xf2, 0x88 };//corte

        byte[] t7 = new byte[] { 0xb4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0b, 0x40, 0x00, 0x07, 0x00, 0x06, 0x08, 0xc9, 0x00, 0x80, 0x80, 0x00, 0x3e, 0x04 };//reconexion
        
        string[] mac = { "34CFF60FF032", "482AE30D0517", "482AE30D0518", "482AE30D0519", "482AE30D0521", "34CFF61565DA", "482AE30D0511" };
        String[] COMPU = new string[10];
        int ok;
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

    }
}
