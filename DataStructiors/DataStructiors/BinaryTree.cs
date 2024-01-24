public class BinaryTree<T> where T : IComparable<T>
{
    Node root;
    readonly IComparer<T> custComparer;

    public BinaryTree()
        : this(null)
    {
    }

    public BinaryTree(IComparer<T> custComparer)
    {
        root = null;                                  //overlload to build a new tree with difrent compare
        this.custComparer = custComparer;
    }

    public void Add(T value)
    {
        if (root == null)
        {
            root = new Node(value);
            return;
        }
        Node tmp = root;

        while (true)
        {
            int compare;
            if (custComparer != null)
                compare = custComparer.Compare(value, tmp.value);
            else
                compare = value.CompareTo(tmp.value);

            if (compare == 0)//can't add the same point
                return;

            else if (compare < 0) // go left
            {
                if (tmp.left == null)
                {
                    tmp.left = new Node(value);
                    break;
                }
                else tmp = tmp.left;//moov action
                if (compare == 0)//can't add the same point
                    return;
            }
            else                 //go right
            {
                if (tmp.right == null)
                {
                    tmp.right = new Node(value);
                    break;
                }
                else tmp = tmp.right;//moov action
                if (compare == 0)//can't add the same point
                    return;
            }
        }
    }

    private void BuildNewTreeByOtherCompare(BinaryTree<T> treeOriginal, BinaryTree<T> treeNew, Node tmp)
    {
        if (tmp == null) return;

        BuildNewTreeByOtherCompare(treeOriginal, treeNew, tmp.left);
        treeNew.Add(tmp.value);
        BuildNewTreeByOtherCompare(treeOriginal, treeNew, tmp.right);
    }

    public void BuildNewTree(BinaryTree<T> treeOriginal, BinaryTree<T> treeNew)
    {
        if (treeOriginal == null)
            throw new ArgumentNullException("first tree in func can't be null");
        BuildNewTreeByOtherCompare(treeOriginal, treeNew, treeOriginal.root);
    }

    public void ScanInOrder(Action<T> act)
    {
        if (act == null) throw new ArgumentNullException("act/func can't be null");
        ScanInOrder(root, act);
    }

    private void ScanInOrder(Node tmp, Action<T> act)
    {
        if (tmp == null) return;
        ScanInOrder(tmp.left, act);
        act(tmp.value);
        ScanInOrder(tmp.right, act);
    }

    public void Clear() => root = null;

    public int GetDeep() => GetDeep(root);

    private int GetDeep(Node tmp)
    {
        if (tmp == null) return 0;
        int deep = Math.Max(GetDeep(tmp.left), GetDeep(tmp.right));
        return deep + 1;

        //return (tmp == null) ? 0 : Math.Max(GetDeep(tmp.left), GetDeep(tmp.right)) + 1;
    }

    public bool Search(T itemToFind, out T foundItem)
    {
        Node tmp = root;

        while (itemToFind.CompareTo(tmp.value) != 0)//item != tmp
        {
            if (itemToFind.CompareTo(tmp.value) < 0)
                tmp = tmp.left; //go left
            else tmp = tmp.right;//go right

            if (tmp == null)//item didn't find
            {
                foundItem = default;
                return false;
            }
        }
        foundItem = tmp.value;
        return true;
    }

    public void ReturnTwoClose(T target, out T closeDown, out T closeUp)
    {
        closeDown = FindClosestFromDown(target);
        closeUp = FindClosestFromUp(target);
    }

    private T FindClosestFromUp(T target)
    {
        Node tmp = root;
        T returnItem;

        if (root != null)
        {
            while (tmp.value.CompareTo(target) < 0 && tmp.right != null)//tmp < target && tmp.nextSmaller != null
                tmp = tmp.right;//go right
            returnItem = tmp.value;

            while (tmp != null)
            {
                if (tmp.value.CompareTo(target) >= 0)//tmp >= target 
                {
                    returnItem = tmp.value;
                    tmp = tmp.left;
                }
                else tmp = tmp.right;//there is no assigment because we want to find bigger value
            }

            return returnItem;
        }
        returnItem = default;
        return returnItem;
    }

    private T FindClosestFromDown(T target)
    {
        Node tmp = root;
        T returnItem;

        if (root != null)
        {
            while (tmp.value.CompareTo(target) > 0 && tmp.left != null)// tmp > target && tmp.nextBigger != null
                tmp = tmp.left;
            returnItem = tmp.value;

            while (tmp != null)
            {
                if (tmp.value.CompareTo(target) >= 0)//tmp <= target
                    tmp = tmp.left;//there is no assigment because we want to find smaller value
                else
                {
                    returnItem = tmp.value;
                    tmp = tmp.right;
                }
            }
            return returnItem;
        }

        returnItem = default;
        return returnItem;

    }

    public bool Remove(T value, out T valueRemoved)
    {
        Node removedValue = root;
        Node removedValuePerent = root;
        bool isRoot = true;
        valueRemoved = default;
        if (removedValue == null) return false; //chek the tree isn't empty

        while (value.CompareTo(removedValue.value) != 0)
        {
            removedValuePerent = SerchingValue(value, ref removedValue);

            if (removedValue == null) return false; //check the value exists

            if (removedValue.value.CompareTo(value) == 0)//check the value isn't the root
                isRoot = false;
        }
        valueRemoved = removedValue.value;

        if (removedValue.right == null && removedValue.left == null)//case the remove value haven't children            
            RemoveLeaf(removedValue, removedValuePerent, isRoot);

        else if (removedValue.right == null || removedValue.left == null)//case the remove value have one childe            
            RemoveValueWithOneChild(removedValue, removedValuePerent, isRoot);

        else//case the remove value have two children           
            RemoveValueWithTwoChildren(removedValue, removedValuePerent, isRoot);

        return true;
    }

    private  Node SerchingValue(T value, ref Node removedValue)
    {
        Node removedValuePerent = removedValue;
        if (value.CompareTo(removedValue.value) < 0)
            removedValue = removedValue.left;                  //find the value and assigmnet his perent   
        else removedValue = removedValue.right;
        return removedValuePerent;
    }

    private void RemoveLeaf(Node removedValue, Node removedValuePerent, bool isRoot)
    {
        if (isRoot)
            Clear();
        else
        {
            if (removedValuePerent.left == removedValue)
                removedValuePerent.left = null;
            else
                removedValuePerent.right = null;
        }
    }

    private void RemoveValueWithOneChild(Node removedValue, Node removedValuePerent, bool isRoot)
    {
        if (removedValue.left == null)//if the child is in the right side           
            RemoveOnRightSide(removedValue, removedValuePerent, isRoot);

        else//if the child is in the left side           
            RemoveOnLeftSide(removedValue, removedValuePerent, isRoot);
    }

    private void RemoveOnLeftSide(Node removedValue, Node removedValuePerent, bool isRoot)
    {
        if (isRoot) root = root.left;
        if (removedValuePerent.left == removedValue)
            removedValuePerent.left = removedValue.left;
        else
            removedValuePerent.right = removedValue.left;
    }

    private void RemoveOnRightSide(Node removedValue, Node removedValuePerent, bool isRoot)
    {
        if (isRoot) root = root.right;
        if (removedValuePerent.left == removedValue)
            removedValuePerent.left = removedValue.right;
        else
            removedValuePerent.right = removedValue.right;
    }

    private void RemoveValueWithTwoChildren(Node removedValue, Node removedValuePerent, bool isRoot)
    {
        Node leftestNode = removedValue.right;//one step right
        Node leftestNodePerent = removedValue;

        FindLeftestNode(ref leftestNode, ref leftestNodePerent);

        if (leftestNodePerent == removedValue)//case the right child haven't left side
        {
            if (removedValuePerent.left == leftestNodePerent)//right side
            {
                removedValue.right.left = removedValue.left;
                if (isRoot) root = root.right;
                removedValuePerent.left = removedValue.right;
            }
            else//left side
            {
                removedValue.right.left = removedValue.left;
                if (isRoot) root = root.left;
                removedValuePerent.right = removedValue.right;
            }
        }


        else//case the right child have left side
        {
            removedValue.value = leftestNode.value;
            leftestNodePerent.left = leftestNode.right;
        }
    }

    private  void FindLeftestNode(ref Node leftestNode, ref Node leftestNodePerent)
    {
        while (leftestNode.left != null)//go left
        {
            leftestNodePerent = leftestNode;
            leftestNode = leftestNode.left;
        }
    }

    class Node
    {
        public T value;
        public Node left;
        public Node right;

        public Node(T value)
        {
            this.value = value;
        }
    }
}



