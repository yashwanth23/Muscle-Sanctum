using com.rfilkov.components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountVisuals : MonoBehaviour
{
    public GameObject ComparePoses;
    //public GameObject RestPeriodVisual;
    private int prevPose = 0;
    private int currPose = 0;

    public int repetition;
    public AudioClip appreciate;

    public GameObject[] leftCountVisuals;
    public GameObject[] rightCountVisuals;

    private bool isappreciated;
    public bool restStarted;
    void Start()
    {
        repetition = 0;
        isappreciated = false;
        //RestPeriodVisual.SetActive(false);
        restStarted = false;
    }


    void Update()
    {
        if(repetition == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                leftCountVisuals[i].SetActive(false);
                rightCountVisuals[i].SetActive(false);
            }
        }
        currPose = ComparePoses.GetComponent<ComparePoses>().identifiedPose;
        if (currPose == 2 && prevPose == 1)
        {
            repetition++;
            visualCount(repetition);
            if(repetition >= 5)
            {
                StartCoroutine(setComplete());
                
            }
            //Debug.Log("THE OFFICIAL COUNT ISSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS      " + repetition);
        }
        prevPose = currPose;
        
        //Debug.Log("Exercise COUNT =     " + repetition);

        if(isappreciated && !this.GetComponent<AudioSource>().isPlaying)
        {
            //RestPeriodVisual.SetActive(true);
            restStarted = true;
            isappreciated = false;
        }
    }

    private void visualCount(int n)
    {
        for(int i = 0; i < n; i++)
        {
            leftCountVisuals[i].SetActive(true);
            rightCountVisuals[i].SetActive(true);
        }
        for(int i = n ; i < 10; i++)
        {
            leftCountVisuals[i].SetActive(false);
            rightCountVisuals[i].SetActive(false);
        }
    }

    IEnumerator setComplete()
    {
        yield return new WaitForSeconds(2);
        completionItems();

    }

    
    private void completionItems()
    {
        this.GetComponent<PoseFeedback>().doneExercise = true;
        this.GetComponent<Animator>().SetBool("setComplete", true);
        isappreciated = true;
        this.GetComponent<AudioSource>().PlayOneShot(appreciate);
        for (int i = 0; i < 10; i++)
        {
            leftCountVisuals[i].SetActive(false);
            rightCountVisuals[i].SetActive(false);
        }
    }
}
