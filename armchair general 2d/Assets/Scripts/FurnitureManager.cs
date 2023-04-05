using System.Collections;
using System.Collections.Generic;
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

        int i = 0;
        foreach (GameObject obj in furniture)
        {
            Vector3 furniturePos = furniture[i].transform.position;
            furniturePos.z = 0f;

            Node setNode = gridReference.GetNodeFromWorldPoint(furniturePos);
            setNode.hasObject = true;
            Vector3 setNodePos = setNode.worldPosition;
            furniture[i].transform.position = setNodePos;

            i++;
        }
    }
}
