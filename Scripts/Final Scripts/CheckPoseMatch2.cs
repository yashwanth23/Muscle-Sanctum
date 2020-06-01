using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;

public class CheckPoseMatch2 : MonoBehaviour
{
    public GameObject BreathingVisuals;
    public GameObject ComparePoseObject;
    public bool isPoseMatched, isCountdown;
    public AudioClip correct;
    public GameObject Countdown;

    // Start is called before the first frame update
    void Start()
    {
        isPoseMatched = false;
        isCountdown = false;
        BreathingVisuals.SetActive(false);
        //ComparePoses.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //We can also check from the script on the trainer like "this.GetComponent<SpawnEffect>().isSpawned"
        if (this.GetComponent<Animator>().GetBool("spawned"))
        {
            //ComparePoses.SetActive(true);

            //Check if the user pose matches the initial pose of an exercise
            if (ComparePoseObject.GetComponent<ComparePoses>().identifiedPose == 1 && !isPoseMatched)
            {
                //Activate Coundown gameobject and move to next step    
                Countdown.SetActive(true);
                isPoseMatched = true;
            }
        }

        //When pose is matched, start countdown animation
        if (isPoseMatched && !isCountdown)
        {
            StartCoroutine(BeginCountdown());
            isCountdown = true;
        }
    }

    IEnumerator BeginCountdown()
    {
        //4 Seconds for the countdown
        yield return new WaitForSeconds(7);
        Countdown.SetActive(false);

        //After countdown animation is done Change the animation on the trainer to start doing the corresponding exercise
        this.GetComponent<Animator>().SetBool("poseMatch", true);

        //Start Breathing visuals when the exercise is started
        BreathingVisuals.SetActive(true);

        //Enabling CountVisuals2 script on the trainer
        //Start counting repetitions and update the Count UI
        this.GetComponent<CountVisuals2>().enabled = true;

        //Start the PoseFeedback script to look for errors in the user's body form
        this.GetComponent<PoseFeedback>().enabled = true;
    }
}
