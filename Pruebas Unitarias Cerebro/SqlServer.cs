using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas_Unitarias_Cerebro
{
    public class SqlServer
    {
        #region Atributos
        private string CadenaConexion = "";



        /// <summary>
        /// Variable de tipo bool que indica si la tabla de datos en donde se almacena el resultado de la consulta contiene por lo menos un registro
        /// </summary>    
        public bool HayDatos;

        /// <summary>
        /// Variable de tipo int que contiene el ultimo ID generado despues de ejecutar una consulta SQL de inserccion a una tabla con PK Identity
        /// </summary>
        public int UltimoIdInsertado;

        /// <summary>
        /// Variable de tipo DataTable que contiene el los registros devueltos por una consulta SQL
        /// </summary>    
        public DataTable TablaDeDatos;

        /// <summary>
        /// Variable de tipo int que contiene el numero de registros afectados en un transaccion de tipo Insert, Delete o Update
        /// </summary>    
        public int RegistrosAfectados;



        /// <summary>
        /// Conexion a la BD
        /// </summary>    
        public SqlConnection Conexion;

        /// <summary>
        /// Comando
        /// </summary>    
        private SqlCommand Comando = new SqlCommand();

        /// <summary>
        /// Es la transaccion que esta en curso para la Instancia.
        /// </summary>
        private SqlTransaction transaccion;
        #endregion
        #region Atributos Encapsulados
        public SqlTransaction Transaccion
        {
            get { return transaccion; }
            set
            {
                transaccion = value;
            }
        }
        #endregion
        #region Contructores
        public SqlServer(string cadenaconexion)
        {
            //Logger.Instancia(Logger.TipoLog.SqlServer).Info("Creando objeto para para la BD");
            CadenaConexion = cadenaconexion;
            Conexion = new SqlConnection(CadenaConexion);
            //Logger.Instancia(Logger.TipoLog.SqlServer).Trace(String.Format("Conexion [{0}]", Conexion.State));

        }
        #endregion

        #region Metodos
        public void Open()
        {
            try
            {
                //Logger.Instancia(Logger.TipoLog.SqlServer).Debug("Abriendo la BD");
                if (Conexion.State == ConnectionState.Open || Conexion.State == ConnectionState.Connecting)
                {
//                    Logger.Instancia(Logger.TipoLog.SqlServer).Warning("La conexion se encuentra en estado [{0}] y no se puede abrir.", Conexion.State);
                    return;
                }

                Conexion.Open();
                Comando = Conexion.CreateCommand();
                Comando.CommandType = CommandType.Text;
                Comando.CommandTimeout = 0;
                Comando.Connection = Conexion;
                //Logger.Instancia(Logger.TipoLog.SqlServer).Trace(String.Format("Estado de la Conexion [{0}]", Conexion.State));
            }
            catch (SqlException ex)
            {
                if (Conexion != null) Conexion.Close();
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error al abrir conexión en SqlServer.Open " + ex.Message));
                throw new Exception(ex.Message);

            }
        }

        public void Select(string cmd)
        {
            inicializaValores();
            try
            {
                Comando.CommandText = cmd;
                TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                HayDatos = TablaDeDatos.Rows.Count > 0;

            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.Select " + ex.Message));
                throw new Exception(ex.Message);
            }
        }

        public void InsertUpdateDelete(string cmd)
        {
            inicializaValores();
            try
            {
                Comando.CommandText = cmd;
                RegistrosAfectados = Comando.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.InsertUpdateDelete " + ex.Message));
                //throw new Exception(ex.Message);
                throw ex;
            }
        }

        public void Close()
        {
            Conexion.Close();
        }

        public void FullSelect(string cmd)
        {
            inicializaValores();
            SqlConnection Conexion = new SqlConnection(CadenaConexion);
            try
            {
                Conexion.Open();
                Comando = Conexion.CreateCommand();
                Comando.CommandType = CommandType.Text;
                Comando.Connection = Conexion;
                Comando.CommandTimeout = 0;
                Comando.CommandText = cmd;
                TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                HayDatos = TablaDeDatos.Rows.Count > 0;
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.FullSelect " + ex.Message));
                throw new Exception(ex.Message);
            }
            finally { if (Conexion != null) Conexion.Close(); }
        }

        public bool FullExisteRegistro(string cmd)
        {
            inicializaValores();
            SqlConnection Conexion = new SqlConnection(CadenaConexion);
            try
            {
                Conexion.Open();
                Comando = Conexion.CreateCommand();
                Comando.CommandType = CommandType.Text;
                Comando.Connection = Conexion;
                Comando.CommandTimeout = 0;
                Comando.CommandText = cmd;
                TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                return TablaDeDatos.Rows.Count > 0;
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.FullExisteRegistro " + ex.Message));
                return false;
            }
            finally { if (Conexion != null) Conexion.Close(); }
        }

        public bool ExisteRegistro(string cmd)
        {
            inicializaValores();
            try
            {
                Comando.CommandText = cmd;
                TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                return TablaDeDatos.Rows.Count > 0;
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.ExisteRegistro " + ex.Message));
                return false;
            }
        }

        public void FullInsertUpdateDelete(string cmd)
        {
            inicializaValores();
            SqlConnection Conexion = new SqlConnection(CadenaConexion);
            try
            {
                Conexion.Open();
                Comando = Conexion.CreateCommand();
                Comando.CommandType = CommandType.Text;
                Comando.Connection = Conexion;
                Comando.CommandTimeout = 0;
                Comando.CommandText = cmd;
                if (this.transaccion != null)
                    Comando.Transaction = this.transaccion;
                RegistrosAfectados = Comando.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.FullInsertUpdateDelete " + ex.Message));
                throw new Exception(ex.Message);
            }
            finally { if (Conexion != null) Conexion.Close(); }
        }



        /// <summary>
        /// Ejecuta un procedimiento almacenado o un enunciado SQL de inserccion y devuelve el ultimo id Insertado.
        /// </summary>
        /// 
        public int ObtieneUltimoId(string cmd_insert)
        {
            inicializaValores();
            Comando.CommandText = cmd_insert + "; SELECT  Scope_Identity() as UltimoId";
            try
            {
                Comando.CommandType = CommandType.Text;
                return Convert.ToInt32(Comando.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.ObtieneUltimoId " + ex.Message));
                return 0;
            }
        }

        public int FullObtieneUltimoId(string comadoInsert)
        {
            inicializaValores();
            SqlConnection Conexion = new SqlConnection(CadenaConexion);
            try
            {
                Conexion.Open();
                Comando = Conexion.CreateCommand();
                Comando.CommandType = CommandType.Text;
                Comando.Connection = Conexion;
                Comando.CommandTimeout = 0;
                Comando.CommandText = comadoInsert + "; SELECT  Scope_Identity() as UltimoId";
                return Convert.ToInt32(Comando.ExecuteScalar().ToString());
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.FullObtieneUltimoId " + ex.Message));
                return 0;
            }
            finally
            {
                if (Conexion != null) Conexion.Close();
            }
        }

        private void inicializaValores()
        {
            inicializaTabla();
            HayDatos = false;
            UltimoIdInsertado = 0;
            RegistrosAfectados = 0;
        }

        private void inicializaTabla()
        {
            if (TablaDeDatos != null)
            {
                TablaDeDatos.Dispose();
                TablaDeDatos = null;
            }
            TablaDeDatos = new DataTable();
        }

        public void FullEjecutaSP(string cmd, bool llenarTabla)
        {
            inicializaValores();
            Comando.CommandText = cmd;
            try
            {
                Comando.CommandType = CommandType.Text;
                Comando.Connection = Conexion;
                //Comando.CommandTimeout = 30000;
                Comando.CommandTimeout = 0;
                Conexion.Open();
                if (llenarTabla)
                {
                    TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                    HayDatos = TablaDeDatos.Rows.Count > 0;
                }
                else
                    HayDatos = Comando.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.FullEjecutaSP " + ex.Message));
                throw new Exception(ex.Message);
            }
            finally
            {
                Conexion.Close();
            }
        }

        public void ejecutaSP(string cmd, bool llenarTabla)
        {
            inicializaValores();
            Comando.CommandText = cmd;
            try
            {
                Comando.CommandTimeout = 0;
                if (llenarTabla)
                {
                    TablaDeDatos.Load(Comando.ExecuteReader(), LoadOption.OverwriteChanges);
                    HayDatos = TablaDeDatos.Rows.Count > 0;
                }
                else
                    HayDatos = Comando.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("Error en SqlServer.ejecutaSP " + ex.Message));
                throw new Exception(ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Se iniciar una transaccion
        /// </summary>
        /// <param name="nombreDeTransaccion">Nombre que se le asignara a la transaccion</param>
        public void IniciarTransaccion(String nombreDeTransaccion)
        {
            if (Conexion.State == ConnectionState.Open)
            {
                this.transaccion = this.Conexion.BeginTransaction(nombreDeTransaccion);
//                Logger.Instancia(Logger.TipoLog.SqlServer).Trace(String.Format("SQL - Iniciando la transaccion [{0}]", this.transaccion));
                Comando.Transaction = this.transaccion;
            }
            else
            {
//                Logger.Instancia(Logger.TipoLog.SqlServer).Error(String.Format("No se pudo inicializar la transaccion en SqlServer.IniciarTransaccion "));
                throw new SQLExceptionKavi("No se pudo inicializar la transaccion.");
            }
        }

        /// <summary>
        /// Ejecuta el commit de la transaccion.
        /// </summary>
        public void CommitTransaccion()
        {
            //Logger.Instancia(Logger.TipoLog.SqlServer).Info(String.Format("SQL - Commit a la transaccion [{0}]", this.transaccion));
            this.transaccion.Commit();
        }

        /// <summary>
        /// Ejecuta el roll back a la transaccion
        /// </summary>
        public void RollBackTransaccion()
        {
            this.transaccion.Rollback();
        }

        public bool TieneTransaccion()
        {
            return (this.transaccion == null) ? false : true;
        }

        #endregion
    }

    public class SQLExceptionKavi : Exception
    {
        #region Constructores
        public SQLExceptionKavi(String msj)
            : base(msj)
        {

        }
        #endregion
    }
}
