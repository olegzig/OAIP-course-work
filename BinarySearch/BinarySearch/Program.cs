using System;

namespace BinarySearch
{
    internal class Program
    {
        private static SearchableArray array;

        private static void Main(string[] args)
        {
            Init();
            SelectSearchableValues();
        }

        private static void Init()
        {
            Console.Write("Для начала работы программы, введите размер массива (по умолчанию 10): ");
            string input = Console.ReadLine();
            int.TryParse(input, out int size);
            array = new SearchableArray(size);

            CLSAfterKeydown();
        }

        private static void SelectSearchableValues()
        {
            Console.WriteLine();
        }

        private static void CLSAfterKeydown()
        {
            ConsoleMessages.ShowInfoMessage("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    internal class SearchableArray
    {
        public int[] array;

        public SearchableArray(int size)
        {
            array = new int[size == 0 ? 10 : size];
            FillArray();
            ConsoleMessages.ShowPositiveMessage("Создание успешно! Размер: " + array.Length);

            if (array.Length <= 1000)
            {
                Print();
            }
            else
            {
                ConsoleMessages.ShowInfoMessage("Так как размер больше 1000, отображатся массив не будет.");
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
    }

    internal static class ConsoleMessages
    {
        public static void ShowPositiveMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void ShowInfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}