using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private ShopManager shopReference;
    [SerializeField] private PlacementManager placementReference;
    [SerializeField] private GameObject[] placementButtons;
    public bool playerTurn = true, enemyTurn = false;
    [SerializeField] private GameObject[] activeUnits;

    public void EndTurn()
    {
        GameObject[] friendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");

        activeUnits = friendlyUnits.Concat(enemyUnits).ToArray();

        playerTurn = !playerTurn;
        enemyTurn = !enemyTurn;

        TurnStartup(friendlyUnits, enemyUnits);
    }

    private void TurnStartup(GameObject[] allies, GameObject[] enemies)
    {
        if (playerTurn)
        {
            shopReference.enabled = false;
            placementReference.enabled = false;

            for (int i = 0; i < allies.Length; i++)
            {
                allies[i].GetComponent<UnitControl>().enabled = true;
                allies[i].GetComponent<UnitControl>().moved = false;
            }
            for (int j = 0; j < placementButtons.Length; j++)
            {
                placementButtons[j].GetComponent<Button>().enabled = true;
            }
        }
        else if (enemyTurn)
        {
            shopReference.enabled = false;
            placementReference.enabled = false;

            for (int i = 0; i < allies.Length; i++)
            {
                allies[i].GetComponent<UnitControl>().enabled = false;
                allies[i].GetComponent<UnitControl>().unitSelected = false;
            }
            for (int j = 0; j < placementButtons.Length; j++)
            {
                placementButtons[j].GetComponent<Button>().enabled = false;
            }
        }
    }
}
