using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;
    public KeyCode shootKey;

    public TMP_Text livesText;

    public int lives = 3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //check if we have a GameManager
        if(GameManager.Instance != null)
        {
            //check if player list exists
            if (GameManager.Instance.players != null)
            {
                //add self to player list
                GameManager.Instance.players.Add(this);
            }
        }

        //setting UI text
        livesText.text = "Lives: " + lives;
    }

    public void OnDestroy()
    {
        base.Start();

        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            //check if player list exists
            if (GameManager.Instance.players != null)
            {
                //remove self from player list
                GameManager.Instance.players.Remove(this);
            }
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (pawn.GetComponent<DeathComponent>() == null)
        {
            base.Update();

            ProcessInputs();
        }
    }

    public void ProcessInputs()
    {
        if (pawn != null)
        {
            if (Input.GetKey(moveForwardKey))
            {
                pawn.MoveForwards();
            }

            if (Input.GetKey(moveBackwardKey))
            {
                pawn.MoveBackwards();
            }

            if (Input.GetKey(rotateClockwiseKey))
            {
                pawn.RotateClockwise();
            }

            if (Input.GetKey(rotateCounterClockwiseKey))
            {
                pawn.RotateCounterClockwise();
            }

            if (Input.GetKeyDown(shootKey))
            {
                pawn.Shoot();
            }
        }
    }
    public override void AddScore(int modifyScore)
    {
        base.AddScore(modifyScore);
        score += modifyScore;
    }

    public void LoseLife()
    {
        //losing a life
        lives--;

        //setting UI text
        livesText.text = "Lives: " + lives;
    }
}
