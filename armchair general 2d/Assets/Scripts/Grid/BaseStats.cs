using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseStats : MonoBehaviour
{
    [Header("Health")]
    public float health;
    [SerializeField] private float maxHealth; //See balancing sheet
    [SerializeField] private Slider healthbar;
    [SerializeField] private GameObject damageEffect;

    private void Awake()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
        damageEffect.GetComponent<ParticleSystem>().Stop();
    }

    private void Update()
    {
        healthbar.maxValue = maxHealth;
        healthbar.value = health;
        CheckToDestroy();
    }
    private void CheckToDestroy()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public IEnumerator DamageEffect()
    {
        damageEffect.SetActive(true);
        damageEffect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSecondsRealtime(1f);
        if (damageEffect != null) damageEffect.GetComponent<ParticleSystem>().Stop();
    }

}
