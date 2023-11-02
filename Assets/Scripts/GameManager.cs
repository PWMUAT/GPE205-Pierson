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
    public List<AIController> AI;
    public List<PlayerSpawn> playerSpawns;
    public List<AISpawn> AISpawns;
    public int numberOfAI;

    //prefabs
    public PlayerController playerControllerPrefab;
    public AIController[] AIControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject AITankPrefab;
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
        //define list of AI
        AI = new List<AIController>();
        //define list of player spawns
        playerSpawns = new List<PlayerSpawn>();
        //define list of AI spawns
        AISpawns = new List<AISpawn>();
    }

    private void Start()
    {

    }
    public void StartGame()
    {
        FillLists();
        SpawnAI();
        SpawnPlayer();
    }
    public void FillLists()
    {
        //define list of player spawns
        playerSpawns = new List<PlayerSpawn>(FindObjectsByType<PlayerSpawn>(FindObjectsSortMode.None));
        //define list of AI spawns
        AISpawns = new List<AISpawn>(FindObjectsByType<AISpawn>(FindObjectsSortMode.None));
    }
    public void SpawnPlayer()
    {
        //spawn controller at world origin
        GameObject newPlayerObject = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity).gameObject;

        //spawn the pawn and connect it to controller
        PlayerSpawn randomSpawnpoint = FindRandomPlayerSpawn();
        GameObject newPawnObject = Instantiate(playerTankPawnPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);

        //get PlayerController and Pawn components
        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        //hook components up
        newController.pawn = newPawn;
    }

    public void RespawnPlayer(GameObject target)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].pawn.GetComponent<DeathComponent>())
            {
                //remove dead player from list
                //players.Remove(target.GetComponent<PlayerController>());
                //destroy dead tank
                Destroy(target);

                //spawn the pawn and connect it to controller
                PlayerSpawn randomSpawnpoint = FindRandomPlayerSpawn();
                GameObject newPawnObject = Instantiate(playerTankPawnPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);
                //get pawn component
                Pawn newPawn = newPawnObject.GetComponent<Pawn>();
                //hook components up
                players[i].pawn = newPawn;

                //spawn a new player
                //SpawnPlayer();
            }
        }
    }

    public void SpawnAI()
    {
        for(int i = 0; i < numberOfAI; i++)
        {
            //spawn controller at world origin
            GameObject newAIObject = Instantiate(AIControllerPrefab[Random.Range(0, AIControllerPrefab.Length)], Vector3.zero, Quaternion.identity).gameObject;

            //spawn the pawn and connect it to controller
            AISpawn randomSpawnpoint = FindRandomAISpawn();
            GameObject newPawnObject = Instantiate(AITankPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);

            //get PlayerController and Pawn components
            Controller newController = newAIObject.GetComponent<Controller>();
            Pawn newPawn = newPawnObject.GetComponent<Pawn>();

            //hook components up
            newController.pawn = newPawn;

            //fill waypoints
            FillAIWaypoints(newAIObject.GetComponent<AIController>(), randomSpawnpoint);
        }
    }

    public PlayerSpawn FindRandomPlayerSpawn()
    {
        int randomIndex = Random.Range(0, playerSpawns.Count);
        return playerSpawns[randomIndex];
    }
    public AISpawn FindRandomAISpawn()
    {
        int randomIndex = Random.Range(0, AISpawns.Count);
        AISpawn returnValue = AISpawns[randomIndex];
        AISpawns.Remove(returnValue);
        return returnValue;
    }
    public void FillAIWaypoints(AIController AI, AISpawn spawnpoint)
    {
        Room spawnParent = spawnpoint.GetComponentInParent<Room>();
        AI.waypoints = spawnParent.waypoints;
    }
}
