using System;
using System.Diagnostics;

namespace BinarySearch
{
    internal class Program
    {
        private static JaggedArray array;

        private static void Main(string[] args)
        {
            Init();
            SelectSearchableValues();
            StartBenchmark();
        }

        private static void Init()
        {
            Console.Write("Для начала работы программы, введите размер массива (по умолчанию 100): ");
            string input = Console.ReadLine();
            int.TryParse(input, out int size);
            array = new JaggedArray(size);

            ConsoleManipulator.CLSAfterKeydown();
        }

        private static void SelectSearchableValues()
        {
            Console.Write("Введите значения для поиска (допускается ввод множ-ва значений через пробел): ");
            Search.setSearchValuesViaString(Console.ReadLine());

            ConsoleManipulator.CLSAfterKeydown();
        }

        private static void StartBenchmark()
        {
            ConsoleManipulator.ShowWarningMessage("Начинается бенчмарк. Ожидайте.");
            Benchmark.Start(array);
        }
    }

    internal class JaggedArray
    {
        public int[][] array;

        public JaggedArray(int size)
        {
            //Для установки значения по умолчанию
            size = size == 0 ? 100 : size;
            array = new int[ArraysAmount(size)][];
            FillArray(size);
            ConsoleManipulator.ShowPositiveMessage("Создание успешно! Количество: " + array.Length);

            if (array.Length < 100)
            {
                Print();
            }
            else
            {
                ConsoleManipulator.ShowInfoMessage("Так как размер больше 100, отображатся массивы не будут.");
            }
        }

        public int ArraysAmount(int size)
        {
            return ((size == 0 ? 10 : size) + 9) / 10;
        }

        private void FillArray(int size)
        {
            int currentSize = 0;
            for (int i = 0; i < array.Length; i++)
            {
                currentSize += 10;
                array[i] = new int[(size - currentSize) > 0 ? currentSize : size];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = j + 1;
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write('[');
                for (int j = 0; j < array[i].Length; j++)
                {
                    Console.Write(j != array[i].Length - 1 ? (array[i][j] + ",") : array[i][j]);
                }
                Console.WriteLine(']');
            }
        }
    }

    internal static class ConsoleManipulator
    {
        public static void ShowPositiveMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ShowMessage(message);
        }

        public static void ShowInfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            ShowMessage(message);
        }

        public static void ShowWarningMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            ShowMessage(message);
        }

        private static void ShowMessage(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void CLSAfterKeydown()
        {
            ShowInfoMessage("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    internal static class Search
    {
        private static int[] searchValues;

        public static int[] getSearchAmount()
        {
            return searchValues;
        }

        public static void setSearchValuesViaString(string str)
        {
            string[] stringArray = str.Split(' ');
            searchValues = new int[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                int.TryParse(stringArray[i], out searchValues[i]);
            }

            ConsoleManipulator.ShowPositiveMessage("Числа получены! Количество: " + searchValues.Length);
            if (searchValues.Length < 100)
            {
                Print();
            }
            else
            {
                ConsoleManipulator.ShowInfoMessage("Так как размер больше 100, отображатся массивы не будут.");
            }
        }

        public static void Print()
        {
            Console.Write('[');
            for (int i = 0; i < searchValues.Length; i++)
            {
                Console.Write(i != searchValues.Length - 1 ? (searchValues[i] + ",") : searchValues[i]);
            }
            Console.WriteLine(']');
        }

        public static int FindElPositionViaBinarySearch(int searchValuePosition, int[] array)
        {
            int left = 0;
            int right = array.Length - 1;
            int index = 0;

            while (left <= right)
            {
                index = (right + left) / 2;
                if (array[index] == searchValues[searchValuePosition])
                {
                    return index;
                }

                if (array[index] < searchValues[searchValuePosition])
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

        public static int FindElPositionByLinearSearch(int searchValuePosition, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == searchValues[searchValuePosition])
                {
                    return i;
                }
            }
            return -1;
        }
    }

    internal static class Benchmark
    {
        //[номер искомого в Search][При размере][Результаты - ms,result(bool)]
        private static long[][][] binarResults;

        private static long[][][] linearResults;


        public static void Start(JaggedArray array)
        {
            binarResults = new long[Search.getSearchAmount().Length][][];
            for(int i = 0; i < binarResults.Length;i++)
            {
                binarResults[i] = new long[array.array.Length][];
            }

            TestBinar(array.array);

            linearResults = new long[Search.getSearchAmount().Length][][];
            for (int i = 0; i < linearResults.Length; i++)
            {
                linearResults[i] = new long[array.array.Length][];
            }

            TestLinear(array.array);
        }

        private static void TestLinear(int[][] array)
        {
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < Search.getSearchAmount().Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    stopwatch.Start();
                    int result = Search.FindElPositionByLinearSearch(i, array[j]);
                    stopwatch.Stop();

                    linearResults[i][j] = new long[] { stopwatch.ElapsedMilliseconds, j, result == -1 ? 0 : 1 };

                }
            }
        }

        private static void TestBinar(int[][] array)
        {
            Stopwatch stopwatch = new Stopwatch();

            for (int i = 0; i < Search.getSearchAmount().Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    stopwatch.Start();
                    int result = Search.FindElPositionViaBinarySearch(i, array[j]);
                    stopwatch.Stop();

                    binarResults[i][j] = new long[] { stopwatch.ElapsedMilliseconds, j, result == -1 ? 0 : 1 };
                }
            }
        }
    }
}