using System;

namespace BinarySearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Для начала работы программы, введите размер массива (по умолчанию 10): ");
            string input = Console.ReadLine();
            int.TryParse(input, out int size);
            SearchableArray array = new SearchableArray(size);

            array.Print();
        }
    }

    internal class SearchableArray
    {
        public int[] array;

        public SearchableArray(int size)
        {
            array = new int[size == 0 ? 10 : size];
            FillArray();
            ShowPositiveMessage("Создание успешно!");
        }

        private void FillArray()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i + 1;
            }
        }

        //Существует ради отладки
        public void Print()
        {
            Console.Write('[');
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(i != array.Length - 1 ? (array[i] + ",") : array[i]);
            }
            Console.WriteLine(']');
        }

        private static void ShowPositiveMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ShowNegativeMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}