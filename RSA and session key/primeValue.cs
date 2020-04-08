using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_and_session_key
{
    //решето аткина https://www.cyberforum.ru/csharp-beginners/thread176749.html
    public static class primeValue
    {
        public static int genPrime(int limit)
        {
            bool[] is_prime = new bool[limit+1];
            int n;

            // Инициализация решета
            int sqr_lim = (int)Math.Sqrt(limit);
            for (int i = 0; i <= limit; i++) is_prime[i] = false;
            is_prime[2] = true;
            is_prime[3] = true;

            // Предположительно простые - это целые с нечетным числом
            // представлений в данных квадратных формах.
            // x2 и y2 - это квадраты i и j (оптимизация).
            int x2 = 0;
            for (int i = 1; i <= sqr_lim; i++)
            {
                x2 += 2 * i - 1;
                int y2 = 0;
                for (int j = 1; j <= sqr_lim; j++)
                {
                    y2 += 2 * j - 1;

                    n = 4 * x2 + y2;
                    if ((n <= limit) && (n % 12 == 1 || n % 12 == 5))
                        is_prime[n] = !is_prime[n];

                    // n = 3 * x2 + y2; 
                    n -= x2; // Оптимизация
                    if ((n <= limit) && (n % 12 == 7))
                        is_prime[n] = !is_prime[n];

                    // n = 3 * x2 - y2;
                    n -= 2 * y2; // Оптимизация
                    if ((i > j) && (n <= limit) && (n % 12 == 11))
                        is_prime[n] = !is_prime[n];
                }
            }

            // Отсеиваем квадраты простых чисел в интервале [5, sqrt(limit)].
            // (основной этап не может их отсеять)
            for (int i = 5; i <= sqr_lim; i++)
            {
                if (is_prime[i])
                {
                    n = i * i;
                    for (int j = n; j <= limit; j += n)
                    {
                        is_prime[j] = false;
                    }
                }
            }

            //модификация под RSA
            int max = 1; 
            for (int i = 2; i <= limit; i++)
            {
                if ((is_prime[i]))
                {
                    if(i>max && i != limit)
                    {
                        max = i;
                    }
                }
            }
            return max;
        }
    }
}
