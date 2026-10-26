using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lab06
{
    class Student
    {
        public string Name;
        public string Group;
        public int Score;
        
        public Student(string name, string group, int score)
        {
            Name = name; Group = group; Score = score;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Завдання 1");
            Task1();

            Console.WriteLine("\nЗавдання 2");
            Task2();

            Console.WriteLine("\nЗавдання 3");
            Task3();

            Console.WriteLine("\nЗавдання 4");
            Task4();

            Console.WriteLine("\nЗавдання 5 (Заміри)");
            Task5();
        }

        static void Task1()
        {
            string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
            Dictionary<char, int> frequencies = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);
                    if (frequencies.ContainsKey(lowerC))
                    {
                        frequencies[lowerC]++;
                    }
                    else
                    {
                        frequencies.Add(lowerC, 1);
                    }
                }
            }

            int threshold = 1 % 3 + 2;
            int uniqueCount = 0;

            Console.WriteLine($"Літери з частотою >= {threshold}:");
            foreach (var kvp in frequencies)
            {
                uniqueCount++;
                if (kvp.Value >= threshold)
                {
                    Console.WriteLine($"'{kvp.Key}': {kvp.Value}");
                }
            }
            Console.WriteLine($"Загальна кількість різних літер: {uniqueCount}");
        }

        static void Task2()
        {
            List<Student> students = new List<Student>
            {
                new Student("Коваленко", "КН-3/1", 88), new Student("Мельник", "КН-3/2", 64),
                new Student("Іщенко", "КН-3/1", 93), new Student("Бондар", "КН-3/1", 71),
                new Student("Савченко", "КН-3/2", 55), new Student("Ткачук", "КН-3/2", 90),
                new Student("Кравець", "КН-3/1", 76), new Student("Іванів", "КН-3/1", 48),
                new Student("Мороз", "КН-3/2", 82), new Student("Шевчук", "КН-3/1", 67),
                new Student("Бойко", "КН-3/2", 95), new Student("Кушнір", "КН-3/2", 73)
            };

            Dictionary<string, List<Student>> grouped = new Dictionary<string, List<Student>>();

            foreach (Student s in students)
            {
                if (!grouped.ContainsKey(s.Group))
                {
                    grouped.Add(s.Group, new List<Student>());
                }
                grouped[s.Group].Add(s);
            }

            foreach (var groupInfo in grouped)
            {
                int count = 0;
                int sum = 0;
                foreach (var s in groupInfo.Value)
                {
                    count++;
                    sum += s.Score;
                }
                double avg = (double)sum / count;
                Console.WriteLine($"Група {groupInfo.Key}: Студентів = {count}, Середній бал = {avg:F2}");
            }
        }

        static void Task3()
        {
            Random rnd = new Random(1);
            List<int> numbers = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                numbers.Add(rnd.Next(1, 8));
            }

            Console.WriteLine("Вихідний список:");
            PrintCollection(numbers);

            HashSet<int> noOrderSet = new HashSet<int>(numbers);
            Console.WriteLine("Спосіб 1 (Без збереження порядку):");
            PrintCollection(noOrderSet);

            HashSet<int> seen = new HashSet<int>();
            List<int> orderedList = new List<int>();
            foreach (int num in numbers)
            {
                if (seen.Add(num))
                {
                    orderedList.Add(num);
                }
            }
            Console.WriteLine("Спосіб 2 (Зі збереженням порядку):");
            PrintCollection(orderedList);
        }

        static void Task4()
        {
            Random rnd1 = new Random(1);
            Random rnd2 = new Random(101);

            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();

            for (int i = 0; i < 30; i++)
            {
                list1.Add(rnd1.Next(1, 22));
                list2.Add(rnd2.Next(1, 22));
            }

            HashSet<int> set1 = new HashSet<int>(list1);
            set1.ExceptWith(list2);

            List<int> manualExcept = new List<int>();
            foreach (int a in list1)
            {
                bool foundInList2 = false;
                foreach (int b in list2)
                {
                    if (a == b)
                    {
                        foundInList2 = true;
                        break;
                    }
                }
                
                if (!foundInList2)
                {
                    bool alreadyInResult = false;
                    foreach (int r in manualExcept)
                    {
                        if (a == r)
                        {
                            alreadyInResult = true;
                            break;
                        }
                    }
                    if (!alreadyInResult) manualExcept.Add(a);
                }
            }

            Console.WriteLine("Залишились у 1-му, відсутні в 2-му (HashSet):");
            PrintCollection(set1);
            Console.WriteLine("Залишились у 1-му, відсутні в 2-му (Вручну):");
            PrintCollection(manualExcept);

            bool match = true;
            if (set1.Count != manualExcept.Count) match = false;
            else
            {
                foreach (int m in manualExcept)
                {
                    if (!set1.Contains(m)) { match = false; break; }
                }
            }
            Console.WriteLine($"Результати збігаються: {match}");
        }

        static void Task5()
        {
            Stopwatch sw = new Stopwatch();
            
            List<int> listData = new List<int>();
            HashSet<int> setData = new HashSet<int>();
            Random r = new Random(1);
            for(int i = 0; i < 100000; i++) {
                int val = r.Next();
                listData.Add(val);
                setData.Add(val);
            }

            int[] targets = new int[10000];
            for (int i = 0; i < 10000; i++) targets[i] = listData[r.Next(listData.Count)];

            // Прогрівочний цикл
            foreach (var t in targets) setData.Contains(t);

            sw.Restart();
            int c1 = 0;
            foreach (var t in targets)
            {
                foreach (var el in listData)
                {
                    if (el == t) { c1++; break; }
                }
            }
            sw.Stop();
            long msList = sw.ElapsedMilliseconds;

            sw.Restart();
            int c2 = 0;
            foreach (var t in targets)
            {
                if (setData.Contains(t)) c2++;
            }
            sw.Stop();
            long msHash = sw.ElapsedMilliseconds;

            Console.WriteLine($"Пошук: Список - {msList} мс, Множина - {msHash} мс. Швидше в { (msHash > 0 ? msList / msHash : msList) } разів");
        }

        static void PrintCollection<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }
}