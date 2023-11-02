using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HealthPowerup : Powerup
{
    public float healthToAdd;

    public override void Apply(PowerupManager target)
    {
        //get health component
        Health targetHealth = target.GetComponent<Health>();
        //if exists
        if (targetHealth != null)
        {
            targetHealth.Heal(healthToAdd);
        }
    }

    public override void Remove(PowerupManager target)
    {
        Debug.LogWarning("Health remove was called.");
    }
}
