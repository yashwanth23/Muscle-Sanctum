using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseFeedback : MonoBehaviour
{
    public GameObject User;
    private bool feedbackStart;
    public bool doneExercise;

    // Start is called before the first frame update
    void Start()
    {
        feedbackStart = false;
        doneExercise = false;
    }

    void Update()
    {   
        //Enable FormCheckFeedback script on the user to look for errors in the body form of the user
        if (!feedbackStart)
        {
            StartCoroutine(startFeedback());
            feedbackStart = true;
        }

        //Disable the FormCheckFeedback script when exercise is done to avoid unnecessary feedback during transition 
        if (doneExercise)
        {
            User.GetComponent<FormCheckFeedback>().enabled = false;
        }
    }

    IEnumerator startFeedback()
    {
        yield return new WaitForSeconds(2);
        User.GetComponent<FormCheckFeedback>().enabled = true;
    }
}
