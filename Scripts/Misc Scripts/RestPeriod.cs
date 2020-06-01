using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestPeriod : MonoBehaviour
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
        if (!isResting && Trainer.GetComponent<CountVisuals>().restStarted)
        {
            RestPeriodVisual.SetActive(true);
            isResting = true;
        }
    }
}
