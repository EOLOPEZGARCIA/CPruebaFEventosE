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
using LibreriaLectorMSR;
using Microsoft.Win32;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualBasic;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace fasoriales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string name = "";
        double va = 0;
        double vb = 0;
        double vc = 0;
        double aa = 0;
        double ab = 0;
        double ac = 0;
        double ava = 0;
        double avb = 0;
        double avc = 0;
        double aaa = 0;
        double aab = 0;
        double aac = 0;
        double uva = 0;
        double uvb = 0;
        double uvc = 0;
        double uaa = 0;
        double uab = 0;
        double uac = 0;
        double uava = 0;
        double uavb = 0;
        double uavc = 0;
        double uaaa = 0;
        double uaab = 0;
        double uaac = 0;
        double Imax = 0;
        double Vmax = 0;
        double U_Imax = 0;
        double U_Vmax = 0;
        int h = 0;
        SerialPort serialPort = new SerialPort();

        Byte[] tramaM0 = new Byte[] { 0x06 };
        Byte[] tramaM1 = new Byte[] { 0x55 };
        Byte[] tramaM2 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x01, 0x20, 0x13, 0x10 };
        Byte[] tramaM3 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x05, 0x61, 0x04, 0x00, 0xff, 0x06, 0xce, 0x5a };
        Byte[] tramaM4 = new Byte[] { 0xee, 0x00, 0x00, 0x00, 0x00, 0x0d, 0x50, 0x00, 0x02, 0x41, 0x64, 0x6d, 0x69, 0x6e, 0x69, 0x73, 0x74, 0x72, 0x61, 0xf9, 0x04 };
        Byte[] tramaM5 = new Byte[] { 0xee, 0x00, 0x20, 0x00, 0x00, 0x15, 0x51, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0xb6, 0xa5 };
        Byte[] tramaM6 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x03, 0x30, 0x00, 0x1C, 0xB2, 0xA5 }; // insta
        Byte[] tramaM7 = new Byte[] { 0xEE, 0x00, 0x20, 0x00, 0x00, 0x01, 0x52, 0x17, 0x20 };
        Byte[] tramaM8 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x01, 0x21, 0x9A, 0x01 };
        Byte[] tramaM9 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x00, 0x06, 0xea, 0x79 };//medidor
        Byte[] tramaM10 = new Byte[] { 0xEE, 0x00, 0x00, 0x00, 0x00, 0x03, 0x30, 0x08, 0x01, 0x95, 0xC3 };//valor PQM

        Byte[] arreglo1 = new Byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x30, 0x31 };
        Byte[] arreglo2 = new Byte[] { 0x15, 0x14, 0x13, 0x12, 0x11, 0x10, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00 };

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

        byte[] encrypted0 = null;
        byte[] encrypted01 = null;
        byte[] encrypted1 = null;
        byte[] encrypted21 = null;
        byte[] encrypted31 = null;
        byte[] encrypted41 = null;
        byte[] encrypted51 = null;
        byte[] encrypted61 = null;
        byte[] encrypted71 = null;
        byte[] encrypted2 = null;
        byte[] encrypted3 = null;
        byte[] encrypted4 = null;
        byte[] encrypted5 = null;
        byte[] encrypted6 = null;
        byte[] encrypted7 = null;
        byte[] encrypted8 = null;
        byte[] encrypted9 = null;
        byte[] encrypted10 = null;
        byte[] encrypted11 = null;
        byte[] encrypted12 = null;
        byte[] encrypted13 = null;
        byte[] encrypted14 = null;
        byte[] encrypted15 = null;
        byte[] encrypted16 = null;
        byte[] encrypted17 = null;
        byte[] encrypted18 = null;
        byte[] encrypted19 = null;

        string dencrypted0 = null;
        string dencrypted1 = null;
        string dencrypted2 = null;
        string dencrypted3 = null;
        string dencrypted4 = null;
        string dencrypted5 = null;
        string dencrypted6 = null;
        string dencrypted7 = null;
        string dencrypted8 = null;
        string dencrypted9 = null;
        string dencrypted10 = null;
        string dencrypted11 = null;
        string dencrypted12 = null;
        string dencrypted13 = null;
        string dencrypted14 = null;
        string dencrypted15 = null;
        string dencrypted16 = null;
        string dencrypted17 = null;
        string dencrypted18 = null;
        string dencrypted19 = null;
        bool bandera = true;

        public MainWindow()
        {

            InitializeComponent();
            cbo_Accion.Text = " ";
            cbo_Accion.Items.Add("Simular");
            cbo_Accion.Items.Add("Ingresar Datos");
            cbo_Accion.Items.Add("Leer Medidor A3");
            cbo_Accion.Items.Add("Leer Archivo MSR");
            cbo_Accion.Items.Add("Lectura Continua A3");
            cbo_Anomalia.Text = "";
            cbo_Anomalia.Items.Add("Fase A Desconectada");
            cbo_Anomalia.Items.Add("Fase C Desconectada");
            cbo_Anomalia.Items.Add("Fase B Desconectada");
            cbo_Anomalia.Items.Add("Fase A Invertida");
            cbo_Anomalia.Items.Add("Fase C Invertida");
            cbo_Anomalia.Items.Add("Fase B Invertida");
            cbo_Anomalia.Items.Add("TC de Fase A Invertido");
            cbo_Anomalia.Items.Add("TC de Fase C Invertido");
            cbo_Anomalia.Items.Add("TC de Fase B Invertido");
            cbo_Anomalia.Items.Add("Puente en la Fase A");
            cbo_Anomalia.Items.Add("Puente en la Fase C");
            cbo_Anomalia.Items.Add("Puente en la Fase B");
            cbo_Anomalia.Items.Add("Sobrevoltaje en la Fase A");
            cbo_Anomalia.Items.Add("Sobrevoltaje en la Fase C");
            cbo_Anomalia.Items.Add("Sobrevoltaje en la Fase B");
            cbo_Anomalia.Items.Add("Bajo voltaje en la Fase A");
            cbo_Anomalia.Items.Add("Bajo voltaje en la Fase C");
            cbo_Anomalia.Items.Add("Bajo voltaje en la Fase B");
            cbo_Puerto.Text = "";
            cbo_COD_MED.Items.Add("KXXX");
            cbo_COD_MED.Items.Add("VXXX");
            cbo_COD_MED.Items.Add("FXXX");
            cbo_COD_MED.Text = "";
            cbo_punto_med.Items.Add("MT");
            cbo_punto_med.Items.Add("BT");
            cbo_punto_med.Text = "";
            cbo_herr.Items.Add("Analizador");
            cbo_herr.Items.Add("Amperimetro");
            cbo_herr.Text = "";
            cbo_punto_med.Focus();

        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
        private double[] Puntos(double va, double vb, double vc, double aa, double ab, double ac, double ava, double avb, double avc, double aaa, double aab, double aac, double Vmax, double Imax)
        {
            double pVA = 150 / Vmax;
            double pVB = 150 / Vmax;
            double pVC = 150 / Vmax;
            double pCA = 150 / Imax;
            double pCB = 150 / Imax;
            double pCC = 150 / Imax;
            double MVA = 145;
            double MVB = 145;
            double MVC = 145;
            double MCA = 75;
            double MCB = 75;
            double MCC = 75;
            /*double MVA = va * pVA;
            double MVB = vb * pVB;
            double MVC = vc * pVC;
             * double MCA = aa * pCA;
            double MCB = ab * pCB;
            double MCC = ac * pCC;
            if (MCA < 20 && MCA > 0.05)
            {
                MCA = 35;
            }
            if (MCB < 20 && MCB > 0.05)
            {
                MCB = 35;
            }
            if (MCC < 20 && MCC > 0.05)
            {
                MCC = 35;
            }
            */
            double aVA = (Math.PI * (ava)) / 180;
            double aVB = (Math.PI * (avb)) / 180;
            double aVC = (Math.PI * (avc)) / 180;
            double aCA = (Math.PI * (aaa)) / 180;
            double aCB = (Math.PI * (aab)) / 180;
            double aCC = (Math.PI * (aac)) / 180;
            double a_VA = Math.Floor(aVA * 10000) / 10000;
            double a_VB = Math.Floor(aVB * 10000) / 10000;
            double a_VC = Math.Floor(aVC * 10000) / 10000;
            double a_CA = Math.Floor(aCA * 10000) / 10000;
            double a_CB = Math.Floor(aCB * 10000) / 10000;
            double a_CC = Math.Floor(aCC * 10000) / 10000;
            double xVA = Math.Cos(a_VA);
            double yVA = Math.Sin(a_VA);
            double xVB = Math.Cos(a_VB);
            double yVB = Math.Sin(a_VB);
            double xVC = Math.Cos(a_VC);
            double yVC = Math.Sin(a_VC);
            double xCA = Math.Cos(a_CA);
            double yCA = Math.Sin(a_CA);
            double xCB = Math.Cos(a_CB);
            double yCB = Math.Sin(a_CB);
            double xCC = Math.Cos(a_CC);
            double yCC = Math.Sin(a_CC);
            double x_VA = Math.Floor(xVA * 10000) / 10000;
            double x_VB = Math.Floor(xVB * 10000) / 10000;
            double x_VC = Math.Floor(xVC * 10000) / 10000;
            double y_VA = Math.Floor(yVA * 10000) / 10000;
            double y_VB = Math.Floor(yVB * 10000) / 10000;
            double y_VC = Math.Floor(yVC * 10000) / 10000;
            double x_CA = Math.Floor(xCA * 10000) / 10000;
            double x_CB = Math.Floor(xCB * 10000) / 10000;
            double x_CC = Math.Floor(xCC * 10000) / 10000;
            double y_CA = Math.Floor(yCA * 10000) / 10000;
            double y_CB = Math.Floor(yCB * 10000) / 10000;
            double y_CC = Math.Floor(yCC * 10000) / 10000;
            double XVA = xVA * MVA;
            double YVA = yVA * MVA;
            double XVB = xVB * MVB;
            double YVB = yVB * MVB;
            double XVC = xVC * MVC;
            double YVC = yVC * MVC;
            double XCA = xCA * MCA;
            double YCA = yCA * MCA;
            double XCB = xCB * MCB;
            double YCB = yCB * MCB;
            double XCC = xCC * MCC;
            double YCC = yCC * MCC;
            double[] RESPUESTA = { XVA, YVA, XVB, YVB, XVC, YVC, XCA, YCA, XCB, YCB, XCC, YCC };
            return RESPUESTA;
        }

        public void Archivo(string mod, string med, string va, string vb, string vc, string aa, string ab, string ac, string uva, string uvb, string uvc, string uaa, string uab, string uac, string ava, string avb, string avc, string aaa, string aab, string aac, string kwa, string kwb, string kwc, string fpa, string fpb, string fpc)
        {
            string fecha = DateTime.Now.ToString("F");
            fecha = fecha.Replace(" ", "_");
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = arreglo1;
                aes.IV = arreglo2;
                encrypted0 = Encrypt(fecha, aes.Key, aes.IV);
                encrypted01 = Encrypt(mod, aes.Key, aes.IV);
                encrypted1 = Encrypt(med, aes.Key, aes.IV);
                encrypted21 = Encrypt(va, aes.Key, aes.IV);
                encrypted31 = Encrypt(vb, aes.Key, aes.IV);
                encrypted41 = Encrypt(vc, aes.Key, aes.IV);
                encrypted51 = Encrypt(aa, aes.Key, aes.IV);
                encrypted61 = Encrypt(ab, aes.Key, aes.IV);
                encrypted71 = Encrypt(ac, aes.Key, aes.IV);
                encrypted2 = Encrypt(uva, aes.Key, aes.IV);
                encrypted3 = Encrypt(uvb, aes.Key, aes.IV);
                encrypted4 = Encrypt(uvc, aes.Key, aes.IV);
                encrypted5 = Encrypt(uaa, aes.Key, aes.IV);
                encrypted6 = Encrypt(uab, aes.Key, aes.IV);
                encrypted7 = Encrypt(uac, aes.Key, aes.IV);
                encrypted8 = Encrypt(ava, aes.Key, aes.IV);
                encrypted9 = Encrypt(avb, aes.Key, aes.IV);
                encrypted10 = Encrypt(avc, aes.Key, aes.IV);
                encrypted11 = Encrypt(aaa, aes.Key, aes.IV);
                encrypted12 = Encrypt(aab, aes.Key, aes.IV);
                encrypted13 = Encrypt(aac, aes.Key, aes.IV);
                encrypted14 = Encrypt(kwa, aes.Key, aes.IV);
                encrypted15 = Encrypt(kwb, aes.Key, aes.IV);
                encrypted16 = Encrypt(kwc, aes.Key, aes.IV);
                encrypted17 = Encrypt(fpa, aes.Key, aes.IV);
                encrypted18 = Encrypt(fpb, aes.Key, aes.IV);
                encrypted19 = Encrypt(fpc, aes.Key, aes.IV);
            }
            string linea0 = BitConverter.ToString(encrypted0);
            string linea1 = BitConverter.ToString(encrypted01);
            string linea2 = BitConverter.ToString(encrypted1);
            string linea3 = BitConverter.ToString(encrypted21);
            string linea4 = BitConverter.ToString(encrypted31);
            string linea5 = BitConverter.ToString(encrypted41);
            string linea6 = BitConverter.ToString(encrypted51);
            string linea7 = BitConverter.ToString(encrypted61);
            string linea8 = BitConverter.ToString(encrypted71);
            string linea9 = BitConverter.ToString(encrypted2);
            string linea10 = BitConverter.ToString(encrypted3);
            string linea11 = BitConverter.ToString(encrypted4);
            string linea12 = BitConverter.ToString(encrypted5);
            string linea13 = BitConverter.ToString(encrypted6);
            string linea14 = BitConverter.ToString(encrypted7);
            string linea15 = BitConverter.ToString(encrypted8);
            string linea16 = BitConverter.ToString(encrypted9);
            string linea17 = BitConverter.ToString(encrypted10);
            string linea18 = BitConverter.ToString(encrypted11);
            string linea19 = BitConverter.ToString(encrypted12);
            string linea20 = BitConverter.ToString(encrypted13);
            string linea21 = BitConverter.ToString(encrypted14);
            string linea22 = BitConverter.ToString(encrypted15);
            string linea23 = BitConverter.ToString(encrypted16);
            string linea24 = BitConverter.ToString(encrypted17);
            string linea25 = BitConverter.ToString(encrypted18);
            string linea26 = BitConverter.ToString(encrypted19);

            //string filepath = "";
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            string path = filepath + ".\\_" + med;
            string path2 = path+ ".\\_" + med;
            string path3 = path + ".\\_" + med+ ".jpg";
            DirectoryInfo Folder = Directory.CreateDirectory(path);


            if (!File.Exists(path))
            {
                
                Folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                var images = CaptureActiveWindow();
                images.Save(path3, ImageFormat.Jpeg);
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path2))
                {
                    sw.WriteLine(linea0);
                    sw.WriteLine(linea1);
                    sw.WriteLine(linea2);
                    sw.WriteLine(linea3);
                    sw.WriteLine(linea4);
                    sw.WriteLine(linea5);
                    sw.WriteLine(linea6);
                    sw.WriteLine(linea7);
                    sw.WriteLine(linea8);
                    sw.WriteLine(linea9);
                    sw.WriteLine(linea10);
                    sw.WriteLine(linea11);
                    sw.WriteLine(linea12);
                    sw.WriteLine(linea13);
                    sw.WriteLine(linea14);
                    sw.WriteLine(linea15);
                    sw.WriteLine(linea16);
                    sw.WriteLine(linea17);
                    sw.WriteLine(linea18);
                    sw.WriteLine(linea19);
                    sw.WriteLine(linea20);
                    sw.WriteLine(linea21);
                    sw.WriteLine(linea22);
                    sw.WriteLine(linea23);
                    sw.WriteLine(linea24);
                    sw.WriteLine(linea25);
                    sw.WriteLine(linea26);
                    File.SetAttributes(path,FileAttributes.Hidden);
                }
            }
            else if (File.Exists(path))
            {
                if ((Folder.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    //Add Hidden flag    
                    Folder.Attributes |= FileAttributes.Hidden;
                }
                var images = CaptureActiveWindow();
                images.Save(path3, ImageFormat.Jpeg);
                TextWriter sw = new StreamWriter(path2);
                sw.WriteLine(linea0);
                sw.WriteLine(linea1);
                sw.WriteLine(linea2);
                sw.WriteLine(linea3);
                sw.WriteLine(linea4);
                sw.WriteLine(linea5);
                sw.WriteLine(linea6);
                sw.WriteLine(linea7);
                sw.WriteLine(linea8);
                sw.WriteLine(linea9);
                sw.WriteLine(linea10);
                sw.WriteLine(linea11);
                sw.WriteLine(linea12);
                sw.WriteLine(linea13);
                sw.WriteLine(linea14);
                sw.WriteLine(linea15);
                sw.WriteLine(linea16);
                sw.WriteLine(linea17);
                sw.WriteLine(linea18);
                sw.WriteLine(linea19);
                sw.Close();
            }


        }
        private double[] Potencias (double va, double vb, double vc, double aa, double ab, double ac, double ava, double avb, double avc, double aaa, double aab, double aac, double TC, double TP )
        {
          
            double VAL_KWA = Math.Abs(ava - aaa);
            double VAL_KWB = Math.Abs(avb - aab);
            double VAL_KWC = Math.Abs(avc - aac);
            
            double aVAL_KWA = (Math.PI * (VAL_KWA)) / 180;
            double aVAL_KWB = (Math.PI * (VAL_KWB)) / 180;
            double aVAL_KWC = (Math.PI * (VAL_KWC)) / 180;
            
            double a_VAL_KWA = Math.Floor(aVAL_KWA * 10000) / 10000;
            double a_VAL_KWB = Math.Floor(aVAL_KWB * 10000) / 10000;
            double a_VAL_KWC = Math.Floor(aVAL_KWC * 10000) / 10000;
            
            double VAL_FPA = Math.Cos(a_VAL_KWA);
            double VAL_FPB = Math.Cos(a_VAL_KWB);
            double VAL_FPC = Math.Cos(a_VAL_KWC);
            
            double FPA = Math.Floor(VAL_FPA * 10000) / 10000;
            double FPB = Math.Floor(VAL_FPB * 10000) / 10000;
            double FPC = Math.Floor(VAL_FPC * 10000) / 10000;
            
            double KWA = Math.Floor(va * aa * TC * TP * FPA)/1000;
            double KWB = Math.Floor(vb * ab * TC * TP * FPB)/1000;
            double KWC = Math.Floor(vc * ac * TC * TP * FPC)/1000;

            double[] resultado = { KWA, KWB, KWC, FPA, FPB, FPC };
            return resultado;
        }
        private void Btn_calcular_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Btn_calcular.IsEnabled = false;
                va = Convert.ToDouble(Voltaje_A.Text);
                vb = Convert.ToDouble(Voltaje_B.Text);
                vc = Convert.ToDouble(Voltaje_C.Text);
                aa = Convert.ToDouble(Corriente_A.Text);
                ab = Convert.ToDouble(Corriente_B.Text); 
                ac = Convert.ToDouble(Corriente_C.Text);
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                aab = Convert.ToDouble(Angulo_Corriente_B.Text); 
                aac = Convert.ToDouble(Angulo_Corriente_C.Text); 
                Lienzo.Children.Clear();
                if (va >=0 && va<= Vmax )
                {
                    if (vb >= 0 && vb <= Vmax)
                    {
                        if (vc >= 0 && vc <= Vmax)
                        {
                            if (aa >= 0 && aa <= Imax)
                            {
                                if (ab >= 0 && ab <= Imax)
                                {
                                    if (ac >= 0 && ac <= Imax)
                                    {
                                        if (ava<=360 && ava >= 0)
                                        {
                                            if (avb <= 360 && avb >= 0)
                                            {
                                                if (avc <= 360 && avc >= 0)
                                                {
                                                    if (aaa <= 360 && aaa >= 0)
                                                    {
                                                        if (aab <= 360 && aab >= 0)
                                                        {
                                                            if (aac <= 360 && aac >= 0)
                                                            {
                                                                Ellipse contorno = new Ellipse();
                                                                contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                                                                contorno.StrokeThickness = 1;
                                                                contorno.Height = Lienzo.ActualHeight;
                                                                contorno.Width = Lienzo.ActualWidth;
                                                                contorno.RenderTransformOrigin = new System.Windows.Point(Lienzo.ActualHeight / 2, Lienzo.ActualWidth / 2);
                                                                @base.RenderTransformOrigin = new System.Windows.Point(Lienzo.ActualHeight / 2, Lienzo.ActualWidth / 2);
                                                                Lienzo.Children.Add(contorno);
                                                                Lienzo.Children.Add(@base);
                                                                Line VA = new Line();
                                                                Line VB = new Line();
                                                                Line VC = new Line();
                                                                Line CA = new Line();
                                                                Line CB = new Line();
                                                                Line CC = new Line();
                                                                Line xs = new Line();
                                                                Line ys = new Line();
                                                                double midx = Lienzo.ActualHeight / 2;
                                                                double midy = Lienzo.ActualWidth / 2;
                                                                VA.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VA.StrokeThickness = 4;
                                                                VB.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VB.StrokeThickness = 4;
                                                                VC.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VC.StrokeThickness = 4;
                                                                CA.Stroke = System.Windows.Media.Brushes.Red;
                                                                CA.StrokeThickness = 2;
                                                                CB.Stroke = System.Windows.Media.Brushes.Red;
                                                                CB.StrokeThickness = 2;
                                                                CC.Stroke = System.Windows.Media.Brushes.Red;
                                                                CC.StrokeThickness = 2;
                                                                xs.Stroke = System.Windows.Media.Brushes.Black;
                                                                xs.StrokeThickness = 0.5;
                                                                ys.Stroke = System.Windows.Media.Brushes.Black;
                                                                ys.StrokeThickness = 0.5;
                                                                xs.X1 = 0;
                                                                xs.X2 = Lienzo.ActualHeight;
                                                                xs.Y1 = midy;
                                                                xs.Y2 = midy;
                                                                ys.X1 = midx;
                                                                ys.X2 = midx;
                                                                ys.Y1 = 0;
                                                                ys.Y2 = Lienzo.ActualWidth;
                                                                VA.X1 = midx;
                                                                VA.Y1 = midy;
                                                                VB.X1 = midx;
                                                                VB.Y1 = midy;
                                                                VC.X1 = midx;
                                                                VC.Y1 = midy;
                                                                CA.X1 = midx;
                                                                CA.Y1 = midy;
                                                                CB.X1 = midx;
                                                                CB.Y1 = midy;
                                                                CC.X1 = midx;
                                                                CC.Y1 = midy;
                                                                double[] punto = Puntos(va,vb,vc,aa,ab,ac,ava,avb,avc,aaa,aab,aac,Vmax,Imax);
                                                                VA.X2 = midx + punto[0];
                                                                VA.Y2 = midy + (punto[1] * (-1));
                                                                VB.X2 = midx + punto[2];
                                                                VB.Y2 = midy + (punto[3] * (-1));
                                                                VC.X2 = midx + punto[4];
                                                                VC.Y2 = midy + (punto[5] * (-1));
                                                                CA.X2 = midx + punto[6];
                                                                CA.Y2 = midy + (punto[7] * (-1));
                                                                CB.X2 = midx + punto[8];
                                                                CB.Y2 = midy + (punto[9] * (-1));
                                                                CC.X2 = midx + punto[10];
                                                                CC.Y2 = midy + (punto[11] * (-1));
                                                                RotateTransform rotateTransformC = new RotateTransform(90-avc, imgC.Width/2, imgC.Height/2);
                                                                @imgC.RenderTransform = rotateTransformC;
                                                                RotateTransform rotateTransformA = new RotateTransform(90-ava, imgA.Width / 2, imgA.Height / 2);
                                                                @imgA.RenderTransform = rotateTransformA;
                                                                RotateTransform rotateTransformB = new RotateTransform(90-avb, imgB.Width / 2, imgB.Height / 2);
                                                                @imgB.RenderTransform = rotateTransformB;
                                                                RotateTransform rotateTransformc = new RotateTransform(90-aac, imgc.Width / 2, imgc.Height / 2);
                                                                @imgc.RenderTransform = rotateTransformc;
                                                                RotateTransform rotateTransforma = new RotateTransform(90-aaa, imga.Width / 2, imga.Height / 2);
                                                                @imga.RenderTransform = rotateTransforma;
                                                                RotateTransform rotateTransformb = new RotateTransform(90-aab, imgb.Width / 2, imgb.Height / 2);
                                                                @imgb.RenderTransform = rotateTransformb;
                                                                Lienzo.Children.Add(xs);
                                                                Lienzo.Children.Add(ys);
                                                                Lienzo.Children.Add(VA);
                                                                Lienzo.Children.Add(VB);
                                                                Lienzo.Children.Add(VC);
                                                                Lienzo.Children.Add(CA);
                                                                Lienzo.Children.Add(CB);
                                                                Lienzo.Children.Add(CC);
                                                                Lienzo.Children.Add(@imgC);
                                                                Lienzo.Children.Add(@imgB);
                                                                Lienzo.Children.Add(@imgA);
                                                                Lienzo.Children.Add(@imgc);
                                                                Lienzo.Children.Add(@imgb);
                                                                Lienzo.Children.Add(@imga);
                                                                double[] potencia = Potencias(va, vb, vc, aa, ab, ac, ava, avb, avc, aaa, aab, aac,1,1);
                                                                KW_A_DIS.Text = potencia[0].ToString();
                                                                KW_B_DIS.Text = potencia[1].ToString();
                                                                KW_C_DIS.Text = potencia[2].ToString();
                                                                FP_A_DIS.Text = potencia[3].ToString();
                                                                FP_B_DIS.Text = potencia[4].ToString();
                                                                FP_C_DIS.Text = potencia[5].ToString();
                                                                cbo_COD_MED.Focus();
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Error: Valor de angulo corriente Fase C incorrecto");
                                                                Angulo_Corriente_C.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Error: Valor de angulo corriente Fase B incorrecto");
                                                            Angulo_Corriente_B.Focus();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Error: Valor de angulo corriente Fase A incorrecto");
                                                        Angulo_Corriente_A.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Error: Valor de angulo voltaje Fase C incorrecto");
                                                    Angulo_Voltaje_C.Focus();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Error: Valor de angulo voltaje Fase B incorrecto");
                                                Angulo_Voltaje_B.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: Valor de angulo voltaje Fase A incorrecto");
                                            Angulo_Voltaje_A.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error: Valor de corriente Fase C incorrecto");
                                        Corriente_C.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error: Valor de corriente Fase B incorrecto");
                                    Corriente_B.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error: Valor de corriente Fase A incorrecto");
                                Corriente_A.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: Valor de voltaje Fase C incorrecto");
                            Voltaje_C.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Error: Valor de voltaje Fase B incorrecto");
                        Voltaje_B.Focus();
                    }
                }
                
                else
                {
                    MessageBox.Show("Error: Valor de voltaje Fase A incorrecto");
                    Voltaje_A.Focus();
                    
                }
                Btn_calcular.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_calcular.IsEnabled = true;
            }
            
        }

        private void Cbo_Accion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            name = cbo_Accion.Items.GetItemAt(cbo_Accion.SelectedIndex).ToString();
            if (name == "Simular")
            {
                Txt_Anomalia.Visibility = Visibility.Visible;
                cbo_Anomalia.Visibility = Visibility.Visible ;
                Btn_Dibujar.Visibility = Visibility.Hidden;
                cbo_Puerto.Visibility = Visibility.Hidden;
                Txt_Com.Visibility = Visibility.Hidden;
                Btn_Leer.Visibility = Visibility.Hidden;
                Medidor.Visibility = Visibility.Hidden;
                Medidor.IsReadOnly = true;
                Txt_Diagnostico.Visibility = Visibility.Hidden;
                Txt_Diag.Visibility = Visibility.Hidden;
                Btn_File.Visibility = Visibility.Hidden;
                Txt_Path.Visibility = Visibility.Hidden;

                Txt_Ruta.Visibility = Visibility.Hidden;
                MessageBox.Show("Ingrese la Anomalia a graficar");
                cbo_Anomalia.Focus();
                Usuario_Angulo_Corriente_A.Text = "";
                Usuario_Angulo_Corriente_B.Text = "";
                Usuario_Angulo_Corriente_C.Text = "";
                Usuario_Angulo_Voltaje_A.Text = "";
                Usuario_Angulo_Voltaje_B.Text = "";
                Usuario_Angulo_Voltaje_C.Text = "";
                Usuario_Corriente_A.Text = "";
                Usuario_Corriente_B.Text = "";
                Usuario_Corriente_C.Text = "";
                Usuario_Voltaje_A.Text = "";
                Usuario_Voltaje_B.Text = "";
                Usuario_Voltaje_C.Text = "";
            }
            else if (name == "Ingresar Datos")
            {
                Txt_Anomalia.Visibility = Visibility.Hidden;
                cbo_Anomalia.Visibility = Visibility.Hidden;
                Btn_Dibujar.Visibility = Visibility.Visible;
                cbo_Puerto.Visibility = Visibility.Hidden;
                Txt_Com.Visibility = Visibility.Hidden;
                Btn_Leer.Visibility = Visibility.Hidden;
                Txt_Diagnostico.Visibility = Visibility.Visible;
                Txt_Diag.Visibility = Visibility.Visible;
                Medidor.Visibility = Visibility.Visible;
                Btn_File.Visibility = Visibility.Hidden;
                Txt_Path.Visibility = Visibility.Hidden;
                Txt_Ruta.Visibility = Visibility.Hidden;
                MessageBox.Show("Ingrese Numero, Codigo y los valores del medidor a graficar");
                Medidor.IsReadOnly = false;
                Usuario_Angulo_Corriente_A.Text = "";
                Usuario_Angulo_Corriente_B.Text = "";
                Usuario_Angulo_Corriente_C.Text = "";
                Usuario_Angulo_Voltaje_A.Text = "";
                Usuario_Angulo_Voltaje_B.Text = "";
                Usuario_Angulo_Voltaje_C.Text = "";
                Usuario_Corriente_A.Text = "";
                Usuario_Corriente_B.Text = "";
                Usuario_Corriente_C.Text = "";
                Usuario_Voltaje_A.Text = "";
                Usuario_Voltaje_B.Text = "";
                Usuario_Voltaje_C.Text = "";
                Medidor.Focus();
            }
            else if (name == "Leer Medidor A3")
            {
                Txt_Anomalia.Visibility = Visibility.Hidden;
                cbo_Anomalia.Visibility = Visibility.Hidden;
                Btn_Dibujar.Visibility = Visibility.Hidden;
                cbo_Puerto.Visibility = Visibility.Visible;
                Txt_Com.Visibility = Visibility.Visible;
                Btn_Leer.Visibility = Visibility.Visible;
                Txt_Diagnostico.Visibility = Visibility.Visible;
                Txt_Diag.Visibility = Visibility.Visible;
                Medidor.Visibility = Visibility.Visible;
                Medidor.IsReadOnly = true;
                Btn_File.Visibility = Visibility.Hidden;
                Txt_Path.Visibility = Visibility.Hidden;
                Txt_Ruta.Visibility = Visibility.Hidden;
                Usuario_Angulo_Corriente_A.Text = "";
                Usuario_Angulo_Corriente_B.Text = "";
                Usuario_Angulo_Corriente_C.Text = "";
                Usuario_Angulo_Voltaje_A.Text = "";
                Usuario_Angulo_Voltaje_B.Text = "";
                Usuario_Angulo_Voltaje_C.Text = "";
                Usuario_Corriente_A.Text = "";
                Usuario_Corriente_B.Text = "";
                Usuario_Corriente_C.Text = "";
                Usuario_Voltaje_A.Text = "";
                Usuario_Voltaje_B.Text = "";
                Usuario_Voltaje_C.Text = "";
                foreach (String puerto in SerialPort.GetPortNames())
                {
                    cbo_Puerto.Items.Add(puerto);
                }
                MessageBox.Show("Ingrese el puerto COM del lector optico");
                cbo_Puerto.Focus();
            }
            else if (name == "Leer Archivo MSR")
            {
                Txt_Anomalia.Visibility = Visibility.Hidden;
                cbo_Anomalia.Visibility = Visibility.Hidden;
                Btn_Dibujar.Visibility = Visibility.Hidden;
                cbo_Puerto.Visibility = Visibility.Hidden;
                Txt_Com.Visibility = Visibility.Hidden;
                Btn_Leer.Visibility = Visibility.Hidden;
                Txt_Diagnostico.Visibility = Visibility.Visible;
                Txt_Diag.Visibility = Visibility.Visible;
                Medidor.Visibility = Visibility.Visible;
                Medidor.IsReadOnly = true;
                Btn_File.Visibility = Visibility.Visible;
                Txt_Path.Visibility = Visibility.Visible;
                Txt_Ruta.Visibility = Visibility.Visible;
                Usuario_Angulo_Corriente_A.Text = "";
                Usuario_Angulo_Corriente_B.Text = "";
                Usuario_Angulo_Corriente_C.Text = "";
                Usuario_Angulo_Voltaje_A.Text = "";
                Usuario_Angulo_Voltaje_B.Text = "";
                Usuario_Angulo_Voltaje_C.Text = "";
                Usuario_Corriente_A.Text = "";
                Usuario_Corriente_B.Text = "";
                Usuario_Corriente_C.Text = "";
                Usuario_Voltaje_A.Text = "";
                Usuario_Voltaje_B.Text = "";
                Usuario_Voltaje_C.Text = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Metercat Stored Reading (*.msr)|*.msr";
               if (openFileDialog.ShowDialog() == true)
                {
                    Txt_Path.Text = openFileDialog.FileName;
                    Btn_File.Focus();
                }
            }
            else if (name == "Lectura Continua A3")
            {
                Txt_Anomalia.Visibility = Visibility.Hidden;
                cbo_Anomalia.Visibility = Visibility.Hidden;
                Btn_Dibujar.Visibility = Visibility.Hidden;
                cbo_Puerto.Visibility = Visibility.Visible;
                Txt_Com.Visibility = Visibility.Visible;
                Btn_Leer.Visibility = Visibility.Hidden;
                Txt_Diagnostico.Visibility = Visibility.Hidden;
                Txt_Diag.Visibility = Visibility.Hidden;
                Medidor.Visibility = Visibility.Visible;
                Btn_Continua.Visibility = Visibility.Visible;
                Medidor.IsReadOnly = true;
                Btn_File.Visibility = Visibility.Hidden;
                Txt_Path.Visibility = Visibility.Hidden;
                Txt_Ruta.Visibility = Visibility.Hidden;
                Usuario_Angulo_Corriente_A.Text = "";
                Usuario_Angulo_Corriente_B.Text = "";
                Usuario_Angulo_Corriente_C.Text = "";
                Usuario_Angulo_Voltaje_A.Text = "";
                Usuario_Angulo_Voltaje_B.Text = "";
                Usuario_Angulo_Voltaje_C.Text = "";
                Usuario_Corriente_A.Text = "";
                Usuario_Corriente_B.Text = "";
                Usuario_Corriente_C.Text = "";
                Usuario_Voltaje_A.Text = "";
                Usuario_Voltaje_B.Text = "";
                Usuario_Voltaje_C.Text = "";
                foreach (String puerto in SerialPort.GetPortNames())
                {
                    cbo_Puerto.Items.Add(puerto);
                }
                MessageBox.Show("Ingrese el puerto COM del lector optico");
                cbo_Puerto.Focus();
            }
        }

        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Btn_Leer.IsEnabled = false;
                va = Convert.ToDouble(Voltaje_A.Text);
                vb = Convert.ToDouble(Voltaje_B.Text);
                vc = Convert.ToDouble(Voltaje_C.Text);
                aa = Convert.ToDouble(Corriente_A.Text);
                ab = Convert.ToDouble(Corriente_B.Text);
                ac = Convert.ToDouble(Corriente_C.Text);
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                aab = Convert.ToDouble(Angulo_Corriente_B.Text);
                aac = Convert.ToDouble(Angulo_Corriente_C.Text);
                byte[] respuestaSerial = null;
                byte[] respuestaSerial2 = null;
                byte[] auxiliar = null;
                byte[] respuestaSerial3 = null;
                byte[] auxiliar2 = null;
                byte[] auxiliar3 = null;
                byte[] respuestaSerial4 = null;
                byte[] auxiliar4 = null;
                int intentos = 0;
                //MessageBox.Show("Favor de adquirir la actualización");
                serialPort = new SerialPort(cbo_Puerto.Text, 9600, Parity.None, 8, StopBits.One);
                serialPort.Close();
                serialPort.Open();
                Task.Delay(500).Wait();
                serialPort.Write(tramaM1, 0, tramaM1.Length);
                Task.Delay(500).Wait();
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
                respuestaSerial = null;
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                serialPort.Write(tramaM3, 0, tramaM3.Length);
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
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                serialPort.Write(tramaM4, 0, tramaM4.Length);
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
                serialPort.Write(tramaM0, 0, tramaM0.Length);
                Task.Delay(500).Wait();
                serialPort.Write(tramaM5, 0, tramaM5.Length);
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
                int a = 0;
                int b = 0;
                int c = 0;
                int d = 0;
                int div = 0;
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
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM9, 0, tramaM9.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
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
                            byte[] bytes3 = new byte[serialPort.BytesToRead];
                            respuestaSerial3 = new byte[auxiliar3.Length + bytes3.Length];
                            Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                            serialPort.Read(bytes3, 0, bytes3.Length);
                            Array.Copy(bytes3, 0, respuestaSerial3, auxiliar3.Length, bytes3.Length);
                        }
                    }
                    serialPort.Write(tramaM6, 0, tramaM6.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
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

                    serialPort.Write(tramaM10, 0, tramaM10.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    while (serialPort.BytesToRead > 0)
                    {
                        if (respuestaSerial4 == null)
                        {
                            respuestaSerial4 = new byte[serialPort.BytesToRead];
                            serialPort.Read(respuestaSerial4, 0, respuestaSerial4.Length);
                        }
                        else
                        {
                            auxiliar4 = new byte[respuestaSerial4.Length];
                            auxiliar4 = respuestaSerial4;
                            byte[] bytes4 = new byte[serialPort.BytesToRead];
                            respuestaSerial4 = new byte[auxiliar4.Length + bytes4.Length];
                            Array.Copy(auxiliar4, respuestaSerial4, auxiliar4.Length);
                            serialPort.Read(bytes4, 0, bytes4.Length);
                            Array.Copy(bytes4, 0, respuestaSerial4, auxiliar4.Length, bytes4.Length);
                        }
                    }

                    serialPort.Write(tramaM7, 0, tramaM7.Length);
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
                    serialPort.Write(tramaM0, 0, tramaM0.Length);
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM8, 0, tramaM8.Length);
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
                    serialPort.Write(tramaM0, 0, tramaM0.Length);

                    for (int i = 0; i < respuestaSerial2.Length; i++)
                    {
                        if (respuestaSerial2[i] == 0xee && respuestaSerial2[i + 1] == 0x00)
                        {
                            a = i;
                            break;
                        }
                    }

                    for (int k = 0; k < respuestaSerial3.Length; k++)
                    {
                        if (respuestaSerial3[k] == 0xee && respuestaSerial3[k + 1] == 0x00)
                        {
                            b = k;
                            break;
                        }
                    }

                    for (int k = 0; k < respuestaSerial4.Length; k++)
                    {
                        if (respuestaSerial4[k] == 0xee && respuestaSerial4[k + 1] == 0x00)
                        {
                            c = k;
                            break;
                        }
                    }
                    if (respuestaSerial2[a + 5] == 0xf4)
                    {
                        d = 0;
                    }
                    else if (respuestaSerial2[a + 5] == 0xd4)
                    {
                        d = 32;
                    }
                    if (respuestaSerial4[c + 44] == 0x31)
                    {
                        div = 10000;
                    }
                    else if (respuestaSerial4[c + 44] == 0x32)
                    {

                        div = 1000;
                    }
                    for (int j = 0; j < 6; j++)
                    {
                        VoltajeA[j] = respuestaSerial2[a + 68 - j - d];
                        CorrienteA[j] = respuestaSerial2[a + 74 - j - d];
                        VoltajeC[j] = respuestaSerial2[a + 86 - j - d];
                        CorrienteC[j] = respuestaSerial2[a + 92 - j - d];
                        AnguloVoltajeC[j] = respuestaSerial2[a + 104 - j - d];
                        VoltajeB[j] = respuestaSerial2[a + 110 - j - d];
                        CorrienteB[j] = respuestaSerial2[a + 116 - j - d];
                        AnguloVoltajeB[j] = respuestaSerial2[a + 128 - j - d];
                        AnguloCorrienteA[j] = respuestaSerial2[a + 176 - j - d];
                        AnguloCorrienteB[j] = respuestaSerial2[a + 182 - j - d];
                        AnguloCorrienteC[j] = respuestaSerial2[a + 188 - j - d];
                        MED[j] = respuestaSerial3[b + 123 + j];
                    }
                    AnguloVoltajeA = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    Medidor.Text = Encoding.ASCII.GetString(MED);
                    Usuario_Voltaje_A.Text = BitConverter.ToString(VoltajeA, 0).Replace("-", string.Empty);
                    uva = (Int32.Parse(Usuario_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uva = uva / div;
                    Usuario_Voltaje_A.Text = uva.ToString();
                    Usuario_Voltaje_B.Text = BitConverter.ToString(VoltajeB, 0).Replace("-", string.Empty);
                    uvb = (Int32.Parse(Usuario_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uvb = uvb / div;
                    Usuario_Voltaje_B.Text = uvb.ToString();
                    Usuario_Voltaje_C.Text = BitConverter.ToString(VoltajeC).Replace("-", string.Empty); 
                    uvc = (Int32.Parse(Usuario_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uvc = uvc / div;
                    Usuario_Voltaje_C.Text = uvc.ToString();
                    Usuario_Corriente_A.Text = BitConverter.ToString(CorrienteA).Replace("-", string.Empty); 
                    uaa = (Int32.Parse(Usuario_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uaa = uaa / div;
                    Usuario_Corriente_A.Text = uaa.ToString();
                    Usuario_Corriente_B.Text = BitConverter.ToString(CorrienteB).Replace("-", string.Empty); 
                    uab = (Int32.Parse(Usuario_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uab = uab / div;
                    Usuario_Corriente_B.Text = uab.ToString();
                    Usuario_Corriente_C.Text = BitConverter.ToString(CorrienteC).Replace("-", string.Empty); 
                    uac = (Int32.Parse(Usuario_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uac = uac / div;
                    Usuario_Corriente_C.Text = uac.ToString();
                    Usuario_Angulo_Voltaje_A.Text = BitConverter.ToString(AnguloVoltajeA).Replace("-", string.Empty); 
                    uava = (Int32.Parse(Usuario_Angulo_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uava = uava / div;
                    Usuario_Angulo_Voltaje_A.Text = uava.ToString();
                    Usuario_Angulo_Voltaje_B.Text = BitConverter.ToString(AnguloVoltajeB).Replace("-", string.Empty); 
                    uavb = (Int32.Parse(Usuario_Angulo_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uavb = uavb / div;
                    Usuario_Angulo_Voltaje_B.Text = uavb.ToString();
                    Usuario_Angulo_Voltaje_C.Text = BitConverter.ToString(AnguloVoltajeC).Replace("-", string.Empty); 
                    uavc = (Int32.Parse(Usuario_Angulo_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uavc = uavc / div;
                    Usuario_Angulo_Voltaje_C.Text = uavc.ToString();
                    Usuario_Angulo_Corriente_A.Text = BitConverter.ToString(AnguloCorrienteA).Replace("-", string.Empty);
                    uaaa = (Int32.Parse(Usuario_Angulo_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uaaa = uaaa / div;
                    Usuario_Angulo_Corriente_A.Text = uaaa.ToString();
                    Usuario_Angulo_Corriente_B.Text = BitConverter.ToString(AnguloCorrienteB).Replace("-", string.Empty);
                    uaab = (Int32.Parse(Usuario_Angulo_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uaab = uaab / div;
                    Usuario_Angulo_Corriente_B.Text = uaab.ToString();
                    Usuario_Angulo_Corriente_C.Text = BitConverter.ToString(AnguloCorrienteC).Replace("-", string.Empty);
                    uaac = (Int32.Parse(Usuario_Angulo_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uaac = uaac / div;
                    Usuario_Angulo_Corriente_C.Text = uaac.ToString();
                    Usuario_Lienzo.Children.Clear();
                    Ellipse contorno = new Ellipse();
                    contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                    contorno.StrokeThickness = 1;
                    contorno.Height = Usuario_Lienzo.ActualHeight;
                    contorno.Width = Usuario_Lienzo.ActualWidth;
                    contorno.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                    @U_base.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                    Usuario_Lienzo.Children.Add(@U_base);
                    Usuario_Lienzo.Children.Add(contorno);
                    Line VA = new Line();
                    Line VB = new Line();
                    Line VC = new Line();
                    Line CA = new Line();
                    Line CB = new Line();
                    Line CC = new Line();
                    Line xs = new Line();
                    Line ys = new Line();
                    double midx = Usuario_Lienzo.ActualHeight / 2;
                    double midy = Usuario_Lienzo.ActualWidth / 2;
                    VA.Stroke = System.Windows.Media.Brushes.Blue;
                    VA.StrokeThickness = 4;
                    VB.Stroke = System.Windows.Media.Brushes.Blue;
                    VB.StrokeThickness = 4;
                    VC.Stroke = System.Windows.Media.Brushes.Blue;
                    VC.StrokeThickness = 4;
                    CA.Stroke = System.Windows.Media.Brushes.Red;
                    CA.StrokeThickness = 2;
                    CB.Stroke = System.Windows.Media.Brushes.Red;
                    CB.StrokeThickness = 2;
                    CC.Stroke = System.Windows.Media.Brushes.Red;
                    CC.StrokeThickness = 2;
                    xs.Stroke = System.Windows.Media.Brushes.Black;
                    xs.StrokeThickness = 0.5;
                    ys.Stroke = System.Windows.Media.Brushes.Black;
                    ys.StrokeThickness = 0.5;
                    xs.X1 = 0;
                    xs.X2 = Usuario_Lienzo.ActualHeight;
                    xs.Y1 = midy;
                    xs.Y2 = midy;
                    ys.X1 = midx;
                    ys.X2 = midx;
                    ys.Y1 = 0;
                    ys.Y2 = Usuario_Lienzo.ActualWidth;
                    VA.X1 = midx;
                    VA.Y1 = midy;
                    VB.X1 = midx;
                    VB.Y1 = midy;
                    VC.X1 = midx;
                    VC.Y1 = midy;
                    CA.X1 = midx;
                    CA.Y1 = midy;
                    CB.X1 = midx;
                    CB.Y1 = midy;
                    CC.X1 = midx;
                    CC.Y1 = midy;
                    double[] punto = Puntos(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, U_Vmax, U_Imax);
                    VA.X2 = midx + punto[0];
                    VA.Y2 = midy + (punto[1] * (-1));
                    VB.X2 = midx + punto[2];
                    VB.Y2 = midy + (punto[3] * (-1));
                    VC.X2 = midx + punto[4];
                    VC.Y2 = midy + (punto[5] * (-1));
                    CA.X2 = midx + punto[6];
                    CA.Y2 = midy + (punto[7] * (-1));
                    CB.X2 = midx + punto[8];
                    CB.Y2 = midy + (punto[9] * (-1));
                    CC.X2 = midx + punto[10];
                    CC.Y2 = midy + (punto[11] * (-1));
                    RotateTransform rotateTransformU_C = new RotateTransform(90 - uavc, U_imgC.Width / 2, U_imgC.Height / 2);
                    @U_imgC.RenderTransform = rotateTransformU_C;
                    RotateTransform rotateTransformU_A = new RotateTransform(90 - uava, U_imgA.Width / 2, U_imgA.Height / 2);
                    @U_imgA.RenderTransform = rotateTransformU_A;
                    RotateTransform rotateTransformU_B = new RotateTransform(90 - uavb, U_imgB.Width / 2, U_imgB.Height / 2);
                    @U_imgB.RenderTransform = rotateTransformU_B;
                    RotateTransform rotateTransformU_c = new RotateTransform(90 - uaac, U_imgc.Width / 2, U_imgc.Height / 2);
                    @U_imgc.RenderTransform = rotateTransformU_c;
                    RotateTransform rotateTransformU_a = new RotateTransform(90 - uaaa, U_imga.Width / 2, U_imga.Height / 2);
                    @U_imga.RenderTransform = rotateTransformU_a;
                    RotateTransform rotateTransformU_b = new RotateTransform(90 - uaab, U_imgb.Width / 2, U_imgb.Height / 2);
                    @U_imgb.RenderTransform = rotateTransformU_b;
                    Usuario_Lienzo.Children.Add(xs);
                    Usuario_Lienzo.Children.Add(ys);
                    Usuario_Lienzo.Children.Add(VA);
                    Usuario_Lienzo.Children.Add(VB);
                    Usuario_Lienzo.Children.Add(VC);
                    Usuario_Lienzo.Children.Add(CA);
                    Usuario_Lienzo.Children.Add(CB);
                    Usuario_Lienzo.Children.Add(CC);
                    Usuario_Lienzo.Children.Add(@U_imgC);
                    Usuario_Lienzo.Children.Add(@U_imgB);
                    Usuario_Lienzo.Children.Add(@U_imgA);
                    Usuario_Lienzo.Children.Add(@U_imgc);
                    Usuario_Lienzo.Children.Add(@U_imgb);
                    Usuario_Lienzo.Children.Add(@U_imga);
                    Txt_Diagnostico.Text = "";
                    double[] potencia_dis = Potencias(va, vb, vc, aa, ab, ac, ava, avb, avc, aaa, aab, aac, 1, 1);
                    double TC = Convert.ToDouble(Txt_TC.Text);
                    double TP = Convert.ToDouble(Txt_TP.Text);
                    double[] potencia_u = Potencias(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, TC, TP);
                    KW_A.Text = potencia_u[0].ToString();
                    KW_B.Text = potencia_u[1].ToString();
                    KW_C.Text = potencia_u[2].ToString();
                    FP_A.Text = potencia_u[3].ToString();
                    FP_B.Text = potencia_u[4].ToString();
                    FP_C.Text = potencia_u[5].ToString();
                    double EA = Math.Floor((potencia_u[0] / potencia_dis[0]) * 100) / 100;
                    double EB = Math.Floor((potencia_u[1] / potencia_dis[1]) * 100) / 100;
                    double EC = Math.Floor((potencia_u[2] / potencia_dis[2]) * 100) / 100;
                    Eficiencia_A.Text = EA.ToString() + "%";
                    Eficiencia_B.Text = EB.ToString() + "%";
                    Eficiencia_C.Text = EC.ToString() + "%";

                        Archivo("Lectura", Medidor.Text, Voltaje_A.Text, Voltaje_B.Text, Voltaje_C.Text, Corriente_A.Text, Corriente_B.Text, Corriente_C.Text, Usuario_Voltaje_A.Text, Usuario_Voltaje_B.Text, Usuario_Voltaje_C.Text, Usuario_Corriente_A.Text, Usuario_Corriente_B.Text, Usuario_Corriente_C.Text, Usuario_Angulo_Voltaje_A.Text, Usuario_Angulo_Voltaje_B.Text, Usuario_Angulo_Voltaje_C.Text, Usuario_Angulo_Corriente_A.Text, Usuario_Angulo_Corriente_B.Text, Usuario_Angulo_Corriente_C.Text, KW_A.Text, KW_B.Text, KW_C.Text, FP_A.Text, FP_B.Text, FP_C.Text);



                        if (uva < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente no este conectada" + "\r\n";
                    }
                    if (uvb < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente no este conectada" + "\r\n";
                    }
                    if (uvc < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente no este conectada" + "\r\n";
                    }
                    if (uava >= 45 && uava <= 315)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente esta invertida" + "\r\n";
                    }
                    if (uavb >= 165 || uavb <= 85)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente esta invertida" + "\r\n";
                    }
                    if (uavc >= 285 || uavc <= 200)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente esta invertida" + "\r\n";
                    }
                    if (uaaa >= 90 && uaaa <= 270)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase A posiblemente esta invertido" + "\r\n";
                    }
                    if (uaac >= 300 || uaac <= 180)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase C posiblemente esta invertido" + "\r\n";
                    }
                    if (uaab >= 210 || uaab <= 60)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase B posiblemente esta invertido" + "\r\n";
                    }
                    if (uaa <= (aa * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase A" + "\r\n";
                    }
                    if (uab <= (ab * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase B" + "\r\n";
                    }
                    if (uac <= (ac * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase C" + "\r\n";
                    }
                    if (uva <= 108 || uva >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase A" + "\r\n";
                    }
                    if (uvb <= 108 || uvb >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase B" + "\r\n";
                    }
                    if (uvc <= 108 || uvc >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase C" + "\r\n";
                    }
                    if (uva >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase A" + "\r\n";
                    }
                    if (uvb >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase B" + "\r\n";
                    }
                    if (uvc >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase C" + "\r\n";
                    }
                    if (uva <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase A" + "\r\n";
                    }
                    if (uvb <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase B" + "\r\n";
                    }
                    if (uvc <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase C" + "\r\n";
                    }

                    serialPort.Close();

                }
                else
                {

                    MessageBox.Show("La contraseña no es");
                    serialPort.Close();
                }
            }
                Btn_Leer.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                serialPort.Close();
                Btn_Leer.IsEnabled = true;
            }
        }

        private void Btn_Dibujar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Btn_Dibujar.IsEnabled = false;
                va = Convert.ToDouble(Voltaje_A.Text);
                vb = Convert.ToDouble(Voltaje_B.Text);
                vc = Convert.ToDouble(Voltaje_C.Text);
                aa = Convert.ToDouble(Corriente_A.Text);
                ab = Convert.ToDouble(Corriente_B.Text);
                ac = Convert.ToDouble(Corriente_C.Text);
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                aab = Convert.ToDouble(Angulo_Corriente_B.Text);
                aac = Convert.ToDouble(Angulo_Corriente_C.Text);
                uva = Convert.ToDouble(Usuario_Voltaje_A.Text);
                uvb = Convert.ToDouble(Usuario_Voltaje_B.Text);
                uvc = Convert.ToDouble(Usuario_Voltaje_C.Text);
                uaa = Convert.ToDouble(Usuario_Corriente_A.Text);
                uab = Convert.ToDouble(Usuario_Corriente_B.Text);
                uac = Convert.ToDouble(Usuario_Corriente_C.Text);
                uava = Convert.ToDouble(Usuario_Angulo_Voltaje_A.Text);
                uavb = Convert.ToDouble(Usuario_Angulo_Voltaje_B.Text);
                uavc = Convert.ToDouble(Usuario_Angulo_Voltaje_C.Text);
                uaaa = Convert.ToDouble(Usuario_Angulo_Corriente_A.Text);
                uaab = Convert.ToDouble(Usuario_Angulo_Corriente_B.Text);
                uaac = Convert.ToDouble(Usuario_Angulo_Corriente_C.Text);
                Usuario_Lienzo.Children.Clear();
                if (uva >= 0 && uva <= U_Vmax)
                {
                    if (uvb >= 0 && uvb <= U_Vmax)
                    {
                        if (uvc >= 0 && uvc <= U_Vmax)
                        {
                            if (uaa >= 0 && uaa <= U_Imax)
                            {
                                if (uab >= 0 && uab <= U_Imax)
                                {
                                    if (uac >= 0 && uac <= U_Imax)
                                    {
                                        if (uava <= 360 && uava >= 0)
                                        {
                                            if (uavb <= 360 && uavb >= 0)
                                            {
                                                if (uavc <= 360 && uavc >= 0)
                                                {
                                                    if (uaaa <= 360 && uaaa >= 0)
                                                    {
                                                        if (uaab <= 360 && uaab >= 0)
                                                        {
                                                            if (uaac <= 360 && uaac >= 0)
                                                            {
                                                                Ellipse contorno = new Ellipse();
                                                                contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                                                                contorno.StrokeThickness = 1;
                                                                contorno.Height = Usuario_Lienzo.ActualHeight;
                                                                contorno.Width = Usuario_Lienzo.ActualWidth;
                                                                contorno.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                                                                @U_base.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                                                                Usuario_Lienzo.Children.Add(@U_base);
                                                                Usuario_Lienzo.Children.Add(contorno);
                                                                Line VA = new Line();
                                                                Line VB = new Line();
                                                                Line VC = new Line();
                                                                Line CA = new Line();
                                                                Line CB = new Line();
                                                                Line CC = new Line();
                                                                Line xs = new Line();
                                                                Line ys = new Line();
                                                                double midx = Usuario_Lienzo.ActualHeight / 2;
                                                                double midy = Usuario_Lienzo.ActualWidth / 2;
                                                                VA.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VA.StrokeThickness = 4;
                                                                VB.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VB.StrokeThickness = 4;
                                                                VC.Stroke = System.Windows.Media.Brushes.Blue;
                                                                VC.StrokeThickness = 4;
                                                                CA.Stroke = System.Windows.Media.Brushes.Red;
                                                                CA.StrokeThickness = 2;
                                                                CB.Stroke = System.Windows.Media.Brushes.Red;
                                                                CB.StrokeThickness = 2;
                                                                CC.Stroke = System.Windows.Media.Brushes.Red;
                                                                CC.StrokeThickness = 2;
                                                                xs.Stroke = System.Windows.Media.Brushes.Black;
                                                                xs.StrokeThickness = 0.5;
                                                                ys.Stroke = System.Windows.Media.Brushes.Black;
                                                                ys.StrokeThickness = 0.5;
                                                                xs.X1 = 0;
                                                                xs.X2 = Usuario_Lienzo.ActualHeight;
                                                                xs.Y1 = midy;
                                                                xs.Y2 = midy;
                                                                ys.X1 = midx;
                                                                ys.X2 = midx;
                                                                ys.Y1 = 0;
                                                                ys.Y2 = Usuario_Lienzo.ActualWidth;
                                                                VA.X1 = midx;
                                                                VA.Y1 = midy;
                                                                VB.X1 = midx;
                                                                VB.Y1 = midy;
                                                                VC.X1 = midx;
                                                                VC.Y1 = midy;
                                                                CA.X1 = midx;
                                                                CA.Y1 = midy;
                                                                CB.X1 = midx;
                                                                CB.Y1 = midy;
                                                                CC.X1 = midx;
                                                                CC.Y1 = midy;
                                                                double[] punto = Puntos(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac,U_Vmax,U_Imax);
                                                                VA.X2 = midx + punto[0];
                                                                VA.Y2 = midy + (punto[1] * (-1));
                                                                VB.X2 = midx + punto[2];
                                                                VB.Y2 = midy + (punto[3] * (-1));
                                                                VC.X2 = midx + punto[4];
                                                                VC.Y2 = midy + (punto[5] * (-1));
                                                                CA.X2 = midx + punto[6];
                                                                CA.Y2 = midy + (punto[7] * (-1));
                                                                CB.X2 = midx + punto[8];
                                                                CB.Y2 = midy + (punto[9] * (-1));
                                                                CC.X2 = midx + punto[10];
                                                                CC.Y2 = midy + (punto[11] * (-1));
                                                                RotateTransform rotateTransformU_C = new RotateTransform(90 - uavc, U_imgC.Width / 2, U_imgC.Height / 2);
                                                                @U_imgC.RenderTransform = rotateTransformU_C;
                                                                RotateTransform rotateTransformU_A = new RotateTransform(90 - uava, U_imgA.Width / 2, U_imgA.Height / 2);
                                                                @U_imgA.RenderTransform = rotateTransformU_A;
                                                                RotateTransform rotateTransformU_B = new RotateTransform(90 - uavb, U_imgB.Width / 2, U_imgB.Height / 2);
                                                                @U_imgB.RenderTransform = rotateTransformU_B;
                                                                RotateTransform rotateTransformU_c = new RotateTransform(90 - uaac, U_imgc.Width / 2, U_imgc.Height / 2);
                                                                @U_imgc.RenderTransform = rotateTransformU_c;
                                                                RotateTransform rotateTransformU_a = new RotateTransform(90 - uaaa, U_imga.Width / 2, U_imga.Height / 2);
                                                                @U_imga.RenderTransform = rotateTransformU_a;
                                                                RotateTransform rotateTransformU_b = new RotateTransform(90 - uaab, U_imgb.Width / 2, U_imgb.Height / 2);
                                                                @U_imgb.RenderTransform = rotateTransformU_b;
                                                                Usuario_Lienzo.Children.Add(xs);
                                                                Usuario_Lienzo.Children.Add(ys);
                                                                Usuario_Lienzo.Children.Add(VA);
                                                                Usuario_Lienzo.Children.Add(VB);
                                                                Usuario_Lienzo.Children.Add(VC);
                                                                Usuario_Lienzo.Children.Add(CA);
                                                                Usuario_Lienzo.Children.Add(CB);
                                                                Usuario_Lienzo.Children.Add(CC);
                                                                Usuario_Lienzo.Children.Add(@U_imgC);
                                                                Usuario_Lienzo.Children.Add(@U_imgB);
                                                                Usuario_Lienzo.Children.Add(@U_imgA);
                                                                Usuario_Lienzo.Children.Add(@U_imgc);
                                                                Usuario_Lienzo.Children.Add(@U_imgb);
                                                                Usuario_Lienzo.Children.Add(@U_imga);
                                                                Txt_Diagnostico.Text = "";
                                                                double[] potencia_dis = Potencias(va, vb, vc, aa, ab, ac, ava, avb, avc, aaa, aab, aac, 1, 1);
                                                                double TC = Convert.ToDouble(Txt_TC.Text);
                                                                double TP = Convert.ToDouble(Txt_TP.Text);
                                                                double[] potencia_u = Potencias(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, TC, TP);
                                                                KW_A.Text = potencia_u[0].ToString();
                                                                KW_B.Text = potencia_u[1].ToString();
                                                                KW_C.Text = potencia_u[2].ToString();
                                                                FP_A.Text = potencia_u[3].ToString();
                                                                FP_B.Text = potencia_u[4].ToString();
                                                                FP_C.Text = potencia_u[5].ToString();
                                                                double EA = Math.Floor((potencia_u[0] / potencia_dis[0]) * 100) / 100;
                                                                double EB = Math.Floor((potencia_u[1] / potencia_dis[1]) * 100) / 100;
                                                                double EC = Math.Floor((potencia_u[2] / potencia_dis[2]) * 100) / 100;
                                                                Eficiencia_A.Text = EA.ToString() + "%";
                                                                Eficiencia_B.Text = EB.ToString() + "%";
                                                                Eficiencia_C.Text = EC.ToString() + "%";

                                                                Archivo("Manual", Medidor.Text, Voltaje_A.Text, Voltaje_B.Text, Voltaje_C.Text, Corriente_A.Text, Corriente_B.Text, Corriente_C.Text, Usuario_Voltaje_A.Text, Usuario_Voltaje_B.Text, Usuario_Voltaje_C.Text, Usuario_Corriente_A.Text, Usuario_Corriente_B.Text, Usuario_Corriente_C.Text, Usuario_Angulo_Voltaje_A.Text, Usuario_Angulo_Voltaje_B.Text, Usuario_Angulo_Voltaje_C.Text, Usuario_Angulo_Corriente_A.Text, Usuario_Angulo_Corriente_B.Text, Usuario_Angulo_Corriente_C.Text, KW_A.Text, KW_B.Text, KW_C.Text, FP_A.Text, FP_B.Text, FP_C.Text);

                                                                if (uva < 1)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente no este conectada" + "\r\n";
                                                                }
                                                                if (uvb < 1)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente no este conectada" + "\r\n";
                                                                }
                                                                if (uvc < 1)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente no este conectada" + "\r\n";
                                                                }
                                                                if (uava >= 45 && uava <= 315)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente esta invertida" + "\r\n";
                                                                }
                                                                if (uavb >= 165 || uavb <= 85)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente esta invertida" + "\r\n";
                                                                }
                                                                if (uavc >= 285 || uavc <= 200)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente esta invertida" + "\r\n";
                                                                }
                                                                if (uaaa >= 90 && uaaa <= 270)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase A posiblemente esta invertido" + "\r\n";
                                                                }
                                                                if (uaac >= 300 || uaac <= 180)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase C posiblemente esta invertido" + "\r\n";
                                                                }
                                                                if (uaab >= 210 || uaab <= 60)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase B posiblemente esta invertido" + "\r\n";
                                                                }
                                                                if ((uaa*TC) <= (aa * .6))
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase A" + "\r\n";
                                                                }
                                                                if ((uab*TC) <= (ab * .6))
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase B" + "\r\n";
                                                                }
                                                                if ((uac*TC) <= (ac * .6))
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase C" + "\r\n";
                                                                }
                                                                if (uva <= 108 || uva >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase A" + "\r\n";
                                                                }
                                                                if (uvb <= 108 || uvb >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase B" + "\r\n";
                                                                }
                                                                if (uvc <= 108 || uvc >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase C" + "\r\n";
                                                                }
                                                                if ((uva*TP) >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase A" + "\r\n";
                                                                }
                                                                if ((uvb*TP) >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase B" + "\r\n";
                                                                }
                                                                if ((uvc*TP) >= U_Vmax)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase C" + "\r\n";
                                                                }
                                                                if (uva <= 50)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase A" + "\r\n";
                                                                }
                                                                if (uvb <= 50)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase B" + "\r\n";
                                                                }
                                                                if (uvc <= 50)
                                                                {
                                                                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase C" + "\r\n";
                                                                }

                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Error: Valor de angulo corriente Fase C incorrecto");
                                                                Usuario_Angulo_Corriente_C.Focus();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Error: Valor de angulo corriente Fase B incorrecto");
                                                            Usuario_Angulo_Corriente_B.Focus();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Error: Valor de angulo corriente Fase A incorrecto");
                                                        Usuario_Angulo_Corriente_A.Focus();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Error: Valor de angulo voltaje Fase C incorrecto");
                                                    Usuario_Angulo_Voltaje_C.Focus();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Error: Valor de angulo voltaje Fase B incorrecto");
                                                Usuario_Angulo_Voltaje_B.Focus();
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error: Valor de angulo voltaje Fase A incorrecto");
                                            Usuario_Angulo_Voltaje_A.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error: Valor de corriente Fase C incorrecto");
                                        Usuario_Corriente_C.Focus();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Error: Valor de corriente Fase B incorrecto");
                                    Usuario_Corriente_B.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error: Valor de corriente Fase A incorrecto");
                                Usuario_Corriente_A.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error: Valor de voltaje Fase C incorrecto");
                            Usuario_Voltaje_C.Focus();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Error: Valor de voltaje Fase B incorrecto");
                        Usuario_Voltaje_B.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Error: Valor de voltaje Fase A incorrecto");
                    Usuario_Voltaje_A.Focus();
                }

                Btn_Dibujar.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_Dibujar.IsEnabled = true;
            }
        }

        private void Cbo_Anomalia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try { 
                string falla = "";
                falla = cbo_Anomalia.Items.GetItemAt(cbo_Anomalia.SelectedIndex).ToString();
                va = Convert.ToDouble(Voltaje_A.Text);
                vb = Convert.ToDouble(Voltaje_B.Text);
                vc = Convert.ToDouble(Voltaje_C.Text);
                aa = Convert.ToDouble(Corriente_A.Text);
                ab = Convert.ToDouble(Corriente_B.Text);
                ac = Convert.ToDouble(Corriente_C.Text);
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                aab = Convert.ToDouble(Angulo_Corriente_B.Text);
                aac = Convert.ToDouble(Angulo_Corriente_C.Text);
                double uva = va;
                double uvb = vb;
                double uvc = vc;
                double uaa = aa;
                double uab = ab;
                double uac = ac;
                double uava = ava;
                double uavb = avb;
                double uavc = avc;
                double uaaa = aaa;
                double uaab = aab;
                double uaac = aac;
                Usuario_Lienzo.Children.Clear();
                Ellipse contorno = new Ellipse();
                contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                contorno.StrokeThickness = 1;
                contorno.Height = Usuario_Lienzo.ActualHeight;
                contorno.Width = Usuario_Lienzo.ActualWidth;
                contorno.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                @U_base.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                Usuario_Lienzo.Children.Add(@U_base);
                Usuario_Lienzo.Children.Add(contorno);
                Line VA = new Line();
                Line VB = new Line();
                Line VC = new Line();
                Line CA = new Line();
                Line CB = new Line();
                Line CC = new Line();
                Line xs = new Line();
                Line ys = new Line();
                double midx = Usuario_Lienzo.ActualHeight / 2;
                double midy = Usuario_Lienzo.ActualWidth / 2;
                VA.Stroke = System.Windows.Media.Brushes.Blue;
                VA.StrokeThickness = 4;
                VB.Stroke = System.Windows.Media.Brushes.Blue;
                VB.StrokeThickness = 4;
                VC.Stroke = System.Windows.Media.Brushes.Blue;
                VC.StrokeThickness = 4;
                CA.Stroke = System.Windows.Media.Brushes.Red;
                CA.StrokeThickness = 2;
                CB.Stroke = System.Windows.Media.Brushes.Red;
                CB.StrokeThickness = 2;
                CC.Stroke = System.Windows.Media.Brushes.Red;
                CC.StrokeThickness = 2;
                xs.Stroke = System.Windows.Media.Brushes.Black;
                xs.StrokeThickness = 0.5;
                ys.Stroke = System.Windows.Media.Brushes.Black;
                ys.StrokeThickness = 0.5;
                xs.X1 = 0;
                xs.X2 = Usuario_Lienzo.ActualHeight;
                xs.Y1 = midy;
                xs.Y2 = midy;
                ys.X1 = midx;
                ys.X2 = midx;
                ys.Y1 = 0;
                ys.Y2 = Usuario_Lienzo.ActualWidth;
                VA.X1 = midx;
                VA.Y1 = midy;
                VB.X1 = midx;
                VB.Y1 = midy;
                VC.X1 = midx;
                VC.Y1 = midy;
                CA.X1 = midx;
                CA.Y1 = midy;
                CB.X1 = midx;
                CB.Y1 = midy;
                CC.X1 = midx;
                CC.Y1 = midy;
            if (falla == "Fase A Desconectada")
            {
                uva = 0;
                uava = 0;
            }
            if (falla == "Fase B Desconectada")
            {
                uvb = 0;
                uavb = 0;
            }
            if (falla == "Fase C Desconectada")
            {
                uvc = 0;
                uavc = 0;
            }
            if (falla == "Fase A Invertida")
            {
                uava = ava + 180;
                    if (uava > 360)
                    {
                        uava = uava - 360;
                    }
            }
            if (falla == "Fase C Invertida")
            {
                uavc = avc + 180;
                    if (uavc > 360)
                    {
                        uavc = uavc - 360;
                    }
                }
            if (falla == "Fase B Invertida")
            {
                uavb = avb + 180;
                    if (uavb > 360)
                    {
                        uavb = uavb - 360;
                    }
                }
            if (falla == "TC de Fase A Invertido")
            {
                uaaa = aaa  + 180;
                    if (uaaa > 360)
                    {
                        uaaa = uaaa - 360;
                    }
                }
            if (falla == "TC de Fase C Invertido")
            {
                uaac = aac + 180;
                if (uaac > 360)
                {
                    uaac = uaac - 360;
                }
            }
            if (falla == "TC de Fase B Invertido")
            {
                uaab = aab + 180;
                if (uaab > 360)
                {
                    uaab = uaab - 360;
                }
            }
            if (falla == "Puente en la Fase A")
            {
                uaa = aa /2;
            }
            if (falla == "Puente en la Fase C")
            {
                uac = ac / 2;
            }
            if (falla == "Puente en la Fase B")
            {
                uab = ab / 2;
            }
            if (falla == "Sobrevoltaje en la Fase A")
            {
                uva = U_Vmax;
            }
            if (falla == "Sobrevoltaje en la Fase B")
            {
                uvb = U_Vmax;
            }
            if (falla == "Sobrevoltaje en la Fase C")
            {
                uvc = U_Vmax;
            }
            if (falla == "Bajo voltaje en la Fase A")
            {
                uva = 50;
            }
            if (falla == "Bajo voltaje en la Fase B")
            {
                uvb = 50;
            }
            if (falla == "Bajo voltaje en la Fase C")
            {
                uvc = 50;
            }

                Usuario_Voltaje_A.Text = uva.ToString();
                Usuario_Voltaje_B.Text = uvb.ToString();
                Usuario_Voltaje_C.Text = uvc.ToString();
                Usuario_Corriente_A.Text = uaa.ToString();
                Usuario_Corriente_B.Text = uab.ToString();
                Usuario_Corriente_C.Text = uac.ToString();
                Usuario_Angulo_Voltaje_A.Text = uava.ToString();
                Usuario_Angulo_Voltaje_B.Text = uavb.ToString();
                Usuario_Angulo_Voltaje_C.Text = uavc.ToString();
                Usuario_Angulo_Corriente_A.Text = uaaa.ToString();
                Usuario_Angulo_Corriente_B.Text = uaab.ToString();
                Usuario_Angulo_Corriente_C.Text = uaac.ToString();
                double[] punto = Puntos(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac,U_Vmax,U_Imax);
                VA.X2 = midx + punto[0];
                VA.Y2 = midy + (punto[1] * (-1));
                VB.X2 = midx + punto[2];
                VB.Y2 = midy + (punto[3] * (-1));
                VC.X2 = midx + punto[4];
                VC.Y2 = midy + (punto[5] * (-1));
                CA.X2 = midx + punto[6];
                CA.Y2 = midy + (punto[7] * (-1));
                CB.X2 = midx + punto[8];
                CB.Y2 = midy + (punto[9] * (-1));
                CC.X2 = midx + punto[10];
                CC.Y2 = midy + (punto[11] * (-1));
                RotateTransform rotateTransformU_C = new RotateTransform(90 - uavc, U_imgC.Width / 2, U_imgC.Height / 2);
                @U_imgC.RenderTransform = rotateTransformU_C;
                RotateTransform rotateTransformU_A = new RotateTransform(90 - uava, U_imgA.Width / 2, U_imgA.Height / 2);
                @U_imgA.RenderTransform = rotateTransformU_A;
                RotateTransform rotateTransformU_B = new RotateTransform(90 - uavb, U_imgB.Width / 2, U_imgB.Height / 2);
                @U_imgB.RenderTransform = rotateTransformU_B;
                RotateTransform rotateTransformU_c = new RotateTransform(90 - uaac, U_imgc.Width / 2, U_imgc.Height / 2);
                @U_imgc.RenderTransform = rotateTransformU_c;
                RotateTransform rotateTransformU_a = new RotateTransform(90 - uaaa, U_imga.Width / 2, U_imga.Height / 2);
                @U_imga.RenderTransform = rotateTransformU_a;
                RotateTransform rotateTransformU_b = new RotateTransform(90 - uaab, U_imgb.Width / 2, U_imgb.Height / 2);
                @U_imgb.RenderTransform = rotateTransformU_b;
                Usuario_Lienzo.Children.Add(xs);
                Usuario_Lienzo.Children.Add(ys);
                Usuario_Lienzo.Children.Add(VA);
                Usuario_Lienzo.Children.Add(VB);
                Usuario_Lienzo.Children.Add(VC);
                Usuario_Lienzo.Children.Add(CA);
                Usuario_Lienzo.Children.Add(CB);
                Usuario_Lienzo.Children.Add(CC);
                Usuario_Lienzo.Children.Add(@U_imgC);
                Usuario_Lienzo.Children.Add(@U_imgB);
                Usuario_Lienzo.Children.Add(@U_imgA);
                Usuario_Lienzo.Children.Add(@U_imgc);
                Usuario_Lienzo.Children.Add(@U_imgb);
                Usuario_Lienzo.Children.Add(@U_imga);
                double[] potencia_dis = Potencias(va, vb, vc, aa, ab, ac, ava, avb, avc, aaa, aab, aac, 1, 1);
                double TC = Convert.ToDouble(Txt_TC.Text);
                double TP = Convert.ToDouble(Txt_TP.Text);
                double[] potencia_u = Potencias(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, TC, TP);
                KW_A.Text = potencia_u[0].ToString();
                KW_B.Text = potencia_u[1].ToString();
                KW_C.Text = potencia_u[2].ToString();
                FP_A.Text = potencia_u[3].ToString();
                FP_B.Text = potencia_u[4].ToString();
                FP_C.Text = potencia_u[5].ToString();
                double EA = Math.Floor((potencia_u[0] / potencia_dis[0]) * 100) / 100;
                double EB = Math.Floor((potencia_u[1] / potencia_dis[1]) * 100) / 100;
                double EC = Math.Floor((potencia_u[2] / potencia_dis[2]) * 100) / 100;
                Eficiencia_A.Text = EA.ToString() + "%";
                Eficiencia_B.Text = EB.ToString() + "%";
                Eficiencia_C.Text = EC.ToString() + "%";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        
        }
        private void Cbo_Puerto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Btn_Leer.Focus();
        }

        private void Voltaje_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                va = Convert.ToDouble(Voltaje_A.Text);
                if (va<0 || va > Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase A");
                    Voltaje_A.Focus();
                }
                else
                {
                    Voltaje_B.Focus();
                }
            }
        }

        private void Voltaje_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                vc = Convert.ToDouble(Voltaje_C.Text);
                if (vc < 0 || vc > Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase C");
                    Voltaje_C.Focus();
                }
                else
                {
                    Angulo_Voltaje_A.Focus();
                }
            }
        }

        private void Voltaje_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                vb = Convert.ToDouble(Voltaje_B.Text);
                if (vb < 0 || vb > Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase B");
                    Voltaje_B.Focus();
                }
                else
                {

                    Voltaje_C.Focus();
                }
            }
        }

        private void Angulo_Voltaje_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                if (ava < 0 || ava > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase A");
                    Angulo_Voltaje_A.Focus();
                }
                else
                {
                    Angulo_Voltaje_B.Focus();
                }
            }
        }

        private void Angulo_Voltaje_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                if (avb < 0 || avb > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase B");
                    Angulo_Voltaje_B.Focus();
                }
                else
                {
                    Angulo_Voltaje_C.Focus();
                   
                }
            }
        }

        private void Angulo_Voltaje_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                if (avc < 0 || avc > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase C");
                    Angulo_Voltaje_C.Focus();
                }
                else
                {
                    Corriente_A.Focus();
                }
            }
        }

        private void Corriente_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                aa = Convert.ToDouble(Corriente_A.Text);
                if (aa < 0 || aa > Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase A");
                    Corriente_A.Focus();
                }
                else
                {
                    Corriente_B.Focus();
                }
            }
        }

        private void Corriente_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ab = Convert.ToDouble(Corriente_B.Text);
                if (ab < 0 || ab > Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase B");
                    Corriente_B.Focus();
                }
                else
                {
                    Corriente_C.Focus();
                }
            }
        }

        private void Corriente_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ac = Convert.ToDouble(Corriente_C.Text);
                if (ac < 0 || ac > Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase C");
                    Corriente_C.Focus();
                }
                else
                {

                    if (h == 0)
                    {
                        Angulo_Corriente_A.Focus();
                    }
                    else if (h == 1)
                    {
                        Btn_calcular.Focus();
                    }
                }
            }
        }

        private void Angulo_Corriente_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                if (aaa < 0 || aaa > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase A");
                    Angulo_Corriente_A.Focus();
                }
                else
                {
                    Angulo_Corriente_B.Focus();
                }
            }
        }

        private void Angulo_Corriente_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                aab = Convert.ToDouble(Angulo_Corriente_B.Text);
                if (aab < 0 || aab > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase B");
                    Angulo_Corriente_B.Focus();
                }
                else
                {
                    Angulo_Corriente_C.Focus();
                }
            }
        }

        private void Angulo_Corriente_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                aac = Convert.ToDouble(Angulo_Corriente_C.Text);
                if (aac < 0 || aac > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase C");
                    Angulo_Corriente_C.Focus();
                }
                else
                {

                    Btn_calcular.Focus();
                }
            }
        }

        private void Usuario_Voltaje_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uva = Convert.ToDouble(Usuario_Voltaje_A.Text);
                if (uva < 0 || uva > U_Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase A");
                    Usuario_Voltaje_A.Focus();
                }
                else
                {
                    Usuario_Voltaje_B.Focus();
                }
            }
        }

        private void Usuario_Voltaje_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uvb = Convert.ToDouble(Usuario_Voltaje_B.Text);
                if (uvb < 0 || uvb > U_Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase B");
                    Usuario_Voltaje_B.Focus();
                }
                else
                {
                    Usuario_Voltaje_C.Focus();
                }
            }
        }

        private void Usuario_Voltaje_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uvc = Convert.ToDouble(Usuario_Voltaje_C.Text);
                if (uvc < 0 || uvc > U_Vmax)
                {
                    MessageBox.Show("Ingrese un valor valido en Voltaje Fase C");
                    Usuario_Voltaje_C.Focus();
                }
                else
                {

                    Usuario_Angulo_Voltaje_A.Focus();
                }
            }
        }

        private void Usuario_Angulo_Voltaje_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uava = Convert.ToDouble(Usuario_Angulo_Voltaje_A.Text);
                if (uava < 0 || uava > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase A");
                    Usuario_Angulo_Voltaje_A.Focus();
                }
                else
                {
                    Usuario_Angulo_Voltaje_B.Focus();
                }
            }
        }

        private void Usuario_Angulo_Voltaje_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uavb = Convert.ToDouble(Usuario_Angulo_Voltaje_B.Text);
                if (uavb < 0 || uavb > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase B");
                    Usuario_Angulo_Voltaje_B.Focus();
                }
                else
                {
                    Usuario_Angulo_Voltaje_C.Focus();
                }
            }
        }

        private void Usuario_Angulo_Voltaje_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uavc = Convert.ToDouble(Usuario_Angulo_Voltaje_C.Text);
                if (uavc < 0 || uavc > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Voltaje Fase C");
                    Usuario_Angulo_Voltaje_C.Focus();
                }
                else
                {

                    Usuario_Corriente_A.Focus();
                }
            }
        }

        private void Usuario_Corriente_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uaa = Convert.ToDouble(Usuario_Corriente_A.Text);
                if (uaa < 0 || uaa > U_Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase A");
                    Usuario_Corriente_A.Focus();
                }
                else
                {
                    Usuario_Corriente_B.Focus();
                }
            }
        }

        private void Usuario_Corriente_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uab = Convert.ToDouble(Usuario_Corriente_B.Text);
                if (uab < 0 || uab > U_Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase B");
                    Usuario_Corriente_B.Focus();
                }
                else
                {
                    Usuario_Corriente_C.Focus();
                }
            }
        }

        private void Usuario_Corriente_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uac = Convert.ToDouble(Usuario_Corriente_C.Text);
                if (uac < 0 || uac > U_Imax)
                {
                    MessageBox.Show("Ingrese un valor valido en Corriente Fase C");
                    Usuario_Corriente_C.Focus();
                }
                else
                {

                    Usuario_Angulo_Corriente_A.Focus();
                }
            }
        }

        private void Usuario_Angulo_Corriente_A_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uaaa = Convert.ToDouble(Usuario_Angulo_Corriente_A.Text);
                if (uaaa < 0 || uaaa > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase A");
                    Usuario_Angulo_Corriente_A.Focus();
                }
                else
                {
                    Usuario_Angulo_Corriente_B.Focus();
                }
            }
        }

        private void Usuario_Angulo_Corriente_B_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uaab = Convert.ToDouble(Usuario_Angulo_Corriente_B.Text);
                if (uaab < 0 || uaab > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase B");
                    Usuario_Angulo_Corriente_B.Focus();
                }
                else
                {
                    Usuario_Angulo_Corriente_C.Focus();
                }
            }
        }

        private void Usuario_Angulo_Corriente_C_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                uaac = Convert.ToDouble(Usuario_Angulo_Corriente_C.Text);
                if (uaac < 0 || uaac > 360)
                {
                    MessageBox.Show("Ingrese un valor valido en Angulo Corriente Fase C");
                    Usuario_Angulo_Corriente_C.Focus();
                }
                else
                {

                    Btn_Dibujar.Focus();
                }
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
       private void Btn_File_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Btn_File.IsEnabled = false;
                va = Convert.ToDouble(Voltaje_A.Text);
                vb = Convert.ToDouble(Voltaje_B.Text);
                vc = Convert.ToDouble(Voltaje_C.Text);
                aa = Convert.ToDouble(Corriente_A.Text);
                ab = Convert.ToDouble(Corriente_B.Text);
                ac = Convert.ToDouble(Corriente_C.Text);
                ava = Convert.ToDouble(Angulo_Voltaje_A.Text);
                avb = Convert.ToDouble(Angulo_Voltaje_B.Text);
                avc = Convert.ToDouble(Angulo_Voltaje_C.Text);
                aaa = Convert.ToDouble(Angulo_Corriente_A.Text);
                aab = Convert.ToDouble(Angulo_Corriente_B.Text);
                aac = Convert.ToDouble(Angulo_Corriente_C.Text);
                DatosMSR datosMSR = new DatosMSR();
                LectorMSR lectorMSR = new LectorMSR();
                datosMSR= lectorMSR.obtenDatosMSR(@Txt_Path.Text);
                if(datosMSR.tInstantaneos.Count >200 )
                {
                    int d = 0;
                    int div = 0;
                    byte [] datos = datosMSR.tInstantaneos.ToArray();
                    
                    if (datos.Length == 0xf0)
                    {
                        d = 0;
                    }
                    else if (datos.Length == 0xd0)
                    {
                        d = 32;
                    }
                    if (datosMSR.exactitud == "01")
                    {
                        div = 10000;
                    }
                    else if (datosMSR.exactitud == "02")
                    {
                        div = 1000;
                    }
                    
                    for (int j = 0; j < 6; j++)
                    {
                        VoltajeA[j] = datos[59 - j - d];
                        CorrienteA[j] = datos[65 - j - d];
                        VoltajeC[j] = datos[77 - j - d];
                        CorrienteC[j] = datos[83 - j - d];
                        AnguloVoltajeC[j] = datos[95 - j - d];
                        VoltajeB[j] = datos[101 - j - d];
                        CorrienteB[j] = datos[107 - j - d];
                        AnguloVoltajeB[j] = datos[119 - j - d];
                        AnguloCorrienteA[j] = datos[167 - j - d];
                        AnguloCorrienteB[j] = datos[173 - j - d];
                        AnguloCorrienteC[j] = datos[179 - j - d];
                    }
                    
                    AnguloVoltajeA = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    Medidor.Text = datosMSR.numeroMedidor;
                    Usuario_Voltaje_A.Text = BitConverter.ToString(VoltajeA, 0).Replace("-", string.Empty);
                    uva = (Int32.Parse(Usuario_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uva = uva / div;
                    Usuario_Voltaje_A.Text = uva.ToString();
                    Usuario_Voltaje_B.Text = BitConverter.ToString(VoltajeB, 0).Replace("-", string.Empty);
                    uvb = (Int32.Parse(Usuario_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uvb = uvb / div;
                    Usuario_Voltaje_B.Text = uvb.ToString();
                    Usuario_Voltaje_C.Text = BitConverter.ToString(VoltajeC).Replace("-", string.Empty); ;
                    uvc = (Int32.Parse(Usuario_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uvc = uvc / div;
                    Usuario_Voltaje_C.Text = uvc.ToString();
                    Usuario_Corriente_A.Text = BitConverter.ToString(CorrienteA).Replace("-", string.Empty); ;
                    uaa = (Int32.Parse(Usuario_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uaa = uaa / div;
                    Usuario_Corriente_A.Text = uaa.ToString();
                    Usuario_Corriente_B.Text = BitConverter.ToString(CorrienteB).Replace("-", string.Empty); ;
                    uab = (Int32.Parse(Usuario_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uab = uab / div;
                    Usuario_Corriente_B.Text = uab.ToString();
                    Usuario_Corriente_C.Text = BitConverter.ToString(CorrienteC).Replace("-", string.Empty); ;
                    uac = (Int32.Parse(Usuario_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uac = uac / div;
                    Usuario_Corriente_C.Text = uac.ToString();
                    Usuario_Angulo_Voltaje_A.Text = BitConverter.ToString(AnguloVoltajeA).Replace("-", string.Empty); ;
                    uava = (Int32.Parse(Usuario_Angulo_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uava = uava / div;
                    Usuario_Angulo_Voltaje_A.Text = uava.ToString();
                    Usuario_Angulo_Voltaje_B.Text = BitConverter.ToString(AnguloVoltajeB).Replace("-", string.Empty); ;
                    uavb = (Int32.Parse(Usuario_Angulo_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uavb = uavb / div;
                    Usuario_Angulo_Voltaje_B.Text = uavb.ToString();
                    Usuario_Angulo_Voltaje_C.Text = BitConverter.ToString(AnguloVoltajeC).Replace("-", string.Empty); ;
                    uavc = (Int32.Parse(Usuario_Angulo_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uavc = uavc / div;
                    Usuario_Angulo_Voltaje_C.Text = uavc.ToString();
                    Usuario_Angulo_Corriente_A.Text = BitConverter.ToString(AnguloCorrienteA).Replace("-", string.Empty); ;
                    uaaa = (Int32.Parse(Usuario_Angulo_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                    uaaa = uaaa / div;
                    Usuario_Angulo_Corriente_A.Text = uaaa.ToString();
                    Usuario_Angulo_Corriente_B.Text = BitConverter.ToString(AnguloCorrienteB).Replace("-", string.Empty); ;
                    uaab = (Int32.Parse(Usuario_Angulo_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                    uaab = uaab / div;
                    Usuario_Angulo_Corriente_B.Text = uaab.ToString();
                    Usuario_Angulo_Corriente_C.Text = BitConverter.ToString(AnguloCorrienteC).Replace("-", string.Empty); ;
                    uaac = (Int32.Parse(Usuario_Angulo_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                    uaac = uaac / div;
                    Usuario_Angulo_Corriente_C.Text = uaac.ToString();
                    Usuario_Lienzo.Children.Clear();
                    Ellipse contorno = new Ellipse();
                    contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                    contorno.StrokeThickness = 1;
                    contorno.Height = Usuario_Lienzo.ActualHeight;
                    contorno.Width = Usuario_Lienzo.ActualWidth;
                    contorno.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                    @U_base.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                    Usuario_Lienzo.Children.Add(@U_base);
                    Usuario_Lienzo.Children.Add(contorno);
                    Line VA = new Line();
                    Line VB = new Line();
                    Line VC = new Line();
                    Line CA = new Line();
                    Line CB = new Line();
                    Line CC = new Line();
                    Line xs = new Line();
                    Line ys = new Line();
                    double midx = Usuario_Lienzo.ActualHeight / 2;
                    double midy = Usuario_Lienzo.ActualWidth / 2;
                    VA.Stroke = System.Windows.Media.Brushes.Blue;
                    VA.StrokeThickness = 4;
                    VB.Stroke = System.Windows.Media.Brushes.Blue;
                    VB.StrokeThickness = 4;
                    VC.Stroke = System.Windows.Media.Brushes.Blue;
                    VC.StrokeThickness = 4;
                    CA.Stroke = System.Windows.Media.Brushes.Red;
                    CA.StrokeThickness = 2;
                    CB.Stroke = System.Windows.Media.Brushes.Red;
                    CB.StrokeThickness = 2;
                    CC.Stroke = System.Windows.Media.Brushes.Red;
                    CC.StrokeThickness = 2;
                    xs.Stroke = System.Windows.Media.Brushes.Black;
                    xs.StrokeThickness = 0.5;
                    ys.Stroke = System.Windows.Media.Brushes.Black;
                    ys.StrokeThickness = 0.5;
                    xs.X1 = 0;
                    xs.X2 = Usuario_Lienzo.ActualHeight;
                    xs.Y1 = midy;
                    xs.Y2 = midy;
                    ys.X1 = midx;
                    ys.X2 = midx;
                    ys.Y1 = 0;
                    ys.Y2 = Usuario_Lienzo.ActualWidth;
                    VA.X1 = midx;
                    VA.Y1 = midy;
                    VB.X1 = midx;
                    VB.Y1 = midy;
                    VC.X1 = midx;
                    VC.Y1 = midy;
                    CA.X1 = midx;
                    CA.Y1 = midy;
                    CB.X1 = midx;
                    CB.Y1 = midy;
                    CC.X1 = midx;
                    CC.Y1 = midy;
                    double[] punto = Puntos(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac,U_Vmax,U_Imax);
                    VA.X2 = midx + punto[0];
                    VA.Y2 = midy + (punto[1] * (-1));
                    VB.X2 = midx + punto[2];
                    VB.Y2 = midy + (punto[3] * (-1));
                    VC.X2 = midx + punto[4];
                    VC.Y2 = midy + (punto[5] * (-1));
                    CA.X2 = midx + punto[6];
                    CA.Y2 = midy + (punto[7] * (-1));
                    CB.X2 = midx + punto[8];
                    CB.Y2 = midy + (punto[9] * (-1));
                    CC.X2 = midx + punto[10];
                    CC.Y2 = midy + (punto[11] * (-1));
                    RotateTransform rotateTransformU_C = new RotateTransform(90 - uavc, U_imgC.Width / 2, U_imgC.Height / 2);
                    @U_imgC.RenderTransform = rotateTransformU_C;
                    RotateTransform rotateTransformU_A = new RotateTransform(90 - uava, U_imgA.Width / 2, U_imgA.Height / 2);
                    @U_imgA.RenderTransform = rotateTransformU_A;
                    RotateTransform rotateTransformU_B = new RotateTransform(90 - uavb, U_imgB.Width / 2, U_imgB.Height / 2);
                    @U_imgB.RenderTransform = rotateTransformU_B;
                    RotateTransform rotateTransformU_c = new RotateTransform(90 - uaac, U_imgc.Width / 2, U_imgc.Height / 2);
                    @U_imgc.RenderTransform = rotateTransformU_c;
                    RotateTransform rotateTransformU_a = new RotateTransform(90 - uaaa, U_imga.Width / 2, U_imga.Height / 2);
                    @U_imga.RenderTransform = rotateTransformU_a;
                    RotateTransform rotateTransformU_b = new RotateTransform(90 - uaab, U_imgb.Width / 2, U_imgb.Height / 2);
                    @U_imgb.RenderTransform = rotateTransformU_b;
                    Usuario_Lienzo.Children.Add(xs);
                    Usuario_Lienzo.Children.Add(ys);
                    Usuario_Lienzo.Children.Add(VA);
                    Usuario_Lienzo.Children.Add(VB);
                    Usuario_Lienzo.Children.Add(VC);
                    Usuario_Lienzo.Children.Add(CA);
                    Usuario_Lienzo.Children.Add(CB);
                    Usuario_Lienzo.Children.Add(CC);
                    Usuario_Lienzo.Children.Add(@U_imgC);
                    Usuario_Lienzo.Children.Add(@U_imgB);
                    Usuario_Lienzo.Children.Add(@U_imgA);
                    Usuario_Lienzo.Children.Add(@U_imgc);
                    Usuario_Lienzo.Children.Add(@U_imgb);
                    Usuario_Lienzo.Children.Add(@U_imga);
                    Txt_Diagnostico.Text = "";
                    double[] potencia_dis = Potencias(va, vb, vc, aa, ab, ac, ava, avb, avc, aaa, aab, aac, 1, 1);
                    double TC = Convert.ToDouble(Txt_TC.Text);
                    double TP = Convert.ToDouble(Txt_TP.Text);
                    double[] potencia_u = Potencias(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, TC, TP);
                    KW_A.Text = potencia_u[0].ToString();
                    KW_B.Text = potencia_u[1].ToString();
                    KW_C.Text = potencia_u[2].ToString();
                    FP_A.Text = potencia_u[3].ToString();
                    FP_B.Text = potencia_u[4].ToString();
                    FP_C.Text = potencia_u[5].ToString();
                    double EA = Math.Floor((potencia_u[0] / potencia_dis[0]) * 100) / 100;
                    double EB = Math.Floor((potencia_u[1] / potencia_dis[1]) * 100) / 100;
                    double EC = Math.Floor((potencia_u[2] / potencia_dis[2]) * 100) / 100;
                    Eficiencia_A.Text = EA.ToString() + "%";
                    Eficiencia_B.Text = EB.ToString() + "%";
                    Eficiencia_C.Text = EC.ToString() + "%";
                    if (uva < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente no este conectada" + "\r\n";
                    }
                    if (uvb < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente no este conectada" + "\r\n";
                    }
                    if (uvc < 1)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente no este conectada" + "\r\n";
                    }
                    if (uava >= 45 && uava <= 315)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase A posiblemente esta invertida" + "\r\n";
                    }
                    if (uavb >= 165 || uavb <= 85)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase B posiblemente esta invertida" + "\r\n";
                    }
                    if (uavc >= 285 || uavc <= 200)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "La Fase C posiblemente esta invertida" + "\r\n";
                    }
                    if (uaaa >= 90 && uaaa <= 270)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase A posiblemente esta invertido" + "\r\n";
                    }
                    if (uaac >= 300 || uaac <= 180)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase C posiblemente esta invertido" + "\r\n";
                    }
                    if (uaab >= 210 || uaab <= 60)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "TC de la Fase B posiblemente esta invertido" + "\r\n";
                    }
                    if (uaa <= (aa * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase A" + "\r\n";
                    }
                    if (uab <= (ab * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase B" + "\r\n";
                    }
                    if (uac <= (ac * .6))
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista un puente en la carga de Fase C" + "\r\n";
                    }
                    if (uva <= 108 || uva >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase A" + "\r\n";
                    }
                    if (uvb <= 108 || uvb >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase B" + "\r\n";
                    }
                    if (uvc <= 108 || uvc >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente error de red en Fase C" + "\r\n";
                    }
                    if (uva >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase A" + "\r\n";
                    }
                    if (uvb >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase B" + "\r\n";
                    }
                    if (uvc >= U_Vmax)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista sobrevoltaje en Fase C" + "\r\n";
                    }
                    if (uva <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase A" + "\r\n";
                    }
                    if (uvb <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase B" + "\r\n";
                    }
                    if (uvc <= 50)
                    {
                        Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Posiblemente exista bajo voltaje en Fase C" + "\r\n";
                    }
                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Numero Total Reset:" + datosMSR.numeroTotalResets + "\r\n";
                    if (datosMSR.medidorEditado == true)
                    {
                     //   Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Medidor editado" + "\r\n";
                    }
                    Txt_Diagnostico.Text = Txt_Diagnostico.Text + "Numero Total Apagones:" + datosMSR.contadorApagones + "\r\n";
                }
                else
                {
                    MessageBox.Show("No se leyo bien el archivo" );
                }
                Btn_File.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_File.IsEnabled = true;
            }
            
        }
        private void COD_MED_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            name = cbo_COD_MED.Items.GetItemAt(cbo_COD_MED.SelectedIndex).ToString();
            if (name == "KXXX")
            {
                U_Vmax = 500;
                U_Imax = 250;
                Txt_TP.Visibility = Visibility.Hidden;
                Txt_TC.Visibility = Visibility.Visible;
                Txt_Rel_TP.Visibility = Visibility.Hidden;
                Txt_Rel_TC.Visibility = Visibility.Visible;
               
            }
            else if (name == "VXXX")
            {
                U_Vmax = 500;
                U_Imax = 25;
                Txt_TC.Visibility = Visibility.Visible;
                Txt_TP.Visibility = Visibility.Visible;
                Txt_Rel_TP.Visibility = Visibility.Visible;
                Txt_Rel_TC.Visibility = Visibility.Visible;
                Txt_TP.Focus();
                //soloicitar multiplicador abrir cuadro tp y tc
            }
            else if (name == "FXXX")
            {
                U_Vmax = 280;
                U_Imax = 5;
                Txt_TP.Visibility = Visibility.Hidden;
                Txt_TC.Visibility = Visibility.Visible;
                Txt_Rel_TP.Visibility = Visibility.Hidden;
                Txt_Rel_TC.Visibility = Visibility.Visible;
                cbo_Accion.Focus();
            }

        }
        private void Cbo_punto_med_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            name = cbo_punto_med.Items.GetItemAt(cbo_punto_med.SelectedIndex).ToString();
            if (name == "MT")
            {
                Vmax = 14000;
                Imax = 250;
                Voltaje_A.Text = "13279";
                Voltaje_B.Text = "13279";
                Voltaje_C.Text = "13279";
                Txt_TP.Text = "110";
                cbo_herr.Focus();
            }
            else if (name == "BT")
            {
                //soloicitar multiplicador abrir cuadro tp y tc
                Vmax = 140;
                Imax = 250;
                Voltaje_A.Text = "120";
                Voltaje_B.Text = "120";
                Voltaje_C.Text = "120";
                cbo_herr.Focus();
            }
        }
        private void Cbo_herr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            name = cbo_herr.Items.GetItemAt(cbo_herr.SelectedIndex).ToString();
            if (name == "Analizador")
            {
                Voltaje_A.Visibility = Visibility.Visible;
                Voltaje_B.Visibility = Visibility.Visible;
                Voltaje_C.Visibility = Visibility.Visible;
                Angulo_Voltaje_A.Visibility = Visibility.Visible;
                Angulo_Voltaje_B.Visibility = Visibility.Visible;
                Angulo_Voltaje_C.Visibility = Visibility.Visible;
                Angulo_Corriente_A.Visibility = Visibility.Visible;
                Angulo_Corriente_B.Visibility = Visibility.Visible;
                Angulo_Corriente_C.Visibility = Visibility.Visible;
                h = 0;
                Voltaje_A.Focus();
            }
            else if (name == "Amperimetro")
            {
                //soloicitar multiplicador abrir cuadro tp y tc
                Voltaje_A.Visibility = Visibility.Hidden;
                Voltaje_B.Visibility = Visibility.Hidden;
                Voltaje_C.Visibility = Visibility.Hidden;
                Angulo_Voltaje_A.Visibility = Visibility.Hidden;
                Angulo_Voltaje_B.Visibility = Visibility.Hidden;
                Angulo_Voltaje_C.Visibility = Visibility.Hidden;
                Angulo_Corriente_A.Visibility = Visibility.Hidden;
                Angulo_Corriente_B.Visibility = Visibility.Hidden;
                Angulo_Corriente_C.Visibility = Visibility.Hidden;
                h = 1;
                Corriente_A.Focus();
            }
        }
        private void Txt_TC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                va = Convert.ToDouble(Txt_TC.Text);
                if (va < 0 || va > 1000)
                {
                    MessageBox.Show("Ingrese un valor valido para la relación");
                    Txt_TC.Focus();
                }
                else
                {
                    cbo_Accion.Focus();
                }
            }
        }
        private void Txt_TP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                va = Convert.ToDouble(Txt_TP.Text);
                if (va < 0 || va > 1000)
                {
                    MessageBox.Show("Ingrese un valor valido para la relación");
                    Txt_TP.Focus();
                }
                else
                {
                    Txt_TC.Focus();
                }
            }
        }

        private void Btn_Continua_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                byte[] respuestaSerial = null;
                byte[] respuestaSerial2 = null;
                byte[] auxiliar = null;
                byte[] respuestaSerial3 = null;
                byte[] auxiliar2 = null;
                byte[] auxiliar3 = null;
                byte[] respuestaSerial4 = null;
                byte[] auxiliar4 = null;
                int intentos = 0;
                int a = 0;
                int b = 0;
                int c = 0;
                int d = 0;
                int div = 0;


                if (Btn_Continua.Content.ToString() == "Stop")
                {
                    bandera = false;
                   

                }

                else 
                {
                    bandera = true;
                    Btn_Continua.Content = "Stop";

                    serialPort = new SerialPort(cbo_Puerto.Text, 9600, Parity.None, 8, StopBits.One);

                    serialPort.Close();
                    serialPort.Open();
                    Task.Delay(500).Wait();
                    serialPort.Write(tramaM1, 0, tramaM1.Length);
                    Task.Delay(500).Wait();
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
                        bandera = false;
                        Btn_Continua.Content = "Continua";

                    }
                    else
                    {
                        respuestaSerial = null;
                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                        Task.Delay(500).Wait();
                        serialPort.Write(tramaM3, 0, tramaM3.Length);
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
                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                        Task.Delay(500).Wait();
                        serialPort.Write(tramaM4, 0, tramaM4.Length);
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
                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                        Task.Delay(500).Wait();
                        serialPort.Write(tramaM5, 0, tramaM5.Length);
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
                            bandera = true;
                            Btn_Continua.Content = "Stop";
                        }
                        else
                        {
                            bandera = false;
                            Btn_Continua.Content = "Continua";

                        }
                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                        Task.Delay(500).Wait();

                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                while (bandera)
                                {

                                    respuestaSerial3 = null;
                                    respuestaSerial2 = null;
                                    respuestaSerial4 = null;
                                    int intent = 0;
                                    Task.Delay(750).Wait();
                                    do
                                    {

                                        serialPort.Write(tramaM9, 0, tramaM9.Length);
                                        Task.Delay(750).Wait();
                                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                                        Task.Delay(750).Wait();
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
                                                byte[] bytes3 = new byte[serialPort.BytesToRead];
                                                respuestaSerial3 = new byte[auxiliar3.Length + bytes3.Length];
                                                Array.Copy(auxiliar3, respuestaSerial3, auxiliar3.Length);
                                                serialPort.Read(bytes3, 0, bytes3.Length);
                                                Array.Copy(bytes3, 0, respuestaSerial3, auxiliar3.Length, bytes3.Length);
                                            }
                                        }

                                        intent++;
                                    } while ((respuestaSerial3 == null || respuestaSerial3.Length < 200) && intent < 3);
                                    if (respuestaSerial3 == null || respuestaSerial3.Length < 200)
                                    {
                                        bandera = false;
                                        serialPort.Close();
                                        this.Dispatcher.Invoke((Action)(() =>
                                        {
                                            Btn_Continua.Content = "Continua";
                                        }));
                                        break;

                                    }
                                    intent = 0;
                                    Task.Delay(750).Wait();
                                    do
                                    {
                                        serialPort.Write(tramaM6, 0, tramaM6.Length);
                                        Task.Delay(750).Wait();
                                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                                        Task.Delay(750).Wait();
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
                                        intent++;
                                    } while ((respuestaSerial2 == null || respuestaSerial2.Length < 200) && intent < 3);

                                    if (respuestaSerial2 == null || respuestaSerial2.Length < 200)
                                    {
                                        bandera = false;
                                        serialPort.Close();
                                        this.Dispatcher.Invoke((Action)(() =>
                                        {
                                            Btn_Continua.Content = "Continua";
                                        }));
                                        break;
                                    }
                                    intent = 0;
                                    Task.Delay(750).Wait();
                                    do
                                    {
                                        serialPort.Write(tramaM10, 0, tramaM10.Length);
                                        Task.Delay(750).Wait();
                                        serialPort.Write(tramaM0, 0, tramaM0.Length);
                                        Task.Delay(750).Wait();
                                        while (serialPort.BytesToRead > 0)
                                        {
                                            if (respuestaSerial4 == null)
                                            {
                                                respuestaSerial4 = new byte[serialPort.BytesToRead];
                                                serialPort.Read(respuestaSerial4, 0, respuestaSerial4.Length);
                                            }
                                            else
                                            {
                                                auxiliar4 = new byte[respuestaSerial4.Length];
                                                auxiliar4 = respuestaSerial4;
                                                byte[] bytes4 = new byte[serialPort.BytesToRead];
                                                respuestaSerial4 = new byte[auxiliar4.Length + bytes4.Length];
                                                Array.Copy(auxiliar4, respuestaSerial4, auxiliar4.Length);
                                                serialPort.Read(bytes4, 0, bytes4.Length);
                                                Array.Copy(bytes4, 0, respuestaSerial4, auxiliar4.Length, bytes4.Length);
                                            }
                                        }

                                        intent++;
                                    } while ((respuestaSerial4 == null || respuestaSerial4.Length < 40) && intent < 3);
                                    if (respuestaSerial4 == null || respuestaSerial4.Length < 40)
                                    {
                                        bandera = false;
                                        serialPort.Close();
                                        this.Dispatcher.Invoke((Action)(() =>
                                        {
                                            Btn_Continua.Content = "Continua";
                                        }));
                                        break;
                                    }
                                    for (int i = 0; i < respuestaSerial2.Length; i++)
                                    {
                                        if (respuestaSerial2[i] == 0xee && respuestaSerial2[i + 1] == 0x00)
                                        {
                                            a = i;
                                            break;
                                        }
                                    }
                                    for (int k = 0; k < respuestaSerial3.Length; k++)
                                    {
                                        if (respuestaSerial3[k] == 0xee && respuestaSerial3[k + 1] == 0x00)
                                        {
                                            b = k;
                                            break;
                                        }
                                    }

                                    for (int k = 0; k < respuestaSerial4.Length; k++)
                                    {
                                        if (respuestaSerial4[k] == 0xee && respuestaSerial4[k + 1] == 0x00)
                                        {
                                            c = k;
                                            break;
                                        }
                                    }
                                    if (respuestaSerial2[a + 5] == 0xf4)
                                    {
                                        d = 0;
                                    }
                                    else if (respuestaSerial2[a + 5] == 0xd4)
                                    {
                                        d = 32;
                                    }
                                    if (respuestaSerial4[c + 44] == 0x31)
                                    {
                                        div = 10000;
                                    }
                                    else if (respuestaSerial4[c + 44] == 0x32)
                                    {
                                        div = 1000;
                                    }
                                    for (int j = 0; j < 6; j++)
                                    {
                                        VoltajeA[j] = respuestaSerial2[a + 68 - j - d];
                                        CorrienteA[j] = respuestaSerial2[a + 74 - j - d];
                                        VoltajeC[j] = respuestaSerial2[a + 86 - j - d];
                                        CorrienteC[j] = respuestaSerial2[a + 92 - j - d];
                                        AnguloVoltajeC[j] = respuestaSerial2[a + 104 - j - d];
                                        VoltajeB[j] = respuestaSerial2[a + 110 - j - d];
                                        CorrienteB[j] = respuestaSerial2[a + 116 - j - d];
                                        AnguloVoltajeB[j] = respuestaSerial2[a + 128 - j - d];
                                        AnguloCorrienteA[j] = respuestaSerial2[a + 176 - j - d];
                                        AnguloCorrienteB[j] = respuestaSerial2[a + 182 - j - d];
                                        AnguloCorrienteC[j] = respuestaSerial2[a + 188 - j - d];
                                        MED[j] = respuestaSerial3[b + 123 + j];
                                    }
                                    AnguloVoltajeA = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Medidor.Text = Encoding.ASCII.GetString(MED);
                                        Usuario_Voltaje_A.Text = BitConverter.ToString(VoltajeA, 0).Replace("-", string.Empty);
                                        uva = (Int32.Parse(Usuario_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                                        uva = uva / div;
                                        Usuario_Voltaje_A.Text = uva.ToString();
                                        Usuario_Voltaje_B.Text = BitConverter.ToString(VoltajeB, 0).Replace("-", string.Empty);
                                        uvb = (Int32.Parse(Usuario_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                                        uvb = uvb / div;
                                        Usuario_Voltaje_B.Text = uvb.ToString();
                                        Usuario_Voltaje_C.Text = BitConverter.ToString(VoltajeC).Replace("-", string.Empty);
                                        uvc = (Int32.Parse(Usuario_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                                        uvc = uvc / div;
                                        Usuario_Voltaje_C.Text = uvc.ToString();
                                        Usuario_Corriente_A.Text = BitConverter.ToString(CorrienteA).Replace("-", string.Empty);
                                        uaa = (Int32.Parse(Usuario_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                                        uaa = uaa / div;
                                        Usuario_Corriente_A.Text = uaa.ToString();
                                        Usuario_Corriente_B.Text = BitConverter.ToString(CorrienteB).Replace("-", string.Empty);
                                        uab = (Int32.Parse(Usuario_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                                        uab = uab / div;
                                        Usuario_Corriente_B.Text = uab.ToString();
                                        Usuario_Corriente_C.Text = BitConverter.ToString(CorrienteC).Replace("-", string.Empty);
                                        uac = (Int32.Parse(Usuario_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                                        uac = uac / div;
                                        Usuario_Corriente_C.Text = uac.ToString();
                                        Usuario_Angulo_Voltaje_A.Text = BitConverter.ToString(AnguloVoltajeA).Replace("-", string.Empty);
                                        uava = (Int32.Parse(Usuario_Angulo_Voltaje_A.Text, System.Globalization.NumberStyles.HexNumber));
                                        uava = uava / div;
                                        Usuario_Angulo_Voltaje_A.Text = uava.ToString();
                                        Usuario_Angulo_Voltaje_B.Text = BitConverter.ToString(AnguloVoltajeB).Replace("-", string.Empty);
                                        uavb = (Int32.Parse(Usuario_Angulo_Voltaje_B.Text, System.Globalization.NumberStyles.HexNumber));
                                        uavb = uavb / div;
                                        Usuario_Angulo_Voltaje_B.Text = uavb.ToString();
                                        Usuario_Angulo_Voltaje_C.Text = BitConverter.ToString(AnguloVoltajeC).Replace("-", string.Empty);
                                        uavc = (Int32.Parse(Usuario_Angulo_Voltaje_C.Text, System.Globalization.NumberStyles.HexNumber));
                                        uavc = uavc / div;
                                        Usuario_Angulo_Voltaje_C.Text = uavc.ToString();
                                        Usuario_Angulo_Corriente_A.Text = BitConverter.ToString(AnguloCorrienteA).Replace("-", string.Empty);
                                        uaaa = (Int32.Parse(Usuario_Angulo_Corriente_A.Text, System.Globalization.NumberStyles.HexNumber));
                                        uaaa = uaaa / div;
                                        Usuario_Angulo_Corriente_A.Text = uaaa.ToString();
                                        Usuario_Angulo_Corriente_B.Text = BitConverter.ToString(AnguloCorrienteB).Replace("-", string.Empty);
                                        uaab = (Int32.Parse(Usuario_Angulo_Corriente_B.Text, System.Globalization.NumberStyles.HexNumber));
                                        uaab = uaab / div;
                                        Usuario_Angulo_Corriente_B.Text = uaab.ToString();
                                        Usuario_Angulo_Corriente_C.Text = BitConverter.ToString(AnguloCorrienteC).Replace("-", string.Empty);
                                        uaac = (Int32.Parse(Usuario_Angulo_Corriente_C.Text, System.Globalization.NumberStyles.HexNumber));
                                        uaac = uaac / div;
                                        Usuario_Angulo_Corriente_C.Text = uaac.ToString();
                                        Usuario_Lienzo.Children.Clear();
                                        Ellipse contorno = new Ellipse();
                                        contorno.Stroke = System.Windows.Media.Brushes.DarkBlue;
                                        contorno.StrokeThickness = 1;
                                        contorno.Height = Usuario_Lienzo.ActualHeight;
                                        contorno.Width = Usuario_Lienzo.ActualWidth;
                                        contorno.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                                        @U_base.RenderTransformOrigin = new System.Windows.Point(Usuario_Lienzo.ActualHeight / 2, Usuario_Lienzo.ActualWidth / 2);
                                        Usuario_Lienzo.Children.Add(@U_base);
                                        Usuario_Lienzo.Children.Add(contorno);
                                        Line VA = new Line();
                                        Line VB = new Line();
                                        Line VC = new Line();
                                        Line CA = new Line();
                                        Line CB = new Line();
                                        Line CC = new Line();
                                        Line xs = new Line();
                                        Line ys = new Line();
                                        double midx = Usuario_Lienzo.ActualHeight / 2;
                                        double midy = Usuario_Lienzo.ActualWidth / 2;
                                        VA.Stroke = System.Windows.Media.Brushes.Blue;
                                        VA.StrokeThickness = 4;
                                        VB.Stroke = System.Windows.Media.Brushes.Blue;
                                        VB.StrokeThickness = 4;
                                        VC.Stroke = System.Windows.Media.Brushes.Blue;
                                        VC.StrokeThickness = 4;
                                        CA.Stroke = System.Windows.Media.Brushes.Red;
                                        CA.StrokeThickness = 2;
                                        CB.Stroke = System.Windows.Media.Brushes.Red;
                                        CB.StrokeThickness = 2;
                                        CC.Stroke = System.Windows.Media.Brushes.Red;
                                        CC.StrokeThickness = 2;
                                        xs.Stroke = System.Windows.Media.Brushes.Black;
                                        xs.StrokeThickness = 0.5;
                                        ys.Stroke = System.Windows.Media.Brushes.Black;
                                        ys.StrokeThickness = 0.5;
                                        xs.X1 = 0;
                                        xs.X2 = Usuario_Lienzo.ActualHeight;
                                        xs.Y1 = midy;
                                        xs.Y2 = midy;
                                        ys.X1 = midx;
                                        ys.X2 = midx;
                                        ys.Y1 = 0;
                                        ys.Y2 = Usuario_Lienzo.ActualWidth;
                                        VA.X1 = midx;
                                        VA.Y1 = midy;
                                        VB.X1 = midx;
                                        VB.Y1 = midy;
                                        VC.X1 = midx;
                                        VC.Y1 = midy;
                                        CA.X1 = midx;
                                        CA.Y1 = midy;
                                        CB.X1 = midx;
                                        CB.Y1 = midy;
                                        CC.X1 = midx;
                                        CC.Y1 = midy;
                                        double[] punto = Puntos(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, U_Vmax, U_Imax);
                                        VA.X2 = midx + punto[0];
                                        VA.Y2 = midy + (punto[1] * (-1));
                                        VB.X2 = midx + punto[2];
                                        VB.Y2 = midy + (punto[3] * (-1));
                                        VC.X2 = midx + punto[4];
                                        VC.Y2 = midy + (punto[5] * (-1));
                                        CA.X2 = midx + punto[6];
                                        CA.Y2 = midy + (punto[7] * (-1));
                                        CB.X2 = midx + punto[8];
                                        CB.Y2 = midy + (punto[9] * (-1));
                                        CC.X2 = midx + punto[10];
                                        CC.Y2 = midy + (punto[11] * (-1));
                                        RotateTransform rotateTransformU_C = new RotateTransform(90 - uavc, U_imgC.Width / 2, U_imgC.Height / 2);
                                        @U_imgC.RenderTransform = rotateTransformU_C;
                                        RotateTransform rotateTransformU_A = new RotateTransform(90 - uava, U_imgA.Width / 2, U_imgA.Height / 2);
                                        @U_imgA.RenderTransform = rotateTransformU_A;
                                        RotateTransform rotateTransformU_B = new RotateTransform(90 - uavb, U_imgB.Width / 2, U_imgB.Height / 2);
                                        @U_imgB.RenderTransform = rotateTransformU_B;
                                        RotateTransform rotateTransformU_c = new RotateTransform(90 - uaac, U_imgc.Width / 2, U_imgc.Height / 2);
                                        @U_imgc.RenderTransform = rotateTransformU_c;
                                        RotateTransform rotateTransformU_a = new RotateTransform(90 - uaaa, U_imga.Width / 2, U_imga.Height / 2);
                                        @U_imga.RenderTransform = rotateTransformU_a;
                                        RotateTransform rotateTransformU_b = new RotateTransform(90 - uaab, U_imgb.Width / 2, U_imgb.Height / 2);
                                        @U_imgb.RenderTransform = rotateTransformU_b;
                                        Usuario_Lienzo.Children.Add(xs);
                                        Usuario_Lienzo.Children.Add(ys);
                                        Usuario_Lienzo.Children.Add(VA);
                                        Usuario_Lienzo.Children.Add(VB);
                                        Usuario_Lienzo.Children.Add(VC);
                                        Usuario_Lienzo.Children.Add(CA);
                                        Usuario_Lienzo.Children.Add(CB);
                                        Usuario_Lienzo.Children.Add(CC);
                                        Usuario_Lienzo.Children.Add(@U_imgC);
                                        Usuario_Lienzo.Children.Add(@U_imgB);
                                        Usuario_Lienzo.Children.Add(@U_imgA);
                                        Usuario_Lienzo.Children.Add(@U_imgc);
                                        Usuario_Lienzo.Children.Add(@U_imgb);
                                        Usuario_Lienzo.Children.Add(@U_imga);
                                        double TC = Convert.ToDouble(Txt_TC.Text);
                                        double TP = Convert.ToDouble(Txt_TP.Text);
                                        double[] potencia_u = Potencias(uva, uvb, uvc, uaa, uab, uac, uava, uavb, uavc, uaaa, uaab, uaac, TC, TP);
                                        KW_A.Text = potencia_u[0].ToString();
                                        KW_B.Text = potencia_u[1].ToString();
                                        KW_C.Text = potencia_u[2].ToString();
                                        FP_A.Text = potencia_u[3].ToString();
                                        FP_B.Text = potencia_u[4].ToString();
                                        FP_C.Text = potencia_u[5].ToString();
                                    }));

                                }
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    Btn_Continua.Content = "Continua";
                                }));
                                Task.Delay(500).Wait();
                                serialPort.Write(tramaM7, 0, tramaM7.Length);
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
                                serialPort.Write(tramaM0, 0, tramaM0.Length);
                                Task.Delay(500).Wait();
                                serialPort.Write(tramaM8, 0, tramaM8.Length);
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
                                serialPort.Write(tramaM0, 0, tramaM0.Length);

                                serialPort.Close();
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message + " " + ex.StackTrace);
                                Btn_Continua.IsEnabled = true;
                            }




                        });

                    }


                }
              
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_Continua.IsEnabled = true;
            }

        }
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public  System.Drawing.Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }

        public  Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public  Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
            }

            return result;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String[] varios = null;
            string path2 = Liga.Text;
            varios = File.ReadAllLines(path2);
            String linea = varios[0];
            String linea1 = varios[1];
            String linea2 = varios[2];
            String linea3 = varios[3];
            String linea4 = varios[4];
            String linea5 = varios[5];
            String linea6 = varios[6];
            String linea7 = varios[7];
            String linea8 = varios[8];
            String linea9 = varios[9];
            String linea10 = varios[10];
            String linea11 = varios[11];
            String linea12 = varios[12];
            String linea13 = varios[13];
            String linea14 = varios[14];
            String linea15 = varios[15];
            String linea16 = varios[16];
            String linea17 = varios[17];
            String linea18 = varios[18];
            String linea19 = varios[19];

            linea = linea.Replace("-", "");
            linea1 = linea1.Replace("-", "");
            linea2 = linea2.Replace("-", "");
            linea3 = linea3.Replace("-", "");
            linea4 = linea4.Replace("-", "");
            linea5 = linea5.Replace("-", "");
            linea6 = linea6.Replace("-", "");
            linea7 = linea7.Replace("-", "");
            linea8 = linea8.Replace("-", "");
            linea9 = linea9.Replace("-", "");
            linea10 = linea10.Replace("-", "");
            linea11 = linea11.Replace("-", "");
            linea12 = linea12.Replace("-", "");
            linea13 = linea13.Replace("-", "");
            linea14 = linea14.Replace("-", "");
            linea15 = linea15.Replace("-", "");
            linea16 = linea16.Replace("-", "");
            linea17 = linea17.Replace("-", "");
            linea18 = linea18.Replace("-", "");
            linea19 = linea19.Replace("-", "");

            Byte[] dato = StringToByteArray(linea);
            Byte[] dato1 = StringToByteArray(linea1);
            Byte[] dato2 = StringToByteArray(linea2);
            Byte[] dato3 = StringToByteArray(linea3);
            Byte[] dato4 = StringToByteArray(linea4);
            Byte[] dato5 = StringToByteArray(linea5);
            Byte[] dato6 = StringToByteArray(linea6);
            Byte[] dato7 = StringToByteArray(linea7);
            Byte[] dato8 = StringToByteArray(linea8);
            Byte[] dato9 = StringToByteArray(linea9);
            Byte[] dato10 = StringToByteArray(linea10);
            Byte[] dato11 = StringToByteArray(linea11);
            Byte[] dato12 = StringToByteArray(linea12);
            Byte[] dato13 = StringToByteArray(linea13);
            Byte[] dato14 = StringToByteArray(linea14);
            Byte[] dato15 = StringToByteArray(linea15);
            Byte[] dato16 = StringToByteArray(linea16);
            Byte[] dato17 = StringToByteArray(linea17);
            Byte[] dato18 = StringToByteArray(linea18);
            Byte[] dato19 = StringToByteArray(linea19);
            
            using (AesManaged aes = new AesManaged())
            {
                aes.Key = arreglo1;
                aes.IV = arreglo2;
                // Encrypt string    
                dencrypted0 = Decrypt(dato, aes.Key, aes.IV);
                dencrypted1 = Decrypt(dato1, aes.Key, aes.IV);
                dencrypted2 = Decrypt(dato2, aes.Key, aes.IV);
                dencrypted3 = Decrypt(dato3, aes.Key, aes.IV);
                dencrypted4 = Decrypt(dato4, aes.Key, aes.IV);
                dencrypted5 = Decrypt(dato5, aes.Key, aes.IV);
                dencrypted6 = Decrypt(dato6, aes.Key, aes.IV);
                dencrypted7 = Decrypt(dato7, aes.Key, aes.IV);
                dencrypted8 = Decrypt(dato8, aes.Key, aes.IV);
                dencrypted9 = Decrypt(dato9, aes.Key, aes.IV);
                dencrypted10 = Decrypt(dato10, aes.Key, aes.IV);
                dencrypted11 = Decrypt(dato11, aes.Key, aes.IV);
                dencrypted12 = Decrypt(dato12, aes.Key, aes.IV);
                dencrypted13 = Decrypt(dato13, aes.Key, aes.IV);
                dencrypted14 = Decrypt(dato14, aes.Key, aes.IV);
                dencrypted15 = Decrypt(dato15, aes.Key, aes.IV);
                dencrypted16 = Decrypt(dato16, aes.Key, aes.IV);
                dencrypted17 = Decrypt(dato17, aes.Key, aes.IV);
                dencrypted18 = Decrypt(dato18, aes.Key, aes.IV);
                dencrypted19 = Decrypt(dato19, aes.Key, aes.IV);
            }
            
            MessageBox.Show(dencrypted0 + "\r\n"+ dencrypted1 + "\r\n" + dencrypted2 + "\r\n" + dencrypted3 + "\r\n" + dencrypted4 + "\r\n" + dencrypted5 + "\r\n" + dencrypted6 + "\r\n" + dencrypted7 + "\r\n" + dencrypted8 + "\r\n" + dencrypted9 + "\r\n" + dencrypted10 + "\r\n" + dencrypted11 + "\r\n" + dencrypted12 + "\r\n" + dencrypted13 + "\r\n" + dencrypted14 + "\r\n" + dencrypted15 + "\r\n" + dencrypted16 + "\r\n" + dencrypted17 + "\r\n" + dencrypted18 + "\r\n" + dencrypted19 );
        }
    }
}
