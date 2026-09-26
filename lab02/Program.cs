using System;

namespace Lab02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Лабораторна Робота 2");
            Console.WriteLine("Варіант: N = 1, K = 1, a = 1, b = 1, c = 1\n");

            int[] mainArray = GenerateArray(16, 0, 5, 1);

            Console.WriteLine("Завдання 1: Агрегація");
            Task1_Aggregation(mainArray);

            Console.WriteLine("\nЗавдання 2: Фільтрація (кратні 3)");
            int[] filteredArray = Task2_FilterDivisibleBy3(mainArray);
            PrintArray("Відфільтрований масив: ", filteredArray);

            Console.WriteLine("\nЗавдання 3: Найдовша серія");
            Task3_LongestSeries(mainArray);

            Console.WriteLine("\nЗавдання 4: Перестановка (зсув вліво на 1)");
            int[] arrayToShift = CopyArray(mainArray);
            PrintArray("До:    ", arrayToShift);
            Task4_ShiftLeftInPlace(arrayToShift);
            PrintArray("Після: ", arrayToShift);

            Console.WriteLine("\nЗавдання 5: Матрица 5x4");
            Task5_MatrixProcessing();

            Console.WriteLine("\nЗавдання 6: Крайові випадки");
            RunEdgeCasesTests();
        }

        static int[] GenerateArray(int size, int min, int max, int seed)
        {
            Random rnd = new Random(seed);
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rnd.Next(min, max + 1);
            }
            return arr;
        }

        static void PrintArray(string title, int[] arr)
        {
            Console.Write(title);
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("[ порожній масив ]");
                return;
            }
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }

        static int[] CopyArray(int[] source)
        {
            if (source == null) return null;
            int[] copy = new int[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                copy[i] = source[i];
            }
            return copy;
        }

        // завдання 1
        static void Task1_Aggregation(int[] arr)
        {
            PrintArray("Початковий масив: ", arr);

            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Результат не визначений: масив порожній.");
                return;
            }

            int sum = 0;
            int min = arr[0];
            int minIndex = 0;
            int max = arr[0];
            int maxIndex = 0;
            int zeroCount = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                sum += val;

                if (val < min)
                {
                    min = val;
                    minIndex = i;
                }

                if (val > max)
                {
                    max = val;
                    maxIndex = i;
                }

                if (val == 0)
                {
                    zeroCount++;
                }
            }

            double average = (double)sum / arr.Length;

            Console.WriteLine($"Сума: {sum}");
            Console.WriteLine($"Середнє значення: {average:F2}");
            Console.WriteLine($"Мінімум: {min} (індекс: {minIndex})");
            Console.WriteLine($"Максимум: {max} (індекс: {maxIndex})");
            Console.WriteLine($"Кількість нулів: {zeroCount}");
        }

        // завдання 2
        static int[] Task2_FilterDivisibleBy3(int[] arr)
        {
            if (arr == null || arr.Length == 0)
                return new int[0];

            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 3 == 0)
                {
                    count++;
                }
            }

            int[] result = new int[count];
            int resultIndex = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 3 == 0)
                {
                    result[resultIndex] = arr[i];
                    resultIndex++;
                }
            }

            return result;
        }

        // завдання 3
        static void Task3_LongestSeries(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Результат не визначений: масив порожній.");
                return;
            }

            int maxVal = arr[0];
            int maxLen = 1;
            int maxStartIndex = 0;

            int currentLen = 1;
            int currentStartIndex = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == arr[i - 1])
                {
                    currentLen++;
                }
                else
                {
                    if (currentLen > maxLen)
                    {
                        maxLen = currentLen;
                        maxVal = arr[currentStartIndex];
                        maxStartIndex = currentStartIndex;
                    }
                    currentLen = 1;
                    currentStartIndex = i;
                }
            }

            if (currentLen > maxLen)
            {
                maxLen = currentLen;
                maxVal = arr[currentStartIndex];
                maxStartIndex = currentStartIndex;
            }

            Console.WriteLine($"Значення: {maxVal}, Довжина серії: {maxLen}, Початковий індекс: {maxStartIndex}");
        }

        // завдання 4
        static void Task4_ShiftLeftInPlace(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return;
            }

            int firstElement = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            arr[arr.Length - 1] = firstElement;
        }

        // завдання 5
        static void Task5_MatrixProcessing()
        {
            int rows = 5;
            int cols = 4;
            int[,] matrix = new int[rows, cols];
            Random rnd = new Random(1);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = rnd.Next(0, 6);
                }
            }

            Console.WriteLine("Матриця:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j],4}");
                }
                Console.WriteLine();
            }

            int maxSum = int.MinValue;
            int maxRowIndex = 0;

            Console.WriteLine("\nСуми рядків:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int rowSum = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    rowSum += matrix[i, j];
                }
                Console.WriteLine($"Рядок {i}: сума = {rowSum}");

                if (rowSum > maxSum)
                {
                    maxSum = rowSum;
                    maxRowIndex = i;
                }
            }
            Console.WriteLine($"\nНомер рядка з найбільшою сумою: {maxRowIndex} (сума = {maxSum})");

            Console.WriteLine("\nМаксимуми стовпців:");
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int colMax = matrix[0, j];
                for (int i = 1; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, j] > colMax)
                    {
                        colMax = matrix[i, j];
                    }
                }
                Console.WriteLine($"Стовпець {j}: максимум = {colMax}");
            }
        }

        // завдання 6
        static void RunEdgeCasesTests()
        {
            int[] empty = new int[0];
            int[] single = new int[] { 5 };
            int[] allSame = new int[] { 2, 2, 2, 2 };

            Console.WriteLine("\nТест 1: Порожній масив");
            Task1_Aggregation(empty);
            PrintArray("Завдання 2: ", Task2_FilterDivisibleBy3(empty));
            Task3_LongestSeries(empty);
            Task4_ShiftLeftInPlace(empty);

            Console.WriteLine("\nТест 2: Один елемент");
            Task1_Aggregation(single);
            PrintArray("Завдання 2: ", Task2_FilterDivisibleBy3(single));
            Task3_LongestSeries(single);
            Task4_ShiftLeftInPlace(single);
            PrintArray("Завдання 4 після зсуву: ", single);

            Console.WriteLine("\nТест 3: Усі елементи однакові");
            Task1_Aggregation(allSame);
            Task3_LongestSeries(allSame);
        }
    }
}