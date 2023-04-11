using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class UnitControl : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private ShopManager shopReference;
    [SerializeField] private GameObject placementIcon;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    public bool unitSelected = false;
    private bool anyUnitsSelected = false;

    [Header("Movement Functionality")]
    private Node previousNode;
    public bool moved = false;
    [SerializeField] private GameObject[] northAnchors, eastAnchors, westAnchors, southAnchors;

    [Header("Attack Functionality")]
    public bool attacked = false;

    [Header("Unit Stats")]
    public int unitType; //Grunt = 0, Sniper = 1, Tank = 2 ; Set in Prefab
    [SerializeField] private int unitAttackRange; // Set in Prefab;
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

    //Selecting
    void OnMouseDown()
    {
        if (CheckSelects()) //Checks if any units are selected to avoid double select
        {
            UnitSelected();
            shopReference.unitSelected = this.gameObject;
            shopReference.DisplayInfo();
        }
    }

    private void UnitSelected()
    {
        if (!unitSelected && !anyUnitsSelected)
        {
            placementIcon.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            placementIcon.GetComponent<SpriteRenderer>().enabled = true;
            unitSelected = true;
            shopReference.unitUpgraded = upgraded;
            shopReference.unitType = unitType;
            CheckMoveValidity();
        }
    }

    private void UnitDeselected()
    {
        if (unitSelected)
        {
            placementIcon.GetComponent<SpriteRenderer>().enabled = false;
            unitSelected = false;
            CheckMoveValidity();

            if (moved)
            {
                shopReference.HideDisplay();
            }
        }
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

    private bool CursorOverGrid()    //Checks to see if cursor is in bounds of grid
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


    //Movement
    private void UnitMovement()
    {
        if (unitSelected && Input.GetMouseButtonDown(0) && !moved)
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);
                previousNode = gridReference.GetNodeFromWorldPoint(transform.position);

                if (!targetNode.hasObject && !targetNode.hasUnit && targetNode.withinMoveRange)
                {
                    moved = true;
                    previousNode.hasUnit = false;
                    targetNode.hasUnit = true;
                    transform.position = targetNode.worldPosition;

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

    private void CheckMoveValidity()
    {
        GameObject[][] allMoveAnchors = new GameObject[][] { northAnchors, eastAnchors, southAnchors, westAnchors };
        
        if (unitSelected && !moved)
        {
            foreach (GameObject[] anchorArray in allMoveAnchors)
            {
                GameObject[] currentArray = anchorArray;

                for (int i = 0; i < currentArray.Length; i++)
                {
                    Node n = gridReference.GetNodeFromWorldPoint(currentArray[i].transform.position);
                    if (!n.hasObject && !n.hasUnit && MoveWithinGrid(currentArray[i].transform.position))
                    {
                        if (i < 1)
                        {
                            n.withinMoveRange = true;
                            currentArray[i].GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else if (currentArray[i - 1].GetComponent<SpriteRenderer>().enabled)
                        {
                            n.withinMoveRange = true;
                            currentArray[i].GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                    else
                    {
                        n.withinMoveRange = false;
                        currentArray[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }

        }
        else
        {
            foreach (GameObject[] anchorArray in allMoveAnchors)
            {
                GameObject[] currentArray = anchorArray;

                for (int i = 0; i < currentArray.Length; i++)
                {
                    currentArray[i].GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            foreach (Node n in gridReference.grid)
            {
                if (n.withinMoveRange)
                {
                    n.withinMoveRange = false;
                }
            }
        }
    }

    private bool MoveWithinGrid(Vector3 moveNodePos)
    {
        if (moveNodePos.x < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.x + gridReference.nodeRadius && moveNodePos.y < gridReference.grid[gridReference.gridSizeX - 1, gridReference.gridSizeY - 1].worldPosition.y + gridReference.nodeRadius)
        {
            if (moveNodePos.x > gridReference.grid[0, 0].worldPosition.x - gridReference.nodeRadius && moveNodePos.y > gridReference.grid[0, 0].worldPosition.y - gridReference.nodeRadius)
            {
                return true;
            }
        }
        return false;
    }


    //Attacking
    private void UnitAttack()
    {
        if (unitSelected && Input.GetMouseButtonDown(0) && !attacked)
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);
                Node currentNode = gridReference.GetNodeFromWorldPoint(transform.position);

                if (targetNode.hasUnit && targetNode.withinAttackRange)
                {
                    attacked = true;
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
}
