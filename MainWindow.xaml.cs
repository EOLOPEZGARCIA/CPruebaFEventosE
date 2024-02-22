using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

namespace PruebaEventos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string RUTA_DESCARGA_INSTALADOR = ":\\ENSAMBLE\\CFE.ENSAMBLE.Instalador\\descarga";
        private string RUTA_INSTALADOR = ":\\ENSAMBLE\\CFE.ENSAMBLE.Instalador\\";
        private string RUTA_BACKUP = ":\\ENSAMBLE\\CFE.ENSAMBLE.Instalador\\backup";
        private string RUTA_VERSION = ":\\ENSAMBLE\\CFE.ENSAMBLE.Instalador\\version";
        private string ARCHIVO_ZIP = ":\\ENSAMBLE\\CFE.ENSAMBLE.Instalador\\descarga\\update.zip";
        private string RUTA_CONTROL_CALIDAD = ":\\ENSAMBLE\\CFE.ENSAMBLE.ControlCalidad";
        private string RUTA_VERSION_CONTROL_CALIDAD = ":\\ENSAMBLE\\CFE.ENSAMBLE.ControlCalidad\\version";

        [DllImport("LibreriaElster.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GeneraLLave")]
        public static extern void GeneraLlave(int[] CadenaE);

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            byte[] fkey = { 0x10, 0x5c, 0x5d, 0xf2, 0x7e, 0x59, 0x41, 0x4b };
            int[] cadenaE = new int[64];

            BitArray bits = new BitArray(fkey.Reverse().ToArray());


            for (int x = 0; x < bits.Count; x++)
            {
                cadenaE[x] = (bits.Get(x) == true) ? 1 : 0;
            }

            Array.Reverse(cadenaE);

            GeneraLlave(cadenaE);

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

            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        public string retorno()
        {
            try
            {
                Console.WriteLine("1");
                return "AB";
            }
            catch (Exception ex)
            {
                return "B";
            }
            finally
            {
                Console.WriteLine("2");
            }
            
            Console.WriteLine("3");
            return "A";
            
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}