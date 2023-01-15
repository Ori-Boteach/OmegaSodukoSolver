using SodukoSolver;

public class Cell
{
    // this class - Cell, is used to represent a single cell in the soduko board
    // the class has a property 'Value' which stores the value of the cell and a property 'PossibleValues' which is a HashSet of all possible values that could be in the cell
    
    public int Value { get; set; }
    public HashSet<int> PossibleValues { get; set; }

    // The constructor takes an int value as input and assigns it to the 'Value' property
    // if the value is 0, it means the cell is empty and all possible values from 1 to SIZE are added to the 'PossibleValues' HashSet (being used to keep track of possible values when solving the soduko using the backtracking algorithm)
    public Cell(int value)
    {
        this.Value = value;
        PossibleValues = new HashSet<int>();

        if (value == 0) // only if the cell is empty
        {   
            // add all possible values to the HashSet -> 1 to SIZE
            for (int i = 1; i <= UI.SIZE; i++)
                PossibleValues.Add(i);
        }
    }
}
