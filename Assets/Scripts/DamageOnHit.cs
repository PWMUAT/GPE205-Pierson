using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    public float damageDone;
    public Pawn owner;

    public void OnTriggerEnter(Collider other)
    {
        //get health component from other object
        Health otherHealth = other.gameObject.GetComponent<Health>();

        //damage it if has health component
        if (otherHealth != null)
        {
            otherHealth.TakeDamage(damageDone, owner);
        }

        //destroy projectile if it hits anything
        Destroy(gameObject);
    }
}
