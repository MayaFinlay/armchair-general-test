using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    [Header("General")]
    public int unitType; //Grunt = 0, Sniper = 1, Tank = 2
    public float health;

    [Header("Health")]
    [SerializeField] private float maxHealth; //See balancing sheet
    [SerializeField] private Slider healthbar;

    [Header("Move")]

    [Header("Attack")]
    public int attackRange; //See balancing sheet
    public float attackDamage; //See balancing sheet

    [Header("Shop")]
    public bool upgraded = false;

    private void Awake()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
    }

    private void Update()
    {
        healthbar.value = health;
    }
}
