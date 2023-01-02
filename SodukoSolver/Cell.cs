using SodukoSolver;

public class Cell
{
    public int Value { get; set; }
    public HashSet<int> PossibleValues { get; set; }

    public Cell(int value)
    {
        Value = value;
        PossibleValues = new HashSet<int>();

        if (value == 0) // only if the cell is empty
        {   
            // add all possible values to the HashSet -> 1 to SIZE
            for (int i = 1; i <= UI.SIZE; i++)
            {
                PossibleValues.Add(i);
            }
        }
    }
}
