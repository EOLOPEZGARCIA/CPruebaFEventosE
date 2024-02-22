using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas_Unitarias_Cerebro
{
    public static class Utils
    {

        public enum ResultadosObtenerConfiguracion
        {
            OK,
            NoExisteArchivo,
            ErrorEnCadenaDeConexion,
            ErrorFatal
        }
        public struct ParametrosConfiguracion
        {
            public ResultadosObtenerConfiguracion Resultado;
            public string dataBaseSQL;
            public string userSQL;
            public string passwordSQL;
            public string CadenaConexionBaseDatos;
            public string dataBaseINF;
            public string userINF;
            public string passwordINF;
            public string CadenaConexionBaseDatosINF;
            public string dataBaseCINT;
            public string userCINT;
            public string passwordCINT;
            public string CadenaConexionBaseDatosCINT;
            public int nivelLog;
            public string dirlog;
            public string dirxml;
            public string[] ParamKavi;
            public int NumSigAmi;
            public int monitorTareaLectura;
            public int intentosFuncionMedidor;
            public int timeoutFuncionMedidor;
            public int intentosMatchMedidor;
            public int timeoutMatchMedidor;
            public bool match2F3F;
            public bool lecturaG2;
            public int timeoutDescWeb;
            public bool archivoGabinetes;
            public string dirArchivoGabinetes;
            public string formatoFecha;
            public string horaTarea;
            public bool lecturaInicial;
            public bool todosMedidores;
            public string horaTareaCasos;
            public string diaCasos;
            public bool lecturaInicialCasos;
            public string dirArchivoCasos;
            public string tableroPuerto;
            public int tableroTimeout;
            public int tableroIntentos;
            public string horaTareaEventos;
            public string horaTareaConsultaInterrupciones;
            public int monitorAlarmas;
            public string horaAlarmasPendientes;
            public string horaTareaLecturasTransformador;

            public string ptoA0;
            public string ptoA1;
            public string ptoA2;
            public string ptoB0;
            public string ptoB1;
            public string ptoB2;
            public string ptoC0;
            public string ptoC1;
            public string ptoC2;
            public string ptoD0;
            public string ptoD1;
            public string ptoD2;
        }

        /// <summary>
        /// Radius of the Earth in Kilometers.
        /// </summary>
        private const double EARTH_RADIUS_KM = 6371;

        public static ParametrosConfiguracion ObtenerConfiguracionSistema(string ArchivoConfiguracion)
        {
            ParametrosConfiguracion parametros = new ParametrosConfiguracion();
            if (File.Exists(ArchivoConfiguracion))
            {
                using (StreamReader sr = new StreamReader(ArchivoConfiguracion))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] variables = linea.Split('=');
                        switch (variables[0].ToUpper().Trim())
                        {
                            case "DATABASESQL":
                                try
                                {
                                    parametros.dataBaseSQL = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.dataBaseSQL = "";
                                }
                                break;

                            case "USERSQL":
                                try
                                {
                                    parametros.userSQL = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.userSQL = "";
                                }
                                break;

                            case "PASSWORDSQL":
                                try
                                {
                                    parametros.passwordSQL = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.passwordSQL = "";
                                }
                                break;

                            case "SQLSERVER":
                                try
                                {
                                    parametros.CadenaConexionBaseDatos = @"Data Source=" + ((string)(variables.Length > 1 ? variables[1].Trim() : "(local)")) + "; database=" + parametros.dataBaseSQL + "; User ID=" + parametros.userSQL + "; Password=" + parametros.passwordSQL + "; Connection LifeTime=0;Pooling=true; Max Pool Size=3000;";
                                }
                                catch
                                {
                                    parametros.Resultado = ResultadosObtenerConfiguracion.ErrorEnCadenaDeConexion;
                                    break;
                                }
                                break;

                            case "DATABASEINF":
                                try
                                {
                                    parametros.dataBaseINF = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.dataBaseINF = "";
                                }
                                break;

                            case "USERINF":
                                try
                                {
                                    parametros.userINF = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.userINF = "";
                                }
                                break;

                            case "PASSWORDINF":
                                try
                                {
                                    parametros.passwordINF = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.passwordINF = "";
                                }
                                break;

                            case "INFSERVER":
                                try
                                {
                                    parametros.CadenaConexionBaseDatosINF = @"Data Source=" + ((string)(variables.Length > 1 ? variables[1].Trim() : "(local)")) + "; database=" + parametros.dataBaseINF + "; User ID=" + parametros.userINF + "; Password=" + parametros.passwordINF + "; Connection LifeTime=0;Pooling=true; Max Pool Size=3000;";
                                }
                                catch
                                {
                                    parametros.Resultado = ResultadosObtenerConfiguracion.ErrorEnCadenaDeConexion;
                                    break;
                                }
                                break;

                            case "DATABASECINT":
                                try
                                {
                                    parametros.dataBaseCINT = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.dataBaseCINT = "";
                                }
                                break;

                            case "USERCINT":
                                try
                                {
                                    parametros.userCINT = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.userCINT = "";
                                }
                                break;

                            case "PASSWORDCINT":
                                try
                                {
                                    parametros.passwordCINT = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.passwordCINT = "";
                                }
                                break;

                            case "CINTSERVER":
                                try
                                {
                                    parametros.CadenaConexionBaseDatosCINT = @"Data Source=" + ((string)(variables.Length > 1 ? variables[1].Trim() : "(local)")) + "; database=" + parametros.dataBaseCINT + "; User ID=" + parametros.userCINT + "; Password=" + parametros.passwordCINT + "; Connection LifeTime=0;Pooling=true; Max Pool Size=3000;";
                                }
                                catch
                                {
                                    parametros.Resultado = ResultadosObtenerConfiguracion.ErrorEnCadenaDeConexion;
                                    break;
                                }
                                break;

                            case "SQLCASOS":
                                try
                                {
                                    parametros.CadenaConexionBaseDatos = @"Data Source=" + ((string)(variables.Length > 1 ? variables[1].Trim() : "(local)")) + "; database=cemadb; User ID=cemauserdb; Password=c3m4p455db; Connection LifeTime=0;Pooling=false; Max Pool Size=3000;";
                                }
                                catch
                                {
                                    parametros.Resultado = ResultadosObtenerConfiguracion.ErrorEnCadenaDeConexion;
                                    break;
                                }
                                break;

                            case "NIVELLOG":
                                try
                                {
                                    parametros.nivelLog = int.Parse(variables[1]);
                                }
                                catch (Exception e)
                                {
//                                    parametros.nivelLog = (int)Logger.LogNivel.INFO;
                                }
                                break;

                            case "DIRLOG":
                                try
                                {
                                    parametros.dirlog = variables[1];
                                    if (String.IsNullOrEmpty(parametros.dirlog))
                                    {
//                                        Logger.Instancia().Warning("El directorio del log es invalido [{0}].", parametros.dirlog);
                                        parametros.dirlog = AppDomain.CurrentDomain.BaseDirectory;
                                    }
                                }
                                catch
                                {
                                    parametros.dirlog = AppDomain.CurrentDomain.BaseDirectory;
//                                    Logger.Instancia().Warning("No se pudo cargar el directorio del log se pone el del default [{0}].", parametros.dirlog);
                                }
                                break;
                            case "DIRXML":
                                try
                                {
                                    parametros.dirxml = variables[1];
                                    if (String.IsNullOrEmpty(parametros.dirxml))
                                    {
//                                        Logger.Instancia().Warning("El directorio del XML es invalido [{0}].", parametros.dirlog);
                                        parametros.dirlog = AppDomain.CurrentDomain.BaseDirectory;
                                    }
                                }
                                catch
                                {
                                    parametros.dirlog = AppDomain.CurrentDomain.BaseDirectory;
//                                    Logger.Instancia().Warning("No se pudo cargar el directorio del XML.", parametros.dirlog);
                                }
                                break;
                            case "URLKAVI":
                                try
                                {
                                    parametros.ParamKavi = variables[1].Split('|');
                                    if (String.IsNullOrEmpty(parametros.ParamKavi[0]))
                                    {
//                                        Logger.Instancia().Warning("No existe parámetros correctos para Kavi [{0}].", parametros.dirlog);
                                        parametros.dirlog = AppDomain.CurrentDomain.BaseDirectory;
                                    }
                                }
                                catch
                                {
                                    parametros.ParamKavi[0] = AppDomain.CurrentDomain.BaseDirectory;
//                                    Logger.Instancia().Warning("No se pudo cargar parámetros del Kavi", parametros.dirlog);
                                }

                                break;
                            case "MONITORLECTURA":
                                try
                                {
                                    parametros.monitorTareaLectura = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.monitorTareaLectura = 8 * 60 * 1000;
                                }
                                break;

                            case "TIMEOUTFUNC":
                                try
                                {
                                    parametros.timeoutFuncionMedidor = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.timeoutFuncionMedidor = 10 * 1000;
                                }
                                break;

                            case "INTENTOSFUNC":
                                try
                                {
                                    parametros.intentosFuncionMedidor = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.intentosFuncionMedidor = 3;
                                }
                                break;

                            case "TIMEOUTMATCH":
                                try
                                {
                                    parametros.timeoutMatchMedidor = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.timeoutMatchMedidor = 10 * 1000;
                                }
                                break;

                            case "INTENTOSMATCH":
                                try
                                {
                                    parametros.intentosMatchMedidor = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.intentosMatchMedidor = 3;
                                }
                                break;

                            case "MATCH2F3F":
                                try
                                {
                                    parametros.match2F3F = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.match2F3F = false;
                                }
                                break;

                            case "LECTURAG2":
                                try
                                {
                                    parametros.lecturaG2 = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.lecturaG2 = true;
                                }
                                break;

                            case "TIMEOUTDES":
                                try
                                {
                                    parametros.timeoutDescWeb = Int32.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.timeoutDescWeb = 10000;
                                }
                                break;

                            case "ARCHIVOGABINETES":
                                try
                                {
                                    parametros.archivoGabinetes = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.archivoGabinetes = false;
                                }
                                break;

                            case "DIRARCHIVOGABINETES":
                                try
                                {
                                    parametros.dirArchivoGabinetes = variables[1];
                                    if (String.IsNullOrEmpty(parametros.dirArchivoGabinetes))
                                    {
//                                        Logger.Instancia().Warning("El directorio del archivo de gabinetes es invalido [{0}].", parametros.dirArchivoGabinetes);
                                        parametros.dirArchivoGabinetes = AppDomain.CurrentDomain.BaseDirectory;
                                    }
                                }
                                catch
                                {
                                    parametros.dirArchivoGabinetes = AppDomain.CurrentDomain.BaseDirectory;
//                                    Logger.Instancia().Warning("No se pudo cargar el directorio del archivo de gabinetes.", parametros.dirArchivoGabinetes);
                                }
                                break;

                            case "FORMATOFECHA":
                                try
                                {
                                    parametros.formatoFecha = variables[1];
                                }
                                catch
                                {
                                    parametros.formatoFecha = "yyyy-dd-MM HH:mm:ss.fff";
                                }
                                break;

                            case "HORATAREA":
                                try
                                {
                                    parametros.horaTarea = variables[1];
                                }
                                catch
                                {
                                    parametros.horaTarea = "01:00";
                                }
                                break;

                            case "LECTURAINICIAL":
                                try
                                {
                                    parametros.lecturaInicial = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.lecturaInicial = false;
                                }
                                break;

                            case "TODOSMEDIDORES":
                                try
                                {
                                    parametros.todosMedidores = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.todosMedidores = false;
                                }
                                break;

                            case "HORACASOS":
                                try
                                {
                                    parametros.horaTareaCasos = variables[1];
                                }
                                catch
                                {
                                    parametros.horaTareaCasos = "22:00";
                                }
                                break;

                            case "DIACASOS":
                                try
                                {
                                    parametros.diaCasos = variables[1];
                                }
                                catch
                                {
                                    parametros.diaCasos = "LUNES";
                                }
                                break;

                            case "CASOSINICIAL":
                                try
                                {
                                    parametros.lecturaInicialCasos = bool.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.lecturaInicialCasos = false;
                                }
                                break;

                            case "DIRARCHIVOCASOS":
                                try
                                {
                                    parametros.dirArchivoCasos = variables[1];
                                    if (String.IsNullOrEmpty(parametros.dirArchivoCasos))
                                    {
//                                        Logger.Instancia().Warning("El directorio del archivo de gabinetes es invalido [{0}].", parametros.dirArchivoGabinetes);
                                        parametros.dirArchivoCasos = AppDomain.CurrentDomain.BaseDirectory;
                                    }
                                }
                                catch
                                {
                                    parametros.dirArchivoCasos = AppDomain.CurrentDomain.BaseDirectory;
//                                    Logger.Instancia().Warning("No se pudo cargar el directorio del archivo de gabinetes.", parametros.dirArchivoGabinetes);
                                }
                                break;

                            case "TABLEROPUERTO":
                                try
                                {
                                    parametros.tableroPuerto = variables[1];
                                }
                                catch
                                {
                                    parametros.tableroPuerto = "COM1";
                                }
                                break;

                            case "TABLEROTIMEOUT":
                                try
                                {
                                    parametros.tableroTimeout = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.tableroTimeout = 10000;
                                }
                                break;

                            case "TABLEROINTENTOS":
                                try
                                {
                                    parametros.tableroIntentos = int.Parse(variables[1]);
                                }
                                catch
                                {
                                    parametros.tableroIntentos = 5;
                                }
                                break;

                            case "HORAEVENTOS":
                                try
                                {
                                    parametros.horaTareaEventos = variables[1];
                                }
                                catch
                                {
                                    parametros.horaTareaEventos = "01:00";
                                }
                                break;

                            case "HORACONSULTAINTERRUPCIONES":
                                try
                                {
                                    parametros.horaTareaConsultaInterrupciones = variables[1];
                                }
                                catch
                                {
                                    parametros.horaTareaConsultaInterrupciones = "01:00";
                                }
                                break;

                            case "HORALECTURASTRANSFORMADOR":
                                try
                                {
                                    parametros.horaTareaLecturasTransformador = variables[1];
                                }
                                catch
                                {
                                    parametros.horaTareaLecturasTransformador = "01:00";
                                }
                                break;

                            case "PTOA0":
                                try
                                {
                                    parametros.ptoA0 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoA0 = "COM1";
                                }
                                break;

                            case "PTOA1":
                                try
                                {
                                    parametros.ptoA1 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoA1 = "COM2";
                                }
                                break;

                            case "PTOA2":
                                try
                                {
                                    parametros.ptoA2 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoA2 = "COM3";
                                }
                                break;

                            case "PTOB0":
                                try
                                {
                                    parametros.ptoB0 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoB0 = "COM4";
                                }
                                break;

                            case "PTOB1":
                                try
                                {
                                    parametros.ptoB1 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoB1 = "COM5";
                                }
                                break;

                            case "PTOB2":
                                try
                                {
                                    parametros.ptoB2 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoB2 = "COM6";
                                }
                                break;

                            case "PTOC0":
                                try
                                {
                                    parametros.ptoC0 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoC0 = "COM7";
                                }
                                break;

                            case "PTOC1":
                                try
                                {
                                    parametros.ptoC1 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoC1 = "COM8";
                                }
                                break;

                            case "PTOC2":
                                try
                                {
                                    parametros.ptoC2 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoC2 = "COM9";
                                }
                                break;

                            case "PTOD0":
                                try
                                {
                                    parametros.ptoD0 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoD0 = "COM10";
                                }
                                break;

                            case "PTOD1":
                                try
                                {
                                    parametros.ptoD1 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoD1 = "COM11";
                                }
                                break;

                            case "PTOD2":
                                try
                                {
                                    parametros.ptoD2 = variables[1];
                                }
                                catch (Exception e)
                                {
                                    parametros.ptoD2 = "COM12";
                                }
                                break;

                            default: break;
                        }
                    }
                    parametros.Resultado = ResultadosObtenerConfiguracion.OK;
                }
            }
            else
                parametros.Resultado = ResultadosObtenerConfiguracion.NoExisteArchivo;

            return parametros;
        }

        public static string ObtenerException(Exception ex)
        {
            string excepcion = LimpiarCadena(ex.Message);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                excepcion += LimpiarCadena(ex.Message);
            }
            return excepcion;
        }

        public static string LimpiarCadena(string cadena)
        {
            return cadena.Replace("'", "").Replace("\n\r", ". ").Replace("\r\n", ". ").Replace("\n", ". ").Replace("\r", ". ").Replace("\"", "");
        }

        public static void CrearArchivoTexto(string directorio, string nombreArchivo, string contenidoArchivo)
        {
            if (!Directory.Exists(directorio))
                Directory.CreateDirectory(directorio);

            StreamWriter sw = new StreamWriter(directorio + @"\" + nombreArchivo);
            sw.Write(contenidoArchivo);
            sw.Close();
            sw.Dispose();
        }

        public static bool EsLyFC(string claveDivision, string claveZona, string claveAgencia)
        {
            return
            (claveDivision.ToUpper().ToString().Equals("DL") ||
            claveDivision.ToUpper().ToString().Equals("DM") ||
            claveDivision.ToUpper().ToString().Equals("DN") ||
            (claveDivision.ToUpper().ToString().Equals("DG") && (claveZona.ToUpper().ToString().Equals("61") || (claveZona.ToUpper().ToString().Equals("61") && claveAgencia.ToUpper().ToString().Equals("Z")))) ||
            (claveDivision.ToUpper().ToString().Equals("DV") && (claveZona.ToUpper().ToString().Equals("11") || claveZona.ToUpper().ToString().Equals("12") || claveZona.ToUpper().ToString().Equals("13"))));
        }

        public static bool EsValle(string Division)
        {
            return (Division.ToUpper().ToString().Equals("DL") || Division.ToUpper().ToString().Equals("DM") || Division.ToUpper().ToString().Equals("DN"));
        }

        public static int ConvertirWh_A_kWh(string lecturaEnWh)
        {
            if (String.IsNullOrEmpty(lecturaEnWh))
                return 0;
            return ConvertirWh_A_kWh(double.Parse(lecturaEnWh));
        }

        public static int ConvertirWh_A_kWh(double lecturaEnWh)
        {
            return (int)((lecturaEnWh / 1000) % 100000);
        }

        public static int ConvertirDemanda(double lecturaEnWh, string procesoDemanda)
        {
            if (procesoDemanda.Equals("3"))
                return (int)Math.Ceiling(lecturaEnWh);
            else
                return (int)(1000 * lecturaEnWh);
        }

        public static string LimitarLongitudCadena(string cadena, int longitudMaxima)
        {
            return (cadena.Length > longitudMaxima ? cadena.Substring(0, longitudMaxima) : cadena);
        }

        public static string md5(string cadena)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(cadena));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
                sb.AppendFormat("{0:x2}", dataMd5[i]);
            return sb.ToString();
        }

        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        public static string GetLocalIPv4StatusUP()
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                            break;
                        }
                    }
                }
            }
            return output;
        }

        public static string obtenerIPEquipo()
        {
            //Traemos la IP Ethernet
            string ipCliente = Utils.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            //Si no está activo el Ethernet nos traemos el wireless
            ipCliente = (ipCliente == null || ipCliente.Equals("")) ? Utils.GetLocalIPv4(NetworkInterfaceType.Wireless80211) : ipCliente;
            //Si tampoco hay WiFi activo traemos el que esté UP
            ipCliente = (ipCliente == null || ipCliente.Equals("")) ? Utils.GetLocalIPv4StatusUP() : ipCliente;

            return ipCliente;
        }

        public static double ToRad(double input)
        {
            return input * (Math.PI / 180);
        }
/*
        public static double obtenDistancia2Puntos(System.Windows.Point punto1, System.Windows.Point punto2)
        {
            double dLat = ToRad(punto2.X - punto1.X);
            double dLon = ToRad(punto2.Y - punto1.Y);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRad(punto1.X)) * Math.Cos(ToRad(punto2.X)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EARTH_RADIUS_KM * c;
            return distance * 1000;
        }

        public static bool IsPointInPolygon(System.Windows.Point[] polygon, System.Windows.Point p)
        {
            double minX = polygon[0].X;
            double maxX = polygon[0].X;
            double minY = polygon[0].Y;
            double maxY = polygon[0].Y;
            for (int i = 1; i < polygon.Length; i++)
            {
                System.Windows.Point q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
            {
                return false;
            }

            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                     p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public static System.Windows.Point[] convierteStringPointArray(CoordinateCollection puntos)
        {
            System.Windows.Point[] arregloPuntos = new System.Windows.Point[puntos.Count];

            for (int i = 0; i < puntos.Count; i++)
            {
                arregloPuntos[i] = new System.Windows.Point();
                arregloPuntos[i].X = puntos.ElementAt(i).Latitude;
                arregloPuntos[i].Y = puntos.ElementAt(i).Longitude;
            }
            return arregloPuntos;
        }

        public static System.Windows.Point[] conviertePointArray(List<System.Windows.Point> puntos)
        {
            System.Windows.Point[] arregloPuntos = new System.Windows.Point[puntos.Count];

            for (int i = 0; i < puntos.Count; i++)
            {
                arregloPuntos[i] = new System.Windows.Point();
                arregloPuntos[i].X = puntos.ElementAt(i).X;
                arregloPuntos[i].Y = puntos.ElementAt(i).Y;
            }
            return arregloPuntos;
        }

        public static void creaDirectorio(String ruta, String nombreCarpeta)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(ruta, nombreCarpeta));
        }

        public static void borraDirectorioConArchivos(String rutaCarpeta)
        {
            Directory.Delete(rutaCarpeta, true);
        }

        public static void ZipFolder(string RootFolder, string CurrentFolder, ZipOutputStream zStream, bool flag)
        {
            string[] SubFolders = Directory.GetDirectories(CurrentFolder);
            string relativePath = "";

            //calls the method recursively for each subfolder
            foreach (string Folder in SubFolders)
            {
                ZipFolder(RootFolder, Folder, zStream, true);
            }

            if (flag)
            {
                relativePath = CurrentFolder.Substring(RootFolder.Length + 1) + "/";
            }
            else
            {
                relativePath = CurrentFolder.Substring(RootFolder.Length) + "/";
            }

            //the path "/" is not added or a folder will be created
            //at the root of the file
            if (relativePath.Length > 1)
            {
                ZipEntry dirEntry;
                dirEntry = new ZipEntry(relativePath);
                dirEntry.DateTime = DateTime.Now;
            }

            //adds all the files in the folder to the zip
            foreach (string file in Directory.GetFiles(CurrentFolder))
            {
                AddFileToZip(zStream, relativePath, file);
            }
        }

        public static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];

            //the relative path is added to the file in order to place the file within
            //this directory in the zip
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                      + Path.GetFileName(file);

            ZipEntry entry = new ZipEntry(fileRelativePath);
            entry.DateTime = DateTime.Now;
            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

        public static List<System.Windows.Point> puntoMedidorKML(List<System.Windows.Point> lineas)
        {
            List<System.Windows.Point> puntos = new List<System.Windows.Point>();

            System.Windows.Point punto = obtenPunto(lineas, "puntoMedidor");

            for (int i = 0; i < lineas.Count; i++)
            {
                System.Windows.Point nuevoPunto = new System.Windows.Point(lineas.ElementAt(i).Y - punto.Y, lineas.ElementAt(i).X - punto.X);
                puntos.Add(nuevoPunto);
            }
            return puntos;
        }

        public static List<System.Windows.Point> coordenadasMedidoresKML(List<System.Windows.Point> puntoMedidor, System.Windows.Point puntoPNG, int x, int y)
        {
            List<System.Windows.Point> puntos = new List<System.Windows.Point>();

            for (int i = 0; i < puntoMedidor.Count; i++)
            {
                System.Windows.Point p = new System.Windows.Point((puntoMedidor.ElementAt(i).X * x) / puntoPNG.Y, (y) - ((puntoMedidor.ElementAt(i).Y * y) / puntoPNG.X));
                puntos.Add(p);
            }

            return puntos;
        }

        public static System.Windows.Point obtenPunto(List<System.Windows.Point> lineas, String caso)
        {
            List<Double> x = new List<Double>();
            List<Double> y = new List<Double>();

            for (int i = 0; i < lineas.Count; i++)
            {
                y.Add(lineas.ElementAt(i).X);
                x.Add(lineas.ElementAt(i).Y);
            }

            x.Sort();
            y.Sort();

            if (caso.Equals("puntoA")) { return new System.Windows.Point(y.Max(), x.Min()); }
            if (caso.Equals("puntoB")) { return new System.Windows.Point(y.Min(), x.Max()); }
            if (caso.Equals("puntoC")) { return new System.Windows.Point(y.Min(), x.Max()); }
            if (caso.Equals("puntoD")) { return new System.Windows.Point(y.Min(), x.Min()); }
            if (caso.Equals("puntoE")) { return new System.Windows.Point(((y.Max() - y.Min()) / 2) + y.Min(), ((x.Max() - x.Min()) / 2) + x.Min()); }
            if (caso.Equals("puntoMedidor")) { return new System.Windows.Point(y.Min(), x.Min()); }
            if (caso.Equals("puntoPNG")) { return new System.Windows.Point(y.Max() - y.Min(), x.Max() - x.Min()); }

            return new System.Windows.Point();
        }

        public static PingReply ping(String ip, int timeout, byte[] buffer, PingOptions options)
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(ip, timeout, buffer, options);
            return reply;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
*/
    }
}
