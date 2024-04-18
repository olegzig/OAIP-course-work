using System;

namespace BinarySearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int size = Convert.ToInt32(Console.ReadLine());
        }
    }

    internal class SearchableArray
    {
        public int[] array;

        public SearchableArray(int size = 10)
        {
            array = new int[size];
            FillArray();
        }

        private void FillArray()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }
        }

        //Существует ради отладки
        public void Print()
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(i != array.Length - 1 ? array[i] + ',' : array[i]);
            }
        }
    }
}