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

namespace CalibracionMed
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Btn_Leer.IsEnabled = false;
                string ruta = string.Empty;
                this.Dispatcher.Invoke((Action)(() => ruta = this.Txt_Prueba.Text));
                using (StreamWriter streamWriter = new StreamWriter(ruta + "\\Resultado.txt"))

                using (StreamWriter streamWriter2 = new StreamWriter(ruta + "\\Resultado2.csv"))
                {
                    string[] filePaths = Directory.GetFiles(ruta, "*.RMC");
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.pgCantidad.Value = 0.0;
                        this.pgCantidad.Maximum = (double)filePaths.Length;
                    }));
                   // streamWriter.WriteLine("RPE FECHA   INICIO DURACIÓN    GRUPO TIPO    FABRICANTE ICLase  % Clase  Kh MESA    No.PATRÓN No.MEDIDOR No.SELLO ENCONTRADO BAJA ENCONTRADO ALTA ENCONTRADO INDUCTIVA DEJADO BAJA DEJADO ALTA DEJADO INDUCTIVA R. ENCONTRADO BAJA  R.ENCONTRADO ALTA  R.ENCONTRADO INDUCTIVA R.DEJADO BAJA  R.DEJADO ALTA  R.DEJADO INDUCTIVA P.RELAY COMENTARIO");
                    //streamWriter2.WriteLine("RPE FECHA   INICIO DURACIÓN    GRUPO TIPO    FABRICANTE ICLase  % Clase  Kh MESA    No.PATRÓN No.MEDIDOR No.SELLO ENCONTRADO BAJA ENCONTRADO ALTA ENCONTRADO INDUCTIVA DEJADO BAJA DEJADO ALTA DEJADO INDUCTIVA R. ENCONTRADO BAJA  R.ENCONTRADO ALTA  R.ENCONTRADO INDUCTIVA R.DEJADO BAJA  R.DEJADO ALTA  R.DEJADO INDUCTIVA P.RELAY COMENTARIO");

                    foreach (string str in filePaths)
                    {
                        FileInfo fileInfo = new FileInfo(str);
                        try
                        {
                            string[] archivo = File.ReadAllLines(str);
                            for (int i=1; i< archivo.Length;i++)
                            {
                                streamWriter2.WriteLine(archivo[i]);
                            }
                            foreach (string linea in archivo)
                            {
                                streamWriter.WriteLine(linea);
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
                MessageBox.Show("Listo ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_Leer.IsEnabled = true;
            }
            Btn_Leer.IsEnabled = true;

        }
    }
}
