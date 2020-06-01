using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float power = 0.05f;
    public float duration = 1.0f;
    public Transform cameraTransform;

    public GameObject leavesEffect;
    public float slowDownAmount;
    public bool shouldShake = false;

    Vector3 startPosition;
    float initialDuration;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosition = cameraTransform.localPosition;
        initialDuration = duration;
        leavesEffect.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShake)
        {
            if(duration > 0)
            {
                //Shift camera to a random position within a Unit spehere every delta frame until the given duration
                cameraTransform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
                cameraTransform.localPosition = startPosition;
            }
        }
    }
}
