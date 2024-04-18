using System;

namespace BinarySearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            Console.Write("Для начала работы программы, введите размер массива (по умолчанию 10): ");
            string input = Console.ReadLine();
            int.TryParse(input, out int size);
            SearchableArray array = new SearchableArray(size);
        }
    }

    internal class SearchableArray
    {
        public int[] array;

        public SearchableArray(int size)
        {
            array = new int[size == 0 ? 10 : size];
            FillArray();
            ShowPositiveMessage("Создание успешно! Размер: " + array.Length);

            if(array.Length <= 1000)
            {
                Print();
            }
            else
            {
                ShowInfoMessage("Так как размер больше 1000, отображатся массив не будет.");
            }
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

        public int FindPositionViaBinarySearch(int elementValue)
        {
            int left = 0;
            int right = array.Length - 1;
            int index = 0;

            while (left <= right)
            {
                index = (right + left) / 2;
                if (array[index] == elementValue)
                {
                    return index;
                } 

                if (array[index] < elementValue)
                {
                    left = index + 1;
                }
                else
                {
                    right = index - 1;
                }
            }
            return -1;
        }

        private static void ShowPositiveMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void ShowInfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}