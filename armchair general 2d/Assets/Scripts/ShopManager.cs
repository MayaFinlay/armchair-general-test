using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{

    [SerializeField] private PlacementManager placementReference;
    public int playerCurrency;
    [SerializeField] private TMP_Text currencyDisplay;
    public int[] shopPrices;

    [SerializeField] private GameObject infoDisplay;
    [SerializeField] private Texture[] unitInfo;
    private int infoToDisplay; //Grunt = 0, Sniper = 1, Tank = 2

    void Update()
    {
        DisplayInfo(); //Display stats for selected unit type
        CheckCurrency(); //Displays current currency
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
            infoToDisplay = placementReference.unitToBePlaced;
            infoDisplay.GetComponent<RawImage>().texture = unitInfo[infoToDisplay];
        }
        else
        {
            infoDisplay.SetActive(false);
        }
    }

}
