using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GridGen gridReference;
    [SerializeField] private PlacementManager placementReference;

    public GameObject[] playerSpawnPoints;
    public GameObject[] enemySpawnPoints;

    void Start()
    {
        SetPlayerSpawnLocations();
        SetEnemySpawnLocations();
    }

    private void SetPlayerSpawnLocations()
    {
        playerSpawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");

        for (int i = 0; i < playerSpawnPoints.Length; i++)
        {
                Node n = gridReference.GetNodeFromWorldPoint(playerSpawnPoints[i].transform.position);
                n.playerSpawnable = true;
        }
    }

    private void SetEnemySpawnLocations()
    {
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            Node n = gridReference.GetNodeFromWorldPoint(enemySpawnPoints[i].transform.position);
            n.enemySpawnable = true;
        }
    }

    public void ShowSpawnPoints()
    {
        if (placementReference.unitSelected)
        {
            for (int i = 0; i < playerSpawnPoints.Length; i++)
            {
                playerSpawnPoints[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < playerSpawnPoints.Length; i++)
            {
                playerSpawnPoints[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }


}
