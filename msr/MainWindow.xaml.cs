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

namespace msr
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
        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        private void btnTxt_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    String ruta = String.Empty;

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ruta = txtRuta.Text;
                    }));

                    string[] filePaths = Directory.GetFiles(ruta, "*.msr");

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        pgCantidad.Value = 0;
                        pgCantidad.Maximum = filePaths.Length;
                        lblCantidad.Content = pgCantidad.Value + " / " + pgCantidad.Maximum;
                        btnArchivo.IsEnabled = false;
                        btnTxt.IsEnabled = false;
                    }));

                    foreach (String filePath in filePaths)
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        try
                        {
                            MSRDao mSRDao = new MSRDao();
                            Byte[] bytes = File.ReadAllBytes(filePath);
                            List<Tabla> tablas = new List<Tabla>();
                            String numeroMedidor = String.Empty;
                            Byte[] numeroMedidorB = new byte[6];
                            int start = (bytes[258] < 0x70) ? (258 + 42) : (258 + 58);
                            int indice = start;
                            int posicion = 0;

                            DateTime ultimaEscrituraPO = new DateTime();
                            UInt16 sesionesEscrituraPO = 0;
                            UInt16 passwordInvalidoPO = 0;
                            DateTime ultimaEscrituraPI = new DateTime();
                            UInt16 sesionesEscrituraPI = 0;
                            UInt16 passwordInvalidoPI = 0;
                            DateTime ultimaFechaEscritura = new DateTime();
                            bool medidorEditado = false;
                            int numeroTotalResets = 0;
                            int diasUltimoReset = 0;
                            int diasUltimoPulso = 0;
                            DateTime fechaUltimoReset = new DateTime();
                            DateTime ultimoApagon = new DateTime();
                            DateTime ultimoEncendido = new DateTime();
                            int contadorApagones = 0;

                            Byte[] patron = this.StringToByteArray("45454D");
                            int i = 0;

                            do
                            {
                                posicion = this.posicion(bytes, patron, i);

                                if (bytes[posicion - 3] != 0x20)
                                {
                                    Array.Copy(bytes, posicion - 66, numeroMedidorB, 0, numeroMedidorB.Length);
                                    numeroMedidor = Encoding.ASCII.GetString(numeroMedidorB);
                                }
                                i = posicion;
                            } while ((bytes[posicion - 3] == 0x20));

                            patron = this.StringToByteArray("200000FFFFFFFF");
                            int fin = this.posicion(bytes, patron, 0);

                            while (indice < fin)
                            {
                                Tabla tabla = new Tabla();
                                tabla.numeroTabla = BitConverter.ToUInt16(bytes, indice);

                                indice += 8;
                                tabla.cantidadDatos = BitConverter.ToUInt32(bytes, indice);

                                indice += 10;
                                tablas.Add(tabla);
                            }
                            fin += 11;

                            foreach (Tabla t in tablas)
                            {
                                t.datos = new byte[t.cantidadDatos];
                                Array.Copy(bytes, fin, t.datos, 0, t.cantidadDatos);
                                fin += (int)(t.cantidadDatos);

                                if (t.numeroTabla == 0x0017)
                                {
                                    numeroTotalResets = t.datos[0];
                                }
                                if (t.numeroTabla == 0x0019)
                                {
                                    if (t.datos[0] == 0x00)
                                        fechaUltimoReset = new DateTime(2000, 1, 1);
                                    else
                                        fechaUltimoReset = new DateTime(2000 + t.datos[0], t.datos[1], t.datos[2], t.datos[3], t.datos[4], 0x00);
                                }
                                if (t.numeroTabla == 0x004a)
                                {

                                    int contadorDatosTabla = 0;
                                    int contadorEventos = 0;
                                    Int16 cantidadRenglones = BitConverter.ToInt16(t.datos, 1);
                                    contadorDatosTabla += 11;
                                    DateTime dt = DateTime.Now;
                                    String grupo = dt.Ticks.ToString();

                                    while (((contadorDatosTabla + 10) < t.datos.Length) && contadorEventos < cantidadRenglones)
                                    {
                                        try
                                        {
                                            Evento evento = new Evento();
                                            evento.fechaEvento = new DateTime((2000 + t.datos[contadorDatosTabla]), t.datos[contadorDatosTabla + 1], t.datos[contadorDatosTabla + 2], t.datos[contadorDatosTabla + 3], t.datos[contadorDatosTabla + 4], t.datos[contadorDatosTabla + 5]);
                                            contadorDatosTabla += 6;
                                            evento.numeroEvento = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 2;
                                            evento.usuario = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 4;
                                            evento.idEvento = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 2;
                                            contadorEventos += 1;
                                            mSRDao.insertaHistoryLog(numeroMedidor, evento, grupo, fileInfo.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            contadorEventos += 1;
                                            Logger.Instancia(Logger.TipoLog.ErrorGeneral).Error("Exception en LectorMSR.consultaDatos.Eventos 4a: " + ex.Message + " Traza: " + ex.StackTrace + " Source" + ex.Source + " Archivo: " + fileInfo.Name);
                                        }
                                    }
                                }
                                if (t.numeroTabla == 0x004c)
                                {
                                    int contadorDatosTabla = 0;
                                    int contadorEventos = 0;
                                    Int16 cantidadRenglones = BitConverter.ToInt16(t.datos, 1);
                                    contadorDatosTabla += 11;
                                    DateTime dt = DateTime.Now;
                                    String grupo = dt.Ticks.ToString();

                                    while (((contadorDatosTabla + 10) < t.datos.Length) && contadorEventos < cantidadRenglones)
                                    {
                                        try
                                        {
                                            Evento evento = new Evento();
                                            evento.fechaEvento = new DateTime((2000 + t.datos[contadorDatosTabla]), t.datos[contadorDatosTabla + 1], t.datos[contadorDatosTabla + 2], t.datos[contadorDatosTabla + 3], t.datos[contadorDatosTabla + 4], t.datos[contadorDatosTabla + 5]);
                                            contadorDatosTabla += 6;
                                            evento.numeroEvento = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 2;
                                            evento.usuario = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 2;
                                            evento.idEvento = BitConverter.ToUInt16(t.datos, contadorDatosTabla);
                                            contadorDatosTabla += 2;
                                            contadorEventos += 1;
                                            mSRDao.insertaEvento(numeroMedidor, evento, grupo, fileInfo.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            contadorEventos += 1;
                                            Logger.Instancia(Logger.TipoLog.ErrorGeneral).Error("Exception en LectorMSR.consultaDatos.Eventos 4c: " + ex.Message + " Traza: " + ex.StackTrace + " Source" + ex.Source + " Archivo: " + fileInfo.Name);
                                        }
                                    }
                                }
                                else if (t.numeroTabla == 0x0801)
                                {
                                    String a = Encoding.ASCII.GetString(t.datos, 34, 2);
                                }
                                else if (t.numeroTabla == 0x0803)
                                {
                                    if (t.datos[45] == 0x00)
                                        ultimaEscrituraPO = new DateTime(2000, 1, 1);
                                    else
                                        ultimaEscrituraPO = new DateTime(2000 + t.datos[45], t.datos[46], t.datos[47], t.datos[48], t.datos[49], t.datos[50]);
                                    sesionesEscrituraPO = BitConverter.ToUInt16(t.datos, 53);
                                    passwordInvalidoPO = t.datos[55];

                                    if (t.datos[56] == 0x00)
                                        ultimaEscrituraPI = new DateTime(2000, 1, 1);
                                    else
                                        ultimaEscrituraPI = new DateTime(2000 + t.datos[56], t.datos[57], t.datos[58], t.datos[59], t.datos[60], t.datos[61]);
                                    sesionesEscrituraPI = BitConverter.ToUInt16(t.datos, 64);
                                    passwordInvalidoPI = t.datos[66];
                                    diasUltimoReset = t.datos[70];
                                    diasUltimoPulso = t.datos[71];
                                    if (t.datos[72] == 0x00)
                                        ultimoApagon = new DateTime(2000, 1, 1);
                                    else
                                        ultimoApagon = new DateTime(2000 + t.datos[72], t.datos[73], t.datos[74], t.datos[75], t.datos[76], t.datos[77]);

                                    if (t.datos[78] == 0x00)
                                        ultimoEncendido = new DateTime(2000, 1, 1);
                                    else
                                        ultimoEncendido = new DateTime(2000 + t.datos[78], t.datos[79], t.datos[80], t.datos[81], t.datos[82], t.datos[83]);
                                    contadorApagones = t.datos[84];
                                }
                                else if (t.numeroTabla == 0x0808)
                                {
                                    if (t.datos[387] == 0x00)
                                        ultimaFechaEscritura = new DateTime(2000, 1, 1);
                                    else
                                        ultimaFechaEscritura = new DateTime(2000 + t.datos[387], t.datos[388], t.datos[389], t.datos[390], t.datos[391], t.datos[392]);
                                    medidorEditado = (t.datos[374] == 0x00) ? false : true;
                                }
                            }
                            mSRDao.insertaRegistroSeguridad(numeroMedidor, ultimaEscrituraPO, sesionesEscrituraPO, passwordInvalidoPO, ultimaEscrituraPI, sesionesEscrituraPI, passwordInvalidoPI, ultimaFechaEscritura, medidorEditado.ToString(), numeroTotalResets, diasUltimoReset, diasUltimoPulso, fechaUltimoReset, ultimoApagon, ultimoEncendido, contadorApagones, fileInfo.Name);
                        }
                        catch (Exception ex)
                        {
                            Logger.Instancia(Logger.TipoLog.ErrorGeneral).Error("Exception en LectorMSR.consultaDatos.foreach: " + ex.Message + " Traza: " + ex.StackTrace + " Source" + ex.Source + " Archivo: " + fileInfo.Name);
                        }

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            pgCantidad.Value += 1;
                            lblCantidad.Content = pgCantidad.Value + " / " + pgCantidad.Maximum;
                        }));
                    }

                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        btnArchivo.IsEnabled = true;
                        btnTxt.IsEnabled = true;
                    }));

                    MessageBox.Show("Proceso Terminado");
                }
                catch (Exception ex)
                {
                    Logger.Instancia(Logger.TipoLog.ErrorGeneral).Error("Exception en LectorMSR.consultaDatos: " + ex.Message + " Traza: " + ex.StackTrace + " Source" + ex.Source);
                }
            });
        }
    }
}
