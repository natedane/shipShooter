using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance { get; private set; }
    float score = 0;
    public Text scoreText;
    void Start()
    {
        instance = this;
        setup();
    }

    void Awake()
    {
        instance = this;
        setup();
    }

	void setup(){
		score = 0;
		draw();
	}

    public void addPoints(float amount){
    	this.score += amount;
    	draw();
    }
    void draw(){
    	scoreText.text = "Score: "+score;
    }


}
