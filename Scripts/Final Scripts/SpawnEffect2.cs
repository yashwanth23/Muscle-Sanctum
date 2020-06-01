using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect2 : MonoBehaviour
{
    [Tooltip("Parameters to spawn the Trainer.")]
    public ParticleSystem Spawn_Particles;
    private ParticleSystem SpawnObject;
    private bool isStarted;
    private float EmergeSpeed = 1.0f;
    public bool isSpawned;

    [Tooltip("UI Gameobject with PoseMatchInstruction animation.")]
    public GameObject PoseMatchInstruction;
    
    private bool isDestroyed;
    private Vector3 TrainerPosition;

    // Start is called before the first frame update
    void Start()
    {
        //When the experience starts make sure to disable scripts and gameobjects that shouldn't run at the start
        isStarted = false;
        isSpawned = false;
        TrainerPosition = this.transform.position + new Vector3(0, 2f, 0);

        PoseMatchInstruction.SetActive(false);

        this.GetComponent<CheckPoseMatch2>().enabled = false;
        this.GetComponent<CountVisuals2>().enabled = false;
        this.GetComponent<PoseFeedback>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //When the trainer emerges from the ground start the instantiate particle effects
        if (!isStarted)
        {
            SpawnObject = Instantiate(Spawn_Particles, TrainerPosition + new Vector3(0, 0.2f, 0), Quaternion.identity);
            isStarted = true;
        }

        if (!isDestroyed)
        {
            //Destroy dead particle effects
            if (!SpawnObject.IsAlive())
            {
                Destroy(SpawnObject);
                isDestroyed = true;
                isSpawned = true;

                //Change the animation from Idle to Exercise pose
                this.GetComponent<Animator>().SetBool("spawned", true);
                this.GetComponent<CheckPoseMatch2>().enabled = true;
                PoseMatchInstruction.SetActive(true);
            }
        }

        //Move the trainer from below the surface to above to give a spawning effect
        transform.position = Vector3.Lerp(transform.position, TrainerPosition, EmergeSpeed * Time.deltaTime);
        

    }
}
