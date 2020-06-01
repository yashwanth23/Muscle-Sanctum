using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject referenceObject;
    public float speed = 0.05f;
  
    // Update is called once per frame
    void Update()
    {
        //Move camera towards the reference point with a delta step
        this.transform.position = Vector3.MoveTowards(transform.position, referenceObject.transform.position, speed * Time.deltaTime);
    }
}
