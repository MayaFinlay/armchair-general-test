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
    [SerializeField] private GameObject damageEffect;

    [Header("Move")]

    [Header("Attack")]
    public int attackRange; //See balancing sheet
    public float attackDamage; //See balancing sheet

    [Header("Shop")]
    public bool upgraded = false;
    [SerializeField] private GameObject glitchEffect;

    private void Awake()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
        glitchEffect.GetComponent<ParticleSystem>().Stop();
        damageEffect.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        healthbar.value = health;
        CheckToDestroy();
    }

    private void CheckToDestroy()
    {
        if (health <= 0)
        {
            Destroy(this); 
        }
    }

    public IEnumerator GlitchEffect()
    {

        glitchEffect.SetActive(true);
        glitchEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(1f);
        glitchEffect.GetComponent<ParticleSystem>().Stop();
    }


    public IEnumerator DamageEffect()
    {
        damageEffect.SetActive(true);
        damageEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(1f);
        damageEffect.GetComponent<ParticleSystem>().Stop();
    }

}
