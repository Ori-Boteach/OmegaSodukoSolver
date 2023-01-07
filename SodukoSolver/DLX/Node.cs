namespace SodukoSolver.DLX
{
    public class Node // Node object
    {
        public Node up { get; set; }
        public Node down { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }
        public HeaderNode columnHeader { get; set; }
        public Node(HeaderNode columnHeader) //  Node object constructor
        {
            up = this;
            down = this;
            left = this;
            right = this;
            this.columnHeader = columnHeader;
        }
        public void LinkRight(Node other) // link given node node (other) to the right of this node
        {
            other.right = right;
            right.left = other;
            right = other;
            other.left = this;
        }
        public void LinkDown(Node other) // link given node node (other) below this node
        {
            other.down = down;
            down.up = other;
            down = other;
            other.up = this;
        }
    }
}
