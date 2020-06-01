using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInstruction : MonoBehaviour
{
    public GameObject[] Steps;
    public int nextClick;

    // Start is called before the first frame update
    void Start()
    {
        nextClick = 0;
    }
    
    //....................................Begin Next Button's Function....................................
    public void NextButton()
    {   
        if(nextClick < 3)
        {
            Steps[nextClick].SetActive(false);
            Steps[nextClick + 1].SetActive(true);
            nextClick++;
        }
    }
    //.....................................End Next Button's Function.....................................

    //..................................Begin Previous Button's Function..................................
    
    public void PreviousButton()
    {
        if (nextClick > 0)
        {
            Steps[nextClick].SetActive(false);
            Steps[nextClick - 1].SetActive(true);
            nextClick--;
        }
    }
    //...................................End Previous Button's Function...................................


}
