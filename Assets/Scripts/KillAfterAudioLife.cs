using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class KillAfterAudioLife : MonoBehaviour
{
    public AudioClip audioClip;
    private float clipLength;

    // Start is called before the first frame update
    void Start()
    {
        clipLength = audioClip.length;
    }

    // Update is called once per frame
    void Update()
    {
        clipLength -= Time.deltaTime;
        if(clipLength < 0 )
        {
            Destroy(this.gameObject);
        }
    }
}
