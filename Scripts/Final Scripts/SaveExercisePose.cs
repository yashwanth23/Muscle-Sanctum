using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    public class SaveExercisePose : MonoBehaviour
    {
        [Tooltip("Model in pose that need to be reached by the user.")]
        public PoseModelHelper poseModel;

        [Tooltip("List of joints to compare.")]
        public List<KinectInterop.JointType> poseJoints = new List<KinectInterop.JointType>();

        [Tooltip("Number of the exercise to be saved (Exercise pose data derived from Motion capture data)")]
        public int Exercise_index;

        private Animator char_animator;

        //Saving all the poses in a Vector3 array
        private Vector3[] savePose;

        private bool flag;

        //public KinectInterop.JointType Head = KinectInterop.JointType.Head;

        // Start is called before the first frame update
        void Start()
        {
            char_animator = this.GetComponent<Animator>();
            flag = false;
            savePose = new Vector3[poseJoints.Count];
        }

        // Update is called once per frame
        void Update()
        {
            KinectManager kinectManager = KinectManager.Instance;
            if (!flag && char_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                /**************************This is one way of storing pose data******************************/
                /*
                for (int i = 0; i < poseJoints.Count; i++)
                {
                    savePose[i] = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(poseJoints[i], false)).position;
                }

                PoseArchive.ArraySavedPoses[Exercise_index] = savePose;
                PoseArchive.saveCounter++;
                flag = true;
                //Debug.Log("Exercise number   " + Exercise_index + "   has one of its bone transform value as " + savePose[4]);
                */

                
                ///Using Head as reference point and calculating joint positions with respect to head 
                ///Save them in the array to be referenced later for comparision with user motion data
                /*
                Transform headTransform = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(Head, false));

                for (int i = 0; i < poseJoints.Count; i++)
                {
                    KinectInterop.JointType joint = poseJoints[i];

                    Transform jointTransform = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(joint, false));
                    
                    if(jointTransform != null && headTransform != null)
                    {
                        savePose[i] = (headTransform.position - jointTransform.position).normalized;
                    }
                }*/


                /****************************This is another way of storing joint data of a pose************************/
                
                ///Get joint data wrt the next joints
                
                for (int i = 0; i < poseJoints.Count; i++)
                {
                    KinectInterop.JointType joint = poseJoints[i];
                    KinectInterop.JointType nextJoint = kinectManager.GetNextJoint(joint);

                    int jointCount = kinectManager.GetJointCount();
                    if (nextJoint != joint && (int)nextJoint >= 0 && (int)nextJoint < jointCount)
                    {
                        Transform avatarTransform1 = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(joint, false));
                        Transform avatarTransform2 = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(nextJoint, false));
                        
                        if (avatarTransform1 != null && avatarTransform2 != null)
                        {
                            savePose[i] = (avatarTransform2.position - avatarTransform1.position).normalized;
                            //Debug.Log(savePose[i]);
                        }
                    }
                }
                
                //Store the poses captured in an array to be referenced later
                PoseArchive.ArraySavedPoses[Exercise_index] = savePose;
                PoseArchive.saveCounter++;
                flag = true;
                //Debug.Log("Exercise number   " + Exercise_index + "   has one of its bone transform value as " + savePose[4]);
                
            }
            
        }
    }
}
