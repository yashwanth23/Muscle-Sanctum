using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public ParticleSystem Spawn_Particles;
    //public Vector3 Instantiate_Point;
    //public Light Reflection_light;

    private bool isStarted;
    private ParticleSystem SpawnObject;
    private float EmergeSpeed = 1.0f;
    //private float IntensitySpeed = 0.5f;

    private bool isDestroyed;

    public bool isSpawned;

    private Vector3 TrainerPosition;
    // Start is called before the first frame update
    void Start()
    {
        isStarted = false;
        isSpawned = false;
        TrainerPosition = this.transform.position + new Vector3(0, 2f, 0);
        //this.transform.position -= new Vector3(0, 2f, 0);
        //Reflection_light.intensity = 0;

        this.GetComponent<CheckPoseMatch>().enabled = false;
        this.GetComponent<CountVisuals>().enabled = false;
        this.GetComponent<PoseFeedback>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            SpawnObject = Instantiate(Spawn_Particles, TrainerPosition + new Vector3(0, 0.2f, 0), Quaternion.identity);
            isStarted = true;
        }

        if (!isDestroyed)
        {
            if (!SpawnObject.IsAlive())
            {
                Destroy(SpawnObject);
                isDestroyed = true;
                isSpawned = true;

                //Changing the animation from Idle to Exercise pose
                this.GetComponent<Animator>().SetBool("spawned", true);
                this.GetComponent<CheckPoseMatch>().enabled = true;
            }
        }
        

        transform.position = Vector3.Lerp(transform.position, TrainerPosition, EmergeSpeed*Time.deltaTime);

        /*
        if(Reflection_light.intensity < 2)
        {
            Reflection_light.intensity += IntensitySpeed * Time.deltaTime;
        }
        */
        /*
        if(transform.position.y >= transform.position.y + 2)
        {
            isSpawned = true;
        }*/

    }
}
