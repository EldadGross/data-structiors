using System.Collections;
using System.Text;

public class MyLinkedList<T> : IEnumerable
{
    public Node Start { get; private set; }
    public Node End { get; private set; }
    public int Count { get; private set; }

    public bool IsEmpty() => Start == null;

    public void AddFirst(T val)
    {
        Node n = new Node(val);
        n.next = Start;
        n.previous = null;
        if (Start != null) Start.previous = n;
        Start = n;
        if (Start.next == null) End = n;
        Count++;
    }

    public void AddLast(T val)
    {
        if (End == null)
        {
            AddFirst(val);
            return;
        }
        Node n = new Node(val);
        End.next = n;
        n.previous = End;
        End = End.next;
        Count++;
    }

    //private void AddLast_badVersion(T val)
    //{
    //    if (start == null)
    //    {
    //        AddFirst(val);
    //        return;
    //    }

    //    Node end = start;

    //    while (end.next != null) end = end.next;

    //    Node node = new Node(val);
    //    end.next = node;
    //}

    public bool RemoovFirst(out T remoovedValue)
    {
        remoovedValue = default;
        if (Start == null) return false;

        remoovedValue = Start.Value;
        Start = Start.next;
        Start.previous = null;
        if (Start == null) End = null;
        Count--;
        return true;
    }

    public bool RemoovLast(out T remoovedValue)
    {
        remoovedValue = default;
        if (Start == null) return false;

        remoovedValue = End.Value;
        End = End.previous;
        End.next = null;
        if (End == null) Start = null;
        Count--;
        return true;
    }

    public bool RemoovAtt(int index, Node node, out T remoovedValue)
    {
        GetAtt(index, out remoovedValue);
        if (End == null) return false;//empty list

        else if (node == Start) RemoovFirst(out remoovedValue);
        else if (node == End) RemoovLast(out remoovedValue);

        else
        {
            remoovedValue = node.Value;
            node.next.previous = node.previous;
            node.previous.next = node.next;
        }
        return true;
    }

    public bool GetAtt(int position, out T found)
    {
        Node tmp = Start;
        found = default;
        for (int i = 0; i < position; i++)
        {
            if (tmp != null) tmp = tmp.next;
            else return false;
        }
        found = tmp.Value;
        return true;
    }

    public bool AddAt(int position, T value)
    {
        Node tmp = Start;
        if (tmp == null)
        {
            AddFirst(value);
            Count++;
            return true;
        }
        if (position == 0)
        {
            AddFirst(value);
            Count++;
            return true;
        }

        for (int i = 0; i < position - 1; i++)
        {
            tmp = tmp.next;
            if (tmp == null) return false;
        }
        if (tmp.next == null) AddLast(value);

        Node tmp2 = new Node(value);
        tmp.next.previous = tmp2;
        tmp2.next = tmp.next;
        tmp.next = tmp2;
        tmp.previous = tmp;
        Count++;
        return true;
    }

    public T this[int index]
    {
        get
        {
            Node tmp = Start;
            for (int i = 0; i < index; i++)
            {
                if (tmp != null) tmp = tmp.next;
                else throw new Exception($"position {index} does not exists");
            }
            return tmp.Value;
        }
    }

    public override string ToString()
    {
        //string s = "";
        //Node tmp = start;

        //while (tmp != null)
        //{
        //    s = s + tmp.value + " ";
        //    tmp = tmp.next;
        //}
        //return s;

        StringBuilder builder = new StringBuilder();
        Node tmp = Start;

        while (tmp != null)
        {
            builder.Append(tmp.Value.ToString() + " ");
            tmp = tmp.next;
        }
        return builder.ToString();
    }

    public void MoovToEndByNode(Node nodeToMoov)
    {
        if (nodeToMoov == null) return;
        else if (nodeToMoov == End) return;

        else if (nodeToMoov.next == End)
        {
            Node tmp = End;
            End = Start;
            Start = tmp;
        }

        else if (nodeToMoov == Start)
        {
            Start = nodeToMoov.next;
            Start.previous = null;
            End.next = nodeToMoov;
            nodeToMoov.previous = End;
            nodeToMoov.next = null;
            End = nodeToMoov;
        }

        else
        {
            nodeToMoov.previous.next = nodeToMoov.next;
            nodeToMoov.next.previous = nodeToMoov.previous;

            End.next = nodeToMoov;
            nodeToMoov.previous = End;
            nodeToMoov.next = null;
            End = nodeToMoov;
        }
    }

    //public IEnumerator<T> GetEnumerator() old version
    //{
    //    ListEnumerator le = new ListEnumerator(start);  //,הבעיה בגרסה הישנה שהיא מצריכה בניה של קלאס שלם 
    //    return le;                                      //ליסטאינומרייטור, וזה מלא בלאגן. הגרסא עם המילה יילד  
    //}                                                   //חוסכת את כל הבניה שלו

    public IEnumerator<T> GetEnumerator()
    {
        Node current = Start;
        while (current != null)
        {
            yield return current.Value;
            current = current.next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    class ListEnumerator : IEnumerator<T>
    {
        Node current;
        bool firstTime;
        public ListEnumerator(Node start)
        {
            firstTime = true;
            current = start;
        }
        public T Current => /*return*/ current.Value;

        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext()
        {
            if (current == null) throw new NullReferenceException();

            if (firstTime)
            {
                firstTime = false;
                return true;
            }

            current = current.next;
            if (current != null) return true;
            else return false;
        }

        public void Dispose() { }

        public void Reset() { }
    }

    public class Node
    {
        public T Value { get; internal set; }
        internal Node next;
        internal Node previous;

        public Node(T value)
        {
            Value = value;
        }
    }
}

