using System.Collections;

public class DictionaryLs<Tkey, TValue> : IEnumerable
{
    LinkedList<Data>[] arr;
    int items;

    public int Count => items;

    public DictionaryLs(int capacity)
    {
        arr = new LinkedList<Data>[capacity];
        items = 0;
    }

    public DictionaryLs()
        : this(1024)
    {
    }


    public void Add(Tkey key, TValue value) // O(1)
    {
        int ind = GetIndex(key);
        if (arr[ind] == null)
            arr[ind] = new LinkedList<Data>();
        else
        {
            if (arr[ind].Any((data) => data.key!.Equals(key)))
            {
                throw new ArgumentException($"An item with the same key: {key} has already been added.");
            }
        }
        arr[ind].AddFirst(new Data(key, value));
        items++;

        if (items > arr.Length)
        {
            ReHash();
        }
    }

    private void ReHash()
    {
        //double the size of array for hash
        LinkedList<Data>[] newArr = new LinkedList<Data>[arr.Length * 2];

        //reallocate all existing items
        foreach (var list in arr)
        {
            foreach (var data in list)
            {
                int ind = data.key!.GetHashCode() % newArr.Length;
                if (newArr[ind] == null)
                    newArr[ind] = new LinkedList<Data>();
                newArr[ind].AddFirst(data);
            }
        }
    }

    public bool Delete(Tkey key, out TValue value)
    {
        int index = GetIndex(key);

        if (arr[index] is null || !arr[index].Any(data => data.key!.Equals(key)))
            throw new ArgumentException("no such item");

        value = arr[index].FirstOrDefault(data => data.key!.Equals(key))!.value;
        var data = arr[index].FirstOrDefault(data => data.key!.Equals(key));
        arr[index].Remove(data!);
        return true;
    }

    private int GetIndex(Tkey key)
    {
        int res = key!.GetHashCode();
        return res % arr.Length; // 0 - len-1
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    class Data
    {
        public Tkey key;
        public TValue value;

        public Data(Tkey key, TValue value) //ctor
        {
            this.key = key;
            this.value = value;
        }
    }
}



