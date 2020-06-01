using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyObject : MonoBehaviour
{
    
    private void Awake()
    {
        
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Background_music");
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    
    }

    /*
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        int buildIndex = currentScene.buildIndex;

        if(buildIndex == 1)
        {
            this.GetComponent<AudioSource>().volume = 0.2F;
        }
        else if(buildIndex == 0)
        {
            this.GetComponent<AudioSource>().volume = 1.0F;
        }
    }*/
}
