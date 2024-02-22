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

namespace Tramas_Elster
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Atributos atributos = new Atributos();
        SerialPort serialPort = new SerialPort();
        public MainWindow()
        {
            InitializeComponent();

            int lMarginTxt = 51, tMarginTxt = 108;
            int lMarginLblC = 21, tMarginLblC = 108;
            int lMarginTxtR = 51, tMarginTxtR = 208;
            int lMarginBtnE = 567, tMarginBtnE = 113;
            int lMarginTxtT = 567, tMarginTxtT = 143;
            int lMarginCbo = 567, tMarginCbo = 173;
            int lMarginChk = 567, tMarginChk = 203;

            CantidadInputs cI = new CantidadInputs();
            cI.ShowDialog();
            atributos = cI.atributos;

            for (int i = 0; i < int.Parse(atributos.inputs); i++)
            {
                Label lblContador = new Label();
                lblContador.Height = 30;
                lblContador.Width = 30;
                lblContador.HorizontalAlignment = HorizontalAlignment.Left;
                lblContador.VerticalAlignment = VerticalAlignment.Top;
                lblContador.Margin = new Thickness(lMarginLblC, tMarginLblC, 0, 0);
                lblContador.Content = i + 1;
                gridTextBox.Children.Add(lblContador);

                TextBox textBox = new TextBox();
                textBox.Height = 60;
                textBox.Width = 477;
                textBox.HorizontalAlignment = HorizontalAlignment.Left;
                textBox.VerticalAlignment = VerticalAlignment.Top;
                textBox.Margin = new Thickness(lMarginTxt, tMarginTxt, 0, 0);
                textBox.Name = "txt_" + i;
                textBox.Foreground = Brushes.Blue;
                gridTextBox.RegisterName(textBox.Name, textBox);
                gridTextBox.Children.Add(textBox);

                TextBox textBoxR = new TextBox();
                textBoxR.Height = 60;
                textBoxR.Width = 477;
                textBoxR.HorizontalAlignment = HorizontalAlignment.Left;
                textBoxR.VerticalAlignment = VerticalAlignment.Top;
                textBoxR.Margin = new Thickness(lMarginTxtR, tMarginTxtR, 0, 0);
                textBoxR.Name = "txtR_" + i;
                textBoxR.Foreground = Brushes.Red;
                gridTextBox.RegisterName(textBoxR.Name, textBoxR);
                gridTextBox.Children.Add(textBoxR);

                Button btnEnviaTramaI = new Button();
                btnEnviaTramaI.Height = 25;
                btnEnviaTramaI.Width = 80;
                btnEnviaTramaI.HorizontalAlignment = HorizontalAlignment.Left;
                btnEnviaTramaI.VerticalAlignment = VerticalAlignment.Top;
                btnEnviaTramaI.Margin = new Thickness(lMarginBtnE, tMarginBtnE, 0, 0);
                btnEnviaTramaI.Name = "btnEnviaTramaI_" + i;
                btnEnviaTramaI.Content = "Envia Trama";
                btnEnviaTramaI.Click += btnEnviaTramasI_Click;
                gridTextBox.RegisterName(btnEnviaTramaI.Name, btnEnviaTramaI);
                gridTextBox.Children.Add(btnEnviaTramaI);

                TextBox txtTimeOut = new TextBox();
                txtTimeOut.Height = 25;
                txtTimeOut.Width = 80;
                txtTimeOut.HorizontalAlignment = HorizontalAlignment.Left;
                txtTimeOut.VerticalAlignment = VerticalAlignment.Top;
                txtTimeOut.Margin = new Thickness(lMarginTxtT, tMarginTxtT, 0, 0);
                txtTimeOut.Name = "txtTime_" + i;
                gridTextBox.RegisterName(txtTimeOut.Name, txtTimeOut);
                gridTextBox.Children.Add(txtTimeOut);

                ComboBox cbo = new ComboBox();
                cbo.Height = 25;
                cbo.Width = 80;
                cbo.HorizontalAlignment = HorizontalAlignment.Left;
                cbo.VerticalAlignment = VerticalAlignment.Top;
                cbo.Margin = new Thickness(lMarginCbo, tMarginCbo, 0, 0);
                cbo.Name = "cbo_" + i;
                cbo.Items.Add("2400");
                cbo.Items.Add("4800");
                cbo.Items.Add("9600");
                cbo.Text = "2400";
                gridTextBox.RegisterName(cbo.Name, cbo);
                gridTextBox.Children.Add(cbo);

                CheckBox checkBox = new CheckBox();
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.VerticalAlignment = VerticalAlignment.Top;
                checkBox.Margin = new Thickness(lMarginChk, tMarginChk, 0, 0);
                checkBox.Name = "chk_" + i;
                checkBox.Content = "Enviar Trama";
                gridTextBox.RegisterName(checkBox.Name, checkBox);
                gridTextBox.Children.Add(checkBox);

                CheckBox checkBoxS = new CheckBox();
                checkBoxS.HorizontalAlignment = HorizontalAlignment.Left;
                checkBoxS.VerticalAlignment = VerticalAlignment.Top;
                checkBoxS.Margin = new Thickness(lMarginChk, tMarginChk + 20, 0, 0);
                checkBoxS.Name = "chkS_" + i;
                checkBoxS.Content = "Esperar Trama";
                gridTextBox.RegisterName(checkBoxS.Name, checkBoxS);
                gridTextBox.Children.Add(checkBoxS);

                tMarginTxt += 200;
                tMarginLblC += 200;
                tMarginTxtR += 200;
                tMarginBtnE += 200;
                tMarginTxtT += 200;
                tMarginCbo += 200;
                tMarginChk += 200;
            }

            Label lblPuerto = new Label();
            lblPuerto.Content = "Puerto COM : ";
            lblPuerto.HorizontalAlignment = HorizontalAlignment.Left;
            lblPuerto.VerticalAlignment = VerticalAlignment.Top;
            lblPuerto.Margin = new Thickness(30, 20, 0, 0);
            gridTextBox.Children.Add(lblPuerto);

            TextBox comboBox = new TextBox();
            comboBox.Width = 60;
            comboBox.Height = 20;
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.VerticalAlignment = VerticalAlignment.Top;
            comboBox.Margin = new Thickness(120, 20, 0, 0);
            comboBox.Name = "cboPort";
            comboBox.Text = "1";
            gridTextBox.RegisterName(comboBox.Name, comboBox);

            //            foreach (string puertoCom in SerialPort.GetPortNames())
            //                comboBox.Items.Add(puertoCom);

            gridTextBox.Children.Add(comboBox);

            Button btnAbrePuerto = new Button();
            btnAbrePuerto.Height = 25;
            btnAbrePuerto.Width = 80;
            btnAbrePuerto.HorizontalAlignment = HorizontalAlignment.Left;
            btnAbrePuerto.VerticalAlignment = VerticalAlignment.Top;
            btnAbrePuerto.Margin = new Thickness(20, 50, 0, 0);
            btnAbrePuerto.Name = "btnAbrePuerto";
            btnAbrePuerto.Content = "Abre puerto";
            btnAbrePuerto.Click += btnAbrePuerto_Click;
            gridTextBox.RegisterName(btnAbrePuerto.Name, btnAbrePuerto);
            gridTextBox.Children.Add(btnAbrePuerto);

            Button btnAbreIusa = new Button();
            btnAbreIusa.Height = 25;
            btnAbreIusa.Width = 80;
            btnAbreIusa.HorizontalAlignment = HorizontalAlignment.Left;
            btnAbreIusa.VerticalAlignment = VerticalAlignment.Top;
            btnAbreIusa.Margin = new Thickness(110, 50, 0, 0);
            btnAbreIusa.Name = "btnAbreIusa";
            btnAbreIusa.Content = "Abre Iusa";
            btnAbreIusa.Click += btnAbreIusa_Click;
            gridTextBox.RegisterName(btnAbreIusa.Name, btnAbreIusa);
            gridTextBox.Children.Add(btnAbreIusa);

            TextBox txtTimeOutG = new TextBox();
            txtTimeOutG.Height = 20;
            txtTimeOutG.Width = 120;
            txtTimeOutG.HorizontalAlignment = HorizontalAlignment.Left;
            txtTimeOutG.VerticalAlignment = VerticalAlignment.Top;
            txtTimeOutG.Margin = new Thickness(200, 20, 0, 0);
            txtTimeOutG.Name = "txtTimeG";
            gridTextBox.RegisterName(txtTimeOutG.Name, txtTimeOutG);
            gridTextBox.Children.Add(txtTimeOutG);

            Button btnTimeG = new Button();
            btnTimeG.Height = 20;
            btnTimeG.Width = 60;
            btnTimeG.Click += btnAplicaTimeG_Click;
            btnTimeG.HorizontalAlignment = HorizontalAlignment.Left;
            btnTimeG.VerticalAlignment = VerticalAlignment.Top;
            btnTimeG.Margin = new Thickness(350, 20, 0, 0);
            btnTimeG.Name = "btnTimeG";
            btnTimeG.Content = "Aplica";
            gridTextBox.RegisterName(btnTimeG.Name, btnTimeG);
            gridTextBox.Children.Add(btnTimeG);

            CheckBox chkPT = new CheckBox();
            chkPT.Content = "Recibe Trama";
            chkPT.HorizontalAlignment = HorizontalAlignment.Left;
            chkPT.VerticalAlignment = VerticalAlignment.Top;
            chkPT.Margin = new Thickness(430, 20, 0, 0);
            chkPT.Name = "chkPT";
            gridTextBox.RegisterName(chkPT.Name, chkPT);
            gridTextBox.Children.Add(chkPT);

            TextBox txtTimePT = new TextBox();
            txtTimePT.Height = 20;
            txtTimePT.Width = 100;
            txtTimePT.HorizontalAlignment = HorizontalAlignment.Left;
            txtTimePT.VerticalAlignment = VerticalAlignment.Top;
            txtTimePT.Margin = new Thickness(430, 40, 0, 0);
            txtTimePT.Name = "txtTimePT";
            gridTextBox.RegisterName(txtTimePT.Name, txtTimePT);
            gridTextBox.Children.Add(txtTimePT);

            Button button = new Button();
            button.Content = "Enviar Tramas";
            button.Width = 85;
            button.Height = 25;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;
            button.Margin = new Thickness(560, 20, 0, 0);
            button.Click += btnEnviaTramas_Click;
            gridTextBox.Children.Add(button);
        }

        public void btnEnviaTramas_Click(object sender, EventArgs e)
        {
            try
            {
                List<Byte[]> cadenas = new List<Byte[]>();
                CheckBox checkBoxPT = (CheckBox)this.FindName("chkPT");
                TextBox textBoxPT = (TextBox)this.FindName("txtTimePT");
                bool primerTrama = true;

                if (checkBoxPT.IsChecked == true)
                {
                    primerTrama = false;
                    Thread.Sleep(int.Parse(textBoxPT.Text));
                    String respuestaCadena = "";

                    while (serialPort.BytesToRead > 0)
                    {
                        primerTrama = true;
                        respuestaCadena = respuestaCadena + serialPort.ReadExisting();
                        Thread.Sleep(100);
                    }
                }

                if (primerTrama)
                {
                    for (int i = 0; i < int.Parse(atributos.inputs); i++)
                    {
                        CheckBox checkBox = (CheckBox)this.FindName("chk_" + i);
                        CheckBox checkBoxS = (CheckBox)this.FindName("chkS_" + i);
                        bool continuar = false;

                        if (checkBox.IsChecked == true)
                        {
                            TextBox textbox = (TextBox)this.FindName("txt_" + i);
                            TextBox textboxR = (TextBox)this.FindName("txtR_" + i);
                            Byte[] cadena = new Byte[textbox.Text.Replace(" ", "").Length / 2];
                            String respuestaCadena = "";


                            for (int j = 0; j < textbox.Text.Replace(" ", "").Length / 2; j++)
                            {
                                cadena[j] = Convert.ToByte(int.Parse(textbox.Text.Replace(" ", "").Substring(j * 2, 2), System.Globalization.NumberStyles.HexNumber));
                            }

                            ComboBox cbo = (ComboBox)this.FindName("cbo_" + i);

                            serialPort.BaudRate = int.Parse(cbo.Text);
                            serialPort.Write(cadena, 0, cadena.Length);

                            Thread.Sleep(600);

                            if (checkBoxS.IsChecked == true)
                            {
                                byte[] respuestaSerial = null;
                                byte[] auxiliar = null;

                                while (serialPort.BytesToRead > 0)
                                {
                                    continuar = true;

                                    if (respuestaSerial == null)
                                    {
                                        respuestaSerial = new byte[serialPort.BytesToRead];
                                        serialPort.Read(respuestaSerial, 0, serialPort.BytesToRead);
                                    }
                                    else
                                    {
                                        auxiliar = new byte[respuestaSerial.Length];
                                        auxiliar = respuestaSerial;
                                        respuestaSerial = new byte[auxiliar.Length + serialPort.BytesToRead];
                                        Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                        byte[] bytes = new byte[serialPort.BytesToRead];
                                        serialPort.Read(bytes, 0, serialPort.BytesToRead);
                                        Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                    }
                                }
                                if (continuar == false)
                                    break;
                                else
                                    textboxR.Text = BitConverter.ToString(respuestaSerial, 0, respuestaSerial.Length).Replace("-", " ");
                            }
                            else
                            {
                                while (serialPort.BytesToRead > 0)
                                {
                                    respuestaCadena = respuestaCadena + serialPort.ReadExisting();
                                    Thread.Sleep(100);
                                }
                            }

                            Thread.Sleep(int.Parse(((TextBox)this.FindName("txtTime_" + i)).Text));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void btnEnviaTramasI_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBoxPT = (CheckBox)this.FindName("chkPT");
                TextBox textBoxPT = (TextBox)this.FindName("txtTimePT");
                Button btnEnviaTrama = (Button)sender;
                int i = int.Parse(btnEnviaTrama.Name.Split('_')[1]);
                bool primerTrama = true;

                if (checkBoxPT.IsChecked == true)
                {
                    primerTrama = false;
                    Thread.Sleep(int.Parse(textBoxPT.Text));
                    String respuestaCadena = "";

                    while (serialPort.BytesToRead > 0)
                    {
                        primerTrama = true;
                        respuestaCadena = respuestaCadena + serialPort.ReadExisting();
                        Thread.Sleep(100);
                    }
                }

                if (primerTrama)
                {

                    CheckBox checkBox = (CheckBox)this.FindName("chk_" + i);
                    CheckBox checkBoxS = (CheckBox)this.FindName("chkS_" + i);

                    if (checkBox.IsChecked == true)
                    {
                        TextBox textbox = (TextBox)this.FindName("txt_" + i);
                        TextBox textboxR = (TextBox)this.FindName("txtR_" + i);
                        Byte[] cadena = new Byte[textbox.Text.Replace(" ", "").Length / 2];
                        String respuestaCadena = "";


                        for (int j = 0; j < textbox.Text.Replace(" ", "").Length / 2; j++)
                        {
                            cadena[j] = Convert.ToByte(int.Parse(textbox.Text.Replace(" ", "").Substring(j * 2, 2), System.Globalization.NumberStyles.HexNumber));
                        }

                        ComboBox cbo = (ComboBox)this.FindName("cbo_" + i);

                        serialPort.BaudRate = int.Parse(cbo.Text);
                        serialPort.Write(cadena, 0, cadena.Length);

                        Thread.Sleep(int.Parse(((TextBox)this.FindName("txtTime_" + i)).Text));

                        if (checkBoxS.IsChecked == true)
                        {
                            byte[] respuestaSerial = null;
                            byte[] auxiliar = null;

                            while (serialPort.BytesToRead > 0)
                            {
                                if (respuestaSerial == null)
                                {
                                    respuestaSerial = new byte[serialPort.BytesToRead];
                                    serialPort.Read(respuestaSerial, 0, serialPort.BytesToRead);
                                }
                                else
                                {
                                    auxiliar = new byte[respuestaSerial.Length];
                                    auxiliar = respuestaSerial;
                                    respuestaSerial = new byte[auxiliar.Length + serialPort.BytesToRead];
                                    Array.Copy(auxiliar, respuestaSerial, auxiliar.Length);
                                    byte[] bytes = new byte[serialPort.BytesToRead];
                                    serialPort.Read(bytes, 0, serialPort.BytesToRead);
                                    Array.Copy(bytes, 0, respuestaSerial, auxiliar.Length, bytes.Length);
                                }
                            }
                                textboxR.Text = BitConverter.ToString(respuestaSerial, 0, respuestaSerial.Length).Replace("-", " ");
                        }
                        else
                        {
                            while (serialPort.BytesToRead > 0)
                            {
                                respuestaCadena = respuestaCadena + serialPort.ReadExisting();
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void btnAplicaTimeG_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < int.Parse(atributos.inputs); i++)
            {

                ((TextBox)this.FindName("txtTime_" + i)).Text = ((TextBox)this.FindName("txtTimeG")).Text;
            }
        }

        public void btnAbrePuerto_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
                TextBox comboBox = (TextBox)this.FindName("cboPort");
                serialPort.PortName = "COM" + comboBox.Text;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void btnAbreIusa_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.Close();
                TextBox comboBox = (TextBox)this.FindName("cboPort");
                serialPort.PortName = "COM" + comboBox.Text;
                serialPort.DataBits = 7;
                serialPort.Parity = Parity.Even;
                serialPort.StopBits = StopBits.One;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
