namespace SodukoSolver.DLX
{
    #pragma warning disable CS8625 // disable -> Cannot convert null literal to non-nullable reference type
    public class HeaderNode : Node // NodeHeader object based on Node object for column headers
    {
        public int name { get; set; } // the name == index of the column header
        public int headerNodeCount { get; set; } // how many nodes there are below the column header
        public HeaderNode(int name) : base(null) //  NodeHeader object constructor -> sets name, initializes counter to zero and it's header to itself
        {
            columnHeader = this;
            this.name = name;
            headerNodeCount = 0;
        }
    }
}
