using System;
using System.Collections.Generic;


public class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public int Score { get; set; }

    public Student(string name, string group, int score)
    {
        Name = name;
        Group = group;
        Score = score;
    }
}


public static class Algorithms
{
    public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
    {
        var result = new List<T>();
        foreach (var item in src)
        {
            if (keep(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
    {
        var result = new List<TOut>();
        foreach (var item in src)
        {
            result.Add(convert(item));
        }
        return result;
    }

    public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
    {
        TAcc acc = start;
        foreach (var item in src)
        {
            acc = step(acc, item);
        }
        return acc;
    }

    public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf)
    {
        var dict = new Dictionary<TKey, List<T>>();
        foreach (var item in src)
        {
            TKey key = keyOf(item);
            if (!dict.ContainsKey(key))
            {
                dict[key] = new List<T>();
            }
            dict[key].Add(item);
        }
        return dict;
    }
}


class Program
{
    static void Main()
    {
        var rnd = new Random(1);
        var numbers = new List<int>();
        for (int i = 0; i < 20; i++) numbers.Add(rnd.Next(-1, 2));

        var strings = new List<string> 
        { 
            "apple", "car", "ice-cream", "elephant", "dog", "umbrella", 
            "sky", "orange", "cat", "hi", "t-shirt", "ocean" 
        };

        var students = new List<Student>
        {
            new Student("Shevchenko", "КН-3/1", 95),
            new Student("Franko", "КН-3/1", 82),
            new Student("Lesya", "КН-3/2", 74),
            new Student("Kostenko", "КН-3/1", 65),
            new Student("Stus", "КН-3/3", 88)
        };

        Console.WriteLine("ЗАВДАННЯ 1: FILTER");
        var filteredNumbers = Algorithms.Filter(numbers, x => x % 3 == 0);
        Console.WriteLine($"Числа (кратні 3): {filteredNumbers.Count} шт. -> [{string.Join(", ", filteredNumbers)}]");

        var filteredStrings = Algorithms.Filter(strings, s => !string.IsNullOrEmpty(s) && "AEIOUYaeiouy".Contains(s[0]));
        Console.WriteLine($"Рядки (з голосної): {filteredStrings.Count} шт. -> [{string.Join(", ", filteredStrings)}]");

        var filteredStudents = Algorithms.Filter(students, s => s.Group == "КН-3/1");
        Console.WriteLine($"Студенти (КН-3/1): {filteredStudents.Count} шт.");


        Console.WriteLine("\nЗАВДАННЯ 2: MAP");
        var mappedNumbers = Algorithms.Map(numbers, x => Math.Abs(x % 3));
        Console.WriteLine($"Числа (остача на 3): [{string.Join(", ", mappedNumbers)}]");

        var mappedStrings = Algorithms.Map(strings, s => s.ToUpper());
        Console.WriteLine($"Рядки (Upper): [{string.Join(", ", mappedStrings)}]");

        var mappedStudents = Algorithms.Map(students, s => $"{s.Name} ({s.Score})");
        Console.WriteLine($"Студенти (формат): [{string.Join(", ", mappedStudents)}]");


        Console.WriteLine("\nЗАВДАННЯ 3: AGGREGATE");
        int sum = Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x);
        int prod = Algorithms.Aggregate(numbers, 1, (acc, x) => acc * x);
        int max = Algorithms.Aggregate(numbers, int.MinValue, (acc, x) => x > acc ? x : acc);
        int maxAbs = Algorithms.Aggregate(numbers, 0, (acc, x) => Math.Abs(x) > Math.Abs(acc) ? x : acc);

        Console.WriteLine($"Сума: {sum}");
        Console.WriteLine($"Добуток: {prod}");
        Console.WriteLine($"Максимум: {max}");
        Console.WriteLine($"Найбільший за модулем: {maxAbs}");


        Console.WriteLine("\nЗАВДАННЯ 4: GROUPBY");
        var byGroup = Algorithms.GroupBy(students, s => s.Group);
        Console.WriteLine("Студенти за групою:");
        foreach (var kvp in byGroup) Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} ст.");

        var byFirstLetter = Algorithms.GroupBy(students, s => s.Name[0]);
        Console.WriteLine("Студенти за 1-ю літерою:");
        foreach (var kvp in byFirstLetter) Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} ст.");

        var byScore = Algorithms.GroupBy(students, s => s.Score < 60 ? "0-59" : s.Score < 75 ? "60-74" : s.Score < 90 ? "75-89" : "90-100");
        Console.WriteLine("Студенти за балами:");
        foreach (var kvp in byScore) Console.WriteLine($"  {kvp.Key}: {kvp.Value.Count} ст.");

        var stringsByLen = Algorithms.GroupBy(strings, s => s.Length);
        Console.WriteLine("Рядки за довжиною:");
        foreach (var kvp in stringsByLen) Console.WriteLine($"  Довжина {kvp.Key}: {kvp.Value.Count} слів");


        Console.WriteLine("\nЗАВДАННЯ 5: ЗАХОПЛЕННЯ ЗМІННИХ (CLOSURES)");
        Task5_Closures();
    }

    static void Task5_Closures()
    {
        int threshold = 1;
        Func<int, bool> checkThreshold = x => x > threshold;
        threshold = 1000;
        Console.WriteLine($"1. Лямбда зі зміненим threshold (x=5): {checkThreshold(5)}"); 

        var lambdasFor = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            lambdasFor.Add(() => i);
        }
        Console.Write("2. Цикл for: ");
        foreach (var f in lambdasFor) Console.Write(f() + " ");
        Console.WriteLine();

        var lambdasForeach = new List<Func<int>>();
        int[] array = { 0, 1, 2 };
        foreach (var item in array)
        {
            lambdasForeach.Add(() => item);
        }
        Console.Write("3. Цикл foreach: ");
        foreach (var f in lambdasForeach) Console.Write(f() + " ");
        Console.WriteLine();
    }
}