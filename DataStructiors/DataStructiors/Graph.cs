
public class Graph
{
    public ushort[,] graphArr;

    public Graph()
    {
        Load();
    }

    private void Load()
    {
        graphArr = new ushort[6, 6]
        {
            {0, 0, 0, 0, 0, 7 },//0
            {0, 0, 2, 4, 12, 0 },//1
            {0, 2, 0, 10, 1, 0  },//2
            {0, 4, 10, 0, 0, 7 },//3
            {0, 12, 1, 0, 0, 0 },//4
            {7, 0, 0, 7, 0, 0 },//5
        };
    }

    public bool IsPathExsist(ushort sorce, ushort dest, int maxStep)
    {
        bool[] markedVertic = new bool[graphArr.GetLength(1)];
        markedVertic[sorce] = true;

        return IsPathExsist(sorce, dest, maxStep, markedVertic);
    }

    private bool IsPathExsist(ushort sorce, ushort dest, int maxStep, bool[] markedVertic)
    {
        if (sorce == dest) return true;
        if (maxStep == 0) return false;
        if (graphArr[sorce, dest] > 0) return true;//only in matrix

        bool isPathFound;

        for (ushort i = 0; i < graphArr.GetLength(1); i++)
        {
            if (graphArr[sorce, i] > 0 && !markedVertic[i])
            {
                markedVertic[i] = true;
                isPathFound = IsPathExsist(i, dest, maxStep - 1, markedVertic);
                if (isPathFound) return true;
                markedVertic[i] = false;
            }
        }
        return false;
    }

    public List<Path> GetAllPaths(ushort sorce, ushort dest)
    {
        List<Path> paths = new List<Path>();
        bool[] markedVertices = new bool[graphArr.GetLength(1)];
        markedVertices[sorce] = true;
        GetAllPaths(sorce, dest, paths, markedVertices);
        return paths;
    }

    private void GetAllPaths(ushort sorce, ushort dest, List<Path> paths, bool[] markedVertices)
    {
        if (sorce == dest)
        {
            paths.Add(new Path());
            return;
        }

        //loop on each fit i
        for (ushort i = 0; i < graphArr.GetLength(1); i++)
        {
            if (graphArr[sorce, i] > 0 && !markedVertices[i])
            {
                markedVertices[i] = true;
                int lenBefore = paths.Count;
                GetAllPaths(i, dest, paths, markedVertices);
                for (int j = lenBefore; j < paths.Count; j++)
                {
                    paths[j].TotalWaze += lenBefore;
                    paths[j].Vertices.Push(i);
                }
                markedVertices[i] = false;
            }
        }
    }

    public DijakstraClass[] GetDijakstras(ushort sorce)
    {
        DijakstraClass[] dijakstras = new DijakstraClass[graphArr.GetLength(0)];
        for (int i = 0; i < graphArr.GetLength(0); i++)
            dijakstras[i] = (new DijakstraClass(i));
        
        Dijakstra(sorce, 0, dijakstras);
        return dijakstras;
    }

    public void Dijakstra(ushort sorce, ushort weightTillNow, DijakstraClass[] dijakstras)
    {
        for (int i = 0; i < dijakstras.Length; i++)
        {
            ushort weight = graphArr[sorce, dijakstras[i].Vertex];
            if (weight > 0 && dijakstras[i].LowestWeight > weightTillNow + weight && !dijakstras[i].Checked)
            {
                dijakstras[i].LowestWeight = weightTillNow + weight;
                dijakstras[i].Checked = true;
                dijakstras[i].PrevVertex = sorce;
                Dijakstra((ushort)dijakstras[i].Vertex, (ushort)(weight + weightTillNow), dijakstras);
                //dijakstras[i].Checked = false;
            }
        }
    }
}
public class Path
{
    public int TotalWaze { get; set; }
    public Stack<ushort> Vertices { get; set; }
    public int Weight { get; set; }

    public Path()
    {
        TotalWaze = 0;
        Vertices = new Stack<ushort>();
        Weight = 0;
    }
}

public class DijakstraClass
{
    public int LowestWeight { get; set; }
    public bool Checked { get; set; }
    public int PrevVertex { get; set; }
    public int Vertex { get; set; }

    public DijakstraClass(int vertex)
    {
        LowestWeight = int.MaxValue;
        Checked = false;
        PrevVertex = -1;
        Vertex = vertex;
    }
}

/*Graph graph = new Graph();
DijakstraClass[] dijakstras = graph.GetDijakstras(5);
for (int i = 0; i < graph.graphArr.GetLength(0); i++)
{
    Console.WriteLine($"vertex -  {dijakstras[i].Vertex} short way to next one is from {dijakstras[i].PrevVertex}");
}
Console.ReadLine();*/