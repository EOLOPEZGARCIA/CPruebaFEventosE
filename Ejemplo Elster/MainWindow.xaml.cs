using IUSA.SAGCFE.SagTabCFE;
using System;
using System.Collections.Generic;
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

namespace Ejemplo_Elster
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (string s in SerialPort.GetPortNames())
            {
                //cboPuertos.Items.Add(s);
            }
        }

        SerialPort sp = new SerialPort();

        enum Tareas
        {
            Lectura = 1,
            Corte = 2,
            Reconexion = 3
        }

        private void btnLectura_Click(object sender, RoutedEventArgs e)
        {
            if(cboPuertos.Text != "")
            { 
                String portCom = cboPuertos.Text;
                String respuesta = "";
                String respuestaElster1 = "", respuestaElster2 = "";
                Byte[] lan = BitConverter.GetBytes(int.Parse(txtIdLan.Text));
                Array.Reverse(lan);

                Task.Factory.StartNew(() =>
                {
                    ManejaGraficos(0);

                        respuesta = TareaElster(1, portCom, lan);
                        if (respuesta != "")
                        {
                            respuestaElster1 = respuesta.Split('-')[1];
                            respuestaElster2 = respuesta.Split('-')[0];
                        }
                        else
                        {
                            respuestaElster1 = "N/A";
                            respuestaElster2 = "N/A";
                        }

                    ManejaGraficos(1);
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        txtRespuesta.Text = respuestaElster1;
                        txtNumSerie.Text = respuestaElster2;
                    }));
                });
            }
            else
                MessageBox.Show("Favor de Seleccionar un Puerto");
        }

        private void btnCorte_Click(object sender, RoutedEventArgs e)
        {
            if (cboPuertos.Text != "")
            {
                String portCom = cboPuertos.Text;
                String respuesta = "";
                String respuestaElster1 = "", respuestaElster2 = "";
                Byte[] lan = BitConverter.GetBytes(int.Parse(txtIdLan.Text));
                Array.Reverse(lan);

                Task.Factory.StartNew(() =>
                {
                    ManejaGraficos(0);

                        respuesta = TareaElster(2, portCom, lan);
                        if (respuesta != "")
                        {
                            respuestaElster1 = respuesta.Split('-')[1];
                            respuestaElster2 = respuesta.Split('-')[0];
                        }
                        else
                        {
                            respuestaElster1 = "N/A";
                            respuestaElster2 = "N/A";
                        }

                    ManejaGraficos(1);
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        txtRespuesta.Text = respuestaElster1;
                        txtNumSerie.Text = respuestaElster2;
                    }));
                });
            }
            else
                MessageBox.Show("Favor de Seleccionar un Puerto");
        }

        private void btnReconexion_Click(object sender, RoutedEventArgs e)
        {
            if (cboPuertos.Text != "")
            {

                String portCom = cboPuertos.Text;
                String respuesta = "";
                String respuestaElster1 = "", respuestaElster2 = "";
                Byte[] lan = BitConverter.GetBytes(int.Parse(txtIdLan.Text));
                Array.Reverse(lan);

                Task.Factory.StartNew(() =>
                {
                    ManejaGraficos(0);

                        respuesta = TareaElster(3, portCom, lan);
                        if (respuesta != "")
                        {
                            respuestaElster1 = respuesta.Split('-')[1];
                            respuestaElster2 = respuesta.Split('-')[0];
                        }
                        else
                        {
                            respuestaElster1 = "N/A";
                            respuestaElster2 = "N/A";
                        }

                    ManejaGraficos(1);
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        txtRespuesta.Text = respuestaElster1;
                        txtNumSerie.Text = respuestaElster2;
                    }));
                });
            }
            else
                MessageBox.Show("Favor de Seleccionar un Puerto");
        }

        private String TareaIusa(Tareas tarea, String portCom)
        {
            Autogestion iusa = new Autogestion(portCom, 5000);
            String Tarea = "";

            sp.Open();
            Thread.Sleep(100);
            sp.Write("I");
            sp.Close();
            Thread.Sleep(500);

            if (tarea == Tareas.Lectura)
            {
                var GX = iusa.Detecta_Generacion_Medidor(5000, "");

                if (GX.ToString() != "")
                {
                    var Lectura = iusa.Consultar_Lectura_Autogestion(GX, false);

                    if (Lectura.ToString() != "")
                        Tarea = Lectura.ToString() + " Wh";
                    else
                        Tarea = "Intente Nuevamente";
                }
                else
                    Tarea = "Intente Nuevamente";
            }
            else if (tarea == Tareas.Corte)
            {
                var Abrir = iusa.Ejecutar_AbrirRelevador(5000);
                //var Abrir = iusaMeter.Ejecutar_AbrirRelevador();

                if (Abrir.ToString() != "")
                    Tarea = Abrir.ToString();
                else
                    Tarea = "Intente Nuevamente";
            }
            else if (tarea == Tareas.Reconexion)
            {
                var Cerrar = iusa.Ejecutar_CerrarRelevador(5000);
                //var Cerrar = iusaMeter.Ejecutar_CerrarRelevador();

                if (Cerrar.ToString() != "")
                    Tarea = Cerrar.ToString();
                else
                    Tarea = "Intente Nuevamente";
            }
            return Tarea;
        }

        private String TareaElster(int tarea, String portCom, byte[]lan)
        {

            LibreriaElster libreria = new LibreriaElster();
            return libreria.EjecutaTareaElster(tarea, portCom, lan);
        }

        public void ManejaGraficos(int accion)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (accion == 0)
                {
                    txtRespuesta.Text = "";
                    txtNumSerie.Text = "";
                    btnLectura.IsEnabled = false;
                    btnCorte.IsEnabled = false;
                    btnReconexion.IsEnabled = false;
                }
                else
                {
                    btnLectura.IsEnabled = true;
                    btnCorte.IsEnabled = true;
                    btnReconexion.IsEnabled = true;
                }
            }));
        }

      
    }
}
