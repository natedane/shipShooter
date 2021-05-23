using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public static HealthBar instance { get; private set; }
    SpriteRenderer red1;
    SpriteRenderer blue1;
    SpriteRenderer blue2;
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
		red1 = transform.FindChild("red1").GetComponent<SpriteRenderer>();
        blue1 = transform.FindChild("blue1").GetComponent<SpriteRenderer>();
        blue2 = transform.FindChild("blue2").GetComponent<SpriteRenderer>();
	}

    void setHealth(int amount){
    	if(amount == 1){
    		red1.enabled = true;
    		blue1.enabled = false;
	   	}
	   	if(amount == 2){
    		red1.enabled = true;
    		blue1.enabled = true;
    		blue2.enabled = false;
	   	}
	   	if(amount == 3){
    		red1.enabled = true;
    		blue1.enabled = true;
    		blue2.enabled = true;
	   	}
    }

}
