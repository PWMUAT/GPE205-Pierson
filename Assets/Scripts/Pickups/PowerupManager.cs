using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerupList;
    private List<Powerup> removeQueue;
    // Start is called before the first frame update
    void Start()
    {
        //initialize lists
        powerupList = new List<Powerup>();
        removeQueue = new List<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementTime();
    }

    private void LateUpdate()
    {
        ApplyRemoveQueue();
    }

    public void Add(Powerup powerup)
    {
        //apply powerup
        powerup.Apply(this);

        //save to list
        powerupList.Add(powerup);
    }
    
    public void Remove(Powerup powerup)
    {
        //remove powerup
        powerup.Remove(this);

        //remove from list
        removeQueue.Add(powerup);
    }

    public void DecrementTime()
    {
        //decrement powerup timers
        foreach (Powerup powerup in powerupList)
        {
            //only decrement if not permanent
            if(!powerup.isPermanent)
            {
                //subtract delta time
                powerup.duration -= Time.deltaTime;

                //remove if time runs out
                if (powerup.duration <= 0)
                {
                    Remove(powerup);
                }
            }
        }
    }

    private void ApplyRemoveQueue()
    {
        //remove powerups from queued list
        foreach(Powerup powerup in removeQueue)
        {
            powerupList.Remove(powerup);
        }

        //clear list
        removeQueue.Clear();
    }
    public void AddPoints(int points)
    {
        
    }
}
