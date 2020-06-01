using com.rfilkov.components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepetitionCount : MonoBehaviour
{
    public GameObject CompareExercise;
    private int prevPose = 0;
    private int currPose = 0;

    public int repetition;
    public Text scoreText;

    void Start()
    {
        repetition = 0;
    }
    // Update is called once per frame
    void Update()
    {
        currPose = CompareExercise.GetComponent<CompareExercise>().identifiedPose;
        if(currPose == 2 && prevPose == 1)
        {
            repetition++;
        }
        prevPose = currPose;
        scoreText.text = "COUNT: " + repetition.ToString();
        //Debug.Log("Exercise COUNT =     " + repetition);
    }
}
