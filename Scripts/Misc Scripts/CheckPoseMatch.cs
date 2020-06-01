using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;

public class CheckPoseMatch : MonoBehaviour
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
            if(ComparePoseObject.GetComponent<ComparePoses>().identifiedPose == 1 && !isPoseMatched)
            {
                Countdown.SetActive(true);
                isPoseMatched = true;
            }
        }

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
        this.GetComponent<Animator>().SetBool("poseMatch", true);
        BreathingVisuals.SetActive(true);
        this.GetComponent<CountVisuals>().enabled = true;
        this.GetComponent<PoseFeedback>().enabled = true;
    }
}
