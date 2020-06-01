using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseArchive : MonoBehaviour
{
    [Tooltip("Gather all the exercises that need to be saved (Motion data captured in Motion capture studio")]
    public static Vector3[][] ArraySavedPoses;

    [Tooltip("Total number of exercises present in the experience.")]
    public int numExercises;

    public int numPoseJoints;

    [Tooltip("Number of the exercise that is accessed.")]
    public static int exerciseIndex;
    private bool flag = false;

    /*
    public class PoseModelData
    {
        public int numExercise;
        public Transform[] BonePos;
    }*/

    [Tooltip("List of SavedPoses using PoseModelDataClass object.")]
    public static List<PoseModelDataClass> AllSavedPoses = new List<PoseModelDataClass>();
    
    public static int saveCounter;

    // Start is called before the first frame update
    void Start()
    {
        //For every exercise, the start pose and the end pose data of the exercise is saved
        //This is in order to check if user is completing both the poses during the exercise and also helps in counting repetitions
        //For the pupose of this project only 3 exercises are saved
        ArraySavedPoses = new Vector3[2 * numExercises][];
    }

    // Update is called once per frame
    void Update()
    {
        if(saveCounter == 6 && !flag)
        {
            for (int i = 0; i < 2 * numExercises; i++)
            {
                //Get the data of the particular pose from the respective exercise, store it PoseModelDataClass object and then add it to the AllSavedPoses array
                PoseModelDataClass pose = new PoseModelDataClass();

                pose.numExercise = exerciseIndex;
                pose.BonePos = ArraySavedPoses[i];

                AllSavedPoses.Add(pose);
            }
            flag = true;
        }
        
    }
}
