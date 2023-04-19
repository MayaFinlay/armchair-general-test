using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using UnityEngine.XR;
using static UnityEngine.UI.CanvasScaler;

public class UnitControl : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private GridGen gridReference;
    [SerializeField] private ShopManager shopReference;
    [SerializeField] private TurnManager turnReference;
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private GameObject placementIcon;
    private Vector3 rawMousePos;
    private Vector3 worldMousePos;

    public bool unitSelected = false;
    [SerializeField] private bool anyUnitsSelected = false;

    [Header("Movement Functionality")]
    public bool moved = false;
    private Node previousNode;
    [SerializeField] private GameObject[] northAnchors, eastAnchors, westAnchors, southAnchors;

    [Header("Attack Functionality")]
    public bool attacked = false;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private SpriteRenderer rangeDisplay;
    [SerializeField] private GameObject weaponEffect;
    [SerializeField] private Sprite targetSprite;

    public int unitPhase = 0;

    public bool isActive = true;


    void Awake()
    {
        gridReference = GameObject.Find("GridGenerator").GetComponent<GridGen>();
        shopReference = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        turnReference = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        placementIcon = GameObject.Find("PlacementManager").transform.Find("PlacementCursorIcon").gameObject;

        rangeDisplay.drawMode = SpriteDrawMode.Sliced;
        //var rangeSize = 2 * unitStats.attackRange;
        //rangeDisplay.size = new Vector2(rangeSize, rangeSize);

        rangeDisplay.size = new Vector2(unitStats.attackRange, unitStats.attackRange);
        rangeDisplay.enabled = false;
    }

    void Update()
    {
        CursorPosition(); //Find cursor position
        UnitMovement(); //Unit movement functionality
        UnitAttack(); //Unit movement functionality
        PlacementIndicatorFollow(); //Indicator follow cursor
        PhaseCheck();
        AimVisibility();
    }

    //Selecting
    void OnMouseDown()
    {
        if (isActive)
        {
            if (CheckSelects()) //Checks if any units are selected to avoid double select
            {
                UnitSelected();
                shopReference.unitSelected = this.gameObject;
                shopReference.DisplayInfo();
            }
        }
    }

    private void UnitSelected()
    {
        if (!unitSelected && !anyUnitsSelected)
        {
            if (unitPhase == 0)
            {
                placementIcon.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
            }
            else if (unitPhase == 1)
            {
                placementIcon.GetComponent<SpriteRenderer>().sprite = targetSprite;
            }

            placementIcon.GetComponent<SpriteRenderer>().enabled = true;
            unitSelected = true;
            shopReference.unitUpgraded = unitStats.upgraded;
            shopReference.unitType = unitStats.unitType;
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
        }
    }

    private bool CheckSelects()
    {
        GameObject[] allUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");

        for (int i = 0; i < allUnits.Length; i++)
        {
            if (allUnits[i].GetComponent<UnitControl>() != null)
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
        if (unitSelected && Input.GetMouseButtonDown(0) && unitPhase == 0)
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

        if (unitSelected && unitPhase == 0)
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
                            currentArray[i].tag = "Walkable";
                            currentArray[i].GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else if (currentArray[i - 1].CompareTag("Walkable")) // needs reworked
                        {
                            n.withinMoveRange = true;
                            currentArray[i].tag = "Walkable";
                            currentArray[i].GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else
                        {
                            n.withinMoveRange = false;
                            currentArray[i].tag = "Untagged";
                            currentArray[i].GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    else
                    {
                        n.withinMoveRange = false;
                        currentArray[i].tag = "Untagged";
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
        if (unitSelected && Input.GetMouseButtonDown(0) && unitPhase == 1)
        {
            if (CursorOverGrid())
            {
                Node targetNode = gridReference.GetNodeFromWorldPoint(worldMousePos);
                Node currentNode = gridReference.GetNodeFromWorldPoint(transform.position);

                float dirX = targetNode.x - currentNode.x;
                float dirY = targetNode.y - currentNode.y;

                Vector3 direction = new Vector3(dirX, dirY);
                float distance = direction.magnitude;

                if (distance <= unitStats.attackRange)
                {
                    RaycastHit2D hit = Physics2D.Linecast(currentNode.worldPosition, targetNode.worldPosition);
                    if (hit.collider != null)
                    {
                        attacked = true;
                        StartCoroutine(AttackEffect(currentNode, targetNode));

                        if (hit.collider.CompareTag("EnemyUnit"))
                        {
                            StartCoroutine(hit.collider.GetComponent<UnitStats>().DamageEffect());
                            hit.collider.gameObject.GetComponent<UnitStats>().health = hit.collider.gameObject.GetComponent<UnitStats>().health - this.GetComponent<UnitStats>().attackDamage;
                        }
                        else if (hit.collider.CompareTag("EnemyBase"))
                        {
                            hit.collider.gameObject.GetComponent<BaseStats>().health = hit.collider.gameObject.GetComponent<BaseStats>().health - this.GetComponent<UnitStats>().attackDamage;
                        }
                        UnitDeselected();
                    }
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

    private void AimVisibility()
    {
        if (unitSelected && unitPhase == 1)
        {
            aimLine.gameObject.SetActive(true);
            aimLine.positionCount = 2;
            aimLine.SetPosition(0, transform.position);
            aimLine.SetPosition(1, worldMousePos);
            rangeDisplay.enabled = true;
        }
        else
        {
            aimLine.gameObject.SetActive(false);
            rangeDisplay.enabled = false;
        }
    }

    public IEnumerator AttackEffect(Node currentPos, Node targetPos)
    {
        float time = 0f;
        while (time < 1f)
        {
            weaponEffect.SetActive(true);
            weaponEffect.transform.position = Vector3.Lerp(currentPos.worldPosition, targetPos.worldPosition, time / 0.25f);
            time += Time.deltaTime;
            yield return null;
        }

        if (time > 1f)
        {
            weaponEffect.SetActive(false);
        }

    }

    private void PhaseCheck()
    {
        if (!moved && !attacked)
        {
            unitPhase = 0;
        }
        else if (moved && !attacked)
        {
            unitPhase = 1;
        }
        else if (moved && attacked)
        {
            unitPhase = 2;
        }
    }
}
