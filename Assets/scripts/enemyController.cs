using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public float speed;
    public float changeTime = 2.0f;
    public GameObject projectilePrefab;
    public float max_rotation = 30f;
    public ParticleSystem explosion;
    float current_rotation;

    public Transform checkpoints;
    Vector2 home;
    Vector2 destination;
    new Rigidbody2D rigidbody2D;
    float timer;
    int mode = 0;
    int counter = 9;
    int id;
    bool armed = false;
    bool dying = false;
    float point_value;

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
      point_value = 100f;
      //Shoot();
      //animator = GetComponent<Animator>();
    }


    public void Launch(Transform ch, Vector2 h, int input_id, bool arm){
      rigidbody2D = GetComponent<Rigidbody2D>();
      timer = changeTime-1.5f;
      checkpoints = ch;
      counter = 0;
      home = h;
      id = input_id;
      armed = arm;
      getDestination();
      current_rotation = max_rotation;

    }

    // Update is called once per frame
    void Update()
    {
      timer -= Time.deltaTime;
      if (timer < (-1 * changeTime))
        {
            timer = changeTime;
        }
        if(timer < 1 && mode < 1)
          getDestination();

    }

    void FixedUpdate()
    {
      Vector2 position = GetComponent<Rigidbody2D>().position;

      if(mode ==0 )
        MoveForward();
      else if(mode == 1)
        MoveForward();
      else if(mode == 2){
        Idle();
      }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
      var name = other.gameObject.name;
      
      if (name=="boundryBot")
      {
          Reset();
      }
       
      shipController player = other.gameObject.GetComponent<shipController >();

      if (player != null)
      {
          player.ChangeHealth(-1);
          Hit();
      }


    }

    public void Reset()
    {
      //Debug.Log( "inside Reset" );
      Vector2 position = GetComponent<Rigidbody2D>().position;
      Vector2 boundry = GameObject.Find("boundryTop").transform.position;
      position.y = boundry.y;
      this.transform.position = position;
      getDestination();
    }

    public void Idle(){
      //Debug.Log( "inside idle" );
      Quaternion toRotation = Quaternion.AngleAxis(0, Vector3.forward);
      if(toRotation == transform.rotation){
        Vector2 position = GetComponent<Rigidbody2D>().position;
        if(timer < 0)
          position.x += 0.1f * speed *Time.deltaTime ;
        else
          position.x += 0.1f * speed * Time.deltaTime * -1;
        transform.position = position;
     }else{
      timer = changeTime/2;
      transform.rotation  = Quaternion.RotateTowards(transform.rotation, toRotation, 270 * Time.deltaTime);
     }

    }

    /*public void Dying(){
      Vector2 position = GetComponent<Rigidbody2D>().position;

      Vector2 move_position = Vector2.MoveTowards(transform.position, destination, speed/10 *Time.deltaTime);
      transform.position = move_position;

      Quaternion toRotation = Quaternion.AngleAxis(0, Vector3.right);

      transform.rotation  = Quaternion.RotateTowards(transform.rotation, toRotation, 10 * Time.deltaTime);
    }*/

    public void Attack(Transform ch)
    {
      armed = true;
      this.checkpoints = ch;
      counter = 0;
      mode = 0;
      setThruster(true);
      current_rotation = max_rotation*2;
      //Debug.Log( "inside attack" );
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Hit()
    {
      //Debug.Log( "inside Hit" );

      GameManager.instance.ShipDestroyed(id, mode);
      mode = 2;
      dying = true;
      transform.GetComponent<BoxCollider2D>().enabled = false;
      //StartCoroutine(removeEnemy());
      removeEnemy();
      ScoreKeeper.instance.addPoints(point_value);
    }

    //IEnumerator
    void removeEnemy(){
      //yield return new WaitForSeconds(1f);
      ParticleSystem expl= Instantiate(explosion, transform.position, Quaternion.identity);
      expl.Play();
      Destroy(expl, 1f);
      Destroy(gameObject);
    }

    void Shoot()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        //transform position to fire Start
        //ransform rotation towards target
        //po . get rigidbody2d.velocity = transform.forward * speed
        Vector2 target = GameObject.Find("ship").transform.position;
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        //Vector2 diff = target - rigidbody2D.position;
        //float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;

        //projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        projectile.Launch(target - rigidbody2D.position, 70);

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
          GameManager.instance.addIdle(id);
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

    public int getMode(){
      return mode;
    }

  void setThruster(bool on){
    Transform child = transform.FindChild("thruster").transform;
     
    SpriteRenderer big = child.FindChild("big").GetComponent<SpriteRenderer>();
    //GameObject small = child.FindChild("small");

    if(on)
      big.enabled = true;
    else
      big.enabled=false;
  }
}
