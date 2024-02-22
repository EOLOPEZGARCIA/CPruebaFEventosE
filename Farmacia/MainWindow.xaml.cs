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

namespace Farmacia
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CboTipo.Text = " ";
           
            MessageBox.Show("Bienvenido Ingrese el nombre del medicamento");
            
            CboTipo.Items.Add("Analgésico");
            CboTipo.Items.Add("Analéptico");
            CboTipo.Items.Add("Anestésico");
            CboTipo.Items.Add("Antiácido");
            CboTipo.Items.Add("Antidepresivo");
            CboTipo.Items.Add("Antibiótico");
            TxtMedicamento.Focus();
        }

        private void Btn_Borrar_Click(object sender, RoutedEventArgs e)
        {
            byte[] a = new byte[] { 0x1e, 0x00 };
           
            
             int b= BitConverter.ToUInt16(a,0) ;
            TxtCantidad.Text = "";
            TxtMedicamento.Text = " ";
            CboTipo.Text = " ";
            Rb_Cemefar.IsChecked = false;
            Rb_Cofarma.IsChecked = false;
            Rb_Empsephar.IsChecked = false;
            Cb_Matriz.IsChecked = false;
            Cb_Suc.IsChecked = false;

        }

        private void Btn_Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Btn_Confirmar.IsEnabled = false;
                int a = 0;
                string distribuidor = "";
                int b = 0;
                string sucursal = "";
                if (Rb_Cemefar.IsChecked == true)
                {
                    a = 1;
                    distribuidor = "Cemefar";
                }
                if (Rb_Cofarma.IsChecked == true)
                {
                    a = 2;
                    distribuidor = "Cofarma";
                }
                if (Rb_Empsephar.IsChecked == true)
                {
                    a = 3;
                    distribuidor = "Empsephar";
                }
                if(Cb_Matriz.IsChecked ==true && Cb_Suc.IsChecked == false)
                {
                    b = 1;
                    sucursal = "Calle de la Rosa n. 28";
                }
                if (Cb_Matriz.IsChecked == false && Cb_Suc.IsChecked == true)
                {
                    b = 2;
                    sucursal = "Calle Alcazabilla n. 3. ";
                }
                if (Cb_Matriz.IsChecked == true && Cb_Suc.IsChecked == true)
                {
                    b = 3;
                    sucursal = "Calle de la Rosa n. 28 y la situada en Calle Alcazabilla n. 3. ";
                }
                if (TxtMedicamento.Text != null && TxtMedicamento.Text.All(char.IsLetterOrDigit))
                {
                    if (CboTipo.Text != " ")
                    {
                        if (int.Parse(TxtCantidad.Text) > 0)
                        {
                            if (a != 0)
                            {
                                if (b != 0)
                                {

                                    var result = MessageBox.Show(TxtCantidad.Text + "unidades del " + CboTipo.Text + " " + TxtMedicamento.Text + "\n" + "Para la farmacia situada en " + sucursal + " " + "\n", "Pedido al distribuidor " + distribuidor, MessageBoxButton.YesNo);
                                    if (result == MessageBoxResult.Yes)
                                    {
                                        MessageBox.Show("Pedido Enviado");
                                        Btn_Confirmar.IsEnabled = true;
                                    }
                                    else
                                    {
                                        System.Windows.Application.Current.Shutdown();
                                        Btn_Confirmar.IsEnabled = true;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Seleccione al menos una sucursal");
                                    Cb_Matriz.Focus();
                                    Btn_Confirmar.IsEnabled = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seleccione un distribuidor");
                                Rb_Cemefar.Focus();
                                Btn_Confirmar.IsEnabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingrese una cantidada de medicamento");
                            TxtCantidad.Focus();
                            Btn_Confirmar.IsEnabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un tipo de Medicamento");
                        CboTipo.Focus();
                        Btn_Confirmar.IsEnabled = true;

                    }
                }
                else
                {
                    MessageBox.Show("Ingrese un nombre de Medicamento valido");
                    TxtMedicamento.Focus();
                    Btn_Confirmar.IsEnabled = true;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Btn_Confirmar.IsEnabled = true;
            }
        }
    }
}
