using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Tramas_Elster
{
    /// <summary>
    /// Lógica de interacción para CantidadInputs.xaml
    /// </summary>
    public partial class CantidadInputs : Window
    {
        public Atributos atributos = new Atributos();

        public CantidadInputs()
        {
            InitializeComponent();

            Closing += this.OnWindowClosing;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            this.atributos.inputs = txtInputs.Text;
        }

    }
}
