using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //tell manager to respawn player
            GameManager.Instance.RespawnPlayer(gameObject);
        }
    }
}
