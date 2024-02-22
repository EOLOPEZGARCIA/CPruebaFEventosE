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
using System.ComponentModel;
using System.Collections;

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
        public class Evento
        {
            public DateTime fechaEvento { get; set; }
            public int numeroEvento { get; set; }
            public int usuario { get; set; }
            public int idEvento { get; set; }
        }
        
        public class Tabla
        {
            public int numeroTabla { get; set; }
            public uint cantidadDatos { get; set; }
            public byte[] datos { get; set; }
        }
        private string[] Potencias(double va, double vb, double vc, double aa, double ab, double ac, double ava, double avb, double avc, double aaa, double aab, double aac)
        {
            double KVAA = (va * aa) / 1000;
            double KVAB = (vb * ab) / 1000;
            double KVAC = (vc * ac) / 1000;
            double AA = Math.Abs(ava - aaa);
            double AB = Math.Abs(ava - aaa);
            double AC = Math.Abs(ava - aaa);
            string KWA = (Math.Cos(AA) * KVAA).ToString();
            string KWB = (Math.Cos(AB) * KVAB).ToString();
            string KWC = (Math.Cos(AC) * KVAC).ToString();
            string KVARA = (Math.Sin(AA) * KVAA).ToString();
            string KVARB = (Math.Sin(AB) * KVAB).ToString();
            string KVARC = (Math.Sin(AC) * KVAC).ToString();
            string[] resultado = { KVAA.ToString(), KVAB.ToString(), KVAC.ToString(), KWA, KWB, KWC, KVARA, KVARB, KVARC };
            return resultado;
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

        public double cambiohex(byte[] val)
        {
           int aux= Int32.Parse(BitConverter.ToString(val, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber);
            return aux;
             
        }

        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Btn_Leer.IsEnabled = false;
                string ruta = string.Empty;
                this.Dispatcher.Invoke((Action)(() => ruta = this.Txt_Prueba.Text));
                string fechanow = DateTime.Now.ToString("yyyyMMdd");
                using (StreamWriter streamWriter = new StreamWriter(ruta + "\\Resultado_"+fechanow+".csv"))
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
                            int startIndex1 = 0;
                            byte[] serie_fact = new byte[16];
                            byte[] serie = new byte[20];
                            byte[] med = new byte[20];
                            byte[] count = new byte[20];
                            byte[] program = new byte[8];
                            byte[] programer = new byte[8];
                            int numeroTotalReset = 0;
                            DateTime fechaUltimoReset = new DateTime();
                            DateTime fechacambioest = new DateTime();
                            string[] instantaneos = new string[22];
                            string[] potencia_dis = new string[9];
                            string[] inst_val = {"voltajeA", "voltajeB", "voltajeC","corrienteA", "corrienteB", "corrienteC","anguloCorrienteA","anguloCorrienteB","anguloCorrienteC","anguloVoltajeA","anguloVoltajeB", "anguloVoltajeC","frecuencia","kVAFaseA","kVAFaseB","kVAFaseC","kWFaseA","kWFaseB","kWFaseC","kVARFaseA","kVARFaseB", "kVARFaseC" };
                            bool  flag3 = false, flag6 = false;
                            string result = "";
                            string verano = string.Empty;
                            string file = string.Empty;
                            file = fileInfo.Name;
                            DateTime fechaUltimaEscrituraPO = new DateTime();
                            DateTime timemedidor = new DateTime();
                            ushort num1 = 0;
                            ushort num2 = 0;
                            DateTime fechaUltimaEscrituraPI = new DateTime();
                            string FECHAPI = string.Empty;
                            string FECHAPO = string.Empty;
                            ushort num3 = 0;
                            ushort num4 = 0;
                            DateTime fechaUltimaProgramacion = new DateTime();
                            string flag = "";
                            int diasUltimoReset = 0;
                            int diasUltimoPulso = 0;
                            DateTime fechaUltimoApagon = new DateTime();
                            DateTime fechaUltimoEncendido = new DateTime();
                            double contadorApagones = 0;
                            double contadorApagonesMom = 0;
                            string tiemposostenido = string.Empty;
                            double contadorApagonesSostenidos = 0;
                            string[] log_name = new string[1];
                            string[] log = new string[1];
                            string[] nom_med = new string[1];
                            string[] mediciones = new string[1];
                            string[] mediciones_p_b = new string[1];
                            string[] mediciones_p_s = new string[1];
                            string[] variables = new string[30];
                            string[] val_var = new string[30];
                            double num_bloques = 0;
                            double num_intxbloq = 0;
                            double Long_Memo = 0;
                            int num_chanel = 0;
                            int tiemp_inter = 0;
                            string[] perfil_name = new string[1];
                            string [] e_perfil = new string[2];
                            string[] bloq_perfil = new string[1];
                            string perfil = "";
                            double cantpaq = 0;
                            int grupos = 0;
                            int rates = 1;
                            int bandera = 0;

                            if (archivo[296] == 0x00)
                            {
                                startIndex1 = 300;
                            }
                            else
                            {
                                startIndex1 = 316;
                            }
                            while (bandera ==0)
                            {
                                Tabla tabla = new Tabla();
                                tabla.numeroTabla = (int)BitConverter.ToUInt16(archivo, startIndex1);
                                int startIndex2 = startIndex1 + 8;
                                tabla.cantidadDatos = BitConverter.ToUInt32(archivo, startIndex2);
                                startIndex1 = startIndex2 + 10;
                                tablaList.Add(tabla);
                                if (archivo[startIndex1] == 0x00 && archivo[startIndex1 +1] == 0x20 && archivo[startIndex1 + 18] == 0x12 && archivo[startIndex1 + 19] == 0x0A)
                                {
                                    bandera = 1;
                                }
                                else if (archivo[startIndex1] == 0x40 && archivo[startIndex1 + 1] == 0x00 && archivo[startIndex1 + 18] == 0x12 && archivo[startIndex1 + 19] == 0x0A)
                                {
                                    bandera = 2;
                                }
                                else if (archivo[startIndex1] == 0x02 && archivo[startIndex1 + 1] == 0x20 && archivo[startIndex1 + 18] == 0x12 && archivo[startIndex1 + 19] == 0x0A)
                                {
                                    bandera = 3;
                                }

                            }
                            int num7 = startIndex1 + 18;
                            foreach (Tabla tabla in tablaList)
                            {
                                tabla.datos = new byte[(int)tabla.cantidadDatos];
                                Array.Copy((Array)archivo, (long)num7, (Array)tabla.datos, 0L, (long)tabla.cantidadDatos);
                                num7 += (int)tabla.cantidadDatos;

                                if (tabla.numeroTabla == 1)
                                {
                                    for (int j = 0; j < 16; j++)
                                    {
                                        serie_fact[j] = tabla.datos[16 + j];
                                    }

                                }
                                else if (tabla.numeroTabla == 5)
                                {
                                    for (int j = 0; j < 20; j++)
                                    {
                                        serie[j] = tabla.datos[j];
                                    }
                                }
                                else if (tabla.numeroTabla == 6)
                                {

                                    for (int j = 0; j < 20; j++)
                                    {
                                        med[j] = tabla.datos[100 + j];
                                        count[j] = tabla.datos[120 + j];
                                    }
                                    for (int j = 0; j < 8; j++)
                                    {
                                        program[j] = tabla.datos[170 + j];
                                        programer[j] = tabla.datos[190 + j];
                                    }

                                }
                                else if (tabla.numeroTabla == 21)
                                {
                                    rates = (int)tabla.datos[tabla.cantidadDatos - 3] + rates;
                                    grupos = (int)tabla.datos[tabla.cantidadDatos - 2] + grupos;
                                }
                                else if (tabla.numeroTabla == 22)
                                {
                                    string aux = "";
                                    byte auxb = 0xFF;
                                    int CantDatos = grupos * 4 * rates + rates;
                                    Array.Resize(ref nom_med, CantDatos);
                                    int j = 0;
                                    int m = 0;
                                    for (j = 0; j < (grupos + 1); j++)
                                    {
                                        auxb = tabla.datos[j];
                                        aux = Convert.ToString(auxb);
                                        switch (aux)
                                        {
                                            case "0":
                                                nom_med[j] = "KWh-Del";
                                                break;
                                            case "1":
                                                nom_med[j] = "KVARh-Del";
                                                break;
                                            case "2":
                                                nom_med[j] = "KWh-Rec";
                                                break;
                                            case "3":
                                                nom_med[j] = "KVARh-Rec";
                                                break;
                                            case "4":
                                                nom_med[j] = "KVARh-Q1";
                                                break;
                                            case "5":
                                                nom_med[j] = "KVARh-Q2";
                                                break;
                                            case "6":
                                                nom_med[j] = "KVARh-Q3";
                                                break;
                                            case "7":
                                                nom_med[j] = "KVARh-Q4";
                                                break;
                                            case "31":
                                                nom_med[j] = "PF KWh-Del y KVARh-Del";
                                                break;
                                            case "32":
                                                nom_med[j] = "PF KWh-Del y KVARh-Q1";
                                                break;
                                        }
                                    }
                                    int d = 0;
                                    for (m = 0; m < grupos; m++)
                                    {
                                        nom_med[j + m + d] = nom_med[m] + " Demanda Maxima Fecha";
                                        d++;
                                        nom_med[j + m + d] = nom_med[m] + " Demanda Acumulada";
                                        d++;
                                        nom_med[j + m + d] = nom_med[m] + " Maxima Demanda";

                                    }
                                    for (int k = 0; k < (rates - 1); k++)
                                    {
                                        for (int l = 0; l < (j + m + (grupos * 2)); l++)
                                        {
                                            nom_med[(((j + m + d) * (k + 1)) + l)] = nom_med[l] + " Rate " + (k + 1).ToString();
                                        }
                                    }
                                }
                                else if (tabla.numeroTabla == 23)
                                {
                                    numeroTotalReset = (int)tabla.datos[0];
                                    byte[] DemMax = new byte[5];
                                    DateTime FDemMax = new DateTime();
                                    byte[] energia = new byte[6];
                                    double valor = 0;
                                    int k = 1;
                                    int i = 0;
                                    int m = 0;
                                    Array.Resize(ref mediciones, nom_med.Length);
                                    for (int x = 1; x <= (rates); x++)
                                    {
                                        for (i = 0; i <= grupos; i++)
                                        {
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones[m] = valor.ToString();
                                            m++;
                                            k = k + 6;
                                        }

                                        for (i = (grupos + 1); i <= (grupos * 4); i++)
                                        {
                                            FDemMax = tabla.datos[k] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[k], (int)tabla.datos[k + 1], (int)tabla.datos[k + 2], (int)tabla.datos[k + 3], (int)tabla.datos[k + 4], 0) : new DateTime(2000, 1, 1);
                                            mediciones[m] = FDemMax.ToString();
                                            m++;
                                            k = k + 5;

                                            i++;
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones[m] = valor.ToString();
                                            i++;
                                            m++;
                                            k = k + 6;
                                            j = 0;
                                            for (j = 0; j < 5; j++)
                                            {
                                                DemMax[j] = tabla.datos[k + j];
                                            }

                                            Array.Reverse(DemMax);
                                            valor = (Int64.Parse(BitConverter.ToString(DemMax, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones[m] = valor.ToString();
                                            m++;
                                            k = k + j;
                                        }

                                    }
                                }
                                else if (tabla.numeroTabla == 24)
                                {
                                    fechacambioest = tabla.datos[0] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[0], (int)tabla.datos[1], (int)tabla.datos[2], (int)tabla.datos[3], (int)tabla.datos[4], 0) : new DateTime(2000, 1, 1);
                                    byte[] DemMax = new byte[5];
                                    DateTime FDemMax = new DateTime();
                                    byte[] energia = new byte[6];
                                    double valor = 0;
                                    int k = 7;
                                    int i = 0;
                                    int m = 0;
                                    Array.Resize(ref mediciones_p_s, nom_med.Length);
                                    for (int x = 1; x <= (rates); x++)
                                    {
                                        for (i = 0; i <= grupos; i++)
                                        {
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_s[m] = valor.ToString();
                                            m++;
                                            k = k + 6;
                                        }

                                        for (i = (grupos + 1); i <= (grupos * 4); i++)
                                        {
                                            FDemMax = tabla.datos[k] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[k], (int)tabla.datos[k + 1], (int)tabla.datos[k + 2], (int)tabla.datos[k + 3], (int)tabla.datos[k + 4], 0) : new DateTime(2000, 1, 1);
                                            mediciones_p_s[m] = FDemMax.ToString();
                                            m++;
                                            k = k + 5;

                                            i++;
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_s[m] = valor.ToString();
                                            i++;
                                            m++;
                                            k = k + 6;
                                            j = 0;
                                            for (j = 0; j < 5; j++)
                                            {
                                                DemMax[j] = tabla.datos[k + j];
                                            }

                                            Array.Reverse(DemMax);
                                            valor = (Int64.Parse(BitConverter.ToString(DemMax, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_s[m] = valor.ToString();
                                            m++;
                                            k = k + j;
                                        }

                                    }


                                }
                                else if (tabla.numeroTabla == 25)
                                {
                                    fechaUltimoReset = tabla.datos[0] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[0], (int)tabla.datos[1], (int)tabla.datos[2], (int)tabla.datos[3], (int)tabla.datos[4], 0) : new DateTime(2000, 1, 1);
                                    byte[] DemMax = new byte[5];
                                    DateTime FDemMax = new DateTime();
                                    byte[] energia = new byte[6];
                                    double valor = 0;
                                    int k = 7;
                                    int i = 0;
                                    int m = 0;
                                    Array.Resize(ref mediciones_p_b, nom_med.Length);
                                    for (int x = 1; x <= (rates); x++)
                                    {
                                        for (i = 0; i <= grupos; i++)
                                        {
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_b[m] = valor.ToString();
                                            m++;
                                            k = k + 6;
                                        }

                                        for (i = (grupos + 1); i <= (grupos * 4); i++)
                                        {
                                            FDemMax = tabla.datos[k] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[k], (int)tabla.datos[k + 1], (int)tabla.datos[k + 2], (int)tabla.datos[k + 3], (int)tabla.datos[k + 4], 0) : new DateTime(2000, 1, 1);
                                            mediciones_p_b[m] = FDemMax.ToString();
                                            m++;
                                            k = k + 5;

                                            i++;
                                            int j = 0;
                                            for (j = 0; j < 6; j++)
                                            {
                                                energia[j] = tabla.datos[k + j];
                                            }
                                            Array.Reverse(energia);
                                            valor = (Int64.Parse(BitConverter.ToString(energia, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_b[m] = valor.ToString();
                                            i++;
                                            m++;
                                            k = k + 6;
                                            j = 0;
                                            for (j = 0; j < 5; j++)
                                            {
                                                DemMax[j] = tabla.datos[k + j];
                                            }

                                            Array.Reverse(DemMax);
                                            valor = (Int64.Parse(BitConverter.ToString(DemMax, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                            mediciones_p_b[m] = valor.ToString();
                                            m++;
                                            k = k + j;
                                        }

                                    }


                                }
                                else if (tabla.numeroTabla == 28)
                                {
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
                                    Byte[] Frecuencia = new Byte[6];
                                    Byte[] AnguloFactorPotenciaA = new Byte[6];
                                    Byte[] AnguloFactorPotenciaC = new Byte[6];
                                    Byte[] AnguloFactorPotenciaB = new Byte[6];
                                    Byte[] KWA = new Byte[6];
                                    Byte[] KWB = new Byte[6];
                                    Byte[] KWC = new Byte[6];
                                    Byte[] KVAA = new Byte[6];
                                    Byte[] KVAB = new Byte[6];
                                    Byte[] KVAC = new Byte[6];
                                    Byte[] KVARA = new Byte[6];
                                    Byte[] KVARB = new Byte[6];
                                    Byte[] KVARC = new Byte[6];


                                    int d = 0;
                                    if (tabla.datos.Length == 0xf0)
                                    {
                                        d = 0;
                                    }
                                    else if (tabla.datos.Length == 0xd0)
                                    {
                                        d = 32;
                                    }

                                    for (int j = 0; j < 6; j++)
                                    {
                                        Frecuencia[j] = tabla.datos[53 - j - d];
                                        VoltajeA[j] = tabla.datos[59 - j - d];
                                        CorrienteA[j] = tabla.datos[65 - j - d];
                                        AnguloFactorPotenciaA[j] = tabla.datos[71 - j - d];
                                        VoltajeC[j] = tabla.datos[77 - j - d];
                                        CorrienteC[j] = tabla.datos[83 - j - d];
                                        AnguloFactorPotenciaC[j] = tabla.datos[89 - j - d];
                                        AnguloVoltajeC[j] = tabla.datos[95 - j - d];
                                        VoltajeB[j] = tabla.datos[101 - j - d];
                                        CorrienteB[j] = tabla.datos[107 - j - d];
                                        AnguloFactorPotenciaB[j] = tabla.datos[113 - j - d];
                                        AnguloVoltajeB[j] = tabla.datos[119 - j - d];
                                        KWA[j] = tabla.datos[125 - j - d];
                                        KWB[j] = tabla.datos[131 - j - d];
                                        KWC[j] = tabla.datos[137 - j - d];
                                        KVAA[j] = tabla.datos[143 - j - d];
                                        KVAB[j] = tabla.datos[149 - j - d];
                                        KVAC[j] = tabla.datos[155 - j - d];
                                        AnguloCorrienteA[j] = tabla.datos[167 - j - d];
                                        AnguloCorrienteB[j] = tabla.datos[173 - j - d];
                                        AnguloCorrienteC[j] = tabla.datos[179 - j - d];
                                        KVARA[j] = tabla.datos[185 - j - d];
                                        KVARB[j] = tabla.datos[191 - j - d];
                                        KVARC[j] = tabla.datos[197 - j - d];

                                    }

                                    // int dev = 1000;
                                    AnguloVoltajeA = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                    double[] valores = new double[13];
                                    valores[0] = cambiohex(VoltajeA);
                                    valores[1] = cambiohex(VoltajeB);
                                    valores[2] = cambiohex(VoltajeC);
                                    valores[3] = cambiohex(CorrienteA);
                                    valores[4] = cambiohex(CorrienteB);
                                    valores[5] = cambiohex(CorrienteC);
                                    valores[6] = cambiohex(AnguloCorrienteA);
                                    valores[7] = cambiohex(AnguloCorrienteB);
                                    valores[8] = cambiohex(AnguloCorrienteC);
                                    valores[9] = cambiohex(AnguloVoltajeA);
                                    valores[10] = cambiohex(AnguloVoltajeB);
                                    valores[11] = cambiohex(AnguloVoltajeC);
                                    valores[12] = cambiohex(Frecuencia);

                                    potencia_dis = Potencias(valores[0], valores[1], valores[2], valores[3], valores[4], valores[5], valores[6], valores[7], valores[8], valores[9], valores[10], valores[11]);
                                    for (int i = 0; i < valores.Length; i++)
                                    {
                                        instantaneos[i] = valores[i].ToString();
                                    }

                                    Array.Copy(potencia_dis, 0, instantaneos, 13, potencia_dis.Length);

                                }
                                else if (tabla.numeroTabla == 54)
                                {
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
                                else if (tabla.numeroTabla == 55)
                                {
                                    timemedidor = new DateTime(2000 + (int)tabla.datos[0], (int)tabla.datos[1], (int)tabla.datos[2], (int)tabla.datos[3], (int)tabla.datos[4], (int)tabla.datos[5]);

                                }
                                else if (tabla.numeroTabla == 61)
                                {
                                    byte[] memlong = new byte[4];
                                    memlong[0] = tabla.datos[0];
                                    memlong[1] = tabla.datos[1];
                                    memlong[2] = tabla.datos[2];
                                    memlong[3] = tabla.datos[3];
                                    Array.Reverse(memlong);
                                    Long_Memo = (Int64.Parse(BitConverter.ToString(memlong, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));

                                    byte[] numbloq = new byte[2];
                                    numbloq[0] = tabla.datos[7];
                                    numbloq[1] = tabla.datos[8];
                                    Array.Reverse(numbloq);
                                    num_bloques = (Int64.Parse(BitConverter.ToString(numbloq, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    byte[] intbloq = new byte[2];
                                    intbloq[0] = tabla.datos[9];
                                    intbloq[1] = tabla.datos[10];
                                    Array.Reverse(intbloq);
                                    num_intxbloq = (Int64.Parse(BitConverter.ToString(intbloq, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    num_chanel = (int)tabla.datos[11];
                                    tiemp_inter = (int)tabla.datos[12];
                                }
                                else if (tabla.numeroTabla == 62)
                                {
                                    string aux = "";
                                    byte auxb = 0xFF;
                                    int s = 1;
                                    Array.Resize(ref perfil_name, num_chanel);
                                    for (int k = 0; k < (num_chanel); k++)
                                    {
                                        auxb = tabla.datos[s];
                                        aux = Convert.ToString(auxb);
                                        switch (aux)
                                        {
                                            case "0":
                                                perfil_name[k] = "KWh-Del";
                                                break;
                                            case "1":
                                                perfil_name[k] = "KVARh-Del";
                                                break;
                                            case "2":
                                                perfil_name[k] = "KWh-Rec";
                                                break;
                                            case "3":
                                                perfil_name[k] = "KVARh-Rec";
                                                break;
                                            case "4":
                                                perfil_name[k] = "KVARh-Q1";
                                                break;
                                            case "5":
                                                perfil_name[k] = "KVARh-Q2";
                                                break;
                                            case "6":
                                                perfil_name[k] = "KVARh-Q3";
                                                break;
                                            case "7":
                                                perfil_name[k] = "KVARh-Q4";
                                                break;

                                        }
                                        s = s + 3;
                                    }
                                }
                                else if (tabla.numeroTabla == 63)
                                {

                                    int k = 4;
                                    byte[] cantpq = new byte[4];
                                    for (int j = 0; j < 4; j++)
                                    {
                                        cantpq[j] = tabla.datos[k - j];
                                    }
                                    cantpaq = (Int64.Parse(BitConverter.ToString(cantpq, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));

                                    //Array.Resize(ref e_perfil, (int)cantpaq * (int)num_intxbloq);
                                }
                                else if (tabla.numeroTabla == 64)
                                {
                                    perfil = "fecha";
                                    for (int j = 0; j < perfil_name.Length; j++)
                                    {
                                        perfil = perfil + ';'+perfil_name[j];
                                    }
                                    perfil=perfil + ";";
                                    e_perfil[0] = perfil;
                                    DateTime fecha_Fin = new DateTime();
                                    double cant = Math.Floor((num_intxbloq + 7) / 8);
                                    byte[] Paquete = new byte[Convert.ToInt32(cant)];
                                    byte[] ene_per_b = new byte[2]; 
                                    byte[] vali = new byte[1];
                                    int ene_per = 0;
                                    int m = 1;
                                    for (int j = 0; j < tabla.cantidadDatos; )
                                    {
                                        fecha_Fin = tabla.datos[j] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[j], (int)tabla.datos[j + 1], (int)tabla.datos[j + 2], (int)tabla.datos[j + 3], (int)tabla.datos[j + 4], 0) : new DateTime(2000, 1, 1);
                                        j += 5;
                                        for (int l = 0; l < 6; l++)
                                        {
                                            Paquete[l] = tabla.datos[j];
                                            j++;
                                        }
                                        BitArray bits = new BitArray(Paquete);
                                        bits = new BitArray(Paquete);
                                        DateTime[] fechas = new DateTime[1];
                                        int min = fecha_Fin.Minute;
                                        int paq = 0;
                                        int cou = 0;
                                        int per = 0;
                                        bool band=false;
                                        for (int q = bits.Count-1; q > -1; q--)
                                        {
                                            if (bits[q]==true)
                                            {
                                                band = true;
                                            }

                                            if (bits[q] == true && band==true)
                                            {
                                                Array.Resize(ref fechas, paq+1);
                                                fechas[paq] = fecha_Fin;

                                                paq++;
                                                per += 1;
                                            }
                                            if (cou == 1 && band == true)
                                            {
                                                    fecha_Fin = fecha_Fin.AddMinutes(-5);

                                            }
                                            if (cou == 0 && band == true)
                                            {
                                                cou += 1;

                                                fecha_Fin = fecha_Fin.AddMinutes(-1);
                                                min =fecha_Fin.Minute;
                                                do
                                                {

                                                    if (min % 5 == 0)
                                                    {
                                                        fecha_Fin= new DateTime(fecha_Fin.Year, fecha_Fin.Month, fecha_Fin.Day, fecha_Fin.Hour, min, 0);
                                                        break;
                                                    }
                                                    min -= 1;
                                                } while (min > 0);
                                            }
                                           
                                        }
                                        Array.Resize(ref bloq_perfil, paq);
                                        Array.Resize(ref vali, paq);
                                        paq = 0;
                                        cou = 0;
                                        band = false;
                                        for (int i = bits.Count-1; i > -1; i--)
                                        {
                                            if (bits[i] == true)
                                            {
                                                band = true;
                                            }
                                            if (bits[i] == true && band == true)
                                            {
                                                perfil = Convert.ToString(tabla.datos[j]);
                                                j += 2;
                                                for (int z = 1; z < num_chanel + 1; z++)
                                                {
                                                    ene_per_b[0] = tabla.datos[j];
                                                    j++;
                                                    ene_per_b[1] = tabla.datos[j];
                                                    j++;
                                                    Array.Reverse(ene_per_b);
                                                    ene_per = Convert.ToInt32(Int64.Parse(BitConverter.ToString(ene_per_b, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                                    perfil = perfil + ';' + ene_per.ToString();
                                                }
                                                perfil = perfil + ";";
                                                bloq_perfil[paq] = perfil;
                                                paq++;
                                            }

                                            /*
                                                if (tabla.datos[j] == 0x10)
                                                {
                                                    perfil = Convert.ToString(fechas[per]);
                                                }
                                                else
                                                {

                                                    perfil = Convert.ToString(fechas[per - 1]);
                                                    per -= 1;
                                                }
                                               
                                            */
                                            
                                            if (bits[i] == false && band == true)
                                            {
                                                j += 2 + (num_chanel * 2);
                                            }
                                        }
                                        j += (48 - paq) * (2 + (num_chanel * 2));
                                        Array.Reverse(bloq_perfil);
                                        bool inco = false;
                                        int inde = 0;
                                        int zz = 0;
                                        for(int i = 0; i < paq; i++)
                                        {
                                            if (bloq_perfil[0].StartsWith("146") && fechas[0].Minute % 5 == 0)
                                            {
                                                inco= true;
                                            }
                                            inde= bloq_perfil[i].IndexOf(";");
                                            if (inco== true && i==1)
                                            {
                                                zz--;
                                                e_perfil[m] = Convert.ToString(fechas[zz]) + ";" + bloq_perfil[i].Substring(inde+1);
                                            }
                                            else
                                            {

                                                e_perfil[m] = Convert.ToString(fechas[zz]) + ";" + bloq_perfil[i].Substring(inde+1);
                                            }
                                           // e_perfil[m] = bloq_perfil[paq-i-1];
                                            m++;
                                            zz++;
                                            Array.Resize(ref e_perfil, m+1);

                                        }
                                    }
                                    using (StreamWriter streamWriter22 = new StreamWriter(ruta + med.ToString() + fechanow + ".csv")) 
                                    {
                                        foreach (string element in e_perfil)
                                        {
                                            streamWriter22.WriteLine(element);
                                        }
                                    }
                                    
                                }
                                else if (tabla.numeroTabla == 76)
                                {
                                    int startIndex2 = 11;
                                    string grupo = DateTime.Now.Ticks.ToString();
                                    flag3 = true;
                                    Array.Resize(ref log, tabla.datos[9]);
                                    Array.Resize(ref log_name, tabla.datos[9]);
                                    Evento evento = new Evento();
                                    for (int j = 0; j < tabla.datos[9]; j++)
                                    {
                                        try
                                        {
                                            evento.fechaEvento = new DateTime(2000 + (int)tabla.datos[startIndex2], (int)tabla.datos[startIndex2 + 1], (int)tabla.datos[startIndex2 + 2], (int)tabla.datos[startIndex2 + 3], (int)tabla.datos[startIndex2 + 4], (int)tabla.datos[startIndex2 + 5]);
                                            startIndex2 += 6;
                                            evento.numeroEvento = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                            startIndex2 += 2;
                                            evento.usuario = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                            startIndex2 += 2;
                                            evento.idEvento = (int)BitConverter.ToUInt16(tabla.datos, startIndex2);
                                            startIndex2 += 2;

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("error" + ex.Message);
                                        }

                                        log[j] = evento.numeroEvento.ToString() + ";" + evento.fechaEvento.ToString() + ";" + evento.idEvento.ToString() + ";" + evento.usuario.ToString() + ";";
                                        log_name[j] = "no evento: " + ";" + "fecha: " + ";" + "id evento: " + ";" + "user: " + ";";
                                    }
                                }
                                else if (tabla.numeroTabla == 2051)
                                {

                                    fechaUltimaEscrituraPO = tabla.datos[45] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[45], (int)tabla.datos[46], (int)tabla.datos[47], (int)tabla.datos[48], (int)tabla.datos[49], (int)tabla.datos[50]) : new DateTime(2000, 1, 1);
                                    num1 = BitConverter.ToUInt16(tabla.datos, 53);
                                    num2 = (ushort)tabla.datos[55];
                                    if (tabla.datos[56] == 0X00 && tabla.datos[57] == 0X00 && tabla.datos[58] == 0X00)
                                    {
                                        FECHAPI = "NONE";
                                    }
                                    else
                                    {
                                        fechaUltimaEscrituraPI = tabla.datos[56] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[56], (int)tabla.datos[57], (int)tabla.datos[58], (int)tabla.datos[59], (int)tabla.datos[60], (int)tabla.datos[61]) : new DateTime(2000, 1, 1);
                                        FECHAPI = fechaUltimaEscrituraPI.ToString();
                                    }
                                    FECHAPO = fechaUltimaEscrituraPO.ToString();
                                    num3 = BitConverter.ToUInt16(tabla.datos, 64);
                                    num4 = (ushort)tabla.datos[66];
                                    diasUltimoReset = (int)tabla.datos[70];
                                    diasUltimoPulso = (int)tabla.datos[71];
                                    Byte[] Bateria = new Byte[4];
                                    fechaUltimoApagon = tabla.datos[72] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[72], (int)tabla.datos[73], (int)tabla.datos[74], (int)tabla.datos[75], (int)tabla.datos[76], (int)tabla.datos[77]) : new DateTime(2000, 1, 1);
                                    fechaUltimoEncendido = tabla.datos[78] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[78], (int)tabla.datos[79], (int)tabla.datos[80], (int)tabla.datos[81], (int)tabla.datos[82], (int)tabla.datos[83]) : new DateTime(2000, 1, 1);
                                    contadorApagones = (int)tabla.datos[84];
                                    for (int j = 0; j < 4; j++)
                                    {
                                        Bateria[j] = tabla.datos[85 + j];
                                    }

                                    Array.Reverse(Bateria);
                                    double bat, aux;
                                    bat = (Int64.Parse(BitConverter.ToString(Bateria, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    aux = Math.Truncate(bat / (60 * 60 * 24));
                                    bat = bat % (60 * 60 * 24);
                                    result = aux.ToString() + " days; ";
                                    aux = Math.Truncate(bat / (60 * 60));
                                    bat = bat % (60 * 60);
                                    result = result + aux.ToString() + " h; ";
                                    aux = Math.Truncate(bat / (60));
                                    bat = bat % 60;
                                    result = result + aux.ToString() + " m; " + bat.ToString() + " s; ";

                                }
                                else if (tabla.numeroTabla == 2056)
                                {
                                    fechaUltimaProgramacion = tabla.datos[387] != (byte)0 ? new DateTime(2000 + (int)tabla.datos[387], (int)tabla.datos[388], (int)tabla.datos[389], (int)tabla.datos[390], (int)tabla.datos[391], (int)tabla.datos[392]) : new DateTime(2000, 1, 1);
                                    if (tabla.datos[379] != 0x00)
                                    {
                                        flag = "S";
                                    }
                                    else
                                    {
                                        flag = "N";
                                    }
                                    //  sal = 5;
                                }
                                else if (tabla.numeroTabla == 2156)
                                {
                                    flag6 = true;
                                    byte[] contApagoSostenidos = new byte[2];
                                    byte[] contaApagonesMom = new byte[2];
                                    contaApagonesMom[0] = tabla.datos[0];
                                    contaApagonesMom[1] = tabla.datos[1];
                                    Array.Reverse(contaApagonesMom);
                                    contApagoSostenidos[0] = tabla.datos[2];
                                    contApagoSostenidos[1] = tabla.datos[3];
                                    Array.Reverse(contApagoSostenidos);
                                    contadorApagonesSostenidos = (Int64.Parse(BitConverter.ToString(contApagoSostenidos, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    contadorApagonesMom = (Int64.Parse(BitConverter.ToString(contaApagonesMom, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    Byte[] Bateria = new Byte[4];
                                    for (int j = 0; j < 4; j++)
                                    {
                                        Bateria[j] = tabla.datos[4 + j];
                                    }

                                    Array.Reverse(Bateria);
                                    double bat, aux;
                                    bat = (Int64.Parse(BitConverter.ToString(Bateria, 0).Replace("-", string.Empty), System.Globalization.NumberStyles.HexNumber));
                                    aux = Math.Truncate(bat / (60 * 60 * 24));
                                    bat = bat % (60 * 60 * 24);
                                    tiemposostenido = aux.ToString() + " days; ";
                                    aux = Math.Truncate(bat / (60 * 60));
                                    bat = bat % (60 * 60);
                                    tiemposostenido = tiemposostenido + aux.ToString() + " h; ";
                                    aux = Math.Truncate(bat / (60));
                                    bat = bat % 60;
                                    tiemposostenido = tiemposostenido + aux.ToString() + " m; " + bat.ToString() + " s; ";
                                }
                                
                            }
                            num7 = num7 + 6;
                            DateTime fechalec =  new DateTime(2000 + (int)archivo[num7], (int)archivo[num7+1], (int)archivo[num7 + 2], (int)archivo[num7 + 3], (int)archivo[num7 + 4], (int)archivo[num7 + 5]);

                            variables[0] = "Archivo";
                            val_var[0] = file; 
                            variables[1] = "NoSerieFabrica";
                            val_var[1] = Encoding.ASCII.GetString(serie_fact); 
                            variables[2] = "NoSerie";
                            val_var[2] = Encoding.ASCII.GetString(serie); ;
                            variables[3] = "Medidor";
                            val_var[3] = Encoding.ASCII.GetString(med);
                            variables[4] = "Cuenta";
                            val_var[4] = Encoding.ASCII.GetString(count);
                            variables[5] = "Programa";
                            val_var[5] = Encoding.ASCII.GetString(program);
                            variables[6] = "Programador";
                            val_var[6] = Encoding.ASCII.GetString(programer);
                            variables[7] = "NumeroTotalReset";
                            val_var[7] = numeroTotalReset.ToString();
                            variables[8] = "FechaCambioEstacion";
                            val_var[8] = fechacambioest.ToString();
                            variables[9] = "FechaCambioEstacion";
                            val_var[9] = fechaUltimoReset.ToString();
                            variables[10] = "horarioVerano";
                            val_var[10] = verano;
                            variables[11] = "fechaUltimaEscrituraPO";
                            val_var[11] = fechaUltimaEscrituraPO.ToString(); 
                            variables[12] = "sesionesEscrituraPO";
                            val_var[12] = num1.ToString(); 
                            variables[13] = "passwordInvalidoPO";
                            val_var[13] = num2.ToString();
                            variables[14] = "fechaUltimaEscrituraPI";
                            val_var[14] = FECHAPI.ToString();
                            variables[15] = "fechaUltimaEscrituraPO";
                            val_var[15] = FECHAPO.ToString();
                            variables[16] = "sesionesEscrituraPI";
                            val_var[16] = num3.ToString();
                            variables[17] = "passwordInvalidoPI";
                            val_var[17] = num4.ToString();
                            variables[18] = "diasUltimoReset";
                            val_var[18] = diasUltimoReset.ToString();
                            variables[19] = "diasUltimoPulso";
                            val_var[19] = diasUltimoPulso.ToString();
                            variables[20] = "fechaUltimoApagon";
                            val_var[20] = fechaUltimoApagon.ToString();
                            variables[21] = "fechaUltimoEncendido";
                            val_var[21] = fechaUltimoEncendido.ToString();
                            variables[22] = "contadorApagones";
                            val_var[22] = contadorApagones.ToString();
                            variables[23] = "tiempoUsoBateria";
                            val_var[23] = result;
                            variables[24] = "fechaUltimaProgramacion";
                            val_var[24] = fechaUltimaProgramacion.ToString();
                            variables[25] = "medidorEditado";
                            val_var[25] = flag.ToString();
                            variables[26] = "contadorApagonesSostenidos";
                            variables[27] = "contadorApagonesMomentaneos";
                            variables[28] = "tiempoApagonesSostenidos";
                            if (flag6)
                            {
                                val_var[26] = contadorApagonesSostenidos.ToString();
                                val_var[27] = contadorApagonesMom.ToString();
                                val_var[28] = tiemposostenido;
                            }
                            else
                            {
                                val_var[26] = "N/A";
                                val_var[27] = "N/A";
                                val_var[28] = "N/A";
                            }
                            variables[29] = "fechaLectura";
                            val_var[29] = fechalec.ToString();


                            

                            Array.Resize(ref variables, variables.Length + (nom_med.Length * 3));
                            Array.Resize(ref val_var, val_var.Length + (mediciones.Length * 3));
                            Array.Copy(mediciones, 0, val_var, 30, mediciones.Length);
                            Array.Copy(nom_med, 0, variables, 30, nom_med.Length);
                            string[] temp = new string[nom_med.Length];

                            for(int j = 0; j < nom_med.Length; j++)
                            {
                                temp[j] = "Previous Season " + nom_med[j];
                            }
                            Array.Copy(mediciones_p_s, 0, val_var, 30+ mediciones.Length, mediciones_p_s.Length);
                            Array.Copy(temp, 0, variables, 30+ nom_med.Length, temp.Length);

                            for (int j = 0; j < nom_med.Length; j++)
                            {
                                nom_med[j] = "Previous Billing " + nom_med[j];
                            }
                            Array.Copy(mediciones_p_b, 0, val_var, 30 + (mediciones.Length*2), mediciones_p_b.Length);
                            Array.Copy(nom_med, 0, variables, 30 + (nom_med.Length*2), nom_med.Length);
                            Array.Resize(ref variables, variables.Length + inst_val.Length);
                            Array.Resize(ref val_var, val_var.Length + instantaneos.Length);
                            Array.Copy(instantaneos, 0, val_var, 30 + (mediciones.Length * 3), instantaneos.Length);
                            Array.Copy(inst_val, 0, variables, 30 + (nom_med.Length * 3), inst_val.Length);

                            if (flag3)
                            {
                                Array.Resize(ref variables, variables.Length + log_name.Length);
                                Array.Resize(ref val_var, val_var.Length + log.Length);
                                Array.Copy(log, 0, val_var, 30 + (mediciones.Length * 3)+ instantaneos.Length, log.Length);
                                Array.Copy(log_name, 0, variables, 30 + (nom_med.Length * 3)+inst_val.Length, log_name.Length);

                            }


                            string impresion = "";
                            for(int n = 0; n < variables.Length; n++)
                            {
                                impresion = impresion + variables[n]+":|"+ val_var[n] + "|";
                            }
                           
                                streamWriter.WriteLine(impresion);
                            




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

       

        private void Btn_COL_Click(object sender, RoutedEventArgs e)
        {
            Txt_MED.Text = "";
            Txt_Prueba.Text = "";
            Btn_COL.Visibility = Visibility.Hidden;
        }

        
    }
}