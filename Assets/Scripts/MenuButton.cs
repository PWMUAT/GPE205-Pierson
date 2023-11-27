using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public Toggle toggleState;
    public Toggle MOTDToggle;

    public void QuitGame()
    {

        #if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        #else 
        Application.Quit();
        #endif
    }

    public void CheckMultiplayer()
    {
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.multiplayer = toggleState.isOn;
        }
    }
    public void MapOfTheDay()
    {
        //check if we have a GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MOTD = MOTDToggle.isOn;
        }
    }
}
