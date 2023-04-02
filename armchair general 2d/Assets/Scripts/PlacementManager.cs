using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject placementIcon;
    [SerializeField] private Sprite[] unitSprites;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    private bool unitSelected = false;
    private int unitToBePlaced; //Grunt = 0, Sniper = 1, Tank = 2

    public void PlaceGrunt()
    {
        unitToBePlaced = 0;
        UnitSelected();
    }

    public void PlaceSniper()
    {
        unitToBePlaced = 1;
        UnitSelected();
    }

    public void PlaceTank()
    {
        unitToBePlaced = 2;
        UnitSelected();
    }

    private void UnitSelected()
    {
        placementIcon.GetComponent<SpriteRenderer>().sprite = unitSprites[unitToBePlaced];
        placementIcon.SetActive(true);
        if (!unitSelected)
            unitSelected = true;
    }

    void Update()
    {
        CursorPosition();
        PlacementIndicatorFollow();
        UnitPlacement();
    }

    private void CursorPosition()
    {
        rawMousePos = Input.mousePosition;
        worldMousePos = Camera.main.ScreenToWorldPoint(rawMousePos);
        worldMousePos.z = 0f;
    }

    private void UnitPlacement()
    {
        if (unitSelected && Input.GetMouseButtonDown(0))
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);

                if (!targetNode.hasUnit)
                {
                    Vector3 gridSquarePos = targetNode.worldPosition;
                    targetNode.hasUnit = true;
                    Instantiate(unitPrefabs[unitToBePlaced], gridSquarePos, Quaternion.identity);
                }
            }
            placementIcon.SetActive(false);
            unitSelected = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            unitSelected = false;
        }
    }

    private void PlacementIndicatorFollow()
    {
        placementIcon.transform.position = worldMousePos;
    }
    
    //Checks to see if cursor is in bounds of grid
    private bool CursorOverGrid()
    {
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
