using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;

    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        //set initial health to max
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduce current health by incoming damage
    /// </summary>
    /// <param name="damage">Amount of damage taken</param>
    /// <param name="source">The source of the damage</param>
    public void TakeDamage(float damage, Pawn source)
    {

        //reduce health by amount of damage
        currentHealth -=  damage;
        //clamp to zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //debug damage
        Debug.Log(source.name + " did " + damage + " damage to " + gameObject.name);
        //other way of doing same thing
        //Debug.LogFormat("{0} did {1} damage to {2}.", source.name, damage, gameObject.name);

        //if health is zero, then die
        if(currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Heal(float healing)
    {
        //increase health
        currentHealth += healing;
        //clamp to zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        //debug healing
        Debug.Log(gameObject.name + " healed for " + healing + " points.");
    }

    public void OverHeal(float healing)
    {
        //can heal without clamp
        currentHealth += healing;
        //debug healing
        Debug.Log(gameObject.name + " healed for " + healing + " points.");
    }

    public void AddMaxHealth(float addedHealth)
    {
        //add max health
        maxHealth += addedHealth;
        //heal for same amount to keep ratio
        Heal(addedHealth);
    }

    public void Die(Pawn source)
    {
        //if source is valid
        if (source != null)
        {
            //debug killed message
            Debug.Log(source.name + " killed " + gameObject.name);
            //mark self as dead
            gameObject.AddComponent<DeathComponent>();
        }
    }
}
