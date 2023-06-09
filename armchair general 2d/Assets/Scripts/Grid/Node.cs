using System;
using UnityEngine;

public class Node : IComparable<Node> 
{
    //Node Attributes
    public int x, y;
    public bool walkable ; // Consider removal
    public Vector2 worldPosition;
    public bool hasUnit = false; 
    public bool hasObject = false; 
    public bool playerSpawnable = false; public bool enemySpawnable = false;
    public bool withinMoveRange = false;
    public bool withinAttackRange = false;

    //Pathfinding
    public Node parent;
    public Node[] neighbours; 
    public int gCost, hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //Constructor
    public Node(int _xCoord, int _yCoord, bool _walkable, Vector2 _worldPosition)
    {
        x = _xCoord;
        y = _yCoord;
        walkable = _walkable;
        worldPosition = _worldPosition;
    }

    //Interface Compare Function For Pathfinding - not in use yet
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return compare;
    }
}
