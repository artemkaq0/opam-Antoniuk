using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        int N = 1;
        string X = "А-00";
        string missingY = "Я-99";

        string[] array = GenerateArray(100_000, N);
        Array.Sort(array, string.Compare);
        string firstElement = array[0];

        Console.WriteLine($"Завдання 1 & 2: Пошуки");
        RunSearches(array, X, "Значення X (А-00)");
        RunSearches(array, firstElement, "Перший елемент масиву");
        RunSearches(array, missingY, "Відсутнє значення (Я-99)");

        Console.WriteLine($"\nЗавдання 2: Поломки");
        TestBrokenSearches(array);

        Console.WriteLine($"\nЗавдання 3: Пошук межі");
        TestBoundarySearch(array, X);

        Console.WriteLine($"\nЗавдання 5: Вимірювання (Час)");
        RunBenchmarks();
    }

    static void RunSearches(string[] a, string target, string title)
    {
        Console.WriteLine($"\nЗапит: {title} ('{target}')");
        
        int idx = Search.LinearSearch(a, target, string.Compare, out int compLin);
        Console.WriteLine($"Лінійний: індекс {idx}, порівнянь {compLin}");

        idx = Search.BinarySearch(a, target, string.Compare, out int compBin);
        Console.WriteLine($"Бінарний: індекс {idx}, порівнянь {compBin}");
        
        idx = Search.BinarySearchNoEquality(a, target, string.Compare, out int compBinNoEq);
        Console.WriteLine($"Бінарний (c=1): індекс {idx}, порівнянь {compBinNoEq}");
    }

    static void TestBrokenSearches(string[] a)
    {
        string[] smallArr = { "А-01", "Б-02", "В-03" };
        int res1 = Broken.BinarySearchBroken1(smallArr, "В-03", string.Compare);
        Console.WriteLine($"Поломка 1 (В-03 у масиві [А-01, Б-02, В-03]): індекс = {res1} (очікували 2)");

        Console.WriteLine("Поломка 2 (Зависання):");
        Broken.BinarySearchBroken2(smallArr, "В-03", string.Compare);

        string[] unsorted = GenerateArray(100_000, 1);
        int notFoundCount = 0;
        Random rand = new Random(42);
        for(int i = 0; i < 100; i++)
        {
            string target = unsorted[rand.Next(unsorted.Length)];
            if(Search.BinarySearch(unsorted, target, string.Compare, out _) == -1)
                notFoundCount++;
        }
        Console.WriteLine($"Поломка 4 (Невідсортований): з 100 наявних елементів не знайдено {notFoundCount}");
    }

    static void TestBoundarySearch(string[] a, string target)
    {
        int comps = 0;
        int lower = Search.BoundarySearch(a, x => { comps++; return string.Compare(x, target) >= 0; });
        
        int comps2 = 0;
        int upper = Search.BoundarySearch(a, x => { comps2++; return string.Compare(x, target) > 0; });

        int lastOccurrence = (upper - 1 >= 0 && a[upper - 1] == target) ? upper - 1 : -1;
        Console.WriteLine($"Нижня межа (LowerBound) для {target}: {lower}");
        Console.WriteLine($"Верхня межа (UpperBound) для {target}: {upper}");
        Console.WriteLine($"Індекс останнього входження {target} (Запит b=1): {lastOccurrence}");
        Console.WriteLine($"Кількість елементів {target}: {upper - lower}");
        Console.WriteLine($"Порівнянь для межі: {comps} (не залежить від кількості дублікатів)");

        int firstWith7 = Search.BoundarySearch(a, x => x.Contains("7"));
        Console.WriteLine($"Пошук межі для 'містить 7': повернуло індекс {firstWith7}. Значення: {(firstWith7 < a.Length ? a[firstWith7] : "немає")}");
        Console.WriteLine("Це значення хибне, оскільки умова 'містить 7' не ділить відсортований масив на дві суцільні частини (false...false, true...true).");
    }

    static void RunBenchmarks()
    {
        int[] qValues = { 1, 5, 10, 20, 30, 50, 100 };
        string[] array = GenerateArray(100_000, 1);
        string[] targets = GenerateArray(100, 2).Take(100).ToArray();

        Search.LinearSearch(array, targets[0], string.Compare, out _);
        string[] copyHeat = (string[])array.Clone();
        Array.Sort(copyHeat, string.Compare);
        Search.BinarySearch(copyHeat, targets[0], string.Compare, out _);

        Console.WriteLine($"q\tЛінійно (мс)\tСортування + бінарно (мс)\tХто швидший");
        
        foreach (int q in qValues)
        {
            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < q; i++)
            {
                Search.LinearSearch(array, targets[i % 100], string.Compare, out _);
            }
            sw.Stop();
            long linTime = sw.ElapsedMilliseconds;

            sw.Restart();
            string[] copy = (string[])array.Clone();
            Array.Sort(copy, string.Compare);
            for (int i = 0; i < q; i++)
            {
                Search.BinarySearch(copy, targets[i % 100], string.Compare, out _);
            }
            sw.Stop();
            long binTime = sw.ElapsedMilliseconds;

            string winner = linTime < binTime ? "Лінійний" : "Сорт + Бінарний";
            Console.WriteLine($"{q}\t{linTime}\t\t{binTime}\t\t\t\t{winner}");
        }
    }

    static string[] GenerateArray(int n, int seed)
    {
        var rand = new Random(seed);
        string[] a = new string[n];
        string alphabet = "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        for (int i = 0; i < n; i++)
        {
            char c = alphabet[rand.Next(alphabet.Length)];
            int num = rand.Next(100);
            a[i] = $"{c}-{num:D2}";
        }
        return a;
    }
}