using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyUnitControl : MonoBehaviour
{

    [SerializeField] private GridGen gridReference;
    [SerializeField] private TurnManager turnReference;
    [SerializeField] private GameObject[] friendlyUnits;
    [SerializeField] private UnitStats unitStats;

    [SerializeField] private GameObject targetUnit;
    [SerializeField] private GameObject unitToAttack;
    [SerializeField] private GameObject weaponEffect;
    public bool attackAttempted = false;
    [SerializeField] private bool unitDetected = false;

    [SerializeField] private CircleCollider2D unitDetectionCol;

    [SerializeField] private GameObject[] northAnchors, eastAnchors, westAnchors, southAnchors;

    public bool moved = false;
    public bool attacked = false;

    private void Awake()
    {
        gridReference = GameObject.Find("GridGenerator").GetComponent<GridGen>();
        turnReference = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        Node currentStart = gridReference.GetNodeFromWorldPoint(transform.position);
        currentStart.hasUnit = true;
        unitDetectionCol.radius = unitStats.attackRange / (2 * Mathf.PI);
    }

    private void Update()
    {
        friendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        AssignTarget();
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
        CheckMoveValidity();

        GameObject[][] allMoveAnchors = new GameObject[][] { northAnchors, eastAnchors, southAnchors, westAnchors };
        int randomDir = Random.Range(0, allMoveAnchors.Length);
        GameObject[] targetDir = allMoveAnchors[randomDir];

        int randomNode = Random.Range(0, targetDir.Length);
        GameObject targetPos = targetDir[randomNode];

        Node targetNode = gridReference.GetNodeFromWorldPoint(targetPos.transform.position);
        Node previousNode = gridReference.GetNodeFromWorldPoint(transform.position);

        if (!targetNode.hasObject && !targetNode.hasUnit && targetNode.withinMoveRange)
        {
            moved = true;
            previousNode.hasUnit = false;
            targetNode.hasUnit = true;
            transform.position = targetNode.worldPosition;
        }

        foreach (Node n in gridReference.grid)
        {
            if (n.withinMoveRange)
            {
                n.withinMoveRange = false;
            }
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
                    else
                    {
                        n.withinMoveRange = false;
                        currentArray[i].tag = "Untagged";
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

    // Attack
    public void UnitAttack()
    {
        if (unitDetected)
        {
            Node targetNode = gridReference.GetNodeFromWorldPoint(unitToAttack.transform.position);
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

                    if (hit.collider.CompareTag("FriendlyUnit"))
                    {
                        StartCoroutine(hit.collider.GetComponent<UnitStats>().DamageEffect());
                        hit.collider.gameObject.GetComponent<UnitStats>().health = hit.collider.gameObject.GetComponent<UnitStats>().health - this.GetComponent<UnitStats>().attackDamage;
                    }
                    else if (hit.collider.CompareTag("FriendlyBase"))
                    {
                        hit.collider.gameObject.GetComponent<BaseStats>().health = hit.collider.gameObject.GetComponent<BaseStats>().health - this.GetComponent<UnitStats>().attackDamage;
                    }
                }
            }
        }

        attackAttempted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FriendlyUnit") || collision.gameObject.CompareTag("FriendlyBase"))
        {
            unitDetected = true;
            unitToAttack = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FriendlyUnit") || collision.gameObject.CompareTag("FriendlyBase"))
        {
            unitDetected = false;
            unitToAttack = null;
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

}
