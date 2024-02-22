using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] nums = {0,1,2,3,4,5,6,7,8};
            int c = 0;
            int b = nums.Length;
           // for (int z = 0; z < b-2; z++)
            //{
                int a = nums.Length;
                int[] aux = new int[a];
                aux[0] = nums[0];
                int ans = 0, ans2 = 0,au = 0, au2 = 0, ans3 = 0, au3 = 0;
                for (int j = 1; j < a; j++)
                {
                    au = nums[j];
                    au2 = aux[j - 1];
                ans = 0; ans2 = 0; ans3 = 0;
                    while (au>0)
                    {
                        au = au & (au - 1);
                        ans++;
                    }
                    while (au2 > 0)
                    {
                        au2 = au2 & (au2 - 1);
                        ans2++;
                    }
                    if (ans < ans2)
                    {
                        aux[j] = nums[j];
                    }
                    else
                    {
                        for (int i = 1; i <= j; i++)
                        {
                            aux[j + 1 - i] = aux[j - i];

                            if (j - i == 0)
                            {
                                aux[0] = nums[j];
                                break;
                            }
                            au3 = aux[j - 1-i];
                        ans3 = 0;
                            while (au3 > 0)
                            {
                                au3 = au3 & (au3 - 1);
                                ans3++;
                            }
                            if (ans < ans3 &&)
                            {
                                aux[j - i] = nums[j];
                                break;
                            }
                        }
                    }
                }
          



                /*
                Array.Resize(ref nums, a-1);
                nums[a-2] = aux[a-1] + aux[a-2];
                c = c + aux[a-1] + aux[a-2];
                if (z == 0)
                {
                    c = c + aux[a - 1] + aux[a - 2];
                }
                for (int y = 0; y < (a - 2); y++)
                {
                    nums[y] = aux[y];
                    if (z == 0)
                    {
                        c = c + aux[y];
                    }
                }
                }
                */
            
            Console.Write(aux);

            
       
        }
    }
}
