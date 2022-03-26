using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity
{
    [Header("Name")]
    public string name;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;
    public int level;

    [Header("Mana")]
    public int currentMana;
    public int maxMana;

    [Header("Stamina")]
    public int currentStamina;
    public int maxStamina;

    [Header("Stats")]
    public int strength     = 1;
    public int resistence   = 1;
    public int willPower    = 1;
    public int intelligence = 1;
    public int damage       = 1;
    public int defense      = 1;
    public float speed      = 1.5f;
    public int points       = 0;

    [Header("Combat")]
    public float attackDistance = 0.5f;
    public float attackTimer = 1f;
    public float cooldown = 2f;
    public bool inCombat = false;
    public GameObject target;
    public bool combatCoroutine = false;
    public bool dead = false;

    [Header("Component")]
    public AudioSource entityAudio;

}
