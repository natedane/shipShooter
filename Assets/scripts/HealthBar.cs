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
    SpriteRenderer black1;
    SpriteRenderer black2;
    SpriteRenderer black3;
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
        black1 = transform.FindChild("black1").GetComponent<SpriteRenderer>();
        black2 = transform.FindChild("black2").GetComponent<SpriteRenderer>();
        black3 = transform.FindChild("black3").GetComponent<SpriteRenderer>();
        setHealth(3);
	}

    public void setHealth(int amount){
    	if(amount == 0){
    		red1.enabled = false;
    		blue1.enabled = false;
    		black1.enabled = true;
    		black2.enabled = true;
    		black3.enabled = true;
	   	}
    	else if(amount == 1){
    		red1.enabled = true;
    		blue1.enabled = false;
    		black1.enabled = false;
    		black2.enabled = true;
    		black3.enabled = true;
	   	}
	   	else if(amount == 2){
    		red1.enabled = true;
    		blue1.enabled = true;
    		blue2.enabled = false;
    		black1.enabled = false;
    		black2.enabled = false;
    		black3.enabled = true;
	   	}
	   	else if(amount == 3){
    		red1.enabled = true;
    		blue1.enabled = true;
    		blue2.enabled = true;
    		black1.enabled = false;
    		black2.enabled = false;
    		black3.enabled = false;
	   	}
    }

}
