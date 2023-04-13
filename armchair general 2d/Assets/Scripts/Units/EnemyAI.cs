using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GridGen gridReference;
    [SerializeField] private EnemyUnitControl enemyReference;

    public int enemyCurrency = 600;
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private int phase = 0;
    [SerializeField] private bool moved = false;
    [SerializeField] private bool attacked = false;

    [SerializeField] private GameObject[] enemyUnits;
    [SerializeField] private int maxEnemies;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private int unitToSpawn; //Grunt = 0, Sniper = 1, Tank = 2

    private void Update()
    {
        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        PhaseCheck();
    }

    public void BuyUnits()
    {
        if (enemyUnits.Length <= maxEnemies)
        {
            switch (enemyCurrency)
            {
                case <= 200:
                    MoveUnits();
                    break;
                case <= 299:
                    unitToSpawn = 0;
                    SpawnUnit();
                    break;
                case <= 399:
                    unitToSpawn = 1;
                    SpawnUnit();
                    break;
                case >= 400:
                    unitToSpawn = 2;
                    SpawnUnit();
                    break;
            }
        }
        else
        {
            MoveUnits();
        }
    }

    private void SpawnUnit()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
        int randomPos = Random.Range(0, spawnPoints.Length);
        Vector3 randomSpawn = spawnPoints[randomPos].transform.position;

        Node targetNode = gridReference.GetNodeFromWorldPoint(randomSpawn);

        if (!targetNode.hasUnit)
        {
            GameObject unit = Instantiate(enemyPrefabs[unitToSpawn], randomSpawn, Quaternion.identity);
            StartCoroutine(unit.GetComponent<UnitStats>().GlitchEffect());

            targetNode.hasUnit = true;
        }
        else return;
    }

    private void MoveUnits()
    {
        if (phase == 0)
        {
            for (int i = 0; i < enemyUnits.Length; i++)
            {
                enemyReference.UnitMovement();
            }
        }
    }

    private void AttackUnits()
    {
        if (phase == 1)
        {
            
        }
    }

    private void PhaseCheck()
    {
        bool allMoved = false;
        for (int i = 0; i < enemyUnits.Length; i++)
        {
            if (moved)
            {
                allMoved = true;
            }
            else
            {
                allMoved = false;
                phase = 0;
            }
        }

        if (allMoved)
        {
            phase = 1;
            AttackUnits();
        }

    }
}
