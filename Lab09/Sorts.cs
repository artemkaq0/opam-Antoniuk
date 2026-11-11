using System;

public static class Sorts
{
    public static void InsertionSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        
        for (int i = 1; i < a.Length; i++)
        {
            T key = a[i];
            moves++;
            int j = i - 1;

            while (j >= 0)
            {
                comparisons++;
                if (cmp(a[j], key) > 0)
                {
                    a[j + 1] = a[j];
                    moves++; 
                    j--;
                }
                else
                {
                    break;
                }
            }
            a[j + 1] = key;
            moves++;
        }
    }

    public static void BottomUpMergeSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;
        T[] temp = new T[n];

        for (int width = 1; width < n; width *= 2)
        {
            for (int i = 0; i < n; i += 2 * width)
            {
                int left = i;
                int mid = Math.Min(i + width, n);
                int right = Math.Min(i + 2 * width, n);

                Merge(a, left, mid, right, temp, cmp, ref comparisons, ref moves);
            }
        }
    }

    private static void Merge<T>(T[] a, int left, int mid, int right, T[] temp, Func<T, T, int> cmp, ref int comp, ref int moves)
    {
        int i = left, j = mid, k = left;

        while (i < mid && j < right)
        {
            comp++;
            if (cmp(a[i], a[j]) <= 0)
            {
                temp[k++] = a[i++];
            }
            else
            {
                temp[k++] = a[j++];
            }
            moves++;
        }

        while (i < mid)
        {
            temp[k++] = a[i++];
            moves++;
        }
        while (j < right)
        {
            temp[k++] = a[j++];
            moves++;
        }

        for (int m = left; m < right; m++)
        {
            a[m] = temp[m];
            moves++;
        }
    }

    public static void BinaryInsertionSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;

        for (int i = 1; i < a.Length; i++)
        {
            T key = a[i];
            moves++;

            int left = 0;
            int right = i - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                comparisons++;
                if (cmp(key, a[mid]) < 0)
                {
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            for (int j = i - 1; j >= left; j--)
            {
                a[j + 1] = a[j];
                moves++;
            }
            a[left] = key;
            moves++;
        }
    }
}