using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using com.rfilkov.kinect;

public class FormCheckFeedback : MonoBehaviour
{
    public int playerIndex = 0;
    public int ExerciseNumber;

    public GameObject VirtualTrainer;
    public GameObject BreathingVisuals;
    
    [Tooltip("Using necessary joints depending on the exercise to evaluate error and give feedback.")]
    public KinectInterop.JointType LeftElbowJoint = KinectInterop.JointType.ElbowLeft;
    public KinectInterop.JointType RightElbowJoint = KinectInterop.JointType.ElbowRight;

    public KinectInterop.JointType LeftShoulderJoint = KinectInterop.JointType.ShoulderLeft;
    public KinectInterop.JointType RightShoulderJoint = KinectInterop.JointType.ShoulderRight;
    
    // reference to KM
    private KinectManager kinectManager = null;

    private float leAngle, reAngle, lsAngle, rsAngle;

    public GameObject AudioInterface;
    public AudioClip error, correct;

    private int ErrorCounter, CorrectCounter;
    private bool errorPlayed, correctPlayed;

    //public RawImage rawImage_left, rawImage_right;
    public VideoPlayer videoPlayer;

    public VideoClip l_outer, r_outer, l_inner, r_inner;

    public GameObject leftCube, rightCube;

    public ParticleSystem Aura;
    private ParticleSystem CorrectPoseAura;

    //This bool is to make sure videoplayer plays only once when the error is detected
    private bool errorDetected;
    
    // Start is called before the first frame update
    void Start()
    {
        // get reference to KM
        kinectManager = KinectManager.Instance;
        ErrorCounter = 0;

        errorDetected = false;
        leftCube.SetActive(false);
        rightCube.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ulong userId = kinectManager.GetUserIdByIndex(playerIndex);

        int lElbowIndex = (int)LeftElbowJoint;
        int rElbowIndex = (int)RightElbowJoint;

        int lShoulderIndex = (int)LeftShoulderJoint;
        int rShoulderIndex = (int)RightShoulderJoint;

        //Get angles made by the joint with the next joint 
        if (kinectManager.IsJointTracked(userId, lElbowIndex))
        {
            leAngle = kinectManager.GetAngleAtJoint(userId, lElbowIndex);
        }

        if (kinectManager.IsJointTracked(userId, rElbowIndex))
        {
            reAngle = kinectManager.GetAngleAtJoint(userId, rElbowIndex);
        }

        if (kinectManager.IsJointTracked(userId, lShoulderIndex))
        {
            lsAngle = kinectManager.GetAngleAtJoint(userId, lShoulderIndex);
        }

        if (kinectManager.IsJointTracked(userId, rShoulderIndex))
        {
            rsAngle = kinectManager.GetAngleAtJoint(userId, rShoulderIndex);
        }


        //If the exercise is Bicep Curl
        if (ExerciseNumber == 1)
        {
            BicepCurlFeedback(lsAngle, rsAngle);
        }
        //If the exercise is Arnold Press
        else if (ExerciseNumber == 2)
        {
            ArnoldPressFeedback(lsAngle, rsAngle, leAngle, reAngle);
        }
        
        //LeftText.text = "SHOULDER:  " + lsAngle.ToString() + "\n" + "ELBOW:  " + leAngle.ToString();
        //RightText.text = "SHOULDER:  " + rsAngle.ToString() + "\n" + "ELBOW:  " + reAngle.ToString();
        //Debug.Log(new Vector2(lAngle, rAngle));
    }

    //To check for feedback in Bicep curl we only need the angles made by Left shoulder and Right shoulder as their form is important during this exercise
    private void BicepCurlFeedback(float ls, float rs)
    {
        //If the angles cross a certain thershold then it is classified as improper form 
        ErrorCounter++;
        if (ls > 135 || ls < 115 || rs > 135 || rs < 115)
        {
            if (!errorPlayed && ErrorCounter > 50)
            {
                //Play error sound
                //PlayErrorSound();
                //When there is error:
                // 1. Deactivate breathing visuals
                // 2. Change the animation state of the trainer to default exercise pose 
                // 3. Play error sound and error feedback video 
                errorPlayed = true;
                ErrorCounter = 0;
                VirtualTrainer.GetComponent<Animator>().SetBool("isError", true);
                BreathingVisuals.SetActive(false);
            }
            correctPlayed = false;
            CorrectCounter = 0;
        }
        //If the angles are inside the safe bounds, it is classified as proper form
        else if (ls <= 135 || ls >= 115 || rs <= 135 || rs >= 115)
        {
            CorrectCounter++;
            
            if (errorPlayed && !correctPlayed && CorrectCounter > 25)
            {
                errorPlayed = false;
                //Play Correct sound
                PlayCorrectSound();
                correctPlayed = true;
                CorrectCounter = 0;
                VirtualTrainer.GetComponent<Animator>().SetBool("isError", false);
                BreathingVisuals.SetActive(true);

                //Play animation
                CorrectPoseAura = Instantiate(Aura, this.transform.position + new Vector3(0, 2.5f, 0), Quaternion.LookRotation(Vector3.up));

                leftCube.SetActive(false);
                rightCube.SetActive(false);
            }
            //prevCircleSize = 0;
            //errorModule.startSize = 0f;
            StopErrorVideo();
            ErrorCounter = 0;

            errorDetected = false;
        }

        if (correctPlayed && !CorrectPoseAura.isPlaying)
        {
            Destroy(CorrectPoseAura);
        }

        if(errorPlayed && !errorDetected)
        {
            if (ls > 135)
            {
                //circleSize = (ls - 135) / 15;
                //errorModule.startSize = Mathf.Lerp(circleSize, prevCircleSize, Time.deltaTime);
                //Play the video on Canvas UI
                StartCoroutine(PlayErrorVideo(l_outer, 0));
                //prevCircleSize = errorModule.startSize.constant;
            }
            else if (ls < 115)
            {
                //Play the video related to correction on Canvas UI
                StartCoroutine(PlayErrorVideo(l_inner, 0));
            }
            else if (rs > 135)
            {
                //circleSize = (ls - 135) / 15;
                //errorModule.startSize = Mathf.Lerp(circleSize, prevCircleSize, Time.deltaTime);
                //Play the video on Canvas UI
                StartCoroutine(PlayErrorVideo(r_outer, 1));
                //prevCircleSize = errorModule.startSize.constant;
            }
            else if (rs < 115)
            {
                //Play the video related to correction on Canvas UI
                StartCoroutine(PlayErrorVideo(r_inner, 1));
            }
            errorDetected = true;
        }
        
    }

    private void ArnoldPressFeedback(float ls, float rs, float le, float re)
    {
        Debug.Log(new Vector2(ls, rs));
    }

    private void PlayErrorSound()
    {
        AudioInterface.GetComponent<AudioSource>().clip = error;
        AudioInterface.GetComponent<AudioSource>().loop = true;
        AudioInterface.GetComponent<AudioSource>().Play();
    }
    
    private void PlayCorrectSound()
    {
        AudioInterface.GetComponent<AudioSource>().clip = correct;
        AudioInterface.GetComponent<AudioSource>().loop = false;
        AudioInterface.GetComponent<AudioSource>().Play();
    }

    IEnumerator PlayErrorVideo(VideoClip clip, int side)
    {
        PlayErrorSound();
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = clip;
        

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5F);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        if (side == 0)
        {
            //rawImage_left.enabled = true;
            //rawImage_left.texture = videoPlayer.texture;
            leftCube.SetActive(true);
            videoPlayer.targetMaterialRenderer = leftCube.GetComponent<MeshRenderer>();
        }
        else if (side == 1)
        {
            //rawImage_right.enabled = true;
            //rawImage_right.texture = videoPlayer.texture;
            rightCube.SetActive(true);
            videoPlayer.targetMaterialRenderer = rightCube.GetComponent<MeshRenderer>();
        }
        videoPlayer.Prepare();
        videoPlayer.Play();
        
    }

    void StopErrorVideo()
    {
        videoPlayer.Stop();
        //rawImage_left.enabled = false;
        //rawImage_right.enabled = false;
    }

}
