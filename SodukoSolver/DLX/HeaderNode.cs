namespace SodukoSolver.DLX
{
    public class HeaderNode : Node // NodeHeader object based on Node object
    {
        public int name { get; set; }
        public int headerNodeCount { get; set; }
        public HeaderNode(int name) : base(null) //  NodeHeader object constructor
        {
            columnHeader = this;
            this.name = name;
            headerNodeCount = 0;
        }
    }
}
