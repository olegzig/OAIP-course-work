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
            Benchmark.PrintResults(array);
        }
    }

    internal class JaggedArray
    {
        public int[][] array;
        private const int step = 100;
        private const int defaultSize = 100;
        private const int maxSize = 700000; //узнал путём эксперементов

        public JaggedArray(int size)
        {
            size = ValidateSize(size);
            array = new int[ArraysAmount(size)][];
            FillArray(size);
            ConsoleManipulator.ShowPositiveMessage("Создание успешно! Количество: " + array.Length);

            if (array[array.Length - 1].Length < 100)
            {
                Print();
            }
            else
            {
                ConsoleManipulator.ShowInfoMessage("Так как размер больше 100, отображатся массивы не будут.");
            }
        }

        private int ValidateSize(int size)
        {
            if (size == 0)
            {
                ConsoleManipulator.ShowInfoMessage("Установлено значение по умолчанию: " + defaultSize);
                return defaultSize;
            }
            if (size > maxSize)
            {
                ConsoleManipulator.ShowWarningMessage("Значения выше " + maxSize + " приводят к заполнению оперативной памяти и зависанию ПК!");
                ConsoleManipulator.ShowInfoMessage("Установлено безопастное значение (выяснено эксперементальным путём. Кушает ~10.2 гигов оперативы): " + maxSize);
                return maxSize;
            }
            return size;
        }

        public int ArraysAmount(int size)
        {
            return ((size == 0 ? 10 : size) + 9) / step;
        }

        private void FillArray(int size)
        {
            int currentSize = 0;
            for (int i = 0; i < array.Length; i++)
            {
                currentSize += step;
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

        public static int[] getSearchValues()
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
        //[номер искомого в Search][При размере][Результаты - ticks,result(bool)]
        private static long[][][] binarResults;

        private static long binarMinimum = long.MaxValue;
        private static long binarMaximum = 0;
        private static double binarAverage;

        //[номер искомого в Search][При размере][Результаты - ticks,result(bool)]
        private static long[][][] linearResults;

        private static long linearMinimum = long.MaxValue;
        private static long linearMaximum = 0;
        private static double linearAverage;

        public static void Start(JaggedArray array)
        {
            binarResults = new long[Search.getSearchValues().Length][][];
            for (int i = 0; i < binarResults.Length; i++)
            {
                binarResults[i] = new long[array.array.Length][];
            }

            TestBinar(array.array);

            linearResults = new long[Search.getSearchValues().Length][][];
            for (int i = 0; i < linearResults.Length; i++)
            {
                linearResults[i] = new long[array.array.Length][];
            }

            TestLinear(array.array);
        }

        private static void TestLinear(int[][] array)
        {
            //вынесено отдельно, для того, чтобы шарпы не выделяли память (влияет на время. Проверено)
            int result;

            for (int i = 0; i < Search.getSearchValues().Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    result = Search.FindElPositionByLinearSearch(i, array[j]);
                    stopwatch.Stop();

                    linearResults[i][j] = new long[] { stopwatch.ElapsedTicks, result };
                }
            }
        }

        private static void TestBinar(int[][] array)
        {
            //вынесено отдельно, для того, чтобы шарпы не выделяли память (влияет на время. Проверено)
            int result;

            for (int i = 0; i < Search.getSearchValues().Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    result = Search.FindElPositionViaBinarySearch(i, array[j]);
                    stopwatch.Stop();

                    binarResults[i][j] = new long[] { stopwatch.ElapsedTicks, result };
                }
            }
        }

        public static void PrintResults(JaggedArray array)
        {
            Console.Clear();
            for (int i = 0; i < binarResults.Length; i++)
            {
                ConsoleManipulator.ShowInfoMessage("Результаты для значения \"" + Search.getSearchValues()[i] + "\":");
                for (int j = 0; j < binarResults[i].Length; j++)
                {
                    PerformansePrint(binarResults[i][j], linearResults[i][j], array.array[j]);
                    SetExtremes(binarResults[i][j], linearResults[i][j]);
                }
            }

            Console.WriteLine("\nИтоговая статистика:");
            CountAverage();

            Console.WriteLine("Среднее время линейного поиска: " + linearAverage);
            Console.WriteLine("Среднее время бинарного поиска: " + binarAverage);
            Console.WriteLine();
            Console.WriteLine("Максимальное время линейного поиска: " + linearMaximum);
            Console.WriteLine("Максимальное время бинарного поиска: " + binarMaximum);
            Console.WriteLine();
            Console.WriteLine("Минимальное время линейного поиска: " + linearMinimum);
            Console.WriteLine("Минимальное время бинарного поиска: " + binarMinimum);
        }

        private static void PerformansePrint(long[] binar, long[] linear, int[] array)
        {
            Console.Write("[0 - " + array.Length + "]: результат - ");
            Console.ForegroundColor = binar[1] >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(binar[1] >= 0 ? "Успешно! " : "Не найдено! ");
            Console.ResetColor();

            Console.WriteLine(" Время линейного: " + linear[0] + " ticks; Время бинарного: " + binar[0] + " ticks");
        }

        private static void SetExtremes(long[] binar, long[] linear)
        {
            if (binar[0] > binarMaximum)
            {
                binarMaximum = binar[0];
            }
            else if (binar[0] < binarMinimum)
            {
                binarMinimum = binar[0];
            }

            if (linear[0] > linearMaximum)
            {
                linearMaximum = linear[0];
            }
            else if (linear[0] < linearMinimum)
            {
                linearMinimum = linear[0];
            }
        }

        private static void CountAverage()
        {
            long bSum = 0;
            int counter = 0;
            long lSum = 0;
            for (int i = 0; i < binarResults.Length; i++)
            {
                for (int j = 0; j < binarResults[i].Length; j++)
                {
                    bSum += binarResults[i][j][0];

                    lSum += linearResults[i][j][0];
                    counter++;
                }
            }

            binarAverage = (double)bSum / counter;
            linearAverage = (double)lSum / counter;
        }
    }
}