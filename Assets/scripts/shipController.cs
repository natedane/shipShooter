using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{


  public float speed = 5.0f;
  public int maxHealth = 3;
  public GameObject projectilePrefab;
  public float yPosition = -6f;
  //public ParticleSystem shield;

  public int health { get { return currentHealth; }}
  int currentHealth;

  public float timeInvincible = 2.0f;
  bool isInvincible;
  float invincibleTimer;

  Rigidbody2D rigidbody2d;
  float horizontal;


  //Animator animator;
  //Vector2 lookDirection = new Vector2(1,0);

  // Start is called before the first frame update
  void Start()
  {
      setup();
      HealthBar.instance.setHealth(currentHealth);

  }
  void Awake()
  {
      setup();
      //HealthBar.instance.setHealth(currentHealth);

  }
  void setup()
  {
      rigidbody2d = GetComponent<Rigidbody2D>();
      //animator = GetComponent<Animator>();
      currentHealth = maxHealth;
  }

  // Update is called once per frame
  void Update()
  {
      horizontal = Input.GetAxis("Horizontal");
      //vertical = Input.GetAxis("Vertical");

      Vector2 move = new Vector2(horizontal, 0f);


      if (isInvincible)
      {
          invincibleTimer -= Time.deltaTime;
          if (invincibleTimer < 0)
              isInvincible = false;
      }

      if(Input.GetKeyDown(KeyCode.Space))
      {
          Launch();
      }
  }

  void FixedUpdate()
  {
      Vector2 position = rigidbody2d.position;
      position.x = position.x + speed * horizontal * Time.deltaTime;
      if(position.y < this.yPosition){
        MoveForward();
      }
      setThruster();


      rigidbody2d.MovePosition(position);
  }

  void MoveForward(){
    Vector3 velocity = new Vector3(0, 2 * Time.deltaTime, 0);
    transform.position += transform.rotation * velocity;
      // Vector2 position = GetComponent<Rigidbody2D>().position;
      // Vector2 destination = new Vector2(position.x, this.yPosition);
      // Vector2 move_position = Vector2.MoveTowards(position, destination, 2 *Time.deltaTime);
      // Debug.Log("moving forward, "+move_position+" from "+ position);

      // rigidbody2d.MovePosition(move_position);
    }

  public void ChangeHealth(int amount)
  {
      if (amount < 0)
      {
          if (isInvincible)
              return;

          isInvincible = true;
          invincibleTimer = timeInvincible;
          transform.FindChild("shield").GetComponent<ParticleSystem>().Play();
      }
      currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
      HealthBar.instance.setHealth(currentHealth);

      if(this.currentHealth <= 0)
        PlayerDeath();
      //Debug.Log(currentHealth + "/" + maxHealth);
  }

  void Launch()
  {
      GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

       Projectile projectile = projectileObject.GetComponent<Projectile>();
       projectile.Launch(new Vector2(0,1), 300);

      //animator.SetTrigger("Launch");
  }

  void setThruster(){
    Transform child = transform.FindChild("thruster").transform;
     
    SpriteRenderer big = child.FindChild("big").GetComponent<SpriteRenderer>();
    //GameObject small = child.FindChild("small");

    if(horizontal != 0 || transform.position.y < this.yPosition)
      big.enabled = true;
    else
      big.enabled=false;
  }

  void PlayerDeath(){

      GameManager.instance.PlayerDestroyed();

      transform.FindChild("explosion").GetComponent<ParticleSystem>().Play();
      Destroy(gameObject);
  }
}
