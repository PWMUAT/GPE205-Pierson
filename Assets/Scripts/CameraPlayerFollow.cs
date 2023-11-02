using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3 (0, 15, 0);
    public int playerNumber;
    public Color lineColor = Color.yellow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null)
        {
            if(GameManager.Instance.players != null)
            {
                //move camera above player
                gameObject.transform.position = GameManager.Instance.players[playerNumber].pawn.transform.position + offset;

                //draw line to player
                Debug.DrawLine(gameObject.transform.position, GameManager.Instance.players[playerNumber].pawn.transform.position, lineColor);
            }
        }
    }
}
