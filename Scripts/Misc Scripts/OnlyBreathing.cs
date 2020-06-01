using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyBreathing : MonoBehaviour
{
    public GameObject VirtualTrainer;
    public GameObject Instruction2;
    public int delay_time;
    // Start is called before the first frame update
    void Start()
    {
        VirtualTrainer.SetActive(false);
        
        //Instruction2.SetActive(false);
        Instruction2.GetComponent<OnFormCorrect>().enabled = false;
        StartCoroutine(BreatheDelay());
    }
    

    IEnumerator BreatheDelay()
    {
        WaitForSeconds delayTime = new WaitForSeconds(delay_time);

        while (true)
        {
            yield return delayTime;
            break;
        }
        VirtualTrainer.SetActive(true);
        Instruction2.GetComponent<OnFormCorrect>().enabled = true;
        Debug.Log("Scripts enabled");
    }
}
