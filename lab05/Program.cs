using System;
using System.Collections.Generic;

namespace Lab5
{
    public class IntStack
    {
        private int[] _array;
        private int _count;

        public IntStack()
        {
            _array = new int[3];
            _count = 0;
        }

        public int Count => _count;
        public int Capacity => _array.Length;

        public void Push(int x)
        {
            if (_count == _array.Length)
            {
                int[] newArray = new int[_array.Length * 2];
                for (int i = 0; i < _count; i++)
                {
                    newArray[i] = _array[i];
                }
                _array = newArray;
            }
            _array[_count] = x;
            _count++;
        }

        public int Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній.");
            _count--;
            return _array[_count];
        }

        public int Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній.");
            return _array[_count - 1];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Завдання 1: Збалансованість дужок (Стандартний Stack)");
            RunTask1Tests();

            Console.WriteLine("\nЗавдання 2: Зворотний польський запис");
            RunTask2Tests();

            Console.WriteLine("\nЗавдання 3: Модель черги обслуговування");
            Task3_QueueModel();

            Console.WriteLine("\nЗавдання 4: Власний стек (IntStack)");
            Task4_TestCustomStack();

            Console.WriteLine("\nПеревірка Завдання 1 на власному IntStack");
            RunTask1CustomTests();
        }

        static (bool isValid, int errorIndex) CheckBracketsStandard(string input)
        {
            if (string.IsNullOrEmpty(input)) return (true, -1);

            Stack<int> stack = new Stack<int>();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '(' || c == '[' || c == '{' || c == '<')
                {
                    stack.Push(i);
                }
                else if (c == ')' || c == ']' || c == '}' || c == '>')
                {
                    if (stack.Count == 0)
                    {
                        return (false, i); 
                    }

                    int openIdx = stack.Pop();
                    char openC = input[openIdx];

                    if (!IsMatchingPair(openC, c))
                    {
                        return (false, i); 
                    }
                }
            }

            if (stack.Count > 0)
            {
                return (false, stack.Pop());
            }

            return (true, -1);
        }

        static bool IsMatchingPair(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '[' && close == ']') ||
                   (open == '{' && close == '}') ||
                   (open == '<' && close == '>');
        }

        static void RunTask1Tests()
        {
            string[] tests = { "(a[b]{c})", "([)]", ")(", "((", "", "<" };
            foreach (string t in tests)
            {
                var res = CheckBracketsStandard(t);
                string status = res.isValid ? "збалансований" : $"незбалансований (індекс помилки: {res.errorIndex})";
                Console.WriteLine($"\"{t}\"\t-> {status}");
            }
        }

        static string EvaluateRPN(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                return "Помилка: порожній вираз";

            Stack<int> stack = new Stack<int>();
            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    stack.Push(number);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
                {
                    if (stack.Count < 2)
                        return $"Помилка: забракло чисел для операції '{token}'";

                    int right = stack.Pop();
                    int left = stack.Pop();

                    switch (token)
                    {
                        case "+": stack.Push(left + right); break;
                        case "-": stack.Push(left - right); break;
                        case "*": stack.Push(left * right); break;
                        case "/":
                            if (right == 0) return "Помилка: ділення на нуль";
                            stack.Push(left / right);
                            break;
                        case "%":
                            if (right == 0) return "Помилка: ділення на нуль (остача)";
                            stack.Push(left % right);
                            break;
                    }
                }
                else
                {
                    return $"Помилка: невідомий токен '{token}'";
                }
            }

            if (stack.Count == 1)
            {
                return stack.Pop().ToString();
            }
            else
            {
                return "Помилка: забагато чисел у виразі";
            }
        }

        static void RunTask2Tests()
        {
            string[] tests = {
                "3 4 + 2 *",
                "5 3 -",
                "8 2 /",
                "2 3 4 * +",
                "5 0 /",
                "10 4 %",
                "5 4 - 2 2 / *"
            };

            foreach (string t in tests)
            {
                string res = EvaluateRPN(t);
                Console.WriteLine($"\"{t}\"\t-> {res}");
            }
        }

        class Request
        {
            public int Id;
            public int Weight;
            public int ArrivalTime;
        }

        static void Task3_QueueModel()
        {
            Random rnd = new Random(1); 
            int requestCount = 9;      
            Request[] requests = new Request[requestCount];

            int arrivalTimeCounter = 0;
            for (int i = 0; i < requestCount; i++)
            {
                arrivalTimeCounter += rnd.Next(0, 4); 
                requests[i] = new Request
                {
                    Id = i + 1,
                    Weight = rnd.Next(1, 10),
                    ArrivalTime = arrivalTimeCounter
                };
            }

            Queue<Request> queue = new Queue<Request>();
            int currentTime = 0;
            int totalIdleTime = 0;
            int maxQueueLength = 0;
            int reqIdx = 0;

            while (reqIdx < requestCount || queue.Count > 0)
            {
                while (reqIdx < requestCount && requests[reqIdx].ArrivalTime <= currentTime)
                {
                    queue.Enqueue(requests[reqIdx]);
                    reqIdx++;
                }

                if (queue.Count > maxQueueLength)
                    maxQueueLength = queue.Count;

                if (queue.Count > 0)
                {
                    Request req = queue.Dequeue();
                    int startTime = currentTime;
                    int finishTime = startTime + req.Weight;
                    Console.WriteLine($"Заявка #{req.Id}: вага = {req.Weight}, прибула = {req.ArrivalTime} хв, старт = {startTime} хв, фініш = {finishTime} хв");
                    currentTime = finishTime;
                }
                else
                {
                    int nextArrival = requests[reqIdx].ArrivalTime;
                    int idle = nextArrival - currentTime;
                    if (idle > 0)
                    {
                        totalIdleTime += idle;
                        currentTime = nextArrival;
                    }
                }
            }

            Console.WriteLine($"\nМаксимальна довжина черги: {maxQueueLength}");
            Console.WriteLine($"Загальний час простою: {totalIdleTime} хв");
        }

        static void Task4_TestCustomStack()
        {
            IntStack stack = new IntStack();
            Console.WriteLine($"Початкова місткість Capacity = {stack.Capacity}");

            for (int i = 1; i <= 5; i++)
            {
                stack.Push(i * 10);
                Console.WriteLine($"Push({i * 10}): Count = {stack.Count}, Capacity = {stack.Capacity}");
            }
        }

        static (bool isValid, int errorIndex) CheckBracketsCustom(string input)
        {
            if (string.IsNullOrEmpty(input)) return (true, -1);

            IntStack stack = new IntStack();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c == '(' || c == '[' || c == '{' || c == '<')
                {
                    stack.Push(i);
                }
                else if (c == ')' || c == ']' || c == '}' || c == '>')
                {
                    if (stack.Count == 0)
                    {
                        return (false, i);
                    }

                    int openIdx = stack.Pop();
                    char openC = input[openIdx];

                    if (!IsMatchingPair(openC, c))
                    {
                        return (false, i);
                    }
                }
            }

            if (stack.Count > 0)
            {
                return (false, stack.Pop());
            }

            return (true, -1);
        }

        static void RunTask1CustomTests()
        {
            string[] tests = { "(a[b]{c})", "([)]", ")(", "((", "", "<" };
            foreach (string t in tests)
            {
                var res = CheckBracketsCustom(t);
                string status = res.isValid ? "збалансований" : $"незбалансований (індекс помилки: {res.errorIndex})";
                Console.WriteLine($"\"{t}\"\t-> {status}");
            }
        }
    }
}