using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private GameObject placementIcon;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;
    //[HideInInspector] 
    [SerializeField] private bool unitSelected = false;
    [SerializeField] private Node previousNode; 

     void Awake()
    {
        gridReference = GameObject.Find("GridGenerator").GetComponent<GridGen>();
        placementIcon = GameObject.Find("PlacementCursorIcon");
    }

    void Update()
    {
        CursorPosition(); //Find cursor position
        UnitMovement(); //Unit movement functionality
        PlacementIndicatorFollow(); //Indicator follow cursor
    }

    void OnMouseDown()
    {
        UnitSelected();
    }

    private void UnitSelected()
    {
        if (!unitSelected)
        {
            placementIcon.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            placementIcon.SetActive(true);
            unitSelected = true;
        }
    }

    private void UnitDeselected()
    {
        if (unitSelected)
        {
            placementIcon.SetActive(false);
            unitSelected = false;
        }
    }


    private void UnitMovement()
    {
        if (unitSelected && Input.GetMouseButtonDown(0))
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);

                if (!targetNode.hasUnit)
                {
                    previousNode = gridReference.GetNodeFromWorldPoint(transform.position);
                    Vector3 gridSquarePos = targetNode.worldPosition;
                    targetNode.hasUnit = true;
                    transform.position = gridSquarePos;

                    previousNode.hasUnit = false;

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
