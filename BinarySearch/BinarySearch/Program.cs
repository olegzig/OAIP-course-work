using System;
using System.Collections.Generic;

namespace BinarySearch
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = Convert.ToInt32(Console.ReadLine());
            int[] a = new int[size];
            List<int> list;

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 1;
            }
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine(a[i]);

            }
        }
    }
}
