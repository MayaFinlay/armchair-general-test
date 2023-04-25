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
    [SerializeField] private bool turnEnded = false;
    [SerializeField] private int turnCounter = 0;

    [Header("Active Units")]
    [SerializeField] private GameObject[] friendlyUnits;
    [SerializeField] private GameObject[] enemyUnits;
    private GameObject[] activeUnits;

    [Header("Active Bases")]
    [SerializeField] private GameObject friendlyBase;
    [SerializeField] private GameObject enemyBase;

    [Header("Game Win/Loss")]
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool playerWon = false;
    [SerializeField] private bool playerLost = false;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winUI, lossUI;
    [SerializeField] private Texture[] gameOverTex; //Victory = 0, Defeat = 1

    [Header("Audio")]
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip lossClip;
    [SerializeField] private AudioClip[] startClip;

    public void Start()
    {
        var random = Random.Range(0, startClip.Length);
        voiceSource.clip = startClip[random];
        voiceSource.Play();
    }

    public void Update()
    {
        friendlyUnits = GameObject.FindGameObjectsWithTag("FriendlyUnit");
        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");

        TurnStartup();
        GameOver();
    }

    public void EndTurn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activeUnits = friendlyUnits.Concat(enemyUnits).ToArray();

        playerTurn = !playerTurn;
        enemyTurn = !enemyTurn;

        turnCounter++;

        turnEnded = true;
    }

    private void TurnStartup()
    {
        if (turnEnded && !gameOver)
        {
            if (playerTurn && !enemyTurn)
            {
                turnEnded = false;

                endButton.GetComponent<Button>().enabled = true;
                endButton.GetComponent<RawImage>().texture = endButtonTex[0];

                enemyReference.enabled = false;

                shopReference.enabled = true;
                placementReference.enabled = true;
                placementIcon.SetActive(true);

                shopReference.playerCurrency = shopReference.playerCurrency + turnReward;

                for (int i = 0; i < friendlyUnits.Length; i++)
                {
                    friendlyUnits[i].GetComponent<UnitControl>().isActive = true;
                    friendlyUnits[i].GetComponent<UnitControl>().moved = false;
                    friendlyUnits[i].GetComponent<UnitControl>().attacked = false;
                    friendlyUnits[i].GetComponent<UnitControl>().unitSelected = false;
                    friendlyUnits[i].GetComponent<UnitControl>().unitPhase = 0;
                }
                for (int j = 0; j < placementButtons.Length; j++)
                {
                    placementButtons[j].GetComponent<Button>().enabled = true;
                }

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if (enemyTurn && !playerTurn)
            {
                turnEnded = false;

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
                    friendlyUnits[i].GetComponent<UnitControl>().isActive = false;
                    friendlyUnits[i].GetComponent<UnitControl>().unitSelected = false;
                }
                for (int j = 0; j < placementButtons.Length; j++)
                {
                    placementButtons[j].GetComponent<Button>().enabled = false;
                }
                for (int i = 0; i < enemyUnits.Length; i++)
                { 
                    enemyUnits[i].GetComponent<EnemyUnitControl>().moved = false;
                    enemyUnits[i].GetComponent<EnemyUnitControl>().attacked = false;
                    enemyUnits[i].GetComponent<EnemyUnitControl>().attackAttempted = false;
                }

            }
        }
    }

    private void GameOver()
    {
        if (!gameOver)
        {
            if (turnCounter >= 3)
            {
                if ((friendlyUnits.Length <= 0 && shopReference.playerCurrency < shopReference.shopPrices[0]) || friendlyBase.GetComponent<BaseStats>().health <= 0 && enemyTurn)
                {
                    AudioStop();
                    gameOver = true;
                    playerLost = true;
                    Defeat();
                }
                else if ((enemyUnits.Length <= 0 && enemyReference.enemyCurrency < shopReference.shopPrices[0]) || enemyBase.GetComponent<BaseStats>().health <= 0 && playerTurn)
                {
                    AudioStop();
                    gameOver = true;
                    playerWon = true;
                    Victory();
                }
            }
        }
    }

    private void AudioStop()
    {
        var allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audios in allAudioSources)
        {
            audios.enabled = false;
            audios.Stop();
            audios.enabled = true;
        }
    }


    private void Victory()
    {
        if (playerWon)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            gameOverScreen.SetActive(true);
            winUI.SetActive(true);
            gameOverScreen.GetComponent<RawImage>().texture = gameOverTex[0];
            voiceSource.PlayOneShot(winClip);
        }
    }

    private void Defeat()
    {
        if (playerLost)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            gameOverScreen.SetActive(true);
            lossUI.SetActive(true);
            gameOverScreen.GetComponent<RawImage>().texture = gameOverTex[1];
            voiceSource.PlayOneShot(lossClip);
        }
    }

}

