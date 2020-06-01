using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCheck : MonoBehaviour
{
    public ParticleSystem confetti;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            Instantiate(confetti, this.transform.position, Quaternion.identity);
            flag = true;
        }
    }
}
