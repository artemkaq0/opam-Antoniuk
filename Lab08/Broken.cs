using System;

public static class Broken
{
    public static int BinarySearchBroken1<T>(T[] a, T target, Func<T, T, int> cmp)
    {
        int lo = 0;
        int hi = a.Length - 1;
        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            int result = cmp(a[mid], target);
            if (result == 0) return mid;
            if (result < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int BinarySearchBroken2<T>(T[] a, T target, Func<T, T, int> cmp)
    {
        int lo = 0;
        int hi = a.Length - 1;
        int limit = 100;
        
        while (lo <= hi && limit > 0)
        {
            limit--;
            int mid = lo + (hi - lo) / 2;
            int result = cmp(a[mid], target);
            if (result == 0) return mid;
            
            if (result < 0) lo = mid;
            else hi = mid - 1;
        }
        if (limit == 0) Console.WriteLine("    [!] Програма зависла (спрацював ліміт ітерацій). Межі не рухаються.");
        return -1;
    }

    public static int BinarySearchBroken3<T>(T[] a, T target, Func<T, T, int> cmp)
    {
        int lo = 0;
        int hi = a.Length - 1;
        while (lo <= hi)
        {
            int mid = (lo + hi) / 2;
            int result = cmp(a[mid], target);
            if (result == 0) return mid;
            if (result < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

}