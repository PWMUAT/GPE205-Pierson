using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    //reference to shooter script
    protected TankShooter shooter;

    //rigidbody for tank
    private Rigidbody tankRB;

    //variables used in firing
    public GameObject shellPrefab;
    public float fireForce;
    public float shellDamage;
    public float shellLifespan;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //getting shooter script from object
        shooter = GetComponent<TankShooter>();

        //getting rigidbody from self
        tankRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForwards()
    {
        Debug.Log("Forwards");
        Vector3 forwardsMovement = gameObject.transform.forward * moveSpeed * Time.deltaTime;
        tankRB.MovePosition(gameObject.transform.position + forwardsMovement);
    }

    public override void MoveBackwards()
    {
        Debug.Log("Backwards");
        Vector3 backwardsMovement = gameObject.transform.forward * -1 * moveSpeed * Time.deltaTime;
        tankRB.MovePosition(gameObject.transform.position + backwardsMovement);
    }

    public override void RotateClockwise()
    {
        Debug.Log("Clockwise");
        Vector3 vectorRotation = new Vector3(0, turnSpeed, 0);
        Quaternion clockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        tankRB.MoveRotation(tankRB.rotation * clockwiseRotation);
    }

    public override void RotateCounterClockwise()
    {
        Debug.Log("C-Clockwise");
        Vector3 vectorRotation = new Vector3(0, -turnSpeed, 0);
        Quaternion counterClockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        tankRB.MoveRotation(tankRB.rotation * counterClockwiseRotation);
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce + tankRB.velocity.magnitude, shellDamage, shellLifespan);
    }
}
