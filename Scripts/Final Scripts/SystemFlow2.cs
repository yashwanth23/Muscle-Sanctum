using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemFlow2 : MonoBehaviour
{
    public GameObject Scene1;
    public GameObject Scene2;

    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        //Initiating Scene 1 and deactivating any other scenes
        Scene1.SetActive(true);
        Scene2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if Scene 1 (Exercise 1) is complete and entered the rest period
        if (Scene1.activeSelf && Scene1.GetComponent<RestPeriod2>().isResting)
        {
            timer += Time.deltaTime;
        }

        //After 30 seconds of rest period Activate the next scene (Exercise 2) and deactivate the previous scene (Exercise 1) 
        if (timer > 30f)
        {
            timer = 0;
            Scene1.SetActive(false);
            Scene2.SetActive(true);
        }

        //Incase there are many exercises a similar workflow is followed
    }
}
