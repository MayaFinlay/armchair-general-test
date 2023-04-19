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

    [Header("Attack")]
    public int attackRange; //See balancing sheet
    public float attackDamage; //See balancing sheet

    [Header("Shop")]
    public bool upgraded = false;
    [SerializeField] private GameObject glitchEffect;

    [Header("Audio")]
    [HideInInspector] public AudioSource voiceSource;
    [HideInInspector] public AudioSource sfxSource;
    public AudioClip[] idleAudio;
    public AudioClip spawnAudio;
    public AudioClip[] moveAudio;
    public AudioClip weaponAudio;
    public AudioClip[] attackAudio;
    public AudioClip[] killAudio;
    public AudioClip[] deathAudio;
    [HideInInspector] public int audioRarity;

    private void Awake()
    {
        health = maxHealth;
        healthbar.maxValue = maxHealth;
        glitchEffect.GetComponent<ParticleSystem>().Stop();
        damageEffect.GetComponent<ParticleSystem>().Stop();

        voiceSource = GameObject.Find("VoiceManager").GetComponent<AudioSource>();
        sfxSource = GameObject.Find("SFXManager").GetComponent<AudioSource>();
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
            StartCoroutine(DeathEffect());
        }
    }

    public int AudioRarity()
    {
        int randomRarity = Random.Range(0, 100);

        switch (randomRarity)
        {
            case <= 40:
                return audioRarity = -1;
            case <= 70:
                return audioRarity = 0;
            case <= 100:
                return audioRarity = 1;
            default:
                return audioRarity = -1;
        }
    }

    public IEnumerator DeathEffect()
    {
        if (AudioRarity() >= 0) voiceSource.PlayOneShot(deathAudio[audioRarity]);
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(this.gameObject);
    }

    public IEnumerator GlitchEffect()
    {
        sfxSource.PlayOneShot(spawnAudio);
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
        if (damageEffect != null) damageEffect.GetComponent<ParticleSystem>().Stop();
    }

}
