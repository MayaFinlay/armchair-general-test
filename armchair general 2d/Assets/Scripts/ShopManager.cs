using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Functionality")]
    [SerializeField] private PlacementManager placementReference;
    [HideInInspector] public int unitType; //Grunt = 0, Sniper = 1, Tank = 2

    [Header("Shop Information")]
    public int playerCurrency;
    public int[] shopPrices;
    public int[] upgradePrices;

    [Header("Displays")]
    [SerializeField] private TMP_Text currencyDisplay;
    [SerializeField] private GameObject infoDisplay;
    [SerializeField] private GameObject upgradeDisplay;
    [SerializeField] private Texture[] unitInfo;
    [SerializeField] private Texture[] unitUpgrade;

    [Header("Upgrade Functionality")]
    [HideInInspector] public bool unitUpgraded = false;
    [HideInInspector] public GameObject unitToUpgrade;
    [SerializeField] private GameObject[] upgradePrefabs;

    void Update()
    {
        CheckCurrency(); //Displays current currency
        HideDisplay(); //Hides display when units are not selected
    }

    public void CheckCurrency()
    {
        currencyDisplay.SetText(playerCurrency.ToString()); 
    }

    public void DisplayInfo()
    {
        if (placementReference.unitSelected)
        {
            infoDisplay.SetActive(true);
            upgradeDisplay.SetActive(false);
            unitType = placementReference.unitToBePlaced;
            infoDisplay.GetComponent<RawImage>().texture = unitInfo[unitType];
        }
        else
        {
            infoDisplay.SetActive(false);
        }
    }

    public void DisplayWithUpgrade()
    {
        if (!unitUpgraded)
        {
            infoDisplay.SetActive(true);
            upgradeDisplay.SetActive(true);
            infoDisplay.GetComponent<RawImage>().texture = unitInfo[unitType];
            upgradeDisplay.GetComponent<RawImage>().texture = unitUpgrade[unitType];
        }
        else
        {
            infoDisplay.SetActive(false);
            upgradeDisplay.SetActive(false);
        }
    }

    public void HideDisplay()
    {
        if (!placementReference.unitSelected && unitUpgraded)
        {
            infoDisplay.SetActive(false);
            upgradeDisplay.SetActive(false);
        }
    }


    public void UpgradeUnit()
    {
        if (!unitToUpgrade.GetComponent<UnitControl>().upgraded && playerCurrency >= upgradePrices[unitType])
        {
            playerCurrency = playerCurrency - upgradePrices[unitType];
            Instantiate(upgradePrefabs[unitType], unitToUpgrade.transform.position, Quaternion.identity);
            Destroy(unitToUpgrade);
        }
    }

}
