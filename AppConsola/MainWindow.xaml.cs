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
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.IO;

namespace AppConsola
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

        private void Btn_Leer_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Btn_Leer.IsEnabled = false;
                string ruta = string.Empty;
                this.Dispatcher.Invoke((Action)(() => ruta = this.Txt_Prueba.Text));
                using (StreamWriter streamWriter = new StreamWriter(ruta + "\\Resultado.txt"))
                {
                    string[] filePaths = Directory.GetFiles(ruta, "*.ork");
                    
                    foreach (string str in filePaths)
                    {
                        FileInfo fileInfo = new FileInfo(str);
                        try
                        {
                            foreach (string line in System.IO.File.ReadLines(@"c:\test.txt"))
                            {
                                System.Console.WriteLine(line);
                                
                            }
                            System.Console.ReadLine();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                          //  streamWriter.WriteLine("Error: " + str);
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
        }
    }
}
