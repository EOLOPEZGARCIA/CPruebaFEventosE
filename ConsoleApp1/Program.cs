using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 15;
            int b = 30;
            int c = a + b;
            Console.WriteLine("EL PRIMER NUMERO ES "+ a);
            Console.WriteLine("EL SEGUNDO NUMERO ES " + b);
            Console.WriteLine("LA SUMA DE AMBOS NUMEROS ES " + c);
            Console.WriteLine("Ingrese un numero");
            var d = Console.ReadLine();
            Console.WriteLine("Ingrese un segundo numero");
            var e = Console.ReadLine();
            float f;
            if(float.Parse(d) < float.Parse(e))
            {

                Console.WriteLine("El valor mayor de esos numeros es " + e);
            }
            else if (float.Parse(d) > float.Parse(e))
            {
                Console.WriteLine("El valor mayor de esos numeros es " + d);
            }
            else

                Console.WriteLine("Ambos numero son iguales");


            Console.WriteLine("Ingrese un dia de la semana en minuscula");
            var dia = Console.ReadLine();
            if (dia== "viernes" || dia == "sabado"|| dia == "domingo")
            {
                Console.WriteLine("Es fin de semana" );

            }
            else if (dia == "lunes" || dia == "martes" || dia == "miercoles"|| dia == "jueves")
            {
                Console.WriteLine("Es fin de semana");

            }
            else
            {
                Console.WriteLine("Error");
            }
            Console.WriteLine("Ingrese precio del producto");
            var x = Console.ReadLine();
            Console.WriteLine("Ingrese forma de pago: efectivo o tarjeta");
            var y = Console.ReadLine();
            if (y == "tarjeta")
            {
                Console.WriteLine("Ingrese numero de cuenta");
                var z = Console.ReadLine();

            }

            for (int i=0; i<101;i++)
            {
                Console.WriteLine(i);

            }
            int j = 0;
            int[] abc = new int[100];
            do
            {
                if ((j % 2) == 0 || (j % 3) == 0)
                {
                    Console.WriteLine(j);
                    abc[j] = j;
                }
                
                j++;

            }
            while (j < 101);
            Console.WriteLine(abc);

            Console.WriteLine("Ingrese primer numero");
            var aa = Console.ReadLine();
            int aa1 = int.Parse(aa);
            if(aa1 % 2 !=0)
            {
                aa1= aa1 * (-1);
            }
            Console.WriteLine("Ingrese segundo numero");
            var ab = Console.ReadLine();
            int ab1 = int.Parse(ab);
            if (ab1 % 2 != 0)
            {
                ab1 = ab1 * (-1);
            }
            Console.WriteLine("Ingrese tercero numero");
            var ac = Console.ReadLine();
            int ac1 = int.Parse(ac);
            if (ac1 % 2 != 0)
            {
                ac1 = ac1 * (-1);
            }
            Console.WriteLine("Ingrese cuarto numero");
            var ad = Console.ReadLine();
            int ad1 = int.Parse(ad);
            if (ad1 % 2 != 0)
            {
                ad1 = ad1 * (-1);
            }
            Console.WriteLine("Ingrese quinto numero");
            var ae = Console.ReadLine();
            int ae1 = int.Parse(ae);
            if (ae1 % 2 != 0)
            {
                ae1 = ae1 * (-1);
            }
            Console.WriteLine("Ingrese sexto numero");
            var af = Console.ReadLine();
            int af1 = int.Parse(af);
            if (af1 % 2 != 0)
            {
                af1 = af1 * (-1);
            }
            Console.WriteLine("Ingrese septimo numero");
            var ag = Console.ReadLine();
            int ag1 = int.Parse(ag);
            if (ag1 % 2 != 0)
            {
                ag1 = ag1 * (-1);
            }
            Console.WriteLine("Ingrese octavo numero");
            var ah = Console.ReadLine();
            int ah1 = int.Parse(ah);
            if (ah1 % 2 != 0)
            {
                ah1 = ah1 * (-1);
            }
            Console.WriteLine("Ingrese noveno numero");
            var ai = Console.ReadLine();
            int ai1 = int.Parse(ai);
            if (ai1 % 2 != 0)
            {
                ai1 = ai1 * (-1);
            }
            Console.WriteLine("Ingrese decimo numero");
            var aj = Console.ReadLine();
            int aj1 = int.Parse(aj);
            if (aj1 % 2 != 0)
            {
                aj1 = aj1 * (-1);
            }
            int az = aa1 + ab1 + ac1 + ad1 + ae1 + af1 + ag1 + ah1 + ai1 + aj1;
            Console.WriteLine("El resultado final es " + az);


            Console.WriteLine("Ingrese un numero del 1 al 7");
            switch
        }
    }
}
