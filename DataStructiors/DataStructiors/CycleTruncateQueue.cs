using System.Collections;
using System.Text;

public class CycleTruncateQueue<T> : IEnumerable<T>
{
    public int First { get; private set; }
    public int Last { get; private set; }
    T[] QArray;

    public CycleTruncateQueue(int size = 5)
    {
        QArray = new T[size];
        First = Last = -1;
    }

    public IEnumerator<T> GetEnumerator()
    {
        int tmp = First;
        while (tmp != Last)
        {
            yield return QArray[tmp];//return how many time you need
            tmp = (tmp + 1) % QArray.Length;
        }
        if (!IsEmpty()) yield return QArray[Last];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool EnQ(T item)
    {
        if (IsFull())
        {
            QArray[First] = item;//Overrides the value that needs to come out
            Last = (Last + 1) % QArray.Length;
            First = (First + 1) % QArray.Length;
            return true;
        }
        if (IsEmpty()) First = 0;

        Last = (Last + 1) % QArray.Length;
        QArray[Last] = item;
        return true;
    }

    public bool DeQ(out T deValue)//pop
    {
        deValue = default;
        if (IsEmpty()) return false;

        deValue = QArray[First];
        if (First == Last)//last value in Q
        {
            First = Last = -1;
            return true;
        }
        First = (First + 1) % QArray.Length;
        return true;
    }

    public override string ToString()
    {
        if (IsEmpty()) return "this shoe never bought";

        StringBuilder builder = new StringBuilder();
        int tmp = First;

        while (tmp != Last)
        {
            builder.Append($"{QArray[tmp]}\n");
            tmp = (tmp + 1) % QArray.Length;
        }
        builder.Append(QArray[tmp]);
        return builder.ToString();
    }

    public T Peek() => QArray[First];

    public T PeekLast() => QArray[Last];

    public bool IsEmpty() => First == -1;

    public bool IsFull() => (Last + 1) % QArray.Length == First;

}

