
Print_rectangle(3, 5);
// adding comment
// commit from vs 
void Print_rectangle(int h, int w)
{
	for (int i = 0; i < h; i++)
	{
        for (int j = 0; j < w; j++)
        {
            Console.Write("# ");
        }
        Console.WriteLine();
    }
	
}