using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellPickup : Pickup
{
    public ShellPowerup powerup;
    public override void OnTriggerEnter(Collider other)
    {
        //store other object's PowerupController if not null
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        //other object has PowerupController
        if (powerupManager != null)
        {
            //Add powerup
            powerupManager.Add(powerup);

            //Destroy self
            Destroy(gameObject);
        }
    }
}
