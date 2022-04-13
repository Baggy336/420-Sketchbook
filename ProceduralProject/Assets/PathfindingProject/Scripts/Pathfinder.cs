using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    // Nested class to hold the squares within the pathfinding algorithm
    public class Square
    {
        public Vector3 pos;
        public float moveCost;

        public List<Square> neighborSquares = new List<Square>();
        public Square parentSquare { get; private set; }

        public float gCost { get; private set; }
        public float hCost { get; private set; }

        public float fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public void UpdateParentAndCost(Square parent, float extraCost = 0)
        {
            this.parentSquare = parent;
            if (parent != null)
            {
                gCost = parent.gCost + moveCost + extraCost;
            }
            else
            {
                gCost = extraCost;
            }
        }

        public void CheckDistance(Square end)
        {
            Vector3 dis = end.pos - this.pos;

            hCost = dis.magnitude;
        }
    }

    public static List<Square> FindPath(Square start, Square end)
    {
        if (start == null || end == null) return new List<Square>();

        // Make lists for checked squares and unchecked squares
        List<Square> uncheckedSquares = new List<Square>();
        List<Square> checkedSquares = new List<Square>();

        start.UpdateParentAndCost(null);
        uncheckedSquares.Add(start);

        while (uncheckedSquares.Count > 0)
        {
            float lowestF = 0;
            Square currentSquare = null;

            foreach(Square s in uncheckedSquares)
            {
                if (s.fCost < lowestF || currentSquare == null)
                {
                    currentSquare = s;
                    lowestF = s.fCost;
                }
            }

            if (currentSquare == end)
            {
                break;
            }

            bool neighborsChecked = false;
            foreach(Square neighbor in currentSquare.neighborSquares)
            {
                if (!checkedSquares.Contains(neighbor))
                {
                    if (!uncheckedSquares.Contains(neighbor))
                    {
                        uncheckedSquares.Add(neighbor);

                        float dis = (neighbor.pos - currentSquare.pos).magnitude;

                        neighbor.UpdateParentAndCost(currentSquare, dis);

                        if (neighbor == end)
                        {
                            neighborsChecked = true;
                        }

                        neighbor.CheckDistance(end);
                    }
                    else
                    {
                        float dis = (neighbor.pos - currentSquare.pos).magnitude;

                        if (currentSquare.gCost + neighbor.moveCost + dis < neighbor.gCost)
                        {
                            neighbor.UpdateParentAndCost(currentSquare, dis);
                        }
                    }
                }
            }

            checkedSquares.Add(currentSquare);

            uncheckedSquares.Remove(currentSquare);

            if (neighborsChecked) break;
        }

        List<Square> path = new List<Square>();
        for (Square temp = end; temp != null; temp = temp.parentSquare)
        {
            path.Add(temp);
        }

        path.Reverse();
        return path;

    }
}
