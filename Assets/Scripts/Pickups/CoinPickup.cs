using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : Pickup
{
    public CoinPowerup powerup;
    public override void OnTriggerEnter(Collider other)
    {
        //store other object's PowerupController if not null
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        //add points
        powerupManager.AddPoints(powerup.pointIncrease);

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
