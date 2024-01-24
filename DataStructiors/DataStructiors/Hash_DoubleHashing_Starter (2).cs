using System.Collections;

class Hash_DoubleHashing<TKey, TValue> : IEnumerable
{
    //Single Cell Status:
    // Empty - data will be null 
    // Deleted - for existing data(data not null) where isDeleted is true
    // Occupied - for existing data (data not null) where isDeleted is false
    Data[] arr;
    const double M = 1.35; //extra 35% space
    int itemsCnt;
    int maxItems;

    public Hash_DoubleHashing()
    {
        arr = new Data[8191];
        maxItems = (int)((double)arr.Length / M);
    }

    public Hash_DoubleHashing(int capacity)
    {
        //Add 35 percent more to the size of the array relative to the maximum expected items
        //size must be prime number
        int size = Hash_DoubleHashing<TKey, TValue>.FindClosestPrimeNumber((int)(M * capacity));

        arr = new Data[size];
        this.maxItems = capacity;
    }

    //    Implement the Indexer - for both get and set
    //get:
    //         caclulate the index for the given key
    //         if the cell is empty no such item
    //         if the cell is occupied by different key -> calculate step and loop till you find the match(this is your item) or empty cell(no such item)

    //         note that the loop is circular
    //set:
    //  caclulate the index for the given key
    //         if the cell is empty no such item
    //         if the cell is occupied by different key -> calculate step and loop till you find the match(this is your item) or empty cell(no such item)
    //if there is a match - update the value

    private static int FindClosestPrimeNumber(int size)
    {
        if (Hash_DoubleHashing<TKey, TValue>.IsCloseToPrime(size))
            return size;

        while (!Hash_DoubleHashing<TKey, TValue>.IsCloseToPrime(size))
            size++;

        return size;
    }

    public static bool IsCloseToPrime(int size)
    {
        int sqrt = (int)Math.Sqrt(size);
        for (int i = 2; i <= 1000; i++)
        {
            if (size % i == 0)
                return false;
            if (i > sqrt)
                return true;
        }
        return true;
    }

    private void AddAction(int index, TKey key, TValue value)
    {
        arr[index] = new Data(key, value);
        itemsCnt++;
    }

    public void Add(TKey key, TValue value)
    {
        int index = CalcHashCode(key);
        if (arr[index] is not null && arr[index].isDeleted || arr[index] is null)
        {
            if (arr.Any(data => data.key!.Equals(key)))
                throw new ArgumentException($"key {key} is allredy exist. try a diffrent key");
            else
                AddAction(index, key, value);
        }

        else
        {
            int step = Step(key);
            while (arr[index] is not null && !arr[index].isDeleted)
            {
                index += step;
                if (index > arr.Length)
                    index %= arr.Length;
            }
            AddAction(index, key, value);
        }

        if (itemsCnt >= maxItems)
            ReHash();
    }

    public bool Delete(TKey keyToDelete, out TValue value)
    {
        //caclulate the index for the given key
        int index = CalcHashCode(keyToDelete);

        // if the cell is empty no such item
        if (arr[index] is null)
            throw new ArgumentException("no such item");

        // if the cell is occupied by different key -> calculate step and loop till you find the match(this is your item) or empty cell(no such item)
        else if (arr[index].key!.Equals(keyToDelete))
        {
            int step = Step(keyToDelete);
            while (arr[index] is not null && !arr[index].key!.Equals(keyToDelete))
            {
                index += step;
                if (index > arr.Length)
                    index %= arr.Length;
            }
            if (arr[index] is null)
                throw new ArgumentException("no such item");
            else
            {
                arr[index].isDeleted = true;
                value = arr[index].val;
                return true;
            }
        }

        //If the key is found - mark the cell as deleted and save its value
        else
        {
            arr[index].isDeleted = true;
            value = arr[index].val;
            return true;
        }

        // note that the loop is circular
    }

    private void ReHash()
    {
        //double the size of array and find the closest prime value to new size
        Data[] newArr = new Data[Hash_DoubleHashing<TKey, TValue>.FindClosestPrimeNumber(arr.Length * 2)];
        // double the maxItems
        maxItems *= 2;
        // realocate all the existed items
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] is null || arr[i].isDeleted)
                continue;
            newArr[i] = arr[i];
        }
        arr = newArr;
    }

    private int CalcHashCode(TKey key)
    {
        int res = key!.GetHashCode();
        return res % arr.Length;
    }

    private int Step(TKey k) //123456
    {
        string? s = k!.ToString();
        int step = (s![0] + s[s.Length - 1]) / 4;
        return step % arr.Length;
    }

    public IEnumerator GetEnumerator()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] is not null && !arr[i].isDeleted)
            {
                yield return arr[i];
            }
        }

        //foreach (var item in arr)
        //{
        //    yield return item;
        //}
    }

    class Data
    {
        public TKey key;
        public TValue val;
        internal bool isDeleted;


        public Data(TKey key, TValue val)
        {
            this.key = key;
            this.val = val;
            isDeleted = false;
        }
    }
}
/*private void ReHash()
    {
        //double the size of array and find the closest prime value to new size
        Data[] newArr = new Data[FindClosestPrimeNumber(arr.Length * 2)];
        // double the maxItems
        maxItems *= 2;
        // realocate all the existed items
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] is null || arr[i].isDeleted)
                continue;
            newArr[i] = arr[i];
        }
        arr = newArr;
    }
what this function should do and does it do it well?*/
