using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Entity entity;

    [Header("Player Shortcuts")]
    public KeyCode attributesKey = KeyCode.C; //Tecla do teclado para abrir o menu de atributos

    [Header("Player UI Attributes")]
    public GameObject attributesPanel;

    [Header("Player UI")]
    public Slider health;
    public Slider mana;
    public Slider stamina;
    public Slider exp;
    public Text expText;
    public Text levelText;
    public Text strengthText;
    public Text resistenceText;
    public Text willPowerText;
    public Text intelligenceText;
    public Text pointsTxt;
    public Button strengthPositiveBtn;
    public Button resistencePositiveBtn;
    public Button willPowerPositiveBtn;
    public Button intelligencePositiveBtn;
    public Button strengthNegativeBtn;
    public Button resistenceNegativeBtn;
    public Button willPowerNegativeBtn;
    public Button intelligenceNegativeBtn;
    
    

    [Header("Exp")]
    public int currentExp;
    public int expBase;
    public int expLeft;
    public float expMod;
    public int givePoints = 5;    
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
        UpdatePoints();
        SetUIButtons();
    }

    private void Update() {
        if(entity.dead){
            return;
        }
        if(entity.currentHealth <= 0){
            Die();
        }

        if(Input.GetKeyDown(attributesKey)){
            attributesPanel.SetActive(!attributesPanel.activeSelf); //Ativa ou desativa o painel de atributos
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
        entity.points = givePoints;
        UpdatePoints();

        entity.currentHealth = entity.maxHealth;

        float newExp = Mathf.Pow((float) expMod, entity.level);
        expLeft = (int) Mathf.Floor((float) expBase * newExp);

        entity.entityAudio.PlayOneShot(levelUpSound);
        Instantiate(LevelUpFX, this.gameObject.transform);
    }

    public void UpdatePoints(){

        strengthText.text     = entity.strength.ToString();
        resistenceText.text   = entity.resistence.ToString();
        willPowerText.text    = entity.willPower.ToString();
        intelligenceText.text = entity.intelligence.ToString();
        pointsTxt.text        = entity.points.ToString();
    }

    public void SetUIButtons(){
        strengthPositiveBtn.onClick.AddListener(() => {
            AddPoints(1);
        });
        resistencePositiveBtn.onClick.AddListener(() => {
            AddPoints(2);
        });
        willPowerPositiveBtn.onClick.AddListener(() => {
            AddPoints(3);
        });
        intelligencePositiveBtn.onClick.AddListener(() => {
            AddPoints(4);
        });

        strengthNegativeBtn.onClick.AddListener(() => {
            RemovePoints(1);
        });
        resistenceNegativeBtn.onClick.AddListener(() => {
            RemovePoints(2);
        });
        willPowerNegativeBtn.onClick.AddListener(() => {
            RemovePoints(3);
        });
        intelligenceNegativeBtn.onClick.AddListener(() => {
            RemovePoints(4);
        });
        
    }

    public void AddPoints(int index){
        if(entity.points > 0){
            if(index == 1){
                entity.strength++;
            }
            else if(index == 2){
                entity.resistence++;
            }
            else if(index == 3){
                entity.willPower++;
            }
            else if(index == 4){
                entity.intelligence++;
            }

            entity.points--;
            UpdatePoints();
            
        }
    }

    public void RemovePoints(int index){
        if(entity.points > 0 ){
            if(index == 1 && entity.strength > 0){
                entity.strength--;
            }
            else if(index == 2 && entity.resistence > 0){
                entity.resistence--;
            }
            else if(index == 3 && entity.willPower > 0){
                entity.willPower--;
            }
            else if(index == 4  && entity.intelligence > 0){
                entity.intelligence--;
            }

            entity.points++;
            UpdatePoints();
        }
    }
}