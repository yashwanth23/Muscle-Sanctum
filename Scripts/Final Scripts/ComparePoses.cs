using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using com.rfilkov.kinect;
using UnityEngine.UI;

namespace com.rfilkov.components
{
    public class ComparePoses : MonoBehaviour
    {
        public int Exercise_number;

        [Tooltip("User avatar model, who needs to reach the target pose.")]
        public PoseModelHelper User;

        [Tooltip("Model in pose that need to be reached by the user.")]
        public PoseModelHelper poseModel;

        [Tooltip("List of joints to compare.")]
        public List<KinectInterop.JointType> poseJoints = new List<KinectInterop.JointType>();

        [Tooltip("Allowed delay in pose match, in seconds. 0 means no delay allowed.")]
        [Range(0f, 10f)]
        public float delayAllowed = 1f;

        [Tooltip("Time between pose-match checks, in seconds. 0 means check each frame.")]
        [Range(0f, 1f)]
        public float timeBetweenChecks = 0.1f;

        [Tooltip("Threshold, above which we consider the pose is matched.")]
        [Range(0.5f, 1f)]
        public float matchThreshold;

        [Tooltip("GUI-Text to display information messages.")]
        public UnityEngine.UI.Text infoText;


        // whether the pose is matched or not
        private bool bPoseMatched = false;
        // match percent (between 0 and 1)
        private float fMatchPercent = 0f;
        // pose-time with best matching
        private float fMatchPoseTime = 0f;

        // initial rotation
        private Quaternion initialUserRotation = Quaternion.identity;
        private Quaternion initialPoseRotation = Quaternion.identity;

        // reference to the avatar controller
        private AvatarController avatarController = null;

        // uncomment to get debug info
        private StringBuilder sbDebug = null; // new StringBuilder();


        // data for each saved pose
        public class PoseModelData
        {
            public float fTime;
            public Vector3[] avBoneDirs;
        }

        // list of saved pose data
        //private List<PoseModelDataClass> alSavedPoses = new List<PoseModelDataClass>();

        // current User pose
        private PoseModelData poseUser = new PoseModelData();

        //Accessing saved pose data 
        //Check out PoseModelDataClass.cs for the attributes
        private PoseModelDataClass poseSaved = new PoseModelDataClass();


        // last time the model pose was saved 
        private float lastPoseSavedTime = 0f;

        //Get the Exercise number so as to check for the poses related to this exercise
        
        private int exerNum = 0;
        public int identifiedPose;

        public GameObject ExerciseMan;

        //public Text PosePercentage; 

        private float[] matchPercentIndex;

        /// <summary>
        /// Determines whether the target pose is matched or not.
        /// </summary>
        /// <returns><c>true</c> if the target pose is matched; otherwise, <c>false</c>.</returns>
        public bool IsPoseMatched()
        {
            return bPoseMatched;
        }


        /// <summary>
        /// Gets the pose match percent.
        /// </summary>
        /// <returns>The match percent (value between 0 and 1).</returns>
        public float GetMatchPercent()
        {
            return fMatchPercent;
        }

        /// <summary>
        /// Gets the last check time.
        /// </summary>
        /// <returns>The last check time.</returns>
        public float GetPoseCheckTime()
        {
            return lastPoseSavedTime;
        }


        private void Awake()
        {
            if (User)
            {
                initialUserRotation = User.transform.rotation;
                avatarController = User.gameObject.GetComponent<AvatarController>();
            }

            if (poseModel)
            {
                initialPoseRotation = poseModel.transform.rotation;
            }

            matchPercentIndex = new float[2];

            if (Exercise_number == 1)
                exerNum = 0;
            if (Exercise_number == 2)
                exerNum = 2;
            if (Exercise_number == 3)
                exerNum = 4;
        }


        // Update is called once per frame
        void Update()
        {
            KinectManager kinectManager = KinectManager.Instance;

            // get mirrored state
            bool isMirrored = avatarController ? avatarController.mirroredMovement : true;  // true by default

            // current time
            float fCurrentTime = Time.realtimeSinceStartup;

            //Wait for all the pose values to get stored in the PoseArchive 
            if (kinectManager != null && kinectManager.IsInitialized() && PoseArchive.saveCounter == 6)
            {
                ExerciseMan.SetActive(false);
                if (User != null && avatarController && kinectManager.IsUserTracked(avatarController.playerId))
                {
                    // get current avatar pose
                    GetUserPose(fCurrentTime, isMirrored);

                    // get the difference
                    GetPoseDifference(isMirrored);

                    if (infoText != null)
                    {
                        //string sPoseMessage = string.Format("Pose match: {0:F0}% {1:F1}s ago {2}", fMatchPercent * 100f, Time.realtimeSinceStartup - fMatchPoseTime,
                        //                                    (bPoseMatched ? "- Matched" : ""));
                        string sPoseMessage = string.Format("Pose match: {0:F0}% {1}", fMatchPercent * 100f, (bPoseMatched ? "- Matched" : ""));
                        if (sbDebug != null)
                            sPoseMessage += sbDebug.ToString();
                        infoText.text = sPoseMessage;

                        //Debug.Log("IDENTIFIED POSE =  " + identifiedPose);

                    }
                }
                else
                {
                    // no user found
                    fMatchPercent = 0f;
                    fMatchPoseTime = 0f;
                    bPoseMatched = false;

                    if (infoText != null)
                    {
                        infoText.text = "Try to follow the model pose.";
                    }
                }
            }
            else
            {
                Debug.Log("STILL WAITING FOR SOME PROCESS TO BE COMPLETED");
            }
        }

        // gets the current User pose
        private void GetUserPose(float fCurrentTime, bool isMirrored)
        {
            KinectManager kinectManager = KinectManager.Instance;
            if (kinectManager == null || User == null || poseJoints == null)
                return;

            //poseUser.fTime = fCurrentTime;
            if (poseUser.avBoneDirs == null)
            {
                poseUser.avBoneDirs = new Vector3[poseJoints.Count];
            }
            
            // Gets real-time joint pose data of the user with respect to the next joints
            for (int i = 0; i < poseJoints.Count; i++)
            {
                KinectInterop.JointType joint = poseJoints[i];
                KinectInterop.JointType nextJoint = kinectManager.GetNextJoint(joint);

                int jointCount = kinectManager.GetJointCount();
                if (nextJoint != joint && (int)nextJoint >= 0 && (int)nextJoint < jointCount)
                {
                    Transform avatarTransform1 = User.GetBoneTransform(User.GetBoneIndexByJoint(joint, isMirrored));
                    Transform avatarTransform2 = User.GetBoneTransform(User.GetBoneIndexByJoint(nextJoint, isMirrored));

                    if (avatarTransform1 != null && avatarTransform2 != null)
                    {
                        poseUser.avBoneDirs[i] = (avatarTransform2.position - avatarTransform1.position).normalized;
                        //Debug.Log(poseUser.avBoneDirs[i]);
                    }
                }
            }
            
        }

        // Gets the difference between the User pose and the list of saved poses
        private void GetPoseDifference(bool isMirrored)
        {
            // by-default values
            bPoseMatched = false;
            fMatchPercent = 0f;
            fMatchPoseTime = 0f;

            KinectManager kinectManager = KinectManager.Instance;
            if (poseJoints == null || poseUser.avBoneDirs == null)
                return;

            if (sbDebug != null)
            {
                sbDebug.Clear();
                sbDebug.AppendLine();
            }

            // check the difference with saved poses, starting from the last one
            for (int p = 0; p < 2; p++)
            {
                float fAngleDiff = 0f;
                float fMaxDiff = 0f;

                PoseModelDataClass modelPose = PoseArchive.AllSavedPoses[p + exerNum];
                for (int i = 0; i < poseJoints.Count; i++)
                {
                    Vector3 vPoseBone = modelPose.BonePos[i];
                    Vector3 vUserBone = poseUser.avBoneDirs[i];

                    if (vPoseBone == Vector3.zero || vUserBone == Vector3.zero)
                        continue;

                    float fDiff = Vector3.Angle(vPoseBone, vUserBone);
                    if (fDiff > 90f)
                        fDiff = 90f;

                    fAngleDiff += fDiff;
                    fMaxDiff += 90f;  // we assume the max diff could be 90 degrees

                    if (sbDebug != null)
                    {
                        sbDebug.AppendFormat("SP: {0}, {1} - angle: {2:F0}, match: {3:F0}%", p, poseJoints[i], fDiff, (1f - fDiff / 90f) * 100f);
                        sbDebug.AppendLine();
                    }

                }

                float fPoseMatch = fMaxDiff > 0f ? (1f - fAngleDiff / fMaxDiff) : 0f;
                if (fPoseMatch > fMatchPercent)
                {
                    fMatchPercent = fPoseMatch;
                    //fMatchPoseTime = poseModel.fTime;
                    bPoseMatched = (fMatchPercent >= matchThreshold);
                    if (bPoseMatched)
                        matchPercentIndex[p] = fMatchPercent * 100f;
                    else
                        matchPercentIndex[p] = 0;
                }
            }

            Debug.Log("THE MATCH PERCENTAGES ARE HERE: " + new Vector2(matchPercentIndex[0], matchPercentIndex[1]));
            //PosePercentage.text = matchPercentIndex[0].ToString() + "   ,   " + matchPercentIndex[1].ToString();

            //This is to find which pose the user is close to
            /*
            if (matchPercentIndex[0] != 0 && matchPercentIndex[1] != 0)
            {
                Debug.Log(new Vector2(matchPercentIndex[0], matchPercentIndex[1]));
                if (matchPercentIndex[0] / matchPercentIndex[1] > 1)
                    identifiedPose = 1;
                else
                    identifiedPose = 2;
            }
            else
            {
                identifiedPose = 0;
            }*/

            //This is to check if user's start and end poses are close to a threshold percentage value
            if (matchPercentIndex[0] != 0 && matchPercentIndex[1] == 0)
            {
                identifiedPose = 1;
            }
            else if (matchPercentIndex[0] == 0 && matchPercentIndex[1] != 0)
            {
                identifiedPose = 2;
            }
        }
    }
}
