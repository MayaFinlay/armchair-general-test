using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GridGen gridReference;
    [SerializeField] private TurnManager turnReference;
    [SerializeField] private ShopManager shopReference;

    public int enemyCurrency = 600;
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private GameObject[] enemyUnits;
    [SerializeField] private int maxEnemies;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private int unitToSpawn; //Grunt = 0, Sniper = 1, Tank = 2

    [SerializeField] private bool[] attackCheck = {};

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    private void Update()
    {
        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        AllOptionsExplored();
    }

    public void BuyUnits()
    {
        if (enemyUnits.Length < maxEnemies && turnReference.enemyTurn)
        {
            switch (enemyCurrency)
            {
                case <= 200:
                    StartCoroutine(MoveUnits());
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
        else if (enemyUnits.Length >= maxEnemies && turnReference.enemyTurn)
        {
            StartCoroutine(MoveUnits());
        }
    }

    private void SpawnUnit()
    {
        int randomPos = Random.Range(0, spawnPoints.Length);
        Vector3 randomSpawn = spawnPoints[randomPos].transform.position;

        Node targetNode = gridReference.GetNodeFromWorldPoint(randomSpawn);

        if (!targetNode.hasUnit)
        {
            GameObject unit = Instantiate(enemyPrefabs[unitToSpawn], randomSpawn, Quaternion.identity);
            enemyCurrency = enemyCurrency - shopReference.shopPrices[unitToSpawn];
            StartCoroutine(unit.GetComponent<UnitStats>().GlitchEffect());

            targetNode.hasUnit = true;
            StartCoroutine(MoveUnits());
        }
        BuyUnits();
    }

    public IEnumerator MoveUnits()
    {
        if (enemyUnits != null && turnReference.enemyTurn)
        {
            yield return new WaitForSecondsRealtime(1f);

            for (int i = 0; i < enemyUnits.Length; i++)
            {
                if (!enemyUnits[i].GetComponent<EnemyUnitControl>().moved)
                {
                    enemyUnits[i].GetComponent<EnemyUnitControl>().UnitMovement();
                }
            }
            StartCoroutine(AttackUnits());
        }
    }

    public IEnumerator AttackUnits()
    {
        if (enemyUnits != null && turnReference.enemyTurn)
        {
            yield return new WaitForSecondsRealtime(1f);

            for (int i = 0; i < enemyUnits.Length; i++)
            {
                if (!enemyUnits[i].GetComponent<EnemyUnitControl>().attacked)
                {
                    enemyUnits[i].GetComponent<EnemyUnitControl>().UnitAttack();
                }
            }
        }
    }

    private void AllOptionsExplored()
    {
        if (turnReference.enemyTurn)
        {
            attackCheck = new bool[enemyUnits.Length];
            for (int i = 0; i < enemyUnits.Length; ++i)
            {
                attackCheck[i] = enemyUnits[i].GetComponent<EnemyUnitControl>().attackAttempted;
            }

            if (attackCheck.Length == enemyUnits.Length && !attackCheck.Contains(false))
            {
                turnReference.EndTurn();
            }
        }
    }
}
