using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pruebas_Unitarias_Cerebro
{
    class Program
    {
        private Utils.ParametrosConfiguracion parametrosConfiguracion;
        private DAO servicioLCRDao;

        public Program()
        {
            servicioLCRDao = new DAO();
            parametrosConfiguracion = Utils.ObtenerConfiguracionSistema(Environment.GetEnvironmentVariable("WINDIR") + @"\config.sigami");
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            Console.WriteLine("Escribe idRed");
            String idRed = Console.ReadLine();
            Console.WriteLine("Escribe CASO");
            String caso = Console.ReadLine();
            
            p.iniciaTareaLecturaCorteReconexion(idRed, caso);
            Console.WriteLine("");
        }

        public void iniciaTareaLecturaCorteReconexion(String red, String caso)
        {
            List<CEMAMedidor> medidores = null;
            int timeoutFuncionMs = parametrosConfiguracion.timeoutFuncionMedidor;
            int intentosFuncion = parametrosConfiguracion.intentosFuncionMedidor;
            int timeoutMatchMs = parametrosConfiguracion.timeoutMatchMedidor;
            int intentosMatch = parametrosConfiguracion.intentosMatchMedidor;
            bool match2F3F = parametrosConfiguracion.match2F3F;

            while (true)
            {
                #region LecturaRapida

//                            Logger.Instancia(Logger.TipoLog.TareasProgramadas).Info("Inicia Lectura Medidores del Gabinete: " + gabinete.idGabinete);
                medidores = servicioLCRDao.buscaMedidoresXRed(red);
//                            Logger.Instancia(Logger.TipoLog.TareasProgramadas).Info("Gabinete: " + gabinete.idGabinete + " con " + medidores.Count + " Medidores en su Red");
                do
                {
                    if (medidores.Count > 0)
                    {
                        medidores.RemoveAll(medidor => medidor.numeroMedidor == medidores.ElementAt(0).numeroMedidor);
                    }
                } while (medidores.Count > 0);


                #endregion

                #region Lectura y Busqueda

//                            Logger.Instancia(Logger.TipoLog.TareasProgramadas).Info("Inicia Lectura Medidores del Gabinete: " + gabinete.idGabinete + " con Caso: " + gabinete.idCaso);
                medidores = servicioLCRDao.buscaMedidoresXCaso(caso);
//                            Logger.Instancia(Logger.TipoLog.TareasProgramadas).Info("Gabinete: " + gabinete.idGabinete + " con " + medidores.Count + " Medidores en el Caso: " + gabinete.idCaso);
                do
                {
                    if (servicioLCRDao.esMedidorProcesado(medidores.ElementAt(0)))
                    {
                        medidores.RemoveAll(medidor => medidor.numeroMedidor == medidores.ElementAt(0).numeroMedidor);
                    }
                    else
                    {
                        if (medidores.Count > 0)
                        {
                            medidores.RemoveAll(medidor => medidor.numeroMedidor == medidores.ElementAt(0).numeroMedidor);
                        }
                    }
                } while (medidores.Count > 0);
            #endregion
            }
        }
    }
}
