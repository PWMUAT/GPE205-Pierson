using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShellPowerup : Powerup
{
    public override void Apply(PowerupManager target)
    {
        //get pawn controller
        TankPawn tank = target.GetComponent<TankPawn>();
        //if exists
        if (tank != null)
        {
            //swap ammo
            tank.swapAmmoType();
        }
    }

    public override void Remove(PowerupManager target)
    {
        //get pawn controller
        TankPawn tank = target.GetComponent<TankPawn>();
        //if exists
        if (tank != null)
        {
            //swap ammo
            tank.swapAmmoType();
        }
    }
}
