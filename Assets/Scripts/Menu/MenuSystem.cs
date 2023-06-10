using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{

    public List<GameObject> Menulist = new List<GameObject>();
    private int ActiveMenuIndex;

    void Awake()
    {
        ActiveMenuIndex = 0; 
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject menu in Menulist)
        {
            menu.SetActive(false);
        }

        Menulist[ActiveMenuIndex].SetActive(true);
        //If pressed "I" or pressed "Escape"
        if (Input.GetButtonDown("Toggle Debug"))
        {
            if (ActiveMenuIndex == 0)
            {
                GameManager.isPaused = true;
                ActiveMenuIndex = 1;
            }
            else 
            {
                GameManager.isPaused = false;
                ActiveMenuIndex = 0;
            }
        }

        if (ActiveMenuIndex == 0)
        {

        }


    }

}
