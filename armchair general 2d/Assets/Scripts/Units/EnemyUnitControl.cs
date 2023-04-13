using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnitControl : MonoBehaviour
{

    [SerializeField] private GridGen gridReference;
    [SerializeField] private GameObject[] friendlyUnits;
    private GameObject targetUnit;

    [SerializeField] private GameObject[] northAnchors, eastAnchors, westAnchors, southAnchors;

    [SerializeField] private bool moved = false;
    [SerializeField] private bool attacked = false;

    private void Awake()
    {
        gridReference = GameObject.Find("GridGenerator").GetComponent<GridGen>();
        Node currentStart = gridReference.GetNodeFromWorldPoint(transform.position);
        currentStart.hasUnit = true; 
    }

    private void Update()
    {
        friendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        AssignTarget();
        CheckMoveValidity();
    }

    private void AssignTarget()
    {
        if (targetUnit = null)
        {
            int randomTarget = Random.Range(0, friendlyUnits.Length);
            targetUnit = friendlyUnits[randomTarget];
        }
    }

    // Movement
    public void UnitMovement()
    {
        Node targetNode = gridReference.GetNodeFromWorldPoint(targetUnit.transform.position);
        Node previousNode = gridReference.GetNodeFromWorldPoint(transform.position);

        if (!targetNode.hasObject && !targetNode.hasUnit && targetNode.withinMoveRange)
        {
            moved = true;
            previousNode.hasUnit = false;
            targetNode.hasUnit = true;
            transform.position = targetNode.worldPosition;
        }

    }

    private void CheckMoveValidity()
    {
        GameObject[][] allMoveAnchors = new GameObject[][] { northAnchors, eastAnchors, southAnchors, westAnchors };

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
                        currentArray[i].tag = "Walkable";
                    }
                    else if (currentArray[i - 1].CompareTag("Walkable"))
                    {
                        n.withinMoveRange = true;
                        currentArray[i].tag = "Walkable";
                    }
                }
                else
                {
                    n.withinMoveRange = false;
                    currentArray[i].tag = "Untagged";
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
}
