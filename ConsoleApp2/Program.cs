using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {

            Console.WriteLine("Ingrese cantidad de Equipos y Partidos hasta el moemnto  separdos por espacio");
            string[] entrada = Console.ReadLine().Split(' ');
            int N = int.Parse(entrada[0]);
            int M = int.Parse(entrada[1]);
            int i = 0,j=0,k=1;
            string[] Equipos = new string [N];
            int[] Puntos = new int[N];
                
            do
            {
                Console.WriteLine("Ingrese el resultado del partido numero  " + k);
                string[] partido = Console.ReadLine().Split(' ');
                string eq0 = partido[0];
                string eqD = partido[1];
                int g0 = int.Parse(partido[2]);
                int gD = int.Parse(partido[3]);
                    bool prima = false;
                    j = 0;
                    do
                    {
                        if (Equipos[j]== null && prima== false)
                        {
                            Equipos[j] = eq0 ;
                            prima = true;
                        }
                        if ( Equipos[j] == eq0)
                        {
                        
                            if(g0 > gD)
                            {
                                Puntos[j] = Puntos[j] + 3;
                                break;
                            }
                            if (g0 == gD)
                            {
                                Puntos[j] = Puntos[j] + 1;
                                break;
                            }
                            if (g0 < gD)
                            {
                                break;
                            }
                        }
                        else
                        {
                            j++;
                        }

                    } while (j < N);
                    j = 0;
                    prima = false;
                    do
                    {
                        if (Equipos[j] == null)
                        {
                             Equipos[j] = eqD;
                            prima = true;
                        }
                        if ( Equipos[j] == eqD)
                        {
                            if (g0 < gD)
                            {
                                Puntos[j] = Puntos[j] + 3;
                                break;
                            }
                            if (g0 == gD)
                            {
                                Puntos[j] = Puntos[j] + 1;
                                break;
                            }
                            if (g0 > gD)
                            {
                                break;
                            }

                        }
                        else
                        {
                            j++;
                        }

                    } while (j < N);

                    i++;
                    k++;

            }
            while (i < M);

            i = 0;
            do
            {
                Console.WriteLine("El equipo " + Equipos[i] + " tiene " + Puntos[i] + " puntos");
                    i++;
            } while (i < N);

            Console.WriteLine();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
