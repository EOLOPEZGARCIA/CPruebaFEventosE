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

namespace CRC
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Crc16Ccitt crc = new Crc16Ccitt();
            Byte[] bte = { 0xee, 0x00, 0x00, 0x00, 0x00, 0x1b, 0x4f, 0x08, 0x66, 0x00, 0x00, 0x01, 0x00, 0x12, 0x4f, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x09, 0x08, 0x3f, 0x00, 0x00, 0xa7, 0xb8, 0xb8, 0x0a, 0x1e, 0x7a, 0xa1 };
            //Byte[] bte = { 0xee, 0xD0, 0x00, 0x00, 0x00, 0x05, 0x00, 0x04, 0x00, 0x80, 0x00 };
            ushort a = crc.CCITT_CRC16(bte);
            Console.Write("");
        }
    }
}
