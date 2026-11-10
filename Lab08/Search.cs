using System;

public static class Search
{
    public static int LinearSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        for (int i = 0; i < a.Length; i++)
        {
            comparisons++;
            if (cmp(a[i], target) == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int BinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            
            comparisons++;
            int result = cmp(a[mid], target);

            if (result == 0) return mid;
            
            comparisons++;
            if (result < 0)
            {
                lo = mid + 1;
            }
            else
            {
                hi = mid - 1;
            }
        }
        return -1;
    }

    public static int BoundarySearch<T>(T[] a, Func<T, bool> condition)
    {
        int lo = 0;
        int hi = a.Length;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (condition(a[mid]))
            {
                hi = mid;
            }
            else
            {
                lo = mid + 1;
            }
        }
        return lo;
    }

    public static int BinarySearchNoEquality<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        if (a.Length == 0) return -1;

        int lo = 0;
        int hi = a.Length - 1;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            if (cmp(a[mid], target) < 0)
            {
                lo = mid + 1;
            }
            else
            {
                hi = mid;
            }
        }

        comparisons++;
        if (cmp(a[lo], target) == 0)
        {
            return lo;
        }

        return -1;
    }
}