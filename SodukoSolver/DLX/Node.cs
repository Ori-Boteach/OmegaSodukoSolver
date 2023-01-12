namespace SodukoSolver.DLX
{
    public class Node // Node object
    {
        public Node up { get; set; } // the node above the current node
        public Node down { get; set; } // the node below the current node 
        public Node left { get; set; } // the node to the left of the current node
        public Node right { get; set; } // the node to the right of the current node
        public HeaderNode columnHeader { get; set; } // the node's column header
        public Node(HeaderNode columnHeader) //  Node object constructor -> link to it's header and itself
        {
            up = this;
            down = this;
            left = this;
            right = this;
            this.columnHeader = columnHeader;
        }
        public void LinkRight(Node other) // link given node (other) to the right of this node
        {
            other.right = right;
            right.left = other;
            right = other;
            other.left = this;
        }
        public void LinkDown(Node other) // link given node (other) below this node
        {
            other.down = down;
            down.up = other;
            down = other;
            other.up = this;
        }
    }
}
