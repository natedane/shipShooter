using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuController : MonoBehaviour
{
    public int level = 0;
    public bool menuUp = false;
    public GameObject StartMenu;


    void Start()
    {

       showStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void showStart(){
    	StartMenu.SetActive(true);
    	menuUp = true;
    	Debug.Log("menu is active");

    }

    public void startButton(){
    	StartMenu.SetActive(false);
    	menuUp = false;
    	Debug.Log("start button hit...");
    	Application.LoadLevel("Level1");
    }

    public void exitButton(){
    	Application.Quit();
    }
}
