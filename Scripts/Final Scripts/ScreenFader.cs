using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {

    //Black screen over the canvas
    public GameObject screenObject;
    private Image blackScreen;

    //Determines the rate at which the screen moves
    public float fadeSpeed;
    public float startAlpha;     //Determines if fading in or out
    public float curAlpha;
    public float tarAlpha;
    public int tarScene;

    //When ready...
    public bool started;

	// Use this for initialization
	void Start () {
        blackScreen = screenObject.GetComponent<Image>();
        setColor();

        if(startAlpha == 0)
        {
            screenObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.R))
        {
            GameObject temp = GameObject.Find("Main Camera");
            Destroy(temp);
            //Application.LoadLevel(0);
            SceneManager.LoadScene(0);
        }

        //While started...
        if (started)
        {
            if (!screenObject.activeSelf)
            {
                screenObject.SetActive(true);
            }

            //While the screen exists....
            if (curAlpha >= 0f && curAlpha <= 1f)
            {
                //If the screen started off...
                if(startAlpha == 1)
                {
                    curAlpha -= fadeSpeed * Time.deltaTime;
                }
                //If the screen started on...
                else
                {
                    curAlpha += fadeSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (startAlpha == 0)
                {
                    SceneManager.LoadScene(tarScene);
                    
                }
                else
                {
                    screenObject.SetActive(false);
                    started = false;
                }


            }



            setColor();
        }
	}

    //Sets image to current alpha
    public void setColor()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, curAlpha);
    }

    //Starts the fade function
    public void start(int x, float y)
    {
        tarScene = x;
        startAlpha = y;
        started = true;
    }
   
    public void start(int x) { start(x, startAlpha); }
    public void start() { start(tarScene); }
}


