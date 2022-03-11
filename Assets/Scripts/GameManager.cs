using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CalculateHealth(Entity entity)
    {
        //Formula (resistence * 10) + (level * 5) + 10
        int result = ( entity.resistence * 10 ) + ( entity.level * 5 ) + 10;
        Debug.LogFormat("CalculateHealth: " + result);
        return result;
    }

    public int CalculateMana(Entity entity)
    {
        //Formula (intelligence * 10) + (level * 5) + 5[base]
        int result = ( entity.intelligence * 10 ) + ( entity.level * 5 ) + 5;
        Debug.LogFormat("CalculateMana: " + result);
        return result;
    }

    public int CalculateStamina(Entity entity)
    {
        //Formula (resistence * willpower) + (level * 2) + 5[base]
        int result = ( entity.resistence + entity.willPower ) + ( entity.level * 2 ) + 5;
        Debug.LogFormat("CalculateStamina: " + result);
        return result;
    }

    public int CalculateDamage(Entity entity, int weaponDamage)
    {
        //Formula (strength * 2) + (weapon * 2) + (level * 3) + random(1-20)
        System.Random rnd = new System.Random();

        int result = ( entity.strength * 2 )  + ( weaponDamage * 2 ) + ( entity.level * 3 ) + rnd.Next(0,20);
        Debug.LogFormat("CalculateDamage: " + result);
        return result;
    }

    public int CalculateDefense(Entity entity, int armorDefense)
    {
        //Formula (endurance[resistence] * 2) + (armor) + (level * 2)
        int result = ( entity.resistence * 2 ) + ( armorDefense ) + ( entity.level * 2 );
        Debug.LogFormat("CalculateDefense: " + result);
        return result;
    }
}
