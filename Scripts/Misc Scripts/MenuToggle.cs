using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Instructions;
    public GameObject Welcome;

    private void Start()
    {
        Instructions.SetActive(false);
        Welcome.SetActive(false);
    }

    public void ToggleMenu()
    {
        MainMenu.SetActive(false);
        Instructions.SetActive(true);
    }

    public void ToggleInstruction()
    {
        Instructions.SetActive(false);
        Welcome.SetActive(true);
    }
}
