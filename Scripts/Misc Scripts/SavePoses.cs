using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.rfilkov.kinect;

namespace com.rfilkov.components
{
    public class SavePoses : MonoBehaviour
    {
        public int Exercises = 3;
        [Tooltip("Model in pose that need to be reached by the user.")]
        public PoseModelHelper poseModel;

        [Tooltip("List of joints to compare.")]
        public List<KinectInterop.JointType> poseJoints = new List<KinectInterop.JointType>();

        /*
        public struct PoseModelData
        {
            public int anim;
            public int b_index;
            public Vector3 BonePos;

            public PoseModelData(int a_anim, int a_b_index, Vector3 a_BonePos)
            {
                anim = a_anim;
                b_index = a_b_index;
                BonePos = a_BonePos;
            }
        }*/

        public class PoseModelData
        {
            public int anim;
            public int b_index;
            public Vector3 BonePos;
            
        }

        //public AnimationClip[] poses;
        //public AnimationClip testanim;

        //public GameObject Test_char;
        protected Animator char_animator;
        protected AnimatorOverrideController animatorOverrideController;
        // Start is called before the first frame update

        private Transform[] bone_data;

        private Transform[,] Save_pose_data;
        private int boneCount;

        /*
        // data for each saved pose
        public class SavePoseData
        {
            public int index;
            public Vector3[] avBoneDirs;
        }*/

        private PoseModelData test = new PoseModelData();

        private bool flag = false;

        public GameObject[] ExerPoses;
        private Animator[] ExerPosesAnims;

        private int[] animChange = { 0, 1, 2, 3, 4, 5 };
        void Start()
        {
            char_animator = this.GetComponent<Animator>();

            for(int i =0; i < ExerPoses.Length; i++)
            {
                ExerPosesAnims[i] = ExerPoses[i].GetComponent<Animator>();
            }
            //animatorOverrideController = new AnimatorOverrideController(char_animator.runtimeAnimatorController);
            //char_animator.runtimeAnimatorController = animatorOverrideController;

            Save_pose_data = new Transform[2 * Exercises, poseJoints.Count];
           
        }

        // Update is called once per frame
        void Update()
        {

            //for (int i = 0; i < ExerPoses.Length; i++)
            //{

            /*
            for(int i = 0; i < poseModel.GetBoneTransformCount(); i++)
            {
                Debug.Log(poseModel.GetBoneTransform(i));
            }*/

            //Every exercise has 2 states Concentric and Eccentric state
            //bool check = char_animator.GetAnimatorTransitionInfo(0).IsName("Entry -> Shoulder start");

            // The initial transition happens with a delay, always ensure that the delay is accouted for, to be on safer side give the time delay as 1 and then find the bone transitions to get the exact positions
            if (!flag && char_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                int numAnims = char_animator.runtimeAnimatorController.animationClips.Length;
                foreach (int i in animChange)
                {

                }
                for (int i = 0; i < 2 * Exercises; i++)
                {
                    //animatorOverrideController["Test"] = exerciseClips[i];
                    for (int j = 0; j < poseJoints.Count; j++)
                    {
                        Save_pose_data[i, j] = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(poseJoints[j], false));
                        //Debug.Log(Save_pose_data[i,j].position);
                        test.anim = i;
                        test.b_index = j;
                        test.BonePos = Save_pose_data[0, j].position;

                        //Debug.Log()
                        Debug.Log("Animation num   " + i + "   Bone number   " + j + "   Bone position   " + Save_pose_data[i, j].position);
                        //Debug.Log(new PoseModelData(i, j, Save_pose_data[i, j].position));
                    }

                    
                    
                    if (i < 5)
                    {
                        char_animator.SetInteger("changeAnim", i + 1);
                        Debug.Log("THIS IS INSANEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
                    }
                        
                    else
                        char_animator.SetInteger("changeAnim", 0);
                        
                }
                flag = true;
            }
            

            /*
            for(int i = 0; i < Save_pose_data.Length; i++)
            {
                Save_pose_data[i,] = GetBoneData(poseModel);
                if(Save_pose_data[i,] != null)
                {
                    if (i != 5)
                        char_animator.SetInteger("changeAnim", i + 1);
                    else
                        char_animator.SetInteger("changeAnim", 0);
                }
                Debug.Log(Save_pose_data[i,]);
                
            }*/           

            //Debug.Log(char_animator.GetBoneTransform(HumanBodyBones.LeftHand).position);
            //Debug.Log(poseModel.GetBoneTransform(8).position);
            //Debug.Log(Save_pose_data);
        }

        /*
        // adds current pose of poseModel to the saved poses list
        private void AddCurrentPoseToSaved(float fCurrentTime, bool isMirrored)
        {

            PoseModelData pose = new PoseModelData();
            pose.fTime = fCurrentTime;
            pose.avBoneDirs = new Vector3[poseJoints.Count];

            // save model rotation
            Quaternion poseModelRotation = poseModel.transform.rotation;


            for (int i = 0; i < poseJoints.Count; i++)
            {
                HumanBodyBones joint = poseJoints[i];
                HumanBodyBones nextJoint = HumanBodyBones.GetNextJoint(joint);

                if (nextJoint != joint && (int)nextJoint >= 0 && (int)nextJoint < jointCount)
                {
                    Transform poseTransform1 = poseModel.GetBoneTransform(poseModel.GetBoneIndexByJoint(joint, isMirrored));
                    Transform poseTransform2 = poseModel.GetBoneTransfsorm(poseModel.GetBoneIndexByJoint(nextJoint, isMirrored));

                    if (poseTransform1 != null && poseTransform2 != null)
                    {
                        pose.avBoneDirs[i] = (poseTransform2.position - poseTransform1.position).normalized;
                    }
                }
            }

            // add pose to the list
            alSavedPoses.Add(pose);

            // restore model rotation
            poseModel.transform.rotation = poseModelRotation;
        }*/

        private Transform[] GetBoneData(PoseModelHelper ModelData)
        {
            for (int j = 0; j < ModelData.GetBoneTransformCount(); j++)
            {
                bone_data[j] = ModelData.GetBoneTransform(j);
                Debug.Log(bone_data[j].position);
            }
            return bone_data;
        }
    }
}

