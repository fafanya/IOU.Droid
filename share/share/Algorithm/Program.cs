using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// settling-debts
/// </summary>
namespace share
{
    class Program
    {
        static void ReadInput(out int N, out int[] b)
        {
            //Console.Write("Количество долгов: ");
            const int K = 5; //Convert.ToInt32(Console.ReadLine());
            //Console.Write("Количество людей: ");
            N = 4; //Convert.ToInt32(Console.ReadLine());
            b = new int[N];

            for(int i = 0; i < N; i++)
            {
                b[i] = 0;
            }

            int[,] k = new int[K, 3] { { 0, 1, 2 }, { 2, 0, 8 }, { 3, 1, 32 }, { 0, 3, 8 }, { 3, 2, 6 } };




            Console.WriteLine("-------------------------------");
            for (int h = 0; h < K; h++)
            {
                int i = k[h, 0];
                int j = k[h, 1];
                int a = k[h, 2];
                b[i] = b[i] + a;
                b[j] = b[j] - a;
            }

        }

        static void WriteOutput(int N, int[] b)
        {
            int i = 0;
            int j = 0;
            int m = 0;

            while(i != N && j != N)
            {
                if(b[i] <= 0)
                {
                    i = i + 1;
                }
                else if(b[j] >= 0)
                {
                    j = j + 1;
                }
                else
                {
                    if(b[i] < -b[j])
                    {
                        m = b[i];
                    }
                    else
                    {
                        m = -b[j];
                    }
                    Console.WriteLine(i.ToString() + "   " + j.ToString() + "  :" + m.ToString());
                    b[i] = b[i] - m;
                    b[j] = b[j] + m;
                }
            }
        }

        static void DebtSettling()
        {
            /*int[] b;
            int N;
            ReadInput(out N, out b);*/
            int[] b = new int[6] {19, 17, 13, 5, -11, -43 };
            int N = 6;
            WriteOutput(N, b);
            Console.ReadLine();
        }
    }
}
