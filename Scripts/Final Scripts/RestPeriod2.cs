using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestPeriod2 : MonoBehaviour
{
    public GameObject Trainer;
    public GameObject RestPeriodVisual;
    public bool isResting;

    // Start is called before the first frame update
    void Start()
    {
        RestPeriodVisual.SetActive(false);
        isResting = false;
    }

    // Update is called once per frame
    void Update()
    {   
        // When the user enters the resting period after an exercise, start rest period animation visuals on the screen 
        // Activate the animation once and get out of the loop
        if (!isResting && Trainer.GetComponent<CountVisuals2>().restStarted)
        {
            RestPeriodVisual.SetActive(true);
            isResting = true;
        }
    }
}
