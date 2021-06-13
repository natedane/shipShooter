using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
	public int value;
	public int type;
	public ParticleSystem explosion;
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }      

	void OnCollisionEnter2D(Collision2D other)
    {
      var name = other.gameObject.name;
      
      if (name=="boundryBot")
      {
          Destroy(gameObject);
      }
       
      shipController player = other.gameObject.GetComponent<shipController >();

      if (player != null)
      {
      	if(type == 0)
    		PointPickup();
        effect();
      }

    }

    void effect(){
    	
    	ParticleSystem expl= Instantiate(explosion, transform.position, Quaternion.identity);
        expl.Play();
        Destroy(expl, 1f);
        Destroy(gameObject);
    }

    void PointPickup(){
    	ScoreKeeper.instance.addPoints(this.value);
    }
    void AddHealth(shipController player){
    	player.ChangeHealth(this.value);
    }
}
