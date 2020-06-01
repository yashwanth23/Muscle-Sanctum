using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.components;

public class CountVisuals2 : MonoBehaviour
{
    [Tooltip("Gameobject to which Compare pose script is attached. This script gathers data from the AllSavedPoses and compares with the exercise that is being performed")]
    public GameObject ComparePoses;

    //public GameObject RestPeriodVisual;

    private int prevPose = 0;
    private int currPose = 0;
    
    public int repetition;

    [Tooltip("Parameters for end of exercise appreciation.")]
    public AudioClip appreciate;
    public ParticleSystem Confetti;
    public Transform ConfettiPoint;
    private ParticleSystem reward;

    [Tooltip("Particle Effect, Soundclips, and UI elements associated to every count.")]
    public ParticleSystem CountEffect;
    private ParticleSystem CountParticles;
    public AudioClip[] CountSounds;
    public GameObject[] leftCountVisuals;
    
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
        //Deactivating all the UI Count elements at the start of the experience
        if (repetition == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                leftCountVisuals[i].SetActive(false);
            }
        }

        //Identify if the pose made by the user is of initial pose or end pose of the corresponding exercise
        //When user changes from initial pose to end pose that and back to initial pose that is when one repetition is complete
        currPose = ComparePoses.GetComponent<ComparePoses>().identifiedPose;
        if (currPose == 2 && prevPose == 1)
        {
            repetition++;
            visualCount(repetition);

            //When repetitions reach 10, the exercise is complete and it moves to appreciation step
            if (repetition >= 10 && !restStarted)
            {
                StartCoroutine(setComplete());

            }
            //Debug.Log("THE OFFICIAL COUNT IS " + repetition);
        }
        prevPose = currPose;

        //Debug.Log("Exercise COUNT =     " + repetition);

        //Destroy any dead particle effects for optimization purposes
        if (isappreciated && !reward.isPlaying)
        {
            //RestPeriodVisual.SetActive(true);
            restStarted = true;
            isappreciated = false;
            Destroy(reward);
        }

        if(repetition > 1 && !CountParticles.isPlaying)
        {
            Destroy(CountParticles);
        }
    }

    private void visualCount(int n)
    {
        //Based on number of repetitions activate an deactivate the corresponding UI Count visual elements to show the repetition count
        this.GetComponent<AudioSource>().PlayOneShot(CountSounds[n-1]);
        CountParticles = Instantiate(CountEffect, this.transform.position, Quaternion.LookRotation(Vector3.up));
        for (int i = 0; i < n; i++)
        {
            leftCountVisuals[i].SetActive(true);
            //rightCountVisuals[i].SetActive(true);
        }
        for (int i = n; i < 10; i++)
        {
            leftCountVisuals[i].SetActive(false);
            //rightCountVisuals[i].SetActive(false);
        }
    }

    IEnumerator setComplete()
    {
        yield return new WaitForSeconds(2);
        completionItems();
    }

    //When the exercise is complete:
    // 1. End looking for errors in the body form
    // 2. Move the animation state of the trainer to appreciation state
    // 3. Instantiate an appreciation particle effect
    // 4. Play appreciation audio
    // 5. Deactivate all the UI Count elements
    private void completionItems()
    {
        this.GetComponent<PoseFeedback>().doneExercise = true;
        this.GetComponent<Animator>().SetBool("setComplete", true);
        isappreciated = true;
        reward = Instantiate(Confetti, ConfettiPoint.position, Quaternion.identity);
        this.GetComponent<AudioSource>().PlayOneShot(appreciate);
        for (int i = 0; i < 10; i++)
        {
            leftCountVisuals[i].SetActive(false);
            //rightCountVisuals[i].SetActive(false);
        }
    }
}
