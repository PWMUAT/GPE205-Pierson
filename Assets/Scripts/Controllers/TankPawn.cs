using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TankPawn : Pawn
{
    //reference to shooter script
    protected TankShooter shooter;

    //rigidbody for tank
    private Rigidbody tankRB;

    //sound component for making AI hear sound
    private NoiseMaker noiseComponent;
    public float noiseMultiplier;

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

        //getting noise component from self
        noiseComponent = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForwards()
    {
        //Create vector for movement
        Vector3 forwardsMovement = gameObject.transform.forward * moveSpeed * Time.deltaTime;
        //move rigidbody
        tankRB.MovePosition(gameObject.transform.position + forwardsMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(5 * noiseMultiplier, 0.5f);
    }

    public override void MoveBackwards()
    {
        //Create vector for movement
        Vector3 backwardsMovement = gameObject.transform.forward * -1 * moveSpeed * Time.deltaTime;
        //move rigidbody
        tankRB.MovePosition(gameObject.transform.position + backwardsMovement);
        //make noise when moving
        noiseComponent.SetNoiseLevel(5 * noiseMultiplier, 0.5f);
    }

    public override void RotateClockwise()
    {
        //Create vector for rotation
        Vector3 vectorRotation = new Vector3(0, turnSpeed, 0);
        //create rotation quaternion from rotator
        Quaternion clockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        //rotate rigidbody
        tankRB.MoveRotation(tankRB.rotation * clockwiseRotation);
        //make noise when rotating
        noiseComponent.SetNoiseLevel(2 * noiseMultiplier, 0.2f);
    }

    public override void RotateCounterClockwise()
    {
        //Create vector for rotation
        Vector3 vectorRotation = new Vector3(0, -turnSpeed, 0);
        //create rotation quaternion from rotator
        Quaternion counterClockwiseRotation = Quaternion.Euler(vectorRotation * Time.deltaTime);
        //rotate rigidbody
        tankRB.MoveRotation(tankRB.rotation * counterClockwiseRotation);
        //make noise when rotating
        noiseComponent.SetNoiseLevel(2 * noiseMultiplier, 0.2f);
    }

    public override void Shoot()
    {
        //shoot
        shooter.Shoot(shellPrefab, fireForce + tankRB.velocity.magnitude, shellDamage, shellLifespan);
        //make a lot of noise
        noiseComponent.SetNoiseLevel(10 * noiseMultiplier, 1f);
    }

    public override void RotateTowards(Vector3 targetPosition, float steerMultiplier)
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * steerMultiplier * Time.deltaTime);
    }
}
