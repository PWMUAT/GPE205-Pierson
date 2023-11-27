using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        int tmpScore = 0;
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            tmpScore = GameManager.Instance.highScore;
        }

        scoreText = GetComponent<TextMeshPro>();
        scoreText.text = "High Score: " + tmpScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
