using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    public float RotateSpeed = 0.5f;
    
    // Update is called once per frame
    void Update()
    {
        //Rotating skybox to give a natural feel for the scene
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
    }
}
