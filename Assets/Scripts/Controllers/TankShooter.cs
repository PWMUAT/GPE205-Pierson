using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class TankShooter : Shooter
{
    //point to fire shells from
    public Transform firepointTransform;

    public AudioSource fireAudio;
    public AudioClip fireAudioClip;

    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float projectileDamage, float lifespan)
    {
        //instantiate projectile on fire point
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);

        fireAudio.PlayOneShot(fireAudioClip);

        #region Modify DamageOnHit component
        //get damage on hit component from shell
        DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

        //if has component
        if(doh != null )
        {
            //set damage to passed in value
            doh.damageDone = projectileDamage;

            //set owner to this pawn
            doh.owner = this.GetComponent<Pawn>();
        }
        #endregion

        #region Launch rigidbody forwards
        //get rigidbody
        Rigidbody rb = newShell.GetComponent<Rigidbody>();

        //if has rigidbody
        if (rb != null)
        {
            //add force forwards
            rb.AddForce(firepointTransform.forward * fireForce);
        }
        #endregion

        //destroy it after lifespan ends
        Destroy(newShell, lifespan);
    }
}
