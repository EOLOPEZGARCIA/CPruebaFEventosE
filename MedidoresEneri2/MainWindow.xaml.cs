using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualBasic;


namespace MedidoresEneri
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        string name = "";
       
        string Medido1 = "";
        string Medido2 = "";
        string Medido3 = "";
        string Medido4 = "";
        string Medido5 = "";
        string Medido6 = "";
        string Medido7 = "";
        string Medido8 = "";
        string Medido9 = "";
        string Medido10 = "";
        string Medido11 = "";
        string Medido12 = "";
        string Medido13 = "";
        string Medido14 = "";
        string Medido15 = "";
        string Medido16 = "";
        string Medido17 = "";
        string Medido18 = "";
        string Medido19 = "";
        string Medido20 = "";
        string Medido21 = "";
        string Medido22 = "";
        string Medido23 = "";
        string Medido24 = "";
        string MEDI1 = "000000";
        string MEDI2 = "000000";
        string MEDI3 = "000000";
        string MEDI4 = "000000";
        string MEDI5 = "000000";
        string MEDI6 = "000000";
        string MEDI7 = "000000";
        string MEDI8 = "000000";
        string MEDI9 = "000000";
        string MEDI10 = "000000";
        string MEDI11 = "000000";
        string MEDI12 = "000000";
        string MEDI13 = "000000";
        string MEDI14 = "000000";
        string MEDI15 = "000000";
        string MEDI16 = "000000";
        string MEDI17 = "000000";
        string MEDI18 = "000000";
        string MEDI19 = "000000";
        string MEDI20 = "000000";
        string MEDI21 = "000000";
        string MEDI22 = "000000";
        string MEDI23 = "000000";
        string MEDI24 = "000000";
        int c = 0;
        public MainWindow()
        {
            InitializeComponent();
            cboMedidores.Text = "";
            cboPuerto.Focus();
            foreach (String puerto in SerialPort.GetPortNames())
            {
                cboPuerto.Items.Add(puerto);
            }
            cboZona.Text = "";
            cboZona.Items.Add("LOS ALTOS");
            cboZona.Items.Add("CIENEGA");
            cboZona.Items.Add("ZAPOTLAN");
            cboZona.Items.Add("COSTA");
            cboZona.Items.Add("MINAS");
            cboZona.Items.Add("CHAPALA");
            cboZona.Items.Add("SANTIAGO");
            cboZona.Items.Add("TEPIC");
            cboZona.Items.Add("VALLARTA");
            cboZona.Items.Add("MET HIDALGO");
            cboZona.Items.Add("MET JUAREZ");
            cboZona.Items.Add("MET LIBERTAD");
            cboZona.Items.Add("MET REFORMA");
        }
        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private void BtnCarga_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnCarga.IsEnabled = false;
                string[] numeromed;
                byte[] posicion;
                byte[] revision;
                int i = 0;
                int k = 1;
                bool a = false;
                posicion = new byte[] { 0xE1, 0x10, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x08, 0x00, 0x01, 0x35, 0x32, 0x31, 0x54, 0x54, 0x48, 0x02, 0x34, 0x37, 0x34, 0x54, 0x54, 0x48, 0x03, 0x35, 0x33, 0x32, 0x54, 0x54, 0x50, 0x04, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x05, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x06, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x07, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x08, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x09, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x10, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x11, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x12, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x13, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x14, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x15, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x16, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x17, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x18, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x19, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x20, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x21, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x22, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x23, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x24, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
                //0xE1, 0x10, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x08, 0xAA, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                numeromed = new string[] { MEDI1, MEDI2, MEDI3, MEDI4, MEDI5, MEDI6, MEDI7, MEDI8, MEDI9, MEDI10, MEDI11, MEDI12, MEDI13, MEDI14, MEDI15, MEDI16, MEDI17, MEDI18, MEDI19, MEDI20, MEDI21, MEDI22, MEDI23, MEDI24 };
                revision = new byte[] { 0xE1, 0x10, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x04, 0x01 };
                byte[] respuestaSerial = null;
                byte[] auxiliar = null;
                int intentos = 0;
                int inserto = 0;
                int value = 0;
                int valu2 = 0;
                do
                {
                    if (numeromed[i] != "000000")
                    {
                        do
                        {
                            if (numeromed[i] == numeromed[k] && i != k)
                            {
                                value = i + 1;
                                valu2 = k + 1;
                                MessageBox.Show("El número de los medidores esta repetido en la posicion " + value + " y la posicion " + valu2);
                                a = false;
                                break;
                            }
                            a = true;
                            k++;
                        } while (k < 23);
                        k = 1;
                        if (a == true)
                        {
                            Byte[] aux2 = Encoding.ASCII.GetBytes(numeromed[i]);
                            inserto = 10 + i*7;
                            Array.Copy(aux2, 0, posicion, inserto, aux2.Length);
                        }
                        else
                        {
                            break;
                        }
                        i++;
                    }
                    else
                    {
                        i++;
                    }

                } while (i < 24);
                i = 0;
                if (a == true)
                {


                    string filepath3 = AppDomain.CurrentDomain.BaseDirectory;
                    string path3 = filepath3 + ".\\Medidores Instalados.csv";
                    if (!File.Exists(path3))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path3))
                        {
                            string encabezado = "NUMERO_MEDIDOR" + "," + "LATITUD" + "," + "LONGITUD" + "," + "RANURA" + "," + "GABINETE" + "," + "MODULO IOT " + "," + "COLUMNA" + "," + "FILA" + "," + "MAC_DISPLAY" + "," + "AREA_INSTALACION";
                            sw.WriteLine(encabezado);

                            do
                            {
                                if (numeromed[i] != "00")
                                {
                                    string text = numeromed[i] + "," + Latitud.Text + "," + Longitud.Text + "," + (i + 1) + "," + GaB.Text + "," + Gabinete.Text + "," + " " + "," + " " + "," + " " + "," + cboZona.Text;
                                    sw.WriteLine(text);
                                    i += 1;
                                }
                                else
                                {
                                    i += 1; ;
                                }
                            } while (i < 24);


                        }
                        i = 0;
                    }
                    else if (File.Exists(path3))
                    {
                        using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(filepath3, ".\\Medidores Instalados.csv"), true))
                        {
                            do
                            {
                                if (numeromed[i] != "00")
                                {
                                    string text = numeromed[i] + "," + Latitud.Text + "," + Longitud.Text + "," + (i + 1) + "," + GaB.Text + "," + Gabinete.Text + "," + " " + "," + " " + "," + " " + "," + cboZona.Text;
                                    sw.WriteLine(text);
                                    i += 1;
                                }
                                else
                                {
                                    i += 1; ;
                                }
                            } while (i < 24);


                        }
                        i = 0;
                    }

                    string filepath4 = AppDomain.CurrentDomain.BaseDirectory;
                    string path4 = filepath4 + ".\\IoT Instalados.csv";
                    if (!File.Exists(path4))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path4))
                        {
                            string Enca = "MODULO IOT" + "," + "CARRIER";
                            sw.WriteLine(Enca);
                            string text2 = name + "," + "Orkan";
                            sw.WriteLine(text2);
                        }
                    }
                    else if (File.Exists(path4))
                    {
                        using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(filepath4, ".\\IoT Instalados.csv"), true))
                        {
                            string text2 = name + "," + "Orkan";
                            sw.WriteLine(text2);
                        }
                    }
                    string filepath5 = AppDomain.CurrentDomain.BaseDirectory;
                    string path5 = filepath5 + ".\\Gabinete construido.csv";
                    if (!File.Exists(path5))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path5))
                        {
                            string Title = "GABINETE" + "," + "MODULO IOT " + "," + "LATITUD" + "," + "LONGITUD" + "," + "AREA_INSTALACION";
                            sw.WriteLine(Title);
                            string text3 = GaB.Text + "," + name + "," + Latitud.Text + "," + Longitud.Text + "," + cboZona.Text;
                            sw.WriteLine(text3);
                        }
                    }
                    else if (File.Exists(path5))
                    {
                        using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(filepath5, ".\\Gabinete construido.csv"), true))
                        {
                            string text3 = GaB.Text + "," + name + "," + Latitud.Text + "," + Longitud.Text + "," + cboZona.Text;
                            sw.WriteLine(text3);
                        }
                    }
                    MessageBox.Show("Listo");
                    GaB.Text = "";
                    Gabinete.Text = "";
                    Medidores1.Text = "";
                    Medidores2.Text = "";
                    Medidores3.Text = "";
                    Medidores4.Text = "";
                    Medidores5.Text = "";
                    Medidores6.Text = "";
                    Medidores7.Text = "";
                    Medidores8.Text = "";
                    Medidores9.Text = "";
                    Medidores10.Text = "";
                    Medidores11.Text = "";
                    Medidores12.Text = "";
                    Medidores13.Text = "";
                    Medidores14.Text = "";
                    Medidores15.Text = "";
                    Medidores16.Text = "";
                    Medidores17.Text = "";
                    Medidores18.Text = "";
                    Medidores19.Text = "";
                    Medidores20.Text = "";
                    Medidores21.Text = "";
                    Medidores22.Text = "";
                    Medidores23.Text = "";
                    Medidores24.Text = "";
                    GaB.Focus();

                    btnCarga.IsEnabled = true;
                    SerialPort serialPort = new SerialPort(cboPuerto.Text, 19200, Parity.Even, 8, StopBits.One);
                    serialPort.Close();
                    Task.Delay(500).Wait();
                    serialPort.Open();
                    Task.Delay(1000).Wait();
                    serialPort.Write(posicion, 0, posicion.Length);
                    Task.Delay(500).Wait();
                    serialPort.Close();

                }
                else
                {
                    MessageBox.Show("Cambie el medidor de la posicion " + value + " o el de la posicion " + valu2);
                    Medidores1.Text = "";
                    Medidores2.Text = "";
                    Medidores3.Text = "";
                    Medidores4.Text = "";
                    Medidores5.Text = "";
                    Medidores6.Text = "";
                    Medidores7.Text = "";
                    Medidores8.Text = "";
                    Medidores9.Text = "";
                    Medidores10.Text = "";
                    Medidores11.Text = "";
                    Medidores12.Text = "";
                    Medidores13.Text = "";
                    Medidores14.Text = "";
                    Medidores15.Text = "";
                    Medidores16.Text = "";
                    Medidores17.Text = "";
                    Medidores18.Text = "";
                    Medidores19.Text = "";
                    Medidores20.Text = "";
                    Medidores21.Text = "";
                    Medidores22.Text = "";
                    Medidores23.Text = "";
                    Medidores24.Text = "";
                    Gabinete.Text = "";
                    btnCarga.IsEnabled = true;
                    Medidores1.Focus();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                btnCarga.IsEnabled = true;
                GaB.Text = "";
                Gabinete.Text = "";
                Medidores1.Text = "";
                Medidores2.Text = "";
                Medidores3.Text = "";
                Medidores4.Text = "";
                Medidores5.Text = "";
                Medidores6.Text = "";
                Medidores7.Text = "";
                Medidores8.Text = "";
                Medidores9.Text = "";
                Medidores10.Text = "";
                Medidores11.Text = "";
                Medidores12.Text = "";
                Medidores13.Text = "";
                Medidores14.Text = "";
                Medidores15.Text = "";
                Medidores16.Text = "";
                Medidores17.Text = "";
                Medidores18.Text = "";
                Medidores19.Text = "";
                Medidores20.Text = "";
                Medidores21.Text = "";
                Medidores22.Text = "";
                Medidores23.Text = "";
                Medidores24.Text = "";
                GaB.Focus();
            }
        }
        private void CboMedidores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                name = cboMedidores.Items.GetItemAt(cboMedidores.SelectedIndex).ToString();
                if (name == "System.Windows.Controls.ComboBoxItem: 12")
                {
                    Medidor1.Visibility = Visibility.Visible;
                    Medidor2.Visibility = Visibility.Visible;
                    Medidor3.Visibility = Visibility.Visible;
                    Medidor4.Visibility = Visibility.Visible;
                    Medidor5.Visibility = Visibility.Visible;
                    Medidor6.Visibility = Visibility.Visible;
                    Medidor13.Visibility = Visibility.Visible;
                    Medidor14.Visibility = Visibility.Visible;
                    Medidor15.Visibility = Visibility.Visible;
                    Medidor16.Visibility = Visibility.Visible;
                    Medidor17.Visibility = Visibility.Visible;
                    Medidor18.Visibility = Visibility.Visible;

                    Medidores1.IsEnabled = true;
                    Medidores2.IsEnabled = true;
                    Medidores3.IsEnabled = true;
                    Medidores4.IsEnabled = true;
                    Medidores5.IsEnabled = true;
                    Medidores6.IsEnabled = true;
                    Medidores13.IsEnabled = true;
                    Medidores14.IsEnabled = true;
                    Medidores15.IsEnabled = true;
                    Medidores16.IsEnabled = true;
                    Medidores17.IsEnabled = true;
                    Medidores18.IsEnabled = true;

                    Medidor7.Visibility = Visibility.Hidden;
                    Medidor8.Visibility = Visibility.Hidden;
                    Medidor9.Visibility = Visibility.Hidden;
                    Medidor10.Visibility = Visibility.Hidden;
                    Medidor11.Visibility = Visibility.Hidden;
                    Medidor12.Visibility = Visibility.Hidden;
                    Medidor19.Visibility = Visibility.Hidden;
                    Medidor20.Visibility = Visibility.Hidden;
                    Medidor21.Visibility = Visibility.Hidden;
                    Medidor22.Visibility = Visibility.Hidden;
                    Medidor23.Visibility = Visibility.Hidden;
                    Medidor24.Visibility = Visibility.Hidden;

                    Medidores7.IsEnabled = false;
                    Medidores8.IsEnabled = false;
                    Medidores9.IsEnabled = false;
                    Medidores10.IsEnabled = false;
                    Medidores11.IsEnabled = false;
                    Medidores12.IsEnabled = false;
                    Medidores19.IsEnabled = false;
                    Medidores20.IsEnabled = false;
                    Medidores21.IsEnabled = false;
                    Medidores22.IsEnabled = false;
                    Medidores23.IsEnabled = false;
                    Medidores24.IsEnabled = false;

                    Medidores7.Visibility = Visibility.Hidden;
                    Medidores8.Visibility = Visibility.Hidden;
                    Medidores9.Visibility = Visibility.Hidden;
                    Medidores10.Visibility = Visibility.Hidden;
                    Medidores11.Visibility = Visibility.Hidden;
                    Medidores12.Visibility = Visibility.Hidden;
                    Medidores19.Visibility = Visibility.Hidden;
                    Medidores20.Visibility = Visibility.Hidden;
                    Medidores21.Visibility = Visibility.Hidden;
                    Medidores22.Visibility = Visibility.Hidden;
                    Medidores23.Visibility = Visibility.Hidden;
                    Medidores24.Visibility = Visibility.Hidden;
                }

                if (name == "System.Windows.Controls.ComboBoxItem: 24")
                {
                    Medidor1.Visibility = Visibility.Visible;
                    Medidor2.Visibility = Visibility.Visible;
                    Medidor3.Visibility = Visibility.Visible;
                    Medidor4.Visibility = Visibility.Visible;
                    Medidor5.Visibility = Visibility.Visible;
                    Medidor6.Visibility = Visibility.Visible;
                    Medidor7.Visibility = Visibility.Visible;
                    Medidor8.Visibility = Visibility.Visible;
                    Medidor9.Visibility = Visibility.Visible;
                    Medidor10.Visibility = Visibility.Visible;
                    Medidor11.Visibility = Visibility.Visible;
                    Medidor12.Visibility = Visibility.Visible;

                    Medidores1.IsEnabled = true;
                    Medidores2.IsEnabled = true;
                    Medidores3.IsEnabled = true;
                    Medidores4.IsEnabled = true;
                    Medidores5.IsEnabled = true;
                    Medidores6.IsEnabled = true;
                    Medidores7.IsEnabled = true;
                    Medidores8.IsEnabled = true;
                    Medidores9.IsEnabled = true;
                    Medidores10.IsEnabled = true;
                    Medidores11.IsEnabled = true;
                    Medidores12.IsEnabled = true;

                    Medidor13.Visibility = Visibility.Visible;
                    Medidor14.Visibility = Visibility.Visible;
                    Medidor15.Visibility = Visibility.Visible;
                    Medidor16.Visibility = Visibility.Visible;
                    Medidor17.Visibility = Visibility.Visible;
                    Medidor18.Visibility = Visibility.Visible;
                    Medidor19.Visibility = Visibility.Visible;
                    Medidor20.Visibility = Visibility.Visible;
                    Medidor21.Visibility = Visibility.Visible;
                    Medidor22.Visibility = Visibility.Visible;
                    Medidor23.Visibility = Visibility.Visible;
                    Medidor24.Visibility = Visibility.Visible;

                    Medidores13.IsEnabled = true;
                    Medidores14.IsEnabled = true;
                    Medidores15.IsEnabled = true;
                    Medidores16.IsEnabled = true;
                    Medidores17.IsEnabled = true;
                    Medidores18.IsEnabled = true;
                    Medidores19.IsEnabled = true;
                    Medidores20.IsEnabled = true;
                    Medidores21.IsEnabled = true;
                    Medidores22.IsEnabled = true;
                    Medidores23.IsEnabled = true;
                    Medidores24.IsEnabled = true;

                    Medidores13.Visibility = Visibility.Visible;
                    Medidores14.Visibility = Visibility.Visible;
                    Medidores15.Visibility = Visibility.Visible;
                    Medidores16.Visibility = Visibility.Visible;
                    Medidores17.Visibility = Visibility.Visible;
                    Medidores18.Visibility = Visibility.Visible;
                    Medidores19.Visibility = Visibility.Visible;
                    Medidores20.Visibility = Visibility.Visible;
                    Medidores21.Visibility = Visibility.Visible;
                    Medidores22.Visibility = Visibility.Visible;
                    Medidores23.Visibility = Visibility.Visible;
                    Medidores24.Visibility = Visibility.Visible;
                }

                Medidores1.Focus();
                Medidores1.BorderBrush = Brushes.LightGreen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores1_KeyUp(object sender, KeyEventArgs e)

        {
            try
            {


                if (e.Key == Key.Enter)
                {
                   if (Medidores1.Text.Length == 16)
                    {
                        Medido1 = Medidores1.Text;
                        MEDI1 = Medido1.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores1.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI1 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores1.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores1.Text = MEDI1;
                        Medidores1.BorderBrush = Brushes.Gray;
                        Medidores2.BorderBrush = Brushes.LightGreen;
                        Medidores2.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores1.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores2.Text.Length == 16)
                    {
                        Medido2 = Medidores2.Text;
                        MEDI2 = Medido2.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores2.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI2 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores2.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores2.Text = MEDI2;
                        Medidores2.BorderBrush = Brushes.Gray;
                        Medidores3.BorderBrush = Brushes.LightGreen;
                        Medidores3.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores2.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores3_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores3.Text.Length == 16)
                    {
                        Medido3 = Medidores3.Text;
                        MEDI3 = Medido3.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores3.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI3 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores3.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores3.Text = MEDI3;
                        Medidores3.BorderBrush = Brushes.Gray;
                        Medidores4.BorderBrush = Brushes.LightGreen;
                        Medidores4.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores3.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores4_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores4.Text.Length == 16)
                    {
                        Medido4 = Medidores4.Text;
                        MEDI4 = Medido4.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores4.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI4 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores4.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores4.Text = MEDI4;
                        Medidores4.BorderBrush = Brushes.Gray;
                        Medidores5.BorderBrush = Brushes.LightGreen;
                        Medidores5.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores4.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores5_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores5.Text.Length == 16)
                    {
                        Medido5 = Medidores5.Text;
                        MEDI5 = Medido5.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores5.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI5 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores5.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores5.Text = MEDI5;
                        Medidores5.BorderBrush = Brushes.Gray;
                        Medidores6.BorderBrush = Brushes.LightGreen;
                        Medidores6.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores5.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores6_KeyUp(object sender, KeyEventArgs e) //
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores6.Text.Length == 16)
                    {
                        Medido6 = Medidores6.Text;
                        MEDI6 = Medido6.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores6.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            MEDI6 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores6.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores6.Text = MEDI6;
                        Medidores6.BorderBrush = Brushes.Gray;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores7.BorderBrush = Brushes.LightGreen;
                            Medidores7.Focus();
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores13.BorderBrush = Brushes.LightGreen;
                            Medidores13.Focus();
                        }
                        c = 0;
                    }
                    else
                    {
                        Medidores6.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores7_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores7.Text.Length == 16)
                    {
                        Medido7 = Medidores7.Text;
                        MEDI7 = Medido7.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores7.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI7 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores7.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores7.Text = MEDI7;
                        Medidores7.BorderBrush = Brushes.Gray;
                        Medidores8.BorderBrush = Brushes.LightGreen;
                        Medidores8.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores7.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores8_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores8.Text.Length == 16)
                    {
                        Medido8 = Medidores8.Text;
                        MEDI8 = Medido8.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores8.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI8 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores8.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores8.Text = MEDI8 ;
                        Medidores8.BorderBrush = Brushes.Gray;
                        Medidores9.BorderBrush = Brushes.LightGreen;
                        Medidores9.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores8.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores9_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores9.Text.Length == 16)
                    {
                        Medido9 = Medidores9.Text;
                        MEDI9 = Medido9.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores9.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI9 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores9.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores9.Text = MEDI9;
                        Medidores9.BorderBrush = Brushes.Gray;
                        Medidores10.BorderBrush = Brushes.LightGreen;
                        Medidores10.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores9.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores10_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores10.Text.Length == 16)
                    {
                        Medido10 = Medidores10.Text;
                        MEDI10 = Medido10.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores10.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI10 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores10.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores10.Text = MEDI10;
                        Medidores10.BorderBrush = Brushes.Gray;
                        Medidores11.BorderBrush = Brushes.LightGreen;
                        Medidores11.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores10.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores11_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores11.Text.Length == 16)
                    {
                        Medido11 = Medidores11.Text;
                        MEDI11 = Medido11.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores11.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI11 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores11.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores11.Text = MEDI11;
                        Medidores11.BorderBrush = Brushes.Gray;
                        Medidores12.BorderBrush = Brushes.LightGreen;
                        Medidores12.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores11.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores12_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores12.Text.Length == 16)
                    {
                        Medido12 = Medidores12.Text;
                        MEDI12 = Medido12.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores12.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI12 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores12.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores12.Text = MEDI12;
                        Medidores12.BorderBrush = Brushes.Gray;
                        Medidores13.BorderBrush = Brushes.LightGreen;
                        Medidores13.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores12.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores13_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores13.Text.Length == 16)
                    {
                        Medido13 = Medidores13.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI13 = Medido13.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI7 = Medido13.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores13.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI13 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI7 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores13.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores13.Text = MEDI13;
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores13.Text = MEDI7;
                        }

                        Medidores13.BorderBrush = Brushes.Gray;
                        Medidores14.BorderBrush = Brushes.LightGreen;
                        Medidores14.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores13.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores14_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores14.Text.Length == 16)
                    {
                        Medido14 = Medidores14.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI14 = Medido14.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI8 = Medido14.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores14.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI14 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI8 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores14.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores14.Text = MEDI14 ;
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores14.Text = MEDI8 ;
                        }

                        Medidores14.BorderBrush = Brushes.Gray;
                        Medidores15.BorderBrush = Brushes.LightGreen;
                        Medidores15.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores14.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Medidores15_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores15.Text.Length == 16)
                    {
                        Medido15 = Medidores15.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI15 = Medido15.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI9 = Medido15.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores15.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI15 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI9 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores15.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores15.Text = MEDI15;
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores15.Text = MEDI9;
                        }

                        Medidores15.BorderBrush = Brushes.Gray;
                        Medidores16.BorderBrush = Brushes.LightGreen;
                        Medidores16.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores15.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores16_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores16.Text.Length == 16)
                    {
                        Medido16 = Medidores16.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI16 = Medido16.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI10 = Medido16.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores16.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI16 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI10 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores16.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores16.Text = MEDI16;
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores16.Text = MEDI10;
                        }

                        Medidores16.BorderBrush = Brushes.Gray;
                        Medidores17.BorderBrush = Brushes.LightGreen;
                        Medidores17.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores16.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores17_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores17.Text.Length == 16)
                    {
                        Medido17 = Medidores17.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI17 = Medido17.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI11 = Medido17.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores17.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI17 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI11 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores17.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores17.Text = MEDI17 ;
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores17.Text = MEDI11 ;
                        }

                        Medidores17.BorderBrush = Brushes.Gray;
                        Medidores18.BorderBrush = Brushes.LightGreen;
                        Medidores18.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores17.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores18_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores18.Text.Length == 16)
                    {
                        Medido18 = Medidores18.Text;
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            MEDI18 = Medido18.Substring(0, 6);
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            MEDI12 = Medido18.Substring(0, 6);
                        }
                        c=2;
                    }
                    else if (Medidores18.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (name == "System.Windows.Controls.ComboBoxItem: 24")
                            {
                                MEDI18 = "000000";
                            }
                            else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                            {
                                MEDI12 = "000000";
                            }
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores18.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        if (name == "System.Windows.Controls.ComboBoxItem: 24")
                        {
                            Medidores18.Text = MEDI18 ;
                            Medidores18.BorderBrush = Brushes.Gray;
                            Medidores19.BorderBrush = Brushes.LightGreen;
                            Medidores19.Focus();
                        }
                        else if (name == "System.Windows.Controls.ComboBoxItem: 12")
                        {
                            Medidores18.Text = MEDI12 ;
                            Medidores18.BorderBrush = Brushes.Gray;
                            Gabinete.Focus();
                        }
                        c = 0;
                    }
                    else
                    {
                        Medidores18.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores19_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores19.Text.Length == 16)
                    {
                        Medido19 = Medidores19.Text;
                        MEDI19 = Medido19.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores19.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI19 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores19.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores19.Text = MEDI19 ;
                        Medidores19.BorderBrush = Brushes.Gray;
                        Medidores20.BorderBrush = Brushes.LightGreen;
                        Medidores20.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores19.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores20_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores20.Text.Length == 16)
                    {
                        Medido20 = Medidores20.Text;
                        MEDI20 = Medido20.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores20.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI20 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores20.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores20.Text = MEDI20 ;
                        Medidores20.BorderBrush = Brushes.Gray;
                        Medidores21.BorderBrush = Brushes.LightGreen;
                        Medidores21.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores20.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores21_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                   if (Medidores21.Text.Length == 16)
                    {
                        Medido21 = Medidores21.Text;
                        MEDI21 = Medido21.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores21.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI21 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores21.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores21.Text = MEDI21;
                        Medidores21.BorderBrush = Brushes.Gray;
                        Medidores22.BorderBrush = Brushes.LightGreen;
                        Medidores22.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores21.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores22_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores22.Text.Length == 16)
                    {
                        Medido22 = Medidores22.Text;
                        MEDI22 = Medido22.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores22.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI22 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores22.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores22.Text = MEDI22 ;
                        Medidores22.BorderBrush = Brushes.Gray;
                        Medidores23.BorderBrush = Brushes.LightGreen;
                        Medidores23.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores22.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores23_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (Medidores23.Text.Length == 16)
                    {
                        Medido23 = Medidores23.Text;
                        MEDI23 = Medido23.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores23.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI23 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores23.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores23.Text = MEDI23 ;
                        Medidores23.BorderBrush = Brushes.Gray;
                        Medidores24.BorderBrush = Brushes.LightGreen;
                        Medidores24.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores23.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void Medidores24_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                { if (Medidores24.Text.Length == 16)
                    {
                        Medido24 = Medidores24.Text;
                        MEDI24 = Medido24.Substring(0, 6);
                        c=2;
                    }
                    else if (Medidores24.Text.Length == 0)
                    {
                        if (MessageBox.Show("Aviso", "Esta vacia la posicion?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            MEDI24 = "000000";
                            c = 2;
                        }
                        else
                        {
                            MessageBox.Show("Escanee Medidor");
                            Medidores24.Focus();
                        }
                    }
                    if (c == 2)
                    {
                        Medidores24.Text = MEDI24 ;
                        Medidores24.BorderBrush = Brushes.Gray;
                        Gabinete.Focus();
                        c = 0;
                    }
                    else
                    {
                        Medidores24.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        } //
        private void GaB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Latitud.Focus();
            }
        }
        private void Latitud_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Latitud.Text.Length > 5)
                {
                    Longitud.Focus();
                }
                else if (Latitud.Text.Length == 0)
                {
                    MessageBox.Show("Ingrese Latitud");
                    Latitud.Focus();
                }
                else
                {
                    MessageBox.Show("Latitud incorrecta");
                    Latitud.Focus();
                }
            }
        }
        private void Longitud_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Longitud.Text.Length > 5)
                {
                    cboMedidores.Focus();
                }
                else if (Longitud.Text.Length == 0)
                {
                    MessageBox.Show("Ingrese Longitud");
                    Longitud.Focus();
                }
                else
                {
                    MessageBox.Show("Longitud incorrecta");
                    Longitud.Focus();
                }
            }

        }
        private void CboPuerto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboZona.Focus();
        }
        private void CboZona_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GaB.Focus();
        }
        private void Gabinete_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Gabinete.Text.Length > 42)
                {

                    string Gabi = Gabinete.Text;
                    name = Gabi.Substring(27, 16);
                    Gabinete.Text = name;
                    btnCarga.Focus();
                }
                else if (Gabinete.Text.Length == 0)
                {
                    MessageBox.Show("Escane Dispositivo");
                    Gabinete.Focus();
                }
                else
                {
                    MessageBox.Show("Codigo incorrecto");
                    Gabinete.Focus();
                }


            }
        }
    }
}
