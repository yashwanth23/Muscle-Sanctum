using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFormCorrect : MonoBehaviour
{
    public GameObject User;
    public GameObject Canvas;
    public float form_delay;
    // Start is called before the first frame update
    void Start()
    {
        User.SetActive(false);
        Canvas.SetActive(false);
        StartCoroutine(FormDelay());
    }
    

    IEnumerator FormDelay()
    {
        WaitForSeconds delayTime = new WaitForSeconds(form_delay);

        while (true)
        {
            yield return delayTime;
            break;
        }
        User.SetActive(true);
        Canvas.SetActive(true);
        Debug.Log("User and Canvas set active");
    }
}
