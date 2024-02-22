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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
namespace ValidarNumMed
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string impresion = "";
        DateTime Quincena = new DateTime(2024, 02, 19, 0, 0, 0);
        public MainWindow()
        {
            int fecha2 = DateTime.Compare(DateTime.Now, Quincena);
            if (fecha2 < 1)
            {
                InitializeComponent();
            }
            else
            {
                MessageBox.Show("Error: Contacte a Jefe de Laboratorio por caducidad");
                this.Close();
            }
        }

        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void Btn_Prueba_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var fd = new FolderBrowserDialog())
                {
                    if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fd.SelectedPath))
                    {
                        Txt_Prueba.Text = fd.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            Task.Factory.StartNew((Action)(() => { }));
        }

        public class Tabla
        {
            public int numeroTabla { get; set; }

            public uint cantidadDatos { get; set; }

            public byte[] datos { get; set; }
        }

        public int posicion(byte[] archivo, byte[] comparar, int comienzo)
        {
            for (int index1 = comienzo; index1 < archivo.Length; ++index1)
            {
                int index2 = index1;
                if ((int)archivo[index1] == (int)comparar[0])
                {
                    byte[] numArray = comparar;
                    for (int index3 = 0; index3 < numArray.Length && (int)numArray[index3] == (int)archivo[index2]; ++index3)
                        ++index2;
                    if (index2 - index1 == comparar.Length)
                        return index1 + comparar.Length - 1;
                }
            }
            return -1;
        }

        private void Btn_COL_Click(object sender, RoutedEventArgs e)
        {
            Txt_MED.Text = "";
            Txt_Prueba.Text = "";
            Btn_COL.Visibility = Visibility.Hidden;
        }
        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Btn_Leer.IsEnabled = false;
                string ruta = string.Empty;
                this.Dispatcher.Invoke((Action)(() => ruta = this.Txt_Prueba.Text));
                string fechanow = DateTime.Now.ToString("yyyyMMdd");
                using (StreamWriter streamWriter = new StreamWriter(ruta + "\\Resultado_" + fechanow + ".csv"))
                    
                {
                    streamWriter.WriteLine("Numedo Medidor" + "," + "ID");
                    string[] filePaths = Directory.GetFiles(ruta, "*.msr");
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.pgCantidad.Value = 0.0;
                        this.pgCantidad.Maximum = (double)filePaths.Length;
                    }));
                    foreach (string str in filePaths)
                    {
                        FileInfo fileInfo = new FileInfo(str);
                        try
                        {
                            byte[] archivo = File.ReadAllBytes(str);
                            List<Tabla> tablaList = new List<Tabla>();
                            Txt_MED.Text = tablaList.Count.ToString();
                            string file = string.Empty;
                            file = fileInfo.Name;
                            string empty = string.Empty;
                            string cuenta = string.Empty;
                            string programa = string.Empty;
                            string verano = string.Empty;
                            string programador = string.Empty;
                            byte[] bytes = new byte[6];
                            int startIndex1 = 0;// archivo[258] < (byte)112 ? 300 : 316;
                            byte[] byteArray1 = this.StringToByteArray("45454D");
                            int a = 0;
                            byte[] med = new byte[20];
                            byte[] count = new byte[20];
                            byte[] program = new byte[8];
                            byte[] programer = new byte[8];
                            int bandera = 0;


                            if (archivo[296] == 0x00)
                            {
                                    startIndex1 = 300;
                            }
                            else 
                            {
                                startIndex1 = 316;
                            }



                            // while (startIndex1 < num6)
                            while (bandera == 0)
                            {
                                Tabla tabla = new Tabla();
                                tabla.numeroTabla = (int)BitConverter.ToUInt16(archivo, startIndex1);
                                int startIndex2 = startIndex1 + 8;
                                tabla.cantidadDatos = BitConverter.ToUInt32(archivo, startIndex2);
                                startIndex1 = startIndex2 + 10;
                                tablaList.Add(tabla);
                                if (archivo[startIndex1] == 0x00 && archivo[startIndex1 + 1] == 0x20 && archivo[startIndex1 + 18] == 0x12 && archivo[startIndex1 + 19] == 0x0A)
                                {
                                    bandera = 1;
                                }
                                else if (archivo[startIndex1] == 0x40 && archivo[startIndex1 + 1] == 0x00 && archivo[startIndex1 + 18] == 0x12 && archivo[startIndex1 + 19] == 0x0A)
                                {
                                    bandera = 2;
                                }

                            }
                            int num7 = startIndex1 + 18;
                            int sal = 0;
                            foreach (Tabla tabla in tablaList)
                            {
                                tabla.datos = new byte[(int)tabla.cantidadDatos];
                                Array.Copy((Array)archivo, (long)num7, (Array)tabla.datos, 0L, (long)tabla.cantidadDatos);
                                num7 += (int)tabla.cantidadDatos;
                                if (tabla.numeroTabla == 6)
                                {
                                    for (int j = 0; j < 20; j++)
                                    {
                                        med[j] = tabla.datos[100 + j + a];
                                        count[j] = tabla.datos[120 + j + a];
                                    }
                                    for (int j = 0; j < 8; j++)
                                    {
                                        program[j] = tabla.datos[170 + j + a];
                                    }
                                    for (int j = 0; j < 8; j++)
                                    {
                                        programer[j] = tabla.datos[190 + j + a];
                                    }

                                    empty = Encoding.ASCII.GetString(med);
                                    cuenta = Encoding.ASCII.GetString(count);
                                    programador = Encoding.ASCII.GetString(programer);
                                    sal = 5;
                                }
                                if (sal == 5)
                                    break;
                            }
                            if(empty == cuenta)
                            {

                            }
                            else
                            {
                                //   impresion = impresion +"El medidor "+empty + " y la cuenta "+cuenta+ " son diferentes " + "y el programador fue "+programador + "|"+ "\n";
                                impresion = "Los medidores que no concuerdan, estan en el archivo csv en la misma carpeta";
                                streamWriter.WriteLine(empty + ","+ cuenta);
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                            streamWriter.WriteLine("Error: " + str);
                            Btn_Leer.IsEnabled = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_Leer.IsEnabled = true;
            }
            Btn_COL.Content = impresion;
            Btn_COL.Visibility = Visibility.Visible;
            Btn_Leer.IsEnabled = true;
        }

    }
}
