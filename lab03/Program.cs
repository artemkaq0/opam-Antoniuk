using System;
using System.Collections.Generic;
using System.IO;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Завдання 1: Максимальна глибина вкладеності папок");
            RunTask1();

            Console.WriteLine("\nЗавдання 2: Кількість елементів > 1 (Розбиття навпіл)");
            RunTask2();

            Console.WriteLine("\nЗавдання 3: Підмножини з сумою 11 (Перебір із поверненням)");
            RunTask3();

            Console.WriteLine("\nЗавдання 4: Трасування стеку викликів (4 елементи)");
            RunTask4();

            Console.WriteLine("\nЗавдання 5: Ітеративна версія та порівняння");
            RunTask5();
        }

        static void RunTask1()
        {
            Console.Write("Введіть шлях до директорії (наприклад, . або C:\\): ");
            string inputPath = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(inputPath) || !Directory.Exists(inputPath))
            {
                Console.WriteLine("Вказана директорія не існує або шлях порожній.");
                return;
            }

            int depth = GetMaxFolderDepth(inputPath);
            Console.WriteLine($"Максимальна глибина вкладеності папок: {depth}");
        }

        static int GetMaxFolderDepth(string currentPath)
        {
            if (!Directory.Exists(currentPath)) return 0;

            int maxSubDepth = 0;
            try
            {
                string[] subDirectories = Directory.GetDirectories(currentPath);
                foreach (string dir in subDirectories)
                {
                    int subDepth = GetMaxFolderDepth(dir);
                    if (subDepth > maxSubDepth)
                    {
                        maxSubDepth = subDepth;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }

            return 1 + maxSubDepth;
        }

        static void RunTask2()
        {
            int[] array = GenerateArray(16, -20, 20, 1);
            Console.Write("Початковий масив (16 елементів): ");
            PrintArray(array);

            int maxRecursionDepth = 0;
            int count = CountGreaterThanOne(array, 0, array.Length - 1, 1, ref maxRecursionDepth);

            Console.WriteLine($"Кількість елементів, більших за 1: {count}");
            Console.WriteLine($"Максимальна глибина рекурсії: {maxRecursionDepth}");
            Console.WriteLine($"Теоретична глибина log2(16): {Math.Log2(16)}");
        }

        static int CountGreaterThanOne(int[] arr, int lo, int hi, int currentDepth, ref int maxDepth)
        {
            if (currentDepth > maxDepth)
            {
                maxDepth = currentDepth;
            }

            if (lo == hi)
            {
                return arr[lo] > 1 ? 1 : 0;
            }

            int mid = lo + (hi - lo) / 2;
            int leftCount = CountGreaterThanOne(arr, lo, mid, currentDepth + 1, ref maxDepth);
            int rightCount = CountGreaterThanOne(arr, mid + 1, hi, currentDepth + 1, ref maxDepth);

            return leftCount + rightCount;
        }

        static void RunTask3()
        {
            int[] array = GenerateArray(10, 1, 9, 1);
            Console.Write("Масив для перебору (10 елементів [1; 9]): ");
            PrintArray(array);

            int targetSum = 11; 
            Console.WriteLine($"\nУсі підмножини із сумою {targetSum}:");

            List<int> currentSubset = new List<int>();
            int totalSubsets = FindSubsetsWithSum(array, 0, 0, targetSum, currentSubset);

            Console.WriteLine($"Загальна кількість знайдених підмножин: {totalSubsets}");
        }

        static int FindSubsetsWithSum(int[] arr, int index, int currentSum, int targetSum, List<int> subset)
        {
            if (currentSum == targetSum)
            {
                PrintList(subset);
                return 1;
            }

            if (index >= arr.Length || currentSum > targetSum)
            {
                return 0;
            }

            int count = 0;

            subset.Add(arr[index]);
            count += FindSubsetsWithSum(arr, index + 1, currentSum + arr[index], targetSum, subset);

            subset.RemoveAt(subset.Count - 1);

            count += FindSubsetsWithSum(arr, index + 1, currentSum, targetSum, subset);

            return count;
        }

        static void RunTask4()
        {
            int[] smallArray = new int[] { 5, -2, 3, 8 };
            Console.Write("Масив для трасування (4 елементи): ");
            PrintArray(smallArray);
            Console.WriteLine("Трасування стеку викликів:");

            TraceCountGreaterThanOne(smallArray, 0, smallArray.Length - 1, 0);
        }

        static int TraceCountGreaterThanOne(int[] arr, int lo, int hi, int depth)
        {
            string indent = new string(' ', depth * 2);

            if (lo == hi)
            {
                int baseResult = arr[lo] > 1 ? 1 : 0;
                Console.WriteLine($"{indent}enter Count({lo}..{hi}) -> {baseResult}");
                return baseResult;
            }

            Console.WriteLine($"{indent}enter Count({lo}..{hi})");

            int mid = lo + (hi - lo) / 2;
            int left = TraceCountGreaterThanOne(arr, lo, mid, depth + 1);
            int right = TraceCountGreaterThanOne(arr, mid + 1, hi, depth + 1);
            int total = left + right;

            Console.WriteLine($"{indent}exit  Count({lo}..{hi}) -> {total}");
            return total;
        }

        static void RunTask5()
        {
            int[] array = GenerateArray(16, -20, 20, 1);

            int maxDepthDummy = 0;
            int recResult = CountGreaterThanOne(array, 0, array.Length - 1, 1, ref maxDepthDummy);
            int iterResult = CountGreaterThanOneIterative(array);

            Console.WriteLine($"Рекурсивний результат: {recResult}");
            Console.WriteLine($"Ітеративний результат: {iterResult}");
            Console.WriteLine($"Результати збігаються: {recResult == iterResult}");
        }

        static int CountGreaterThanOneIterative(int[] arr)
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 1)
                {
                    count++;
                }
            }
            return count;
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

        static void PrintArray(int[] arr)
        {
            Console.Write("[");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + (i < arr.Length - 1 ? ", " : ""));
            }
            Console.WriteLine("]");
        }

        static void PrintList(List<int> list)
        {
            Console.Write("  [ ");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i] + (i < list.Count - 1 ? ", " : ""));
            }
            Console.WriteLine(" ]");
        }
    }
}