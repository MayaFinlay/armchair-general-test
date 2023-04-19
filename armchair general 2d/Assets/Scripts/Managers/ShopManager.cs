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
    [SerializeField] private Texture[] upgradedInfo;
    [SerializeField] private Texture[] unitUpgrade;

    [Header("Upgrade Functionality")]
    [HideInInspector] public bool unitUpgraded = false;
    [HideInInspector] public GameObject unitSelected;
    [SerializeField] private GameObject[] upgradePrefabs;

    void Update()
    {
        CheckCurrency(); //Displays current currency
        //HideDisplay(); //Hides display when units are not selected
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
            unitType = placementReference.unitToBePlaced;
           
            infoDisplay.GetComponent<RawImage>().texture = unitInfo[unitType];
        }
        else if(unitSelected.GetComponent<UnitControl>().unitSelected)
        {
            infoDisplay.SetActive(true);
            unitType = unitSelected.GetComponent<UnitStats>().unitType;

            if (unitSelected.GetComponent<UnitControl>().unitSelected && !unitUpgraded && !unitSelected.GetComponent<UnitControl>().moved)
            {
                infoDisplay.GetComponent<RawImage>().texture = unitInfo[unitType];
                upgradeDisplay.SetActive(true);
                upgradeDisplay.GetComponent<RawImage>().texture = unitUpgrade[unitType];
            }
            else if (unitSelected.GetComponent<UnitControl>().unitSelected && unitUpgraded)
            {
                infoDisplay.GetComponent<RawImage>().texture = upgradedInfo[unitType];
            }
            else
            {
                upgradeDisplay.SetActive(false);
            }
        }
        else
        {
            HideDisplay();
        }
    }

    public void HideDisplay()
    {
        infoDisplay.SetActive(false);
        upgradeDisplay.SetActive(false);
    }


    public void UpgradeUnit()
    {
        if (!unitSelected.GetComponent<UnitStats>().upgraded && playerCurrency >= upgradePrices[unitType])
        {
            HideDisplay();
            playerCurrency = playerCurrency - upgradePrices[unitType];
            unitUpgraded = true;
            DisplayInfo();
            GameObject unit = Instantiate(upgradePrefabs[unitType], unitSelected.transform.position, Quaternion.identity);
            Destroy(unitSelected);
        }
    }

}
