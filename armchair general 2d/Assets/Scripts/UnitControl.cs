using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private ShopManager shopReference;
    [SerializeField] private GameObject placementIcon;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    public bool unitSelected = false;
    [SerializeField] private bool anyUnitsSelected = false;

    [SerializeField] private Node previousNode;

    [Header("Unit Stats")]
    public int unitType; //Grunt = 0, Sniper = 1, Tank = 2 ; Set in Prefab
    //[SerializeField] private int unitSpeed; //Grunt = 3, Sniper = 1, Tank = 2; Set in Prefab
    public bool upgraded = false;
    


    void Awake()
    {
        gridReference = GameObject.Find("GridGenerator").GetComponent<GridGen>();
        shopReference = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        placementIcon = GameObject.Find("PlacementManager").transform.Find("PlacementCursorIcon").gameObject; 
    }

    void Update()
    {
        CursorPosition(); //Find cursor position
        UnitMovement(); //Unit movement functionality
        PlacementIndicatorFollow(); //Indicator follow cursor
    }

    void OnMouseDown()
    {
        if (CheckSelects()) //Checks if any units are selected to avoid double select
        {
            UnitSelected();
            shopReference.unitToUpgrade = this.gameObject;
            shopReference.DisplayWithUpgrade();
        }
    }

    private void UnitSelected()
    {
        if (!unitSelected && !anyUnitsSelected)
        {
            placementIcon.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            placementIcon.SetActive(true);
            unitSelected = true;
            shopReference.unitUpgraded = upgraded;
            shopReference.unitType = unitType;
        }
    }

    private void UnitDeselected()
    {
        placementIcon.SetActive(false);
        unitSelected = false;
    }

    private bool CheckSelects()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        
        for(int i = 0; i < allUnits.Length; i++)
        {
            if(allUnits[i].GetComponent<UnitControl>() != null)
            {
                if (allUnits[i].GetComponent<UnitControl>().unitSelected)
                {
                    return false;
                }
            }
        }
        return true;
    }


    private void UnitMovement()
    {
        if (unitSelected && Input.GetMouseButtonDown(0))
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);
                previousNode = gridReference.GetNodeFromWorldPoint(transform.position);

                if (!targetNode.hasUnit && !targetNode.hasObject)
                {
                    previousNode.hasUnit = false;
                    targetNode.hasUnit = true;
                    transform.position = targetNode.worldPosition;
                    UnitDeselected();
                }
                else if(targetNode.hasObject)
                {
                    UnitDeselected();
                }
            }
            else
            {
                UnitDeselected();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            UnitDeselected();
        }
    }

    private void CursorPosition()
    {
        rawMousePos = Input.mousePosition;
        worldMousePos = Camera.main.ScreenToWorldPoint(rawMousePos);
        worldMousePos.z = 0f;
    }

    private void PlacementIndicatorFollow()
    {
        if (unitSelected)
            placementIcon.transform.position = worldMousePos;
    }

    //Checks to see if cursor is in bounds of grid
    private bool CursorOverGrid()
    {
        if (worldMousePos.x < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.x + gridReference.nodeRadius && worldMousePos.y < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.y + gridReference.nodeRadius)
        {
            if (worldMousePos.x > gridReference.grid[0, 0].worldPosition.x - gridReference.nodeRadius && worldMousePos.y > gridReference.grid[0, 0].worldPosition.y - gridReference.nodeRadius)
            {
                return true;
            }
        }
        return false;
    }
}
