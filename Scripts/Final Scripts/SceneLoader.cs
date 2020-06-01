using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    //Load next scene when a corresponding button is pressed
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
}
