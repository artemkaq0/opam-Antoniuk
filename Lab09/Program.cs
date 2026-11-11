using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public int Score { get; set; }

    public override string ToString() => $"{Name} ({Score})";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Лабораторна 9. Перевірки та вимірювання\n");
        RunEdgeCases();
        RunMeasurements(100);
        RunMeasurements(1000);
        RunMeasurements(10000);
        TestStability();
        FindAcceptabilityLimit();
    }

    static void RunEdgeCases()
    {
        Console.WriteLine("Перевірка крайових випадків");
        int[][] cases = {
            Array.Empty<int>(),
            new[] { 42 },
            new[] { 1, 2 },
            new[] { 2, 1 },
            new[] { 7, 7, 7, 7, 7 },
            new[] { 1, 1, 1, 2, 2, 2 }
        };

        foreach (var c in cases)
        {
            var testArr = (int[])c.Clone();
            Sorts.BottomUpMergeSort(testArr, (x, y) => x.CompareTo(y), out _, out _);
            bool isSorted = IsSorted(testArr, (x, y) => x.CompareTo(y));
            bool isPerm = IsPermutationOf(testArr, c);
            Console.WriteLine($"Вхід довжини {c.Length}: IsSorted={isSorted}, IsPermutation={isPerm}");
        }
        Console.WriteLine();
    }

    static void RunMeasurements(int n)
    {
        Console.WriteLine($"Розмір масиву N = {n}");
        var rnd = new Random(1);
        int[] baseArr = new int[n];
        for (int i = 0; i < n; i++) baseArr[i] = rnd.Next(0, 21);

        int[] randomArr = (int[])baseArr.Clone();
        int[] sortedArr = (int[])baseArr.Clone(); Array.Sort(sortedArr);
        int[] reverseArr = (int[])sortedArr.Clone(); Array.Reverse(reverseArr);
        int[] almostArr = (int[])sortedArr.Clone();
        Swap(almostArr, rnd.Next(n), rnd.Next(n)); // 1 обмін (K=1)

        RunAlgorithm("Вставками (Завдання 1)", Sorts.InsertionSort, randomArr, sortedArr, reverseArr, almostArr);
        RunAlgorithm("Злиттям знизу вгору (Завдання 2)", Sorts.BottomUpMergeSort, randomArr, sortedArr, reverseArr, almostArr);
        RunAlgorithm("Бінарні вставки (Завдання 3)", Sorts.BinaryInsertionSort, randomArr, sortedArr, reverseArr, almostArr);
        Console.WriteLine();
    }

    delegate void SortAction<T>(T[] a, Func<T, T, int> cmp, out int comp, out int moves);

    static void RunAlgorithm(string name, SortAction<int> algo, int[] r, int[] s, int[] rev, int[] alm)
    {
        Console.WriteLine($"{name}:");
        PrintStats("Випадковий", algo, r);
        PrintStats("Відсортований", algo, s);
        PrintStats("Навпаки", algo, rev);
        PrintStats("Майже", algo, alm);
    }

    static void PrintStats(string type, SortAction<int> algo, int[] input)
    {
        int[] arr = (int[])input.Clone();
        algo(arr, (x, y) => x.CompareTo(y), out int comp, out int moves);
        Console.WriteLine($"  {type,-15}: Порівнянь = {comp,-10} | Переставлянь = {moves}");
    }

    static void FindAcceptabilityLimit()
    {
        Console.WriteLine("Межа прийнятності (Просте сортування)");
        int size = 1000;
        var rnd = new Random(1);
        Stopwatch sw = new Stopwatch();

        while (true)
        {
            int[] arr = Enumerable.Range(0, size).Select(_ => rnd.Next(100)).ToArray();
            
            sw.Restart();
            Sorts.InsertionSort(arr, (x, y) => x.CompareTo(y), out _, out _);
            sw.Stop();

            if (sw.ElapsedMilliseconds > 1000)
            {
                Console.WriteLine($"Розмір масиву, при якому час перетнув 1 секунду: ~{size}");
                break;
            }
            size += 5000;
        }
    }

    static void TestStability()
    {
        Console.WriteLine("\nПеревірка стабільності");
        var students = new[]
        {
            new Student { Name = "Boyko", Score = 85 },
            new Student { Name = "Ivanov", Score = 90 },
            new Student { Name = "Koval", Score = 85 },
            new Student { Name = "Melnyk", Score = 90 },
            new Student { Name = "Shevchenko", Score = 85 }
        };

        var arr1 = (Student[])students.Clone();
        Sorts.InsertionSort(arr1, (x, y) => x.Score.CompareTo(y.Score), out _, out _);
        Console.WriteLine("Вставками: " + string.Join(", ", arr1.Select(s => s.Name)));

        var arr2 = (Student[])students.Clone();
        Sorts.BottomUpMergeSort(arr2, (x, y) => x.Score.CompareTo(y.Score), out _, out _);
        Console.WriteLine("Злиттям:   " + string.Join(", ", arr2.Select(s => s.Name)));

        var arr3 = (Student[])students.Clone();
        Array.Sort(arr3, (x, y) => x.Score.CompareTo(y.Score));
        Console.WriteLine("Array.Sort:" + string.Join(", ", arr3.Select(s => s.Name)));
    }

    static void Swap(int[] a, int i, int j) => (a[i], a[j]) = (a[j], a[i]);

    static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
    {
        for (int i = 1; i < a.Length; i++)
            if (cmp(a[i - 1], a[i]) > 0) return false;
        return true;
    }

    static bool IsPermutationOf<T>(T[] a, T[] original)
    {
        if (a.Length != original.Length) return false;
        var dict = new Dictionary<T, int>();
        foreach (var item in original)
        {
            if (!dict.ContainsKey(item)) dict[item] = 0;
            dict[item]++;
        }
        foreach (var item in a)
        {
            if (!dict.ContainsKey(item) || dict[item] == 0) return false;
            dict[item]--;
        }
        return true;
    }
}