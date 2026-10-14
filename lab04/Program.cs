using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<int> mainList = GenerateMainList(20, 1);

            Console.WriteLine("Початковий список");
            PrintList("Основний список", mainList);

            Console.WriteLine("\nЗавдання 1: Побудова за критерієм (кратні 3)");
            Task1_BuildByCriterion(mainList);

            Console.WriteLine("\nЗавдання 2: Вилучення елементів (кратні 3)");
            Task2_RemoveElements(mainList);

            Console.WriteLine("\nЗавдання 3: Злиття двох упорядкованих списків");
            Task3_MergeSortedLists();

            Console.WriteLine("\nЗавдання 4: Розділення на парні та непарні");
            Task4_SplitList(mainList);

            Console.WriteLine("\nЗавдання 5: Замір часу операцій");
            Task5_Benchmark();

            Console.WriteLine("\nДемонстрація крайових випадків");
            RunEdgeCases();
        }

        static List<int> GenerateMainList(int count, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(rnd.Next(-9, 10));
            }
            return list;
        }

        static void PrintList(string title, List<int> list)
        {
            Console.Write($"{title} (Count = {list.Count}): [");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i] + (i < list.Count - 1 ? ", " : ""));
            }
            Console.WriteLine("]");
        }

        static List<int> Task1_BuildByCriterion(List<int> input)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] % 3 == 0)
                {
                    result.Add(input[i]);
                }
            }
            PrintList("Елементи кратні 3", result);
            return result;
        }

        static void Task2_RemoveElements(List<int> input)
        {
            List<int> list1 = new List<int>(input);
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] % 3 == 0)
                {
                    list1.RemoveAt(i);
                }
            }
            PrintList("Варіант 1 (Хибний, з початку)", list1);

            List<int> list2 = new List<int>(input);
            for (int i = list2.Count - 1; i >= 0; i--)
            {
                if (list2[i] % 3 == 0)
                {
                    list2.RemoveAt(i);
                }
            }
            PrintList("Варіант 2 (Правильний, з кінця)", list2);

            List<int> list3 = new List<int>();
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] % 3 != 0)
                {
                    list3.Add(input[i]);
                }
            }
            PrintList("Варіант 3 (Новий список)", list3);

            bool areEqual = AreListsEqual(list2, list3);
            Console.WriteLine($"Результати Варіанта 2 та Варіанта 3 однакові: {areEqual}");
        }

        static bool AreListsEqual(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        static void Task3_MergeSortedLists()
        {
            int len1 = 10 + 1 % 5; 
            int len2 = 6 + 1 % 4; 

            List<int> list1 = GenerateSortedList(len1, 1);
            List<int> list2 = GenerateSortedList(len2, 101);

            PrintList("Список 1 (впорядкований)", list1);
            PrintList("Список 2 (впорядкований)", list2);

            List<int> merged = MergeLists(list1, list2);
            PrintList("Злитий список", merged);

            bool isSorted = CheckIsSorted(merged);
            Console.WriteLine($"Злитий список впорядкований: {isSorted}");
        }

        static List<int> GenerateSortedList(int count, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            if (count == 0) return list;

            int current = rnd.Next(1, 10);
            list.Add(current);

            for (int i = 1; i < count; i++)
            {
                current += rnd.Next(1, 10);
                list.Add(current);
            }
            return list;
        }

        static List<int> MergeLists(List<int> l1, List<int> l2)
        {
            List<int> result = new List<int>(l1.Count + l2.Count);
            int i = 0, j = 0;

            while (i < l1.Count && j < l2.Count)
            {
                if (l1[i] <= l2[j])
                {
                    result.Add(l1[i]);
                    i++;
                }
                else
                {
                    result.Add(l2[j]);
                    j++;
                }
            }

            while (i < l1.Count)
            {
                result.Add(l1[i]);
                i++;
            }

            while (j < l2.Count)
            {
                result.Add(l2[j]);
                j++;
            }

            return result;
        }

        static bool CheckIsSorted(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] > list[i + 1]) return false;
            }
            return true;
        }

        static void Task4_SplitList(List<int> input)
        {
            List<int> evenList = new List<int>();
            List<int> oddList = new List<int>();

            for (int i = 0; i < input.Count; i++)
            {
                if (input[i] % 2 == 0)
                {
                    evenList.Add(input[i]);
                }
                else
                {
                    oddList.Add(input[i]);
                }
            }

            PrintList("Парні елементи", evenList);
            PrintList("Непарні елементи", oddList);

            bool isCountValid = (evenList.Count + oddList.Count) == input.Count;
            Console.WriteLine($"Сума Count обох списків дорівнює вихідній: {isCountValid}");
        }

        static void Task5_Benchmark()
        {
            int n = 100000;

            List<int> warmup = new List<int>();
            for (int i = 0; i < 1000; i++) warmup.Add(i);
            for (int i = 0; i < 1000; i++) warmup.Insert(0, i);
            bool dummy = warmup.Contains(-1);

            Stopwatch sw = Stopwatch.StartNew();
            List<int> l1 = new List<int>();
            for (int i = 0; i < n; i++) l1.Add(i);
            sw.Stop();
            long t1 = sw.ElapsedMilliseconds;

            sw.Restart();
            List<int> l2 = new List<int>(n);
            for (int i = 0; i < n; i++) l2.Add(i);
            sw.Stop();
            long t2 = sw.ElapsedMilliseconds;

            sw.Restart();
            List<int> l3 = new List<int>();
            for (int i = 0; i < n; i++) l3.Insert(0, i);
            sw.Stop();
            long t3 = sw.ElapsedMilliseconds;

            sw.Restart();
            int notFound = -99999;
            for (int i = 0; i < 1000; i++)
            {
                bool exists = l2.Contains(notFound);
            }
            sw.Stop();
            long t4 = sw.ElapsedMilliseconds;

            Console.WriteLine($"100 000 x Add(x) (без місткості): {t1} мс");
            Console.WriteLine($"100 000 x Add(x) (з місткістю):  {t2} мс");
            Console.WriteLine($"100 000 x Insert(0, x):          {t3} мс");
            Console.WriteLine($"1 000 x Contains (відсутнє):      {t4} мс");
        }

        static void RunEdgeCases()
        {
            List<int> emptyList = new List<int>();
            List<int> singleList = new List<int> { 5 };

            Console.WriteLine("\nПорожній список");
            PrintList("Task 1 порожній", Task1_BuildByCriterion(emptyList));

            Console.WriteLine("\nЗлиття з порожнім списком");
            List<int> merged = MergeLists(new List<int> { 1, 3, 5 }, emptyList);
            PrintList("Злитий (один порожній)", merged);
        }
    }
}