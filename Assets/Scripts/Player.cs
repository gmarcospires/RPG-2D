using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Entity entity;

    [Header("Player UI")]
    public Slider health;
    public Slider mana;
    public Slider stamina;
    public Slider exp;
    public Text expText;
    public Text levelText;

    [Header("Exp")]
    public int currentExp;
    public int expBase;
    public int expLeft;
    public float expMod;    
    public GameObject LevelUpFX;
    public AudioClip levelUpSound;

    [Header("Respawn")]
    public float respawnTime = 5f;
    public GameObject prefab;

    [Header("Player Regen System")]
    public bool regenHPEnabled = true;
    public float regenHPTime = 5f;
    public int regenHPValue = 1;

    public bool regenMPEnabled = true;
    public float regenMPTime = 10f;
    public int regenMPValue = 2;

    [Header("Game Manager")]
    public GameManager manager;



    void Start()
    {
        if (manager == null)
        {
            Debug.LogError("Você precisa anexar o game manager aqui no player");
            return;
        }

        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);

        entity.currentHealth  = entity.maxHealth;
        entity.currentMana    = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        health.maxValue = entity.maxHealth;
        health.value    = health.maxValue;

        mana.maxValue = entity.maxMana;
        mana.value    = mana.maxValue;

        stamina.maxValue = entity.maxStamina;
        stamina.value    = stamina.maxValue;
        
        exp.value = currentExp;
        exp.maxValue = expLeft;

        expText.text = String.Format("{0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();
        //Rotinas
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
    }

    private void Update() {
        if(entity.dead){
            return;
        }
        if(entity.currentHealth <= 0){
            Die();
        }
        
        health.value  = entity.currentHealth;
        mana.value    = entity.currentMana;
        stamina.value = entity.currentStamina;
        exp.value     = currentExp;
        exp.maxValue  = expLeft;

        expText.text = String.Format("{0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();
    }

    //Rotina
    IEnumerator RegenHealth()
    {
        while (true)
        {
            if (regenHPEnabled)
            {
                if (entity.currentHealth < entity.maxHealth)
                {
                    Debug.LogFormat("Recuperando HP do Jogador");
                    entity.currentHealth += regenHPValue;
                    yield return new WaitForSeconds(regenHPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator RegenMana()
    {
        while (true)
        {
            if (regenMPEnabled)
            {
                if (entity.currentMana < entity.maxMana)
                {
                    Debug.LogFormat("Recuperando MP do Jogador");
                    entity.currentMana += regenMPValue;
                    yield return new WaitForSeconds(regenMPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    void Die(){
        entity.currentHealth = 0;
        entity.dead = true;
        entity.target = null;
        StopAllCoroutines();

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        GetComponent<PlayerController>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
 
        GameObject newPlayer = Instantiate(prefab, transform.position, transform.rotation, null);
        newPlayer.name = prefab.name;
        newPlayer.GetComponent<Player>().entity.dead = false;
        newPlayer.GetComponent<Player>().entity.combatCoroutine = false;
        newPlayer.GetComponent<PlayerController>().enabled = true;

        Destroy(this.gameObject);
        
    }

    public void GainExp(int amount){
        currentExp += amount;
        if( currentExp >= expLeft ){
            LevelUp();
        }
    }

    public void LevelUp(){
        currentExp -= expLeft;
        entity.level++;

        entity.currentHealth = entity.maxHealth;

        float newExp = Mathf.Pow((float) expMod, entity.level);
        expLeft = (int) Mathf.Floor((float) expBase * newExp);

        entity.entityAudio.PlayOneShot(levelUpSound);
        Instantiate(LevelUpFX, this.gameObject.transform);
    }
}
