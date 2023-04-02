using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private GameObject[] unitPrefabs;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    private bool unitSelected = false;
    private int unitToBePlaced; //Grunt = 0, Sniper = 1, Tank = 2

    public void PlaceGrunt()
    {
        UnitSelected();
        unitToBePlaced = 0;
    }

    public void PlaceSniper()
    {
        UnitSelected();
        unitToBePlaced = 1;
    }

    public void PlaceTank()
    {
        UnitSelected();
        unitToBePlaced = 2;
    }

    private void UnitSelected()
    {
        if (!unitSelected)
            unitSelected = true;
    }

    void Update()
    {
        if (unitSelected && Input.GetMouseButtonDown(0))
        {
            if (CursorOverGrid())
            {
                rawMousePos = Input.mousePosition;
                worldMousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);
                
                if(!targetNode.hasUnit)
                {
                    Vector3 gridSquarePos = targetNode.worldPosition;
                    targetNode.hasUnit = true;
                    Instantiate(unitPrefabs[unitToBePlaced], gridSquarePos, Quaternion.identity);
                }
            }
            unitSelected = false;
        }
    }
    //Checks to see if cursor is in bounds of grid
    private bool CursorOverGrid()
    {
        rawMousePos = Input.mousePosition;

        worldMousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

        if(worldMousePos.x < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.x + gridReference.nodeRadius && worldMousePos.y < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.y + gridReference.nodeRadius)
        {
            if (worldMousePos.x > gridReference.grid[0, 0].worldPosition.x - gridReference.nodeRadius && worldMousePos.y > gridReference.grid[0, 0].worldPosition.y - gridReference.nodeRadius)
            {
                return true;
            }
        }    
        return false;
    }
}
