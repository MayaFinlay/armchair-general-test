using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private ShopManager shopReference;
    [SerializeField] private SpawnManager spawnReference;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject placementIcon;
    [SerializeField] private Sprite[] unitSprites;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    [HideInInspector] public bool unitSelected = false;
    [HideInInspector] public int unitToBePlaced; //Grunt = 0, Sniper = 1, Tank = 2

    public void PlaceGrunt()
    {
        unitToBePlaced = 0;
        if (shopReference.playerCurrency >= shopReference.shopPrices[unitToBePlaced])
        {
            UnitSelected();
        }

    }

    public void PlaceSniper()
    {
        unitToBePlaced = 1;
        if (shopReference.playerCurrency >= shopReference.shopPrices[unitToBePlaced])
        {
            UnitSelected();
        }
    }

    public void PlaceTank()
    {
        unitToBePlaced = 2;
        if (shopReference.playerCurrency >= shopReference.shopPrices[unitToBePlaced])
        {
            UnitSelected();
        }
    }

    private void UnitSelected()
    {
        if (!unitSelected)
        {
            placementIcon.GetComponent<SpriteRenderer>().sprite = unitSprites[unitToBePlaced];
            placementIcon.SetActive(true);
            unitSelected = true;
            spawnReference.ShowSpawnPoints();
        }    
    }

    private void UnitDeselected()
    {
        if (unitSelected)
        {
            placementIcon.SetActive(false);
            unitSelected = false;
            spawnReference.ShowSpawnPoints();
        }    
    }

    void Update()
    {
        CursorPosition(); //Find cursor position
        UnitPlacement(); //Unit placement functionality
        PlacementIndicatorFollow(); //Indicator follow cursor
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

                if (!targetNode.hasUnit && !targetNode.hasObject && targetNode.playerSpawnable)
                {
                    Vector3 gridSquarePos = targetNode.worldPosition;
                    targetNode.hasUnit = true;
                    Instantiate(unitPrefabs[unitToBePlaced], gridSquarePos, Quaternion.identity);
                    shopReference.playerCurrency = shopReference.playerCurrency - shopReference.shopPrices[unitToBePlaced];
                }
            }
            UnitDeselected();
            shopReference.HideDisplay();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            UnitDeselected();
        }
    }

    private void PlacementIndicatorFollow()
    {
        if(unitSelected)
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
