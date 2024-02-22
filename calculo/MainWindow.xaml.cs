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

namespace calculo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

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

            // string resultado = "Linea 3";
           

            /* Byte[] energy2 = new Byte[] { 0x45, 0x19, 0x09, 0x87 };//[4];

             double Energia2 = (BitConverter.ToSingle(energy2, 0)) / 1000.0D;

             Array.Reverse(energy2);
             double Energia = (BitConverter.ToSingle(energy2, 0)) / 1000.0D;

             resultado = Energia.ToString();
             resultado2 = Energia2.ToString();
             Txt_Sum0.Text = resultado;
            
            
            string fecha = DateTime.Now.ToString("F");
            string fecha1 = DateTime.Now.ToString("yyyyMMdd");

            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = filepath + ".\\_LEC_" ;
            string path2 = path + ".\\_LEC_" + fecha1;

            bool result = File.Exists(path2);
            if (result == true)
            {
                 if ((Folder.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                                  {
                                      //Add Hidden flag    
                                      Folder.Attributes |= FileAttributes.Hidden;
                                  }
                                  
                TextWriter sw = new StreamWriter(path2, true);
                sw.WriteLine(resultado);
                sw.Close();

            }
            else 
            {
                DirectoryInfo Folder = Directory.CreateDirectory(path);

                //   Folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(resultado);
                    //   File.SetAttributes(path, FileAttributes.Hidden);
                }
            }
            */
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

        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Btn_Leer.IsEnabled = false;
                string ruta = string.Empty;
                this.Dispatcher.Invoke((Action)(() => ruta = this.Txt_Prueba.Text));
                using (StreamWriter streamWriter = new StreamWriter(ruta + "\\Resultado.txt"))
                {
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
                            DateTime fechaUltimaEscrituraPO = new DateTime();
                            ushort num1 = 0;
                            ushort num2 = 0;
                            DateTime fechaUltimaEscrituraPI = new DateTime();
                            string FECHAPI = string.Empty;
                            ushort num3 = 0;
                            ushort num4 = 0;
                            DateTime fechaUltimaProgramacion = new DateTime();
                            string flag = "";
                            int numeroTotalReset = 0;
                            int diasUltimoReset = 0;
                            int diasUltimoPulso = 0;
                            DateTime fechaUltimoReset = new DateTime();
                            DateTime fechaUltimoApagon = new DateTime();
                            DateTime fechaUltimoEncendido = new DateTime();
                            int contadorApagones = 0;
                            byte[] byteArray1 = this.StringToByteArray("45454D");
                            int a = 0;
                            int num6 =0;
                            byte[] med = new byte[20];
                            byte[] count = new byte[20];
                            byte[] program = new byte[8];
                            byte[] programer = new byte[8];

                            /* do
                             {
                                 num5 = this.posicion(archivo, byteArray1, comienzo);
                                 if ((archivo[num5 - 3] == 0x20) &&( archivo[num5 - 4] == 0x20))
                                 {
                                     Array.Copy((Array)archivo, num5 - 66, (Array)bytes, 0, bytes.Length);
                                     empty = Encoding.ASCII.GetString(bytes);
                                 }
                                 comienzo = num5;
                                 if ((archivo[num5 - 3] != 0x20) && (archivo[num5 - 4] != 0x20))
                                 {
                                     bandera = 1;
                                 }
                             }
                             while (bandera ==0);
                             */
                           byte[] byteArray2 = this.StringToByteArray("200000FFFFFFFF");
                           byte[] byteArray21 = this.StringToByteArray("400000000000000046");
                           byte[] byteArray22 = this.StringToByteArray("400000000000000006");
                           byte[] byteArray23 = this.StringToByteArray("4000000000000000C0");
                           byte[] byteArray24 = this.StringToByteArray("08000000000000");
                            int aux2 = 0;

                            for (int k = 0; k < archivo.Length; k++)
                            {
                                if (k == 410)
                                { 

                                }
                                    if (archivo[k] == byteArray2[0] && archivo[k + 1] == byteArray2[1] && archivo[k + 2] == byteArray2[2] && archivo[k + 3] == byteArray2[3] && archivo[k + 4] == byteArray2[4] && archivo[k + 5] == byteArray2[5] && archivo[k + 6] == byteArray2[6] && archivo[k + 17] == 0x12 && archivo[k + 18] == 0x0A && archivo[k + 19] == 0x9A && archivo[k + 20] == 0x45 && archivo[k + 21] == 0x45)
                                {
                                   // num6 = this.posicion(archivo, byteArray2, 0);
                                    num6 = k + 6;
                                    break;
                                }
                                if (archivo[k] == byteArray21[0] && archivo[k + 1] == byteArray21[1] && archivo[k + 2] == byteArray21[2] && archivo[k + 3] == byteArray21[3] && archivo[k + 4] == byteArray21[4] && archivo[k + 5] == byteArray21[5] && archivo[k + 6] == byteArray21[6] && archivo[k + 7] == byteArray21[7] && archivo[k + 8] == byteArray21[8])
                                {
                                   // num6 =-1+ this.posicion(archivo, byteArray21, 0);
                                    num6 = k +7;
                                    break;
                                }
                                if (archivo[k] == byteArray22[0] && archivo[k + 1] == byteArray22[1] && archivo[k + 2] == byteArray22[2] && archivo[k + 3] == byteArray22[3] && archivo[k + 4] == byteArray22[4] && archivo[k + 5] == byteArray22[5] && archivo[k + 6] == byteArray22[6] && archivo[k + 7] == byteArray22[7] && archivo[k + 8] == byteArray22[8] && archivo[k + 18] == 0x12 && archivo[k + 19] == 0x0A && archivo[k + 20] == 0x9A && archivo[k + 21] == 0x45 && archivo[k + 22] == 0x45)
                                {
                                   // num6 = -1 + this.posicion(archivo, byteArray22, 0);
                                    num6 = k + 7;
                                    break;
                                }
                                if (archivo[k] == byteArray23[0] && archivo[k + 1] == byteArray23[1] && archivo[k + 2] == byteArray23[2] && archivo[k + 3] == byteArray23[3] && archivo[k + 4] == byteArray23[4] && archivo[k + 5] == byteArray23[5] && archivo[k + 6] == byteArray23[6] && archivo[k + 7] == byteArray23[7] && archivo[k + 8] == byteArray23[8])
                                {
                                   // num6 = -1 + this.posicion(archivo, byteArray23, 0);
                                    num6 = k + 7;
                                    break;
                                }
                                if (archivo[k] == byteArray24[0] && archivo[k + 1] == byteArray24[1] && archivo[k + 2] == byteArray24[2] && archivo[k + 3] == byteArray24[3] && archivo[k + 4] == byteArray24[4] && archivo[k + 5] == byteArray24[5] && archivo[k + 6] == byteArray24[6] && archivo[k + 18] == 0x0A && archivo[k + 19] == 0x9A && archivo[k + 20] == 0x45 && archivo[k + 21] == 0x45)
                                {
                                   // num6 = this.posicion(archivo, byteArray24, 0);
                                    num6 = k + 6;
                                    break;
                                }

                                if (archivo[k] == byteArray2[0] && archivo[k + 1] == byteArray2[1] && archivo[k + 2] == byteArray2[2] && archivo[k + 3] == byteArray2[3] && archivo[k + 4] == byteArray2[4] && archivo[k + 5] == byteArray2[5] && archivo[k + 6] == byteArray2[6] && archivo[k + 19] == 0x20 && archivo[k + 20] == 0x00 && archivo[k + 21] == 0x00 && archivo[k + 22] == 0xFF && archivo[k + 23] == 0xFF && archivo[k + 24] == 0xFF && archivo[k + 25] == 0xFF)
                                {
                                    aux2 = 18;
                                    //num6 = this.posicion(archivo, byteArray2, 0);
                                    num6 = k + 6;
                                    break;
                                }

                            }


                            byte[] byteArrayF1 = this.StringToByteArray("00004F00000000000000");
                            byte[] byteArrayF2 = this.StringToByteArray("00008000000000000000");
                            int num9 = 0;
                            int num10 = 0;
                            int num11 = 0;
                            int VAL = 3;
                            int aux = 0;
                            if (this.posicion(archivo, byteArrayF1, 0) != -1)
                            {
                                num9 = this.posicion(archivo, byteArrayF1, 0);
                            }
                            if (this.posicion(archivo, byteArrayF2, 0) != -1)
                            {
                                num10 = this.posicion(archivo, byteArrayF2, 0);
                            }

                            if (num9 != 0 && (num9 < num10) && (num9 < num6))
                            {
                                num11 = num9;
                                aux = 79;
                            }
                            if (num10 != 0 && (num10 < num9) && (num10 < num6))
                            {
                                num11 = num10;
                                aux = 128;
                            }
                            if (num9 != 0 && num10 == 0 && (num9 < num6))
                            {
                                num11 = num9;
                                aux = 79;
                            }
                            if (num10 != 0 && num9 == 0 && (num10 < num6))
                            {
                                num11 = num10;
                                aux = 128;
                            }


                           
                            startIndex1 = num11 + VAL;


                            while (startIndex1 < num6)
                            {
                                Tabla tabla = new Tabla();
                                tabla.numeroTabla = (int)BitConverter.ToUInt16(archivo, startIndex1);
                                int startIndex2 = startIndex1 + 8;
                                tabla.cantidadDatos = BitConverter.ToUInt32(archivo, startIndex2);
                                startIndex1 = startIndex2 + 10;
                                tablaList.Add(tabla);
                            }

                            int num7 = num6 + 11 + aux + aux2;
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
                                        med[j] = tabla.datos[100 + j+a];
                                        count[j] = tabla.datos[120 + j+a];
                                    }
                                    for (int j = 0; j < 8; j++)
                                    {
                                        program[j] = tabla.datos[170 + j+a];
                                    }
                                    for (int j = 0; j < 8; j++)
                                    {
                                        programer[j] = tabla.datos[190 + j+a];
                                    }

                                    empty = Encoding.ASCII.GetString(med);
                                    cuenta = Encoding.ASCII.GetString(count);
                                    programa = Encoding.ASCII.GetString(program);
                                    programador = Encoding.ASCII.GetString(programer);
                                    sal = 1;
                                }
                                else if (tabla.numeroTabla == 23)
                                {
                                    numeroTotalReset = (int)tabla.datos[0+a];
                                    sal = 2;

                                }
                                else if (tabla.numeroTabla == 25)
                                {
                                    fechaUltimoReset = tabla.datos[0+a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[0+a], (int)tabla.datos[1+a], (int)tabla.datos[2+a], (int)tabla.datos[3+a], (int)tabla.datos[4+a], 0) : new DateTime(2000, 1, 1);
                                    sal = 3;
                                }

                                
                                else if (tabla.numeroTabla == 54)
                                {
                                    byte[] byteArrayINI = this.StringToByteArray("240801");
                                    byte[] byteArrayFIN = this.StringToByteArray("2AC802");
                                    int k = 0;
                                    bool inicio = false;
                                    bool fin = false;

                                    do
                                    {
                                        if (tabla.datos[k] == 0x24 && tabla.datos[k + 1] == 0x08 && tabla.datos[k + 2] == 0x01)
                                        {
                                            inicio = true;
                                        }
                                        else if (tabla.datos[k] == 0x24 && tabla.datos[k + 1] == 0x08 && tabla.datos[k + 2] == 0x41)
                                        {
                                            inicio = true;
                                        }

                                        if (tabla.datos[k] == 0x2A && tabla.datos[k + 1] == 0xC8 && tabla.datos[k + 2] == 0x02)
                                        {
                                            fin = true;
                                        }
                                        else if (tabla.datos[k] == 0x2A && tabla.datos[k + 1] == 0xC8 && tabla.datos[k + 2] == 0x42)
                                        {
                                            fin = true;
                                        }
                                        k++;
                                    }
                                    while (k < tabla.cantidadDatos);
                                   
                                    if (inicio == true && fin == true)
                                    {
                                        verano = "si";
                                    }
                                    else
                                    {
                                        verano = "no";
                                    }
                                }
                                /*
                                else if (tabla.numeroTabla == 76)
                                 {
                                     int num8 = 0;
                                     int num9 = 0;
                                     int startIndex2 = num8 + 11;
                                     string grupo = DateTime.Now.Ticks.ToString();
                                     while (startIndex2 < (int)tabla.datos[1])
                                     {
                                         try
                                         {
                                             Evento evento = new Evento();
                                             evento.fechaEvento = new DateTime(2000 + (int)tabla.datos[startIndex2], (int)tabla.datos[startIndex2 + 1], (int)tabla.datos[startIndex2 + 2], (int)tabla.datos[startIndex2 + 3], (int)tabla.datos[startIndex2 + 4], (int)tabla.datos[startIndex2 + 5]);
                                             startIndex2 += 6;
                                             evento.numeroEvento = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                             startIndex2 += 2;
                                             evento.usuario = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                             startIndex2 += 2;
                                             evento.idEvento = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                             startIndex2 += 2;
                                             ++num9;
                                         }
                                         catch (Exception ex)
                                         {
                                             Console.WriteLine("");
                                         }
                                     }
                                 }*/
                                else if (tabla.numeroTabla == 2051)
                                {
                                    
                                    fechaUltimaEscrituraPO = tabla.datos[45+a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[45+a], (int)tabla.datos[46+a], (int)tabla.datos[47+a], (int)tabla.datos[48+a], (int)tabla.datos[49+a], (int)tabla.datos[50+a]) : new DateTime(2000, 1, 1);

                                    num1 = BitConverter.ToUInt16(tabla.datos, 53 + a);
                                    num2 = (ushort)tabla.datos[55 + a];
                                    if (tabla.datos[56 + a] == 0X00 && tabla.datos[57 + a] == 0X00 && tabla.datos[58 + a] == 0X00)
                                    {
                                       FECHAPI = "NONE";
                                    }
                                    else
                                    {
                                        fechaUltimaEscrituraPI = tabla.datos[56 + a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[56 + a], (int)tabla.datos[57 + a], (int)tabla.datos[58 + a], (int)tabla.datos[59 + a], (int)tabla.datos[60 + a], (int)tabla.datos[61 + a]) : new DateTime(2000, 1, 1);
                                        FECHAPI = fechaUltimaEscrituraPI.ToString();
                                    }
                                    num3 = BitConverter.ToUInt16(tabla.datos, 64 + a);
                                    num4 = (ushort)tabla.datos[66 + a];
                                    diasUltimoReset = (int)tabla.datos[70 + a];
                                    diasUltimoPulso = (int)tabla.datos[71 + a];
                                    fechaUltimoApagon = tabla.datos[72 + a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[72 + a], (int)tabla.datos[73 + a], (int)tabla.datos[74 + a], (int)tabla.datos[75 + a], (int)tabla.datos[76 + a], (int)tabla.datos[77 + a]) : new DateTime(2000, 1, 1);
                                    fechaUltimoEncendido = tabla.datos[78 + a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[78 + a], (int)tabla.datos[79 + a], (int)tabla.datos[80 + a], (int)tabla.datos[81 + a], (int)tabla.datos[82 + a], (int)tabla.datos[83 + a]) : new DateTime(2000, 1, 1);
                                    contadorApagones = (int)tabla.datos[84 + a];
                                    sal = 4;
                                }
                                else if (tabla.numeroTabla == 2056)
                                {
                                    fechaUltimaProgramacion = tabla.datos[387+a] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[387+a], (int)tabla.datos[388+a], (int)tabla.datos[389+a], (int)tabla.datos[390+a], (int)tabla.datos[391+a], (int)tabla.datos[392+a]) : new DateTime(2000, 1, 1);
                                    if (tabla.datos[379+a] != 0x00)
                                    {
                                        flag = "S";
                                    }
                                    else
                                    {
                                        flag = "N";
                                    }
                                    sal = 5;
                                }
                                if (sal == 5)
                                    break;
                            }

                            streamWriter.WriteLine("-Archivo: " + file + "-Medidor: " + empty + "-Cuenta: " + cuenta + "-Programa: " + programa + "-Programador: " + programador + "-fechaUltimaEscrituraPO: " + fechaUltimaEscrituraPO + "-sesionesEscrituraPO: " + num1 + "-passwordInvalidoPO: " + num2 + "-fechaUltimaEscrituraPI: " + FECHAPI + "-sesionesEscrituraPI " + num3 + "-passwordInvalidoPI: " + num4 + "-fechaUltimaProgramacion: " + fechaUltimaProgramacion + "-medidorEditado: " + flag + "-numeroTotalReset: " + numeroTotalReset + "-diasUltimoReset: " + diasUltimoReset + "-diasUltimoPulso: " + diasUltimoPulso + "-fechaUltimoReset: " + fechaUltimoReset + "-fechaUltimoApagon: " + fechaUltimoApagon + "-fechaUltimoEncendido:" + fechaUltimoEncendido + "-contadorApagones:" + contadorApagones + "-horarioVerano:" + verano );
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
            Btn_COL.Visibility = Visibility.Visible;
            Btn_Leer.IsEnabled = true;
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

       
    }
}