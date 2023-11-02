using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] gridPrefabs;
    public int rows;
    public int columns;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    private Room[,] grid;
    public int randomSeed;
    public bool mapOfTheDay;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        if(mapOfTheDay)
        {
            randomSeed += Convert.ToInt32(System.DateTime.Today.Ticks);
        }

        UnityEngine.Random.InitState(randomSeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetRandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public void GenerateMap()
    {
        //clear grid
        grid = new Room[columns, rows];

        //for each row
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            for (int currentCol = 0;currentCol < columns; currentCol++)
            {
                #region spawn rooms
                //determine location
                float xPos = roomWidth * currentCol;
                float zPos = roomHeight * currentRow;
                Vector3 newPosition = new Vector3 (xPos, 0, zPos);

                //create new room at location
                GameObject roomObj = Instantiate(GetRandomRoomPrefab(), newPosition, Quaternion.identity);

                //set parent
                roomObj.transform.parent = this.transform;

                //give useful name
                roomObj.name = "Room_" + currentCol + " , " + currentRow;

                //get room object
                Room tempRoom = roomObj.GetComponent<Room>();

                //save to grid array
                grid[currentCol, currentRow] = tempRoom;
                #endregion

                #region open doors
                //row doors
                if (currentRow == 0)
                {
                    //hide north door if in first row
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows-1)
                {
                    //hide south door if in top row
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    //hide both doors if in between
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                //column doors
                if(currentCol == 0)
                {
                    //hide east door if in first column
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == columns-1)
                {
                    //hide west door if in last column
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    //hide both doors if in between
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                #endregion
            }
        }

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
}
