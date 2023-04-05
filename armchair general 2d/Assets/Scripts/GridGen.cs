using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGen : MonoBehaviour
{
    [HideInInspector]
    public Node[,] grid { get; private set; }

    [SerializeField] private Vector2 gridWorldSize;

    [HideInInspector] public int gridSizeX;
    [HideInInspector] public int gridSizeY;

    public float nodeRadius;
    private float nodeDiameter;

    [SerializeField] private bool debugMode = false;

    private void Awake()
    {
        nodeDiameter = nodeRadius + nodeRadius;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void Update()
    {
        if (debugMode)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            print(GetNodeFromWorldPoint(mouseWorldPos).x + " " + GetNodeFromWorldPoint(mouseWorldPos).y);
        }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSizeX / 2 - Vector3.up * gridSizeY / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * nodeDiameter + nodeRadius)) + (Vector3.up * (y * nodeDiameter + nodeRadius));
                grid[x, y] = new Node(x, y, true, worldPoint);
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(Mathf.Clamp((gridSizeX) * percentX, 0, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp((gridSizeY) * percentY, 0, gridSizeY - 1));

        return grid[x, y];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0f));

        if (grid != null && debugMode)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (n.hasObject) Gizmos.color = Color.blue;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            if (x == 0)
                continue;

            int checkX = node.x + x;

            if (checkX >= 0 && checkX < gridSizeX)
            {
                neighbours.Add(grid[checkX, node.y]);
            }
        }

        for (int y = -1; y <= 1; y++)
        {
            if (y == 0)
                continue;


            int checkY = node.y + y;

            if (checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[node.x, checkY]);
            }
        }

        return neighbours;
    }
}
