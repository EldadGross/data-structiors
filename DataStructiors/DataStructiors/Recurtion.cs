public class Recurtion
{

    int count = 0;
    #region Covered
    public int SumArrayItems(int[] arr)
    {
        return SumArrayItems(arr, 0);
    }

    private int SumArrayItems(int[] arr, int index) // O(n)
    {
        if (index == arr.Length - 1) return arr[index];
        return SumArrayItems(arr, index + 1) + arr[index];
    }


    public  bool IsPalindrom(string s)
    {
        return IsPalindrom(s, 0);
    }

    private  bool IsPalindrom(string s, int index) // abba
    {
        if (index >= s.Length / 2) return true;
        if (s[index] != s[s.Length - 1 - index]) return false;

        return IsPalindrom(s, index + 1);
    }

    public  int GCD(int num1, int num2)
    {
        int max = Math.Max(num1, num2);
        int min = Math.Min(num1, num2);

        if (max % min == 0) return min;
        return GCD(min, max - min);
    }
    #endregion

    public  int FibIterative(int n) // O(n)
    {
        if (n <= 2) return 1;
        int n1 = 1, n2 = 1, n3 = 0;

        for (int i = 0; i < n - 2; i++) // 3n 
        {
            n3 = n1 + n2;
            n1 = n2;
            n2 = n3;
        }

        return n3;
    }

    public  int FibRecursive(int n) // O(2^n)
    {
        if (n <= 2) return 1;
        return FibRecursive(n - 1) + FibRecursive(n - 2);
    }

    public  void PrintDirTree(DirectoryInfo parentDir)
    {
        PrintDirTree(parentDir, 0);
    }

    private  void PrintDirTree(DirectoryInfo parentDir, int spaces)
    {
        DirectoryInfo[] childDirs = parentDir.GetDirectories();
        if (childDirs.Length == 0) return;
       
        foreach (DirectoryInfo subDir in childDirs)
        {
            Console.WriteLine($"{new string('\t', spaces)}{subDir.Name}");
            PrintDirTree(subDir, spaces + 1);
        }
    }

    public  void Hanoi(char source, char temp, char dest, int n)
    {
        HanoiInner(source, temp, dest, n);
        Console.WriteLine(count);
    }

    private void HanoiInner(char source, char temp, char dest, int n)
    {
        if (n == 0) return;
        count++;
        HanoiInner(source, dest, temp, n - 1);
        Console.WriteLine($"{source} -> {dest}");
        HanoiInner(temp, source, dest, n - 1);
    }

}

