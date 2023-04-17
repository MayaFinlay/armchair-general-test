using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    [SerializeField] private GridGen gridReference;

    public GameObject[] furniture;

     void Start()
    {
        SetFurnitureToGrid();
    }

    public void SetFurnitureToGrid()
    {
        furniture = GameObject.FindGameObjectsWithTag("Furniture");
        var enemyBase = GameObject.FindGameObjectsWithTag("EnemyBase");
        var friendlyBase = GameObject.FindGameObjectsWithTag("FriendlyBase");

        furniture = furniture.Concat(enemyBase).Concat(friendlyBase).ToArray();
        for (int i = 0; i < furniture.Length; i++)
        {
            Transform[] anchorPoints = furniture[i].transform.GetComponentsInChildren<Transform>();
            
            for(int j = 0; j < anchorPoints.Length; j++)
            {
                Node n = gridReference.GetNodeFromWorldPoint(anchorPoints[j].gameObject.transform.position);
                n.hasObject = true;
            }
        }
    }
}
