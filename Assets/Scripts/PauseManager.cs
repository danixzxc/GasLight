using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static void PauseGame()
    {
        foreach (var lamp in FindObjectsOfType<ButtonController>())
        {
            lamp.Pause();
        }
                Debug.Log("Pausing");
    }
    public static void UnpauseGame()
    {
        foreach (var lamp in FindObjectsOfType<ButtonController>())
        {
            lamp.Unpause();
        }
                Debug.Log("Unpausing");
    }
}
