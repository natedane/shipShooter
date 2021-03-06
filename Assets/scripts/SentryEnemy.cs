using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEnemy : MonoBehaviour
{


    public float speed;
    public float changeTime = 2.0f;
    public float fireTimeMax = .7f;
    public GameObject projectilePrefab;
    public float max_rotation = 30f;
    public float health = 10;
    public ParticleSystem explosion;

    float current_rotation;

    public Transform checkpoints;
    Vector2 home;
    Vector2 destination;
    new Rigidbody2D rigidbody2D;
    float timer;
    float fireTimer;
    int mode = 0;
    int counter = 9;
    GameManager GM;
    int id;
    bool armed = false;
    float point_value = 1000;

    //int verticle = -1;

    //Animator animator;
    // Start is called before the first frame update
    void Start()
    {
      rigidbody2D = GetComponent<Rigidbody2D>();
      timer = changeTime-1.5f;
      getDestination();
      id = 0;
      current_rotation = max_rotation;
      //Shoot();
      //animator = GetComponent<Animator>();
    }


    public void Launch(Transform ch, Vector2 h, GameManager game, int input_id, bool arm){
      rigidbody2D = GetComponent<Rigidbody2D>();
      GM = game;
      timer = changeTime-1.5f;
      checkpoints = ch;
      counter = 0;
      home = h;
      id = input_id;
      armed = arm;
      getDestination();
      current_rotation = max_rotation;
      setThruster(true);
      speed = 4;
    }

    // Update is called once per frame
    void Update()
    {
      timer -= Time.deltaTime;
      if (timer < (-1 * changeTime))
        {
            timer = changeTime;
        }

      	fireTimer -= Time.deltaTime;
	    if(fireTimer <= 0){
	    	Shoot();
	    	fireTimer = fireTimeMax;
	    }
    }

    void FixedUpdate()
    {
      //Vector2 position = GetComponent<Rigidbody2D>().position;

      if(mode ==0 )
        MoveForward();
      if(mode == 1)
        MoveForward();
      if(mode == 2){
        Idle();
      }
       //Debug.Log("position is..."+position);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
      var name = other.gameObject.name;
      
      // if (name=="boundryBot")
      // {
      //     Reset();
      // }
       
      shipController player = other.gameObject.GetComponent<shipController >();

      if (player != null)
      {
          player.ChangeHealth(-1);
          GM.ShipDestroyed(id, mode);
          //mode = 2;
          Destroy(gameObject);
          return;
      }


    }

    // public void Reset()
    // {
    //   //Debug.Log( "inside Reset" );
    //   Vector2 position = GetComponent<Rigidbody2D>().position;
    //   Vector2 boundry = GameObject.Find("boundryTop").transform.position;
    //   position.y = boundry.y;
    //   this.transform.position = position;
    //   getDestination();
    // }

public void Idle(){
      //Debug.Log( "inside idle" );
    Quaternion toRotation = Quaternion.AngleAxis(0, Vector3.forward);
    if(toRotation == transform.rotation){
      Vector2 position = GetComponent<Rigidbody2D>().position;
        if(timer < 0)
          position.x += 0.1f * speed *Time.deltaTime ;
        else
          position.x += 0.1f * speed * Time.deltaTime * -1;
        position.y = home.y;
        transform.position = position;
     }else{
      timer = changeTime/2;
      transform.rotation  = Quaternion.RotateTowards(transform.rotation, toRotation, 270 * Time.deltaTime);
     }


}


    //Public because we want to call it from elsewhere like the projectile script
public void Hit()
{
  //Debug.Log( "inside Hit" );
	Debug.Log("big ship hit "+health);
    health--;
    ScoreKeeper.instance.addPoints(point_value);
    if(health < 1){

	    GM.ShipDestroyed(id, 3);
	    Instantiate(explosion, transform.position, Quaternion.identity).Play();
    	Destroy(gameObject);
  	}
        //animator.SetTrigger("Fixed");
  	 transform.FindChild("shield").GetComponent<ParticleSystem>().Play();

}

void Shoot()
{
	Transform rays = transform.FindChild("rays").transform;
	GameObject t = GameObject.FindGameObjectWithTag("Player");

	if(t == null)
        return;	
	Vector2 target = t.transform.position;
    int i = 0;
    if(armed){
    	i = 1;
    	armed = false;
    }
    else
    	armed = true;
	//for(int i =0; i < rays.childCount; i++){
	Vector2 ray = rays.GetChild(i).position;
    GameObject projectileObject = Instantiate(projectilePrefab, ray + Vector2.up * 0.4f, Quaternion.identity);
    Projectile projectile = projectileObject.GetComponent<Projectile>();
    projectile.Launch(target - ray,30);
	//}

    //animator.SetTrigger("Launch");
}

    void getDestination(){
      if(checkpoints.transform.childCount == counter){
        //Debug.Log( "go home "+home );
        destination = home;
        mode ++;
        if(mode == 1)
          current_rotation = max_rotation*10;
        if(mode == 2){
          current_rotation = max_rotation;
          GM.addIdle(id);
          armed = false;
          setThruster(false);
        }
      }
      else{

        destination = checkpoints.GetChild(counter).position;
        //Shoot();
        counter++;
        if( counter == 7 && armed)
          Shoot();
        if(counter == 1)
          current_rotation = max_rotation;
        //Debug.Log( "next destination is " +counter);
      }
      timer = changeTime;
    }

    void MoveForward(){
      Vector2 position = GetComponent<Rigidbody2D>().position;

      //Vector2 move_position = Vector2.MoveTowards(transform.position, destination, speed *Time.deltaTime);
      //rigidbody2D.MovePosition(move_position);
      //Vector2 diff = move_position - position;

      Vector2 diff = destination - position;
      if(Mathf.Abs(diff.x) < .1f && Mathf.Abs(diff.y)< .1f){
          getDestination();
      }

      diff.Normalize();
      float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;
      Quaternion rot = Quaternion.Euler(0,0,angle);
      //float rot = Quaternion.AngleAxis(angle, Vector3.forward);
      transform.rotation  = Quaternion.RotateTowards(transform.rotation, rot, current_rotation * Time.deltaTime);
      //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


      Vector3 velocity = new Vector3(0, -1*speed * Time.deltaTime, 0);
      transform.position += transform.rotation * velocity;
    }

	void setThruster(bool on){
	    Transform thrusters = transform.FindChild("thruster").transform;
	     
		for(int i =0; i < thrusters.childCount; i++){
			Transform child = thrusters.GetChild(i);
			SpriteRenderer big = child.FindChild("big").GetComponent<SpriteRenderer>();
	   		SpriteRenderer small = child.FindChild("small").GetComponent<SpriteRenderer>();
			if(on){
		      big.enabled = true;
		      small.enabled = false;
		    }
		    else{
		      big.enabled=false;
		      small.enabled = true;
		   	 }
		}
    
	}

}
