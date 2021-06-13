using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public int level = 1;
    public bool menuUp = false;
    public static MenuSetup instance { get; private set; }
    public GameObject PauseScreen;
    public GameObject GOScreen;
    public GameObject heroPrefab;
    public Text LevelText;
    public Text ScoreText;
    float timer = 3f;
    bool paused = false;


    void Start()
    {
    	paused = false;
    	instance = this;
        drawLevel();
        GameObject ship = Instantiate(heroPrefab, new Vector2 (0,-10), Quaternion.identity);
        PauseScreen.SetActive(false);
        GOScreen.SetActive(false);
        ScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(menuUp == true){
        	timer -= Time.deltaTime;
        	if(timer <= 0){
         		starting();
 				timer = 3f;
       		}
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            	resume();
            else
            	pause();
        }
    }

    public void nextLevel(){
    	level ++;
    	drawLevel();
    	//GameManager.instance.Setup(id, mode);

    }
    public void gameOver(){
    	showGO();
    }

    void showGO(){
    	LevelText.text = "Game Over";
    	GOScreen.SetActive(true);
    	ScoreText.text = "Final Score: ";
    }

    public void starting(){
    	hideLevel();
    	//Debug.Log("start level...");
    	int enemys = 12 + (level *4);
    	int bEnemy =0;
    	int cEnemy = 0;
		if(level < 4)
    		GameManager.instance.Setup(enemys,level-1,bEnemy,cEnemy);
        else if(level == 4)
          GameManager.instance.Setup(18, 5,1,0);
        else if(level == 5)
          GameManager.instance.Setup(24, 5,3,0);
        else if(level < 8)
          GameManager.instance.Setup(24, 5,0,1);
        

        

    }
    public void restart(){
    	Application.LoadLevel("Level1");

    }
    public void exitButton(){
    	Application.LoadLevel("MainMenu");
    }

    void drawLevel(){
    	//LevelScreen.SetActive(true);
    	//LevelText.SetActive(true);
    	LevelText.text = "Level "+level;
    	menuUp = true;
    }
    void hideLevel(){
    	//LevelScreen.SetActive(true);
    	LevelText.text = "";
    	menuUp = false;
    }

    void pause(){
    	Time.timeScale = 0;
    	LevelText.text = "Paused";
    	paused = true;
    	PauseScreen.SetActive(true);
    }
    public void resume(){
    	Time.timeScale = 1;
    	LevelText.text = "";
    	paused = false;
    	PauseScreen.SetActive(false);
    }
}
