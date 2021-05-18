using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public float speed;
    public float changeTime = 3.0f;
    public GameObject projectilePrefab;
    public float max_rotation = 30f;

    public Transform checkpoints;
    Vector2 home;
    Vector2 destination;
    new Rigidbody2D rigidbody2D;
    float timer;
    int mode = 0;
    int counter = 9;
    GameManager GM;
    int id;
    //bool armed = true;

    //int verticle = -1;

    //Animator animator;
    // Start is called before the first frame update
    void Start()
    {
      rigidbody2D = GetComponent<Rigidbody2D>();
      timer = changeTime-1.5f;
      getDestination();
      id = 0;
      //Shoot();
      //animator = GetComponent<Animator>();
    }


    public void Launch(Transform ch, Vector2 h, GameManager game, int input_id){
      rigidbody2D = GetComponent<Rigidbody2D>();
      GM = game;
      timer = changeTime-1.5f;
      checkpoints = ch;
      counter = 0;
      home = h;
      id = input_id;
      getDestination();

    }

    // Update is called once per frame
    void Update()
    {
      timer -= Time.deltaTime;
      if (timer < (-1 * changeTime))
        {
            timer = changeTime;
        }

    }

    void FixedUpdate()
    {
      Vector2 position = GetComponent<Rigidbody2D>().position;

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
        shipController player = other.gameObject.GetComponent<shipController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
            Destroy(gameObject);
            return;
        }
        var boundBot = other.gameObject.name;
        //Debug.Log( "collide (name) : " + boundBot );
        if (boundBot=="boundryBot")
        {
            Reset();
        }
        // if (boundBot=="checkpoint")
        // {
        //     counter++;
        // }


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

    public void Attack()
    {
      //Debug.Log( "inside attack" );
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Hit()
    {
      GM.ShipDestroyed(id, mode);
      Destroy(gameObject);
        //animator.SetTrigger("Fixed");
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
        projectile.Launch(target - rigidbody2D.position, 100);

        //animator.SetTrigger("Launch");
    }

    void getDestination(){
      if(checkpoints.transform.childCount == counter){
        //Debug.Log( "go home "+home );
        destination = home;
        mode ++;
      }
      else{
        destination = checkpoints.GetChild(counter).position;
        //Debug.Log( "counter going "+counter+ " "+ checkpoints.transform.childCount);

        //Debug.Log("going to "+destination);
        //Shoot();
        counter++;
      }
    }

    void MoveForward(){
      Vector2 position = GetComponent<Rigidbody2D>().position;

      //Vector2 move_position = Vector2.MoveTowards(transform.position, destination, speed *Time.deltaTime);
      //rigidbody2D.MovePosition(move_position);
      //Vector2 diff = move_position - position;

      Vector2 diff = destination - position;
      if(Mathf.Abs(diff.x) < .5f && Mathf.Abs(diff.y)< .5f){
          getDestination();
      }

      diff.Normalize();
      float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;
      Quaternion rot = Quaternion.Euler(0,0,angle);
      //float rot = Quaternion.AngleAxis(angle, Vector3.forward);
      transform.rotation  = Quaternion.RotateTowards(transform.rotation, rot, max_rotation * Time.deltaTime);
      //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


      Vector3 velocity = new Vector3(0, -1*speed * Time.deltaTime, 0);
      transform.position += transform.rotation * velocity;

    }
}
