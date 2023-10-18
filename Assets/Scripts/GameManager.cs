using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region variables
    /// <summary>
    /// The static instance of this class which can only exist once
    /// </summary>
    public static GameManager Instance;

    public List<PlayerController> players;
    public List<AIController> AIs;

    //prefabs
    public PlayerController playerControllerPrefab;
    public GameObject tankPawnPrefab;
    public Transform playerSpawnTransform;
    #endregion

    private void Awake()
    {
        //if we dont have an instance, make one
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //if another instance exists, delete this one
        else
        {
            Destroy(gameObject);
        }

        //define the list of players
        players = new List<PlayerController>();

        AIs = new List<AIController>();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        //spawn player at world origin
        GameObject newPlayerObject = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity).gameObject;

        //spawn the pawn and connect it to controller
        GameObject newPawnObject = Instantiate(tankPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);

        //get PlayerController and Pawn components
        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        //hook components up
        newController.pawn = newPawn;
    }

}
