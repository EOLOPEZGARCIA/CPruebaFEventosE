using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas_Unitarias_Cerebro
{
    public class DAO
    {
        private Utils.ParametrosConfiguracion parametrosConfiguracion;

        public DAO()
        {
            parametrosConfiguracion = Utils.ObtenerConfiguracionSistema(Environment.GetEnvironmentVariable("WINDIR") + @"\config.sigami");
        }
        public Boolean esMedidorProcesado(CEMAMedidor medidor)
        {
            SqlServer sql = new SqlServer(parametrosConfiguracion.CadenaConexionBaseDatos);
            bool estaProcesado = false;
            string query = "SELECT NUMERO_MEDIDOR FROM C_MEDIDORES WHERE NUMERO_MEDIDOR = '" + medidor.numeroMedidor + "' " +
                            "AND ID_RED_UNION IS NOT NULL;";

            try
            {
                sql.Open();
                sql.Select(query);
                if (sql.HayDatos)
                {
                    estaProcesado = true;
                }
            }
            catch (Exception ex)
            {
                sql.Close();
                //Logger.Instancia(Logger.TipoLog.TareasProgramadas).Error("Exception en ServicioLecturaCorteReconexionDAO.esMedidorProcesado: " + ex.Message);
            }
            finally
            {
                sql.Close();
            }
            return estaProcesado;
        }

        public List<CEMAMedidor> buscaMedidoresXRed(String idRed)
        {
            SqlServer sql = new SqlServer(parametrosConfiguracion.CadenaConexionBaseDatos);
            List<CEMAMedidor> medidores = new List<CEMAMedidor>();

            string query = "SELECT DISTINCT(TL.NUMERO_MEDIDOR), TL.FECHA_INICIO_LECTURA, CM.DIRECCION_MEDIDOR 'MEDIDOR'" +
                            "FROM C_MEDIDORES CM " +
                            "INNER JOIN T_LECTURAS TL " +
                            "ON CM.NUMERO_MEDIDOR = TL.NUMERO_MEDIDOR " +
                            "WHERE ID_RED_UNION = '" + idRed + "' " +
                            "AND FECHA_INICIO_LECTURA = " +
                            "( " +
                                "SELECT MAX(TL2.FECHA_INICIO_LECTURA) " +
                                "FROM T_LECTURAS TL2 " +
                                "WHERE TL2.NUMERO_MEDIDOR = TL.NUMERO_MEDIDOR " +
                            ") " +
                            "ORDER BY TL.FECHA_INICIO_LECTURA ASC;";

            try
            {
                sql.Open();
                sql.Select(query);
                if (sql.HayDatos)
                {
                    DataTable tabla = sql.TablaDeDatos;

                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        CEMAMedidor medidor = new CEMAMedidor();
                        medidor.numeroMedidor = tabla.Rows[i]["MEDIDOR"].ToString();
                        medidores.Add(medidor);
                    }
                }
            }
            catch (Exception ex)
            {
                sql.Close();
//                Logger.Instancia(Logger.TipoLog.TareasProgramadas).Error("Exception en ServicioLecturaCorteReconexionDAO.buscaMedidoresXRed: " + ex.Message);
            }
            finally
            {
                sql.Close();
            }
            return medidores;
        }

        public List<CEMAMedidor> buscaMedidoresXCaso(String idCaso)
        {
            SqlServer sql = new SqlServer(parametrosConfiguracion.CadenaConexionBaseDatos);
            List<CEMAMedidor> medidores = new List<CEMAMedidor>();

            string query = "SELECT NUMERO_MEDIDOR, GETDATE() 'FECHA_INICIO_LECTURA', DIRECCION_MEDIDOR 'MEDIDOR'" +
                            "FROM C_MEDIDORES " +
                            "WHERE ID_CASO = " + idCaso + " " +
                            "AND ID_RED_UNION IS NULL " +
                            "AND NUMERO_MEDIDOR NOT IN " +
                            "( " +
                               "SELECT DISTINCT(TL.NUMERO_MEDIDOR) " +
                                "FROM C_MEDIDORES CM " +
                                "INNER JOIN T_LECTURAS TL " +
                                "ON CM.NUMERO_MEDIDOR = TL.NUMERO_MEDIDOR " +
                                "WHERE ID_CASO = " + idCaso + " " +
                                "AND ID_RED_UNION IS NULL " +
                            ")";

            string query2 = "SELECT DISTINCT(TL.NUMERO_MEDIDOR), TL.FECHA_INICIO_LECTURA, CM.DIRECCION_MEDIDOR 'MEDIDOR' " +
                            "FROM C_MEDIDORES CM " +
                            "INNER JOIN T_LECTURAS TL " +
                            "ON CM.NUMERO_MEDIDOR = TL.NUMERO_MEDIDOR " +
                            "WHERE ID_CASO = " + idCaso + " " +
                            "AND ID_RED_UNION IS NULL " +
                            "AND FECHA_INICIO_LECTURA = " +
                            "( " +
                                "SELECT MAX(TL2.FECHA_INICIO_LECTURA) " +
                                "FROM T_LECTURAS TL2 " +
                                "WHERE TL2.NUMERO_MEDIDOR = TL.NUMERO_MEDIDOR " +
                            ") " +
                            "ORDER BY TL.FECHA_INICIO_LECTURA ASC";

            try
            {
                sql.Open();
                sql.Select(query);
                if (sql.HayDatos)
                {
                    DataTable tabla = sql.TablaDeDatos;

                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        CEMAMedidor medidor = new CEMAMedidor();
                        medidor.numeroMedidor = tabla.Rows[i]["MEDIDOR"].ToString();
                        medidores.Add(medidor);
                    }
                }

                sql.Select(query2);
                if (sql.HayDatos)
                {
                    DataTable tabla = sql.TablaDeDatos;

                    for (int i = 0; i < tabla.Rows.Count; i++)
                    {
                        CEMAMedidor medidor = new CEMAMedidor();
                        medidor.numeroMedidor = tabla.Rows[i]["MEDIDOR"].ToString();
                        medidores.Add(medidor);
                    }
                }
            }
            catch (Exception ex)
            {
                sql.Close();
//                Logger.Instancia(Logger.TipoLog.TareasProgramadas).Error("Exception en ServicioLecturaCorteReconexionDAO.buscaMedidoresXCaso: " + ex.Message);
            }
            finally
            {
                sql.Close();
            }
            return medidores;
        }
    }
}
