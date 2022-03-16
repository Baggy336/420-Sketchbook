using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    // Declare a class to hold a node within the pathfinding algorithm (nested class)
    public class Node
    {
        public Vector3 pos;

        public float G { get; private set; }
        public float H { get; private set; }

        public float F
        {
            get
            {
                return G + H;
            }
        }
        public float moveCost = 1; // This value can be changed to create different types of terrain

        public List<Node> neighbors = new List<Node>();

        private Node _parent;
        public Node parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (_parent != null)
                {
                    G = _parent.G + moveCost;
                }
                else
                {
                    G = 0;
                }
            }
        }

        public void DoHeuristic(Node end)
        {
            // Find distance between start and end 
            Vector3 d = end.pos - this.pos;

            // Set H to the distance between these objects
            H = d.magnitude;
        }
    }

    public static List<Node> Solve(Node start, Node end)
    {
        // Opening list of considered nodes
        List<Node> open = new List<Node>();

        // List of nodes which have already been considered
        List<Node> closed = new List<Node>();

        // Clear the parent node and add the start to the open list
        start.parent = null;
        open.Add(start);

        // 1. Travel from start to end
        while(open.Count > 0)
        {
            // Find node in open list with smallest F (total cost) value
            float bestF = 0;
            Node current = null;
            foreach(Node n in open)
            {
                // If the node we are checking has the lowest F value or we are at the first node
                if (n.F < bestF || current == null)
                {
                    // Set the node we are checking to the current node, and update the best F value
                    current = n;
                    bestF = n.F;
                }
            }

            // If the node is the end 
            if (current == end)
            {
                // break out of the loop
                break;
            }

            bool isDone = false;
            foreach (Node neighbor in current.neighbors)
            {
                // If the node is not in the already checked list
                if (!closed.Contains(neighbor))
                {
                    // If the node is not in the open list
                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                        // Set the neighbor's parent
                        neighbor.parent = current;

                        // If the current neighbor is the end
                        if (neighbor == end)
                        {
                            isDone = true;
                        }

                        // Set neighbor's G, H
                        neighbor.DoHeuristic(end);
                    }
                    else // Node is in the open list already
                    {
                        // If G cost is lower than the current stored value, change neighbor's parent to this node
                        if (current.G + neighbor.moveCost < neighbor.G )
                        {
                            // Shorter to move to neighbor from current
                            neighbor.parent = current;
                        }
                    }
                }               
            }
            if (isDone) break;
        }

        // 2. Travel from the end to start via lowest G values
        List<Node> path = new List<Node>();
        for (Node temp = end; temp != null; temp = temp.parent)
        {
            // Add each backstep to the path
            path.Add(temp);
        }

        // 3. Reverse the path back to start to end
        path.Reverse();

        return path;
    }
}

/*
 A* 
    Pick the path with the lowest G cost in total.
        add neighbors to the open list
        record how far from the start the node is
  
 */
