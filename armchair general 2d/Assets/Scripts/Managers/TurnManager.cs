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
    [SerializeField] private EnemyAI enemyReference;
    [SerializeField] private GameObject[] placementButtons;
    [SerializeField] private GameObject placementIcon;
    [SerializeField] private GameObject endButton;

    [Header("Turn Change")]
    [SerializeField] private int turnReward = 65;
    public bool playerTurn = true, enemyTurn = false;
    [SerializeField] private Texture[] endButtonTex; //Player Turn = 0, Enemy Turn = 1

    [Header("Active Units")]
    [SerializeField] private GameObject[] friendlyUnits;
    [SerializeField] private GameObject[] enemyUnits;
    private GameObject[] activeUnits;

    [Header("Phase Management")]
    public int currentPhase = 0; //Buy Phase = 0, Move Phase = 1, Attack Phase = 2

    public void Update()
    {
        friendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
    }

    public void EndTurn()
    {
        activeUnits = friendlyUnits.Concat(enemyUnits).ToArray();

        playerTurn = !playerTurn;
        enemyTurn = !enemyTurn;

        TurnStartup();
    }

    private void TurnStartup()
    {
        if (playerTurn)
        {
            endButton.GetComponent<Button>().enabled = true;
            endButton.GetComponent<RawImage>().texture = endButtonTex[0];

            enemyReference.enabled = false;

            shopReference.enabled = true;
            placementReference.enabled = true;
            placementIcon.SetActive(true);

            shopReference.playerCurrency = shopReference.playerCurrency + turnReward;

            for (int i = 0; i < friendlyUnits.Length; i++)
            {
                friendlyUnits[i].GetComponent<UnitControl>().moved = false;
                friendlyUnits[i].GetComponent<UnitControl>().unitSelected = false;
            }
            for (int j = 0; j < placementButtons.Length; j++)
            {
                placementButtons[j].GetComponent<Button>().enabled = true;
            }

            currentPhase = 0;
        }
        else if (enemyTurn)
        {
            endButton.GetComponent<Button>().enabled = false;
            endButton.GetComponent<RawImage>().texture = endButtonTex[1];

            enemyReference.enabled = true;
            enemyReference.enemyCurrency = enemyReference.enemyCurrency + turnReward;
            enemyReference.BuyUnits();

            shopReference.enabled = false;
            placementReference.enabled = false;
            placementIcon.SetActive(false);

            for (int i = 0; i < friendlyUnits.Length; i++)
            {
                friendlyUnits[i].GetComponent<UnitControl>().enabled = false;
                friendlyUnits[i].GetComponent<UnitControl>().unitSelected = false;
            }
            for (int j = 0; j < placementButtons.Length; j++)
            {
                placementButtons[j].GetComponent<Button>().enabled = false;
            }
        }
    }

}
