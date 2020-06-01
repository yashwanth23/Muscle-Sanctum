using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemFlow : MonoBehaviour
{
    public GameObject Scene1;
    public GameObject Scene2;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        Scene1.SetActive(true);
        Scene2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Scene1.activeSelf && Scene1.GetComponent<RestPeriod>().isResting)
        {
            timer += Time.deltaTime;
        }

        if(timer > 30f)
        {
            timer = 0;
            Scene1.SetActive(false);
            Scene2.SetActive(true);
        }
    }
}
