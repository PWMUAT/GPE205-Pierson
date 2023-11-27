using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    private GameObject mapSpawnerObject;
    public bool multiplayer;
    public int highScore;
    private int totalLives;
    public Camera p1Camera;
    public Camera p2Camera;
    public bool MOTD;
    public TMP_Text p1LivesText;
    public TMP_Text p2LivesText;
    public AudioSource menuMusic;
    public AudioSource gameMusic;

    //prefabs
    public PlayerController playerControllerPrefab;
    public PlayerController player2ControllerPrefab;
    public AIController[] AIControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject AITankPrefab;
    public GameObject MapGeneratorPrefab;
    #endregion

    //game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;

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

        highScore = PlayerPrefs.GetInt("Highscore", 0);
    }

    private void Start()
    {
        DeactivateAllStates();
        ActivateTitleScreen();
    }
    public void StartGame()
    {
        FillLists();
        SpawnAI();
        SpawnPlayer();
        foreach (PlayerController plr in players)
        {
            plr.score = 0;
        }
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
        newPlayerObject.transform.parent = GameplayStateObject.transform;

        //spawn the pawn and connect it to controller
        PlayerSpawn randomSpawnpoint = FindRandomPlayerSpawn();
        GameObject newPawnObject = Instantiate(playerTankPawnPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);
        newPawnObject.transform.parent = mapSpawnerObject.transform;

        //get PlayerController and Pawn components
        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        //hook components up
        newController.pawn = newPawn;
        newPawn.controller = newController;

        //add lives to total count
        totalLives += newPlayerObject.GetComponent<PlayerController>().lives;
        newPlayerObject.GetComponent<PlayerController>().livesText = p1LivesText;

        if (multiplayer)
        {
            //spawn controller at world origin
            GameObject newPlayer2Object = Instantiate(player2ControllerPrefab, Vector3.zero, Quaternion.identity).gameObject;
            newPlayer2Object.transform.parent = GameplayStateObject.transform;

            //spawn the pawn and connect it to controller
            PlayerSpawn randomSpawnpoint2 = FindRandomPlayerSpawn();
            GameObject newPawnObject2 = Instantiate(playerTankPawnPrefab, randomSpawnpoint2.transform.position, randomSpawnpoint2.transform.rotation);
            newPawnObject2.transform.parent = mapSpawnerObject.transform;

            //get PlayerController and Pawn components
            Controller newController2 = newPlayer2Object.GetComponent<Controller>();
            Pawn newPawn2 = newPawnObject2.GetComponent<Pawn>();

            //hook components up
            newController2.pawn = newPawn2;
            newPawn2.controller = newController2;

            //set camera size
            p1Camera.rect = new Rect(0, 0, 0.5f, 1);
            //turn other camera on
            p2Camera.gameObject.SetActive(true);

            //add lives to total count
            totalLives += newPlayer2Object.GetComponent<PlayerController>().lives;
            newPlayer2Object.GetComponent<PlayerController>().livesText = p2LivesText;
        }
        else
        {
            //set camera size
            p1Camera.rect = new Rect(0, 0, 1, 1);
        }
    }

    public void RespawnPlayer(GameObject target)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].pawn.GetComponent<DeathComponent>())
            {
                //if lives are left
                if (players[i].lives > 0)
                {
                    //minus lives
                    players[i].LoseLife();

                    //remove dead player from list
                    //players.Remove(target.GetComponent<PlayerController>());
                    //destroy dead tank
                    Destroy(target);

                    //spawn the pawn and connect it to controller
                    PlayerSpawn randomSpawnpoint = FindRandomPlayerSpawn();
                    GameObject newPawnObject = Instantiate(playerTankPawnPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);
                    newPawnObject.transform.parent = mapSpawnerObject.transform;
                    //get pawn component
                    Pawn newPawn = newPawnObject.GetComponent<Pawn>();
                    //hook components up
                    players[i].pawn = newPawn;
                    newPawn.controller = players[i];
                }
                //otherwise they lose
                if(totalLives < 0)
                {
                    ActivateGameOverScreen();
                }
            }
        }
    }

    public void SpawnAI()
    {
        for (int i = 0; i < numberOfAI; i++)
        {
            //spawn controller at world origin
            GameObject newAIObject = Instantiate(AIControllerPrefab[Random.Range(0, AIControllerPrefab.Length)], Vector3.zero, Quaternion.identity).gameObject;
            newAIObject.transform.parent = mapSpawnerObject.transform;

            //spawn the pawn and connect it to controller
            AISpawn randomSpawnpoint = FindRandomAISpawn();
            GameObject newPawnObject = Instantiate(AITankPrefab, randomSpawnpoint.transform.position, randomSpawnpoint.transform.rotation);
            newPawnObject.transform.parent = mapSpawnerObject.transform;

            //get PlayerController and Pawn components
            Controller newController = newAIObject.GetComponent<Controller>();
            Pawn newPawn = newPawnObject.GetComponent<Pawn>();

            //hook components up
            newController.pawn = newPawn;
            newPawn.controller = newController;

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
    private void DeactivateAllStates()
    {
        //Deactivate all states
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
        DestroyMap();
    }
    public void ActivateTitleScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only title screen
        TitleScreenStateObject.SetActive(true);
        //play music
        menuMusic.Play();
    }
    public void ActivateMainMenuScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only main menu
        MainMenuStateObject.SetActive(true);
    }
    public void ActivateOptionsMenuScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only options screen
        OptionsScreenStateObject.SetActive(true);
    }
    public void ActivateCreditsScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only credits screen
        CreditsScreenStateObject.SetActive(true);
    }
    public void ActivateGameplayState()
    {
        //stop and play music
        menuMusic.Stop();
        gameMusic.Play();
        //Deactivate states
        DeactivateAllStates();
        //Activate only gameplay
        GameplayStateObject.SetActive(true);
        //start the gameplay
        mapSpawnerObject = Instantiate(MapGeneratorPrefab, Vector3.zero, Quaternion.identity).gameObject;
        mapSpawnerObject.transform.parent = GameplayStateObject.transform;
        mapSpawnerObject.GetComponent<MapGenerator>().mapOfTheDay = MOTD;

        //activate cameras
        p1Camera.GetComponent<CameraPlayerFollow>().enabled = true;
        p2Camera.GetComponent<CameraPlayerFollow>().enabled = true;
    }
    public void ActivateGameOverScreen()
    {
        //Deactivate states
        DeactivateAllStates();
        //Activate only game over screen
        GameOverScreenStateObject.SetActive(true);

        PlayerPrefs.SetInt("Highscore", highScore);
        PlayerPrefs.Save();
    }
    public void DestroyMap()
    {
        //destroy map spawner and all its children
        //...that sounds bad but its funny so im leavin it
        Destroy(mapSpawnerObject);
        CameraPlayerFollow[] activeCameras = FindObjectsByType<CameraPlayerFollow>(FindObjectsSortMode.None);
        foreach (CameraPlayerFollow cam in activeCameras)
        {
            cam.enabled = false;
        }
    }
}
