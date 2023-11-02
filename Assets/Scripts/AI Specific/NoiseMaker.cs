using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour {

    private float volumeDistance;
    private float noiseDuration;
    private bool taskCompleted = false;

    private void Update()
    {
        if(noiseDuration >= 0) 
        {
            IncrementTime();
        }
        else
        {
            if(!taskCompleted)
            {
                taskCompleted = true;
                volumeDistance = 0;
            }
        }
    }
    public float GetVolumeDistance()
    {
        return volumeDistance;
    }
    public void SetNoiseLevel(float level, float duration)
    {
        //set distance for noice to be heard
        volumeDistance = level;
        //set duration noise will last in seconds
        noiseDuration = duration;
        //allow update function to start
        taskCompleted = false;
    }
    private void IncrementTime()
    {
        noiseDuration -= Time.deltaTime;
    }
}
