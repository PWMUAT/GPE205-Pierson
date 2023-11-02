using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedPowerup : Powerup
{
    public float speedIncrease;

    public override void Apply(PowerupManager target)
    {
        //get pawn controller
        TankPawn tank = target.GetComponent<TankPawn>();
        //if exists
        if (tank != null )
        {
            //increase speed
            tank.AddMoveSpeed(speedIncrease);
        }
    }

    public override void Remove(PowerupManager target)
    {
        //get pawn controller
        TankPawn tank = target.GetComponent<TankPawn>();
        //if exists
        if (tank != null)
        {
            //decrease speed
            tank.AddMoveSpeed(-speedIncrease);
        }
    }
}
